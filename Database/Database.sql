USE master 
GO 
IF EXISTS(SELECT * FROM sys.sysdatabases WHERE name = 'DataTramXangDau')
	DROP DATABASE DataTramXangDau
GO 

CREATE DATABASE DataTramXangDau
ON PRIMARY 
(
	name = DataTramXangDau,
	filename = 'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\DataTramXangDau.mdf',
	SIZE = 10MB,
	FILEGROWTH = 10MB
)
LOG ON 
(
	name = DataTramXangDau_log,
	filename = 'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\DataTramXangDau_log.ldf',
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
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Username NVARCHAR(50) NOT NULL,
	Password NVARCHAR(50) NOT NULL
)
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'Employees')
	DROP TABLE Employees
GO 

CREATE TABLE Employees
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Birth DATE NOT NULL,
	Gender BIT NOT NULL,
	Phone NVARCHAR(20) NOT NULL,
	Email NVARCHAR(50) NOT NULL,
	Address NVARCHAR(100) NOT NULL,
	Salary INT NOT NULL
)
GO 

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
	Date DATE NOT NULL,
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
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	InputDate DATE NOT NULL,
	IdPartner INT NOT NULL,
	Product INT NOT NULL,
	Amount FLOAT NOT NULL,
	UnitPrice FLOAT NOT NULL,
	IntoMoney FLOAT NOT NULL
)
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'Partners')
	DROP TABLE Partners
GO 

CREATE TABLE Partners
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	Name NVARCHAR(100) NOT NULL,
	Address NVARCHAR(500) NOT NULL,
	Contact NVARCHAR(50) NOT NULL,
	Phone NVARCHAR(20) NOT NULL,
	Email NVARCHAR(50) NOT NULL,
)
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'PayTable')
	DROP TABLE PayTable
GO 

CREATE TABLE PayTable 
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	InputDate DATE NOT NULL,
	IdReceiver INT NOT NULL,
	IdPayer INT NOT NULL,
	Describe NVARCHAR(300) NOT NULL,
	Money FLOAT NOT NULL
)
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'ReceiveTable')
	DROP TABLE ReceiveTable
GO 

CREATE TABLE ReceiveTable 
(
	Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
	InputDate DATE NOT NULL,
	IdReceiver INT NOT NULL,
	IdPayer INT NOT NULL,
	idProduct INT NOT NULL,
	ExportProduct FLOAT NOT NULL,
	PriceInDay INT NOT NULL,
	Describe NVARCHAR(300) NOT NULL,
	Money FLOAT NOT NULL
)
GO 

---------------------Relationship-------------------------
ALTER TABLE dbo.ImportProducts ADD CONSTRAINT fk_ip_p FOREIGN KEY (Product) REFERENCES dbo.Products(Id)
GO 
ALTER TABLE dbo.ImportProducts ADD CONSTRAINT fk_ip_pn FOREIGN KEY (IdPartner) REFERENCES dbo.Partners(Id)
GO 
ALTER TABLE dbo.ReceiveTable ADD CONSTRAINT fk_rt_p FOREIGN KEY (idProduct) REFERENCES dbo.Products(Id)
GO 
ALTER TABLE dbo.ReceiveTable ADD CONSTRAINT fk_rt_ac FOREIGN KEY (IdReceiver) REFERENCES dbo.Accounts(Id)
GO 
ALTER TABLE dbo.ReceiveTable ADD CONSTRAINT fk_rt_emp FOREIGN KEY (IdPayer) REFERENCES dbo.Employees(Id)
GO 
ALTER TABLE dbo.PayTable ADD CONSTRAINT fk_p_emp FOREIGN KEY (IdReceiver) REFERENCES dbo.Employees(Id)
GO 
ALTER TABLE dbo.PayTable ADD CONSTRAINT fk_p_ac FOREIGN KEY (IdPayer) REFERENCES dbo.Accounts(Id)
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

CREATE PROC USP_EditProduct @id INT, @name NVARCHAR(100)
AS
	BEGIN 
		UPDATE dbo.Products SET Name = @name WHERE Id = @id
	END 
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_IP')
	DROP PROCEDURE dbo.USP_IP
GO 

CREATE PROC USP_IP @inputDate DATE,@idPartners INT, @product INT, @amount FLOAT, @unitPrice FLOAT  
AS
	BEGIN 
		INSERT dbo.ImportProducts VALUES  (GETDATE() ,@idPartners ,@product,@amount, @unitPrice,@amount*@unitPrice)
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

CREATE PROC USP_UpdateProduct @id INT, @name NVARCHAR(100)
AS
	BEGIN 
		UPDATE dbo.Products
		SET Name = @name
		WHERE Id = @id
	END 
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetAllReceipt')
	DROP PROCEDURE dbo.USP_GetAllReceipt
GO 

CREATE PROC USP_GetAllReceipt
AS
	BEGIN 
		SELECT rctb.Id, rctb.InputDate, ac.Name AS Receiver,emp.Name AS Payer, p.Name AS Product, rctb.ExportProduct, rctb.PriceInDay, rctb.Describe, rctb.Money
		FROM dbo.ReceiveTable rctb, dbo.Employees emp, dbo.Accounts ac, dbo.Products p
		WHERE ac.Id = rctb.IdReceiver AND emp.Id = rctb.IdPayer AND rctb.idProduct = p.Id
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetReceiptById')
	DROP PROCEDURE dbo.USP_GetReceiptById
GO 

CREATE PROC USP_GetReceiptById @id INT
AS
	BEGIN 
		SELECT * FROM dbo.ReceiveTable WHERE Id = @id
	END
GO 


IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_InsertReceipt')
	DROP PROCEDURE dbo.USP_InsertReceipt
GO 


CREATE PROC USP_InsertReceipt @inputDate DATE, @idReceiver INT, @idPayer INT,@idProduct INT,@exportProduct FLOAT,@priceInDay INT, @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		INSERT dbo.ReceiveTable VALUES  ( @inputDate, @idReceiver, @idPayer, @idProduct, @exportProduct, @priceInDay, @describe, @money)
	END
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_UpdateReceipt')
	DROP PROCEDURE dbo.USP_UpdateReceipt
GO 


CREATE PROC USP_UpdateReceipt @id INT,@inputDate DATE, @idReceiver INT, @idPayer INT, @idProduct INT, @exportProduct FLOAT, @priceInDay INT, @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		UPDATE dbo.ReceiveTable 
		SET InputDate = @inputDate, IdReceiver = @idReceiver, IdPayer = @idPayer, idProduct = @idProduct, ExportProduct = @exportProduct, PriceInDay = @priceInDay, Describe = @describe, Money = @money
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
		SELECT p.Id, p.InputDate, emp.Name AS Receiver,ac.Name AS Payer, p.Describe, p.Money
		FROM dbo.PayTable p, dbo.Employees emp, dbo.Accounts ac
		WHERE ac.Id = p.IdPayer AND emp.Id = p.IdReceiver 
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_InsertPay')
	DROP PROCEDURE dbo.USP_InsertPay
GO 

CREATE PROC USP_InsertPay @inputDate DATE, @idReceiver INT, @idPayer INT, @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		INSERT dbo.PayTable VALUES  ( @inputDate ,@idReceiver ,@idPayer ,@describe ,@money)
	END
GO

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_UpdatePay')
	DROP PROCEDURE dbo.USP_UpdatePay
GO 


CREATE PROC USP_UpdatePay @id INT,@inputDate DATE, @idReceiver INT, @idPayer INT, @describe NVARCHAR(300), @money FLOAT
AS
	BEGIN 
		UPDATE dbo.PayTable 
		SET InputDate = @inputDate, IdReceiver = @idReceiver, IdPayer = @idPayer, Describe = @describe, Money = @money
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

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_SearchRevenue')
	DROP PROCEDURE dbo.USP_SearchRevenue
GO 

CREATE PROC USP_SearchRevenue @fromDate DATE, @toDate DATE
AS
	BEGIN
		SELECT * FROM dbo.Revenues WHERE Date >= @fromDate AND Date <= @toDate
	END
GO 

--EXEC dbo.USP_SearchRevenue @fromDate = '2018-11-13', @toDate = '2018-11-14'

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetProductById')
	DROP PROCEDURE dbo.USP_GetProductById
GO 

CREATE PROC USP_GetProductById @id INT
AS
	BEGIN
		SELECT * FROM dbo.Products WHERE Id = @id
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetListReceiver')
	DROP PROCEDURE dbo.USP_GetListReceiver
GO 

CREATE PROC USP_GetListReceiver
AS
	BEGIN
		SELECT Name FROM dbo.Accounts
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetListPayer')
	DROP PROCEDURE dbo.USP_GetListPayer
GO 


CREATE PROC USP_GetListPayer
AS
	BEGIN
		SELECT Name FROM dbo.Employees
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetListNameProducts')
	DROP PROCEDURE dbo.USP_GetListNameProducts
GO 


CREATE PROC USP_GetListNameProducts
AS
	BEGIN
		SELECT Name FROM dbo.Products
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetIdReceiverByName')
	DROP PROCEDURE dbo.USP_GetIdReceiverByName
GO 


CREATE PROC USP_GetIdReceiverByName @name NVARCHAR(50)
AS
	BEGIN
		SELECT Id FROM dbo.Accounts WHERE Name = @name
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetIdPayerByName')
	DROP PROCEDURE dbo.USP_GetIdPayerByName
GO 


CREATE PROC USP_GetIdPayerByName @name NVARCHAR(50) 
AS
	BEGIN
		SELECT Id FROM dbo.Employees WHERE Name = @name
	END
GO 

IF EXISTS(SELECT * FROM sys.sysobjects WHERE name = 'USP_GetIdProductByName')
	DROP PROCEDURE dbo.USP_GetIdProductByName
GO 


CREATE PROC USP_GetIdProductByName @name NVARCHAR(50)
AS
	BEGIN
		SELECT Id FROM dbo.Products WHERE Name = @name
	END
GO 



------------------------Add DataSet-----------------------

INSERT INTO ACCOUNTS VALUES(N'Tuân','admin','admin')
GO 
