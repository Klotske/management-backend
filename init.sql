CREATE TABLE [dbo].[Positions]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Title] nvarchar(30) NOT NULL
)

INSERT INTO [dbo].[Positions] ([Title])
VALUES (N'Инженер-конструктор'), (N'Инженер-технолог')

CREATE TABLE [dbo].[Departments]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [Name] nvarchar(30) NOT NULL
)

INSERT INTO [dbo].[Departments] ([Name])
VALUES (N'КБ'), (N'Технологический отдел')

CREATE TABLE [dbo].[Rates]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [PositionId] INT NOT NULL,
    [StartDate] DateTime NOT NULL,
    [Amount] INT NOT NULL
)

CREATE TABLE [dbo].[Schedules]
(
    [Id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
    [DepartmentId] INT NOT NULL,
    [PositionId] INT NOT NULL,
    [StartDate] DateTime NOT NULL,
    [Quantity] INT NOT NULL
)