
-- BAI 4.2: DANG KY KHOA HOC

IF DB_ID(N'Bai42RegistrationDB') IS NULL
BEGIN
    CREATE DATABASE Bai42RegistrationDB;
END
GO

USE Bai42RegistrationDB;
GO

-- Bang khoa hoc
IF OBJECT_ID(N'dbo.KhoaHoc', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.KhoaHoc
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        TenKhoaHoc NVARCHAR(100) NOT NULL,
        HocPhi DECIMAL(18,2) NOT NULL
    );
END
GO

-- Bang dang ky
IF OBJECT_ID(N'dbo.DangKy', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.DangKy
    (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        HoTen NVARCHAR(100) NOT NULL,
        SoDienThoai VARCHAR(20) NOT NULL,
        NgaySinh DATE NOT NULL,
        GioiTinh NVARCHAR(10) NOT NULL,
        KhoaHocId INT NOT NULL,
        HinhThucHoc NVARCHAR(30) NOT NULL,
        NhanThongBao BIT NOT NULL DEFAULT 0,
        NgayDangKy DATETIME2 NOT NULL
            DEFAULT SYSDATETIME(),

        CONSTRAINT FK_DangKy_KhoaHoc
            FOREIGN KEY (KhoaHocId)
            REFERENCES dbo.KhoaHoc(Id)
    );
END
GO

-- Them khoa hoc mau neu chua co
IF NOT EXISTS (SELECT 1 FROM dbo.KhoaHoc)
BEGIN
    INSERT INTO dbo.KhoaHoc (TenKhoaHoc, HocPhi)
    VALUES
        (N'Lập trình C#', 1500000),
        (N'Cơ sở dữ liệu SQL Server', 1200000),
        (N'Lập trình Web', 1800000);
END
GO

-- Kiem tra du lieu
SELECT * FROM dbo.KhoaHoc;
SELECT * FROM dbo.DangKy;
GO
