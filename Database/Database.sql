USE master 
GO 
IF EXISTS(SELECT * FROM sys.sysdatabases WHERE name = 'DataTramXangDau')
	DROP DATABASE DataTramXangDau
GO 

CREATE DATABASE DataTramXangDau
ON PRIMARY 
(
	name = DataTramXangDau,
	filename = 'E:\PMQLTramXangDau\Database\DataTramXangDau.mdf',
	SIZE = 10MB,
	FILEGROWTH = 10MB
)
LOG ON 
(
	name = DataTramXangDau_log,
	filename = 'E:\PMQLTramXangDau\Database\DataTramXangDau_log.ldf',
	SIZE = 10MB,
	FILEGROWTH = 10MB
)
GO

USE DataTramXangDau
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'Accounts')
	DROP TABLE Accounts
GO 

CREATE TABLE Accounts
(
	--Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Username NVARCHAR(50) NOT NULL,
	Password NVARCHAR(50) NOT NULL
)
GO
INSERT INTO ACCOUNTS
VALUES(N'Tuân','admin','admin')

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'Products')
	DROP TABLE Products
GO 

CREATE TABLE Products
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Amount FLOAT NOT NULL
)

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'Revenues')
	DROP TABLE Revenues
GO 

CREATE TABLE Revenues
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Date DATETIME NOT NULL,
	OutOfGas92 FLOAT NOT NULL,
	PriceOfGas92 INT NOT NULL,
	OutOfGas95 FLOAT NOT NULL,
	PriceOfGas95 INT NOT NULL,
	OutOfOil FLOAT NOT NULL,
	PriceOfOil INT NOT NULL,
	Income FLOAT  NOT NULL,
	Spend FLOAT NOT NULL,
	Inventory FLOAT NOT NULL
)

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'InventoryMoney')
	DROP TABLE InventoryMoney
GO 

CREATE TABLE InventoryMoney
(
	Amount FLOAT NOT NULL
)
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'ImportProducts')
	DROP TABLE ImportProducts
GO 

CREATE TABLE ImportProducts
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	InputDate DATETIME NOT NULL,
	Partners NVARCHAR(100) NOT NULL,
	Product INT NOT NULL,
	Amount FLOAT NOT NULL,
	UnitPrice FLOAT NOT NULL,
	IntoMoney FLOAT NOT NULL
)
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'PayTable')
	DROP TABLE PayTable
GO 

CREATE TABLE PayTable 
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	InputDate DATETIME NOT NULL,
	Receiver NVARCHAR(100) NOT NULL,
	Payer NVARCHAR(100) NOT NULL,
	Describe NVARCHAR(300) NOT NULL,
	Money FLOAT NOT NULL
)
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'ReceiveTable')
	DROP TABLE ReceiveTable
GO 

CREATE TABLE ReceiveTable 
(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	InputDate DATETIME NOT NULL,
	Receiver NVARCHAR(100) NOT NULL,
	Payer NVARCHAR(100) NOT NULL,
	Describe NVARCHAR(300) NOT NULL,
	Money FLOAT NOT NULL
)
GO 

---------------------Relationship-------------------------
ALTER TABLE dbo.ImportProducts ADD CONSTRAINT fk_ip_p FOREIGN KEY (Product) REFERENCES dbo.Products(Id)
GO 



---------------------Store Procedure------------------
CREATE PROC USP_LogIn @Username NVARCHAR(50), @Password NVARCHAR(50)
AS 
	BEGIN 
		SELECT * FROM dbo.Accounts WHERE Username = @Username AND Password = @Password
	END 
GO

CREATE PROC USP_ChangePassword @Username NVARCHAR(50), @NewPassword NVARCHAR(50)
AS 
	BEGIN 
		UPDATE dbo.Accounts SET Password = @NewPassword WHERE Username = @Username
	END 
GO

CREATE PROC USP_EditProduct @id INT, @name NVARCHAR(100), @amount FLOAT
AS
	BEGIN 
		UPDATE dbo.Products SET Name = @name, Amount = @amount WHERE Id = @id
	END 
GO 

CREATE PROC USP_IP @partners NVARCHAR(100), @product INT, @amount FLOAT, @unitPrice FLOAT  
AS
	BEGIN 
		INSERT dbo.ImportProducts VALUES  (GETDATE() ,@partners ,@product,@amount, @unitPrice,@amount*@unitPrice)
		BEGIN
			UPDATE dbo.Products 
			SET Amount = @amount + (SELECT Amount FROM dbo.Products WHERE Id = @product)
			WHERE Id = @product
		END 
	END 
GO 

CREATE PROC USP_DeleteProduct @id INT 
AS
	BEGIN 
		DECLARE @exist INT = 0
		SELECT @exist = COUNT(*) FROM dbo.Products WHERE Id = @id
		IF(@exist > 0)
		BEGIN
			DELETE dbo.Products WHERE Id = @id
		END 
	END
GO 


