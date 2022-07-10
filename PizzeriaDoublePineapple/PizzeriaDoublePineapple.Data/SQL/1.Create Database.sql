CREATE DATABASE [PizzeriaDoublePineappleDb];

USE [PizzeriaDoublePineappleDb];

CREATE TABLE [Sauces](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR (255) NOT NULL,
	[Price] FLOAT (8)
)

CREATE TABLE [Ingredients](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR (255) NOT NULL
)

CREATE TABLE [Pizzas] (
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[Name] VARCHAR(255),
	[PriceS] FLOAT (8),
	[PriceM] FLOAT (8),
	[PriceL] FLOAT (8),
	[SauceId] INT FOREIGN KEY ([SauceId]) REFERENCES [Sauces]([Id]) NOT NULL
)

CREATE TABLE [PizzaIngredients](
	[Id] INT IDENTITY(1,1) PRIMARY KEY,
	[PizzaId] INT FOREIGN KEY ([PizzaId]) REFERENCES [Pizzas]([Id]) NOT NULL,
	[IngredientId] INT FOREIGN KEY ([IngredientId]) REFERENCES [Ingredients]([Id]) NOT NULL
)

CREATE TABLE [Clients](
    [PhoneNumber] VARCHAR(255) PRIMARY KEY,
    [Email] VARCHAR(255) NOT NULL,
    [Name] VARCHAR(255) NOT NULL,
    [Surname] VARCHAR(255) NOT NULL,
    [Address] VARCHAR(255) NOT NULL
)

CREATE TABLE [Orders] (
	[Id] INT IDENTITY (1,1) PRIMARY KEY,
	[PhoneNumber] VARCHAR (255) FOREIGN KEY (PhoneNumber) REFERENCES [Clients]([PhoneNumber]) NOT NULL,
	[Date] DateTime2,
	[TotalCost] FLOAT (8)
);

CREATE TABLE [PizzasOrders] (
	[Id] INT IDENTITY (1,1) PRIMARY KEY,
	[OrderId] INT,
	[PizzaId] INT FOREIGN KEY ([PizzaId]) REFERENCES [Pizzas]([Id]),
	[PizzaName] VARCHAR (255) NOT NULL,
	[PriceS] FLOAT (8)  NOT NULL,
	[PriceM] FLOAT (8)  NOT NULL,
	[PriceL] FLOAT (8)  NOT NULL,
	[PizzaSize] VARCHAR (255) NOT NULL
);

CREATE TABLE [SaucesOrders] (
	[Id] INT IDENTITY (1,1) PRIMARY KEY,
	[OrderId] INT,
	[SauceId] INT FOREIGN KEY ([SauceId]) REFERENCES [Sauces]([Id]),
	[SauceName] VARCHAR (255)  NOT NULL,
	[Price] FLOAT (8)  NOT NULL
);