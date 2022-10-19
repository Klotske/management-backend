using management_api.Data;
using management_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace management_api.Controllers
{
    [Route("api/report")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly MSSQLContext _context;

        public ReportController(MSSQLContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult<Report>> Get(DateTime startDate, DateTime endDate)
        {
            var report = new Report()
            {
                Start = startDate,
                End = new DateTime(endDate.Year, endDate.Month, DateTime.DaysInMonth(endDate.Year, endDate.Month)),
            };

            report.Months = Enumerable.Range(0, Int32.MaxValue)
                .Select(report.Start.AddMonths)
                .TakeWhile(e => e <= report.End)
                .Select(e => new ReportMonthItem()
                {
                    Start = e,
                    End = e.AddMonths(1),
                })
                .ToList();

            foreach (var month in report.Months)
            {
                var monthSchedules = await _context.IntervalSchedules
                    .FromSqlRaw("SELECT *, LEAD([StartDate], 1, DATEFROMPARTS(9999, 12, 31)) OVER (PARTITION BY ([PositionId]) ORDER BY [StartDate]) [EndDate] FROM [dbo].[Schedules]")
                    .Include(s => s.Department)
                    .Include(s => s.Position)
                    .Where(s => month.Start.CompareTo(s.EndDate) <= 0 && s.StartDate.CompareTo(month.End) <= 0)
                    .ToListAsync();
                
                var monthRates = await _context.IntervalRates
                    .FromSqlRaw("SELECT *, LEAD([StartDate], 1, DATEFROMPARTS(9999, 12, 31)) OVER (PARTITION BY ([PositionId]) ORDER BY [StartDate]) [EndDate] FROM [dbo].[Rates]")
                    .Include(ir => ir.Position)
                    .Where(ir => month.Start.CompareTo(ir.EndDate) <= 0 && ir.StartDate.CompareTo(month.End) <= 0)
                    .ToListAsync();

                month.Departments = monthSchedules.Select(s => s.Department)
                    .Distinct()
                    .Select(s => new ReportDepartmentItem()
                    {
                        Department = s,
                    }).ToList();

                foreach (var department in month.Departments)
                {
                    var schedules = monthSchedules
                        .Where(s => s.Department == department.Department)
                        .ToList();

                    foreach (var schedule in schedules)
                    {
                        var rates = monthRates
                            .Where(r => r.PositionId == schedule.PositionId)
                            .Where(r => schedule.StartDate.CompareTo(r.EndDate) <= 0 && r.StartDate.CompareTo(schedule.EndDate) <= 0)
                            .ToList();

                        foreach (var rate in rates)
                        {
                            var rateStart = new[] { rate.StartDate, month.Start }.Max();
                            var rateEnd = new[] { rate.EndDate, month.End }.Min();
                        
                            var scheduleStart = new[] { schedule.StartDate, month.Start }.Max();
                            var scheduleEnd = new[] { schedule.EndDate, month.End }.Min();
                        
                            var time = DateIntersection(rateStart, rateEnd, scheduleStart, scheduleEnd);

                            var workingDays = GetWorkingDays(rate.StartDate, rate.StartDate.AddDays((int)time.TotalDays));
                        
                            var amount = rate.Amount / 20 * workingDays * schedule.Quantity;

                            department.MonthTotal += amount;
                        }
                    }
                }
            }
        
            return report;
        }
        
        private static TimeSpan DateIntersection(DateTime leftStart, DateTime leftEnd, DateTime rightStart, DateTime rightEnd)
        {
            if(leftStart == leftEnd || rightStart == rightEnd)
                return TimeSpan.Zero;

            if (leftStart == rightStart || leftEnd == rightEnd)
                return leftEnd - leftStart;

            if(leftStart < rightStart)
            {
                // Left begins earlier and finishes in the middle
                if(leftEnd > rightStart && leftEnd < rightEnd)
                    return rightStart - leftEnd;
            
                // Left includes right
                if (leftEnd > rightEnd)
                    return rightEnd - rightEnd;
            }
            else
            {
                // Right begins earlier and finishes in the middle
                if (rightEnd > leftStart && rightEnd < leftEnd)
                    return leftStart - rightEnd;

                // Right includes left
                if(rightEnd > leftEnd)
                    return leftEnd - leftStart; // Condition 4
            }
        
            return TimeSpan.Zero;
        }
    
        private static int GetWorkingDays(DateTime from, DateTime to)
        {
            var dayDifference = (int)to.Subtract(from).TotalDays;
            return Enumerable
                .Range(1, dayDifference)
                .Select(x => from.AddDays(x))
                .Count(x => x.DayOfWeek != DayOfWeek.Saturday && x.DayOfWeek != DayOfWeek.Sunday);
        }
    }
}
