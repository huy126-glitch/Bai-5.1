
IF DB_ID(N'Bai41LoginDB') IS NULL
    CREATE DATABASE Bai41LoginDB;
GO

USE Bai41LoginDB;
GO

IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TaiKhoan
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Username NVARCHAR(50) NOT NULL UNIQUE,
        PasswordHash VARCHAR(64) NOT NULL
    );
END
GO

IF NOT EXISTS (
    SELECT 1 FROM dbo.TaiKhoan WHERE Username = N'admin'
)
BEGIN
    INSERT INTO dbo.TaiKhoan (Username, PasswordHash)
    VALUES
    (
        N'admin',
        CONVERT(VARCHAR(64),
            HASHBYTES('SHA2_256', N'123456'), 2)
    );
END
GO

SELECT * FROM dbo.TaiKhoan;
GO
