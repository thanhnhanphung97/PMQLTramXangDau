USE master 
GO 
IF EXISTS(SELECT * FROM sys.sysdatabases WHERE name = 'DataTramXangDau')
	DROP DATABASE DataTramXangDau
GO 

CREATE DATABASE DataTramXangDau
ON PRIMARY 
(
	name = DataTramXangDau,
	filename = 'D:\Study\Git\PMQLTramXangDau\Database\DataTramXangDau.mdf',
	SIZE = 10MB,
	FILEGROWTH = 10MB
)
LOG ON 
(
	name = DataTramXangDau_log,
	filename = 'D:\Study\Git\PMQLTramXangDau\Database\DataTramXangDau_log.ldf',
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
IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_LogIn')
	DROP PROCEDURE dbo.USP_LogIn
GO 

CREATE PROC USP_LogIn @Username NVARCHAR(50), @Password NVARCHAR(50)
AS 
	BEGIN 
		SELECT * FROM dbo.Accounts WHERE Username = @Username AND Password = @Password
	END 
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_ChangePassword')
	DROP PROCEDURE dbo.USP_ChangePassword
GO 

CREATE PROC USP_ChangePassword @Username NVARCHAR(50), @NewPassword NVARCHAR(50)
AS 
	BEGIN 
		UPDATE dbo.Accounts SET Password = @NewPassword WHERE Username = @Username
	END 
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_EditProduct')
	DROP PROCEDURE dbo.USP_EditProduct
GO 

CREATE PROC USP_EditProduct @id INT, @name NVARCHAR(100), @amount FLOAT
AS
	BEGIN 
		UPDATE dbo.Products SET Name = @name, Amount = @amount WHERE Id = @id
	END 
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_IP')
	DROP PROCEDURE dbo.USP_IP
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

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_DeleteProduct')
	DROP PROCEDURE dbo.USP_DeleteProduct
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

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_InsertProduct')
	DROP PROCEDURE dbo.USP_InsertProduct
GO 

CREATE PROC USP_InsertProduct @name NVARCHAR(100)
AS
	DECLARE @amount FLOAT = 0.0
	BEGIN 
		INSERT dbo.Products VALUES  ( @name,@amount)
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetAllProduct')
	DROP PROCEDURE dbo.USP_GetAllProduct
GO 

CREATE PROC USP_GetAllProduct
AS
	BEGIN 
		SELECT * FROM dbo.Products
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_UpdateProduct')
	DROP PROCEDURE dbo.USP_UpdateProduct
GO 

CREATE PROC USP_UpdateProduct @id INT, @name NVARCHAR(100), @amount FLOAT
AS
	BEGIN 
		UPDATE dbo.Products
		SET Amount = @amount, Name = @name
		WHERE Id = @id
	END 
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetAllReceipt')
	DROP PROCEDURE dbo.USP_GetAllReceipt
GO 

CREATE PROC USP_GetAllReceipt
AS
	BEGIN 
		SELECT * FROM dbo.ReceiveTable
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_InsertReceipt')
	DROP PROCEDURE dbo.USP_InsertReceipt
GO 


CREATE PROC USP_InsertReceipt @inputDate DATETIME, @receiver NVARCHAR(100), @payer NVARCHAR(100), @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		INSERT dbo.ReceiveTable VALUES  ( @inputDate ,@receiver ,@payer ,@describe ,@money)
	END
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_UpdateReceipt')
	DROP PROCEDURE dbo.USP_UpdateReceipt
GO 


CREATE PROC USP_UpdateReceipt @id INT,@inputDate DATETIME, @receiver NVARCHAR(100), @payer NVARCHAR(100), @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		UPDATE dbo.ReceiveTable 
		SET InputDate = @inputDate, Receiver = @receiver, Payer = @payer, Describe = @describe, Money = @money
		WHERE Id = @id
	END
GO  

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_DeleteReceipt')
	DROP PROCEDURE dbo.USP_DeleteReceipt
GO 


CREATE PROC USP_DeleteReceipt @id INT
AS
	BEGIN 
		DELETE dbo.ReceiveTable WHERE Id = @id
	END
GO  

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetAllPay')
	DROP PROCEDURE dbo.USP_GetAllPay
GO 

CREATE PROC USP_GetAllPay
AS
	BEGIN 
		SELECT * FROM dbo.PayTable
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_InsertPay')
	DROP PROCEDURE dbo.USP_InsertPay
GO 


CREATE PROC USP_InsertPay @inputDate DATETIME, @receiver NVARCHAR(100), @payer NVARCHAR(100), @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		INSERT dbo.PayTable VALUES  ( @inputDate ,@receiver ,@payer ,@describe ,@money)
	END
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_UpdatePay')
	DROP PROCEDURE dbo.USP_UpdatePay
GO 


CREATE PROC USP_UpdatePay @id INT,@inputDate DATETIME, @receiver NVARCHAR(100), @payer NVARCHAR(100), @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		UPDATE dbo.PayTable 
		SET InputDate = @inputDate, Receiver = @receiver, Payer = @payer, Describe = @describe, Money = @money
		WHERE Id = @id
	END
GO  

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_DeletePay')
	DROP PROCEDURE dbo.USP_DeletePay
GO 


CREATE PROC USP_DeletePay @id INT
AS
	BEGIN 
		DELETE dbo.PayTable WHERE Id = @id
	END
GO  

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetAllRevenue')
	DROP PROCEDURE dbo.USP_GetAllRevenue
GO 


CREATE PROC USP_GetAllRevenue
AS
	BEGIN 
		SELECT * FROM dbo.Revenues
	END
GO  

