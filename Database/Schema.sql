SET XACT_ABORT ON;
IF OBJECT_ID(N'dbo.TaiKhoan', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.TaiKhoan (
        Id int IDENTITY PRIMARY KEY,
        TenDangNhap nvarchar(50) NOT NULL UNIQUE,
        HoTen nvarchar(100) NOT NULL,
        MatKhauHash varbinary(32) NOT NULL,
        Salt varbinary(16) NOT NULL,
        VaiTro varchar(10) NOT NULL CHECK (VaiTro IN ('Admin','Staff')),
        HoatDong bit NOT NULL DEFAULT 1
    );
END;
IF OBJECT_ID(N'dbo.HocVien', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.HocVien (
        Id int IDENTITY PRIMARY KEY,
        MaHV varchar(20) NOT NULL UNIQUE,
        HoTen nvarchar(100) NOT NULL,
        NgaySinh date NOT NULL,
        GioiTinh nvarchar(10) NOT NULL CHECK (GioiTinh IN (N'Nam',N'Nữ',N'Khác')),
        DienThoai varchar(16) NOT NULL,
        Email nvarchar(100) NOT NULL DEFAULT N'',
        DiaChi nvarchar(200) NOT NULL DEFAULT N'',
        HoatDong bit NOT NULL DEFAULT 1
    );
    CREATE TABLE dbo.KhoaHoc (
        Id int IDENTITY PRIMARY KEY,
        MaKhoa varchar(20) NOT NULL UNIQUE,
        TenKhoa nvarchar(100) NOT NULL,
        SoGio int NOT NULL CHECK (SoGio BETWEEN 1 AND 2000),
        HocPhi decimal(18,2) NOT NULL CHECK (HocPhi BETWEEN 0 AND 999999999),
        HoatDong bit NOT NULL DEFAULT 1
    );
    CREATE TABLE dbo.LopHoc (
        Id int IDENTITY PRIMARY KEY,
        MaLop varchar(20) NOT NULL UNIQUE,
        KhoaHocId int NOT NULL REFERENCES dbo.KhoaHoc(Id),
        GiangVien nvarchar(100) NOT NULL,
        NgayBatDau date NOT NULL,
        NgayKetThuc date NOT NULL,
        SiSoToiDa int NOT NULL CHECK (SiSoToiDa BETWEEN 1 AND 1000),
        TrangThai nvarchar(20) NOT NULL CHECK (TrangThai IN (N'Mở',N'Đang học',N'Kết thúc')),
        CHECK (NgayKetThuc >= NgayBatDau)
    );
    CREATE TABLE dbo.DangKy (
        Id int IDENTITY PRIMARY KEY,
        HocVienId int NOT NULL REFERENCES dbo.HocVien(Id),
        LopHocId int NOT NULL REFERENCES dbo.LopHoc(Id),
        NgayDangKy datetime NOT NULL DEFAULT GETDATE(),
        HocPhi decimal(18,2) NOT NULL CHECK (HocPhi >= 0),
        TrangThai nvarchar(20) NOT NULL DEFAULT N'Đang học'
            CHECK (TrangThai IN (N'Đang học',N'Hoàn thành',N'Đã hủy')),
        UNIQUE (HocVienId,LopHocId)
    );
    CREATE TABLE dbo.KetQua (
        DangKyId int PRIMARY KEY REFERENCES dbo.DangKy(Id),
        DiemQuaTrinh decimal(4,2) NOT NULL CHECK (DiemQuaTrinh BETWEEN 0 AND 10),
        DiemCuoiKy decimal(4,2) NOT NULL CHECK (DiemCuoiKy BETWEEN 0 AND 10),
        DiemTB AS CAST(ROUND(DiemQuaTrinh*0.4+DiemCuoiKy*0.6,2) AS decimal(4,2)) PERSISTED,
        GhiChu nvarchar(200) NOT NULL DEFAULT N''
    );
    CREATE INDEX IX_DangKy_Lop ON dbo.DangKy(LopHocId);

    INSERT dbo.HocVien(MaHV,HoTen,NgaySinh,GioiTinh,DienThoai,Email,DiaChi) VALUES
    ('HV001',N'Nguyễn Văn An','20030515',N'Nam','0901000001','an@example.com',N'Hà Nội'),
    ('HV002',N'Trần Thị Bình','20030820',N'Nữ','0901000002','binh@example.com',N'Hà Nội'),
    ('HV003',N'Lê Hoàng Cường','20021110',N'Nam','0901000003','cuong@example.com',N'Hà Nội'),
    ('HV004',N'Phạm Thu Hà','20040612',N'Nữ','0901000004','ha@example.com',N'Hà Nội');
    INSERT dbo.KhoaHoc(MaKhoa,TenKhoa,SoGio,HocPhi) VALUES
    ('THCB',N'Tin học cơ bản',30,1500000),('EXCEL',N'Excel ứng dụng',36,1800000),
    ('CSHARP',N'Lập trình C# WinForms',60,3000000);
    INSERT dbo.LopHoc(MaLop,KhoaHocId,GiangVien,NgayBatDau,NgayKetThuc,SiSoToiDa,TrangThai) VALUES
    ('THCB-01',1,N'Thầy Minh',DATEADD(day,-15,GETDATE()),DATEADD(day,30,GETDATE()),20,N'Đang học'),
    ('EXCEL-01',2,N'Cô Lan',DATEADD(day,5,GETDATE()),DATEADD(day,45,GETDATE()),15,N'Mở'),
    ('CSHARP-01',3,N'Thầy Quân',DATEADD(day,10,GETDATE()),DATEADD(day,70,GETDATE()),20,N'Mở');
    INSERT dbo.DangKy(HocVienId,LopHocId,HocPhi) VALUES (1,1,1500000),(2,1,1500000),(3,2,1800000);
    INSERT dbo.KetQua(DangKyId,DiemQuaTrinh,DiemCuoiKy,GhiChu) VALUES
    (1,8,9,N'Tham gia đầy đủ'),(2,7,8,N'');
END;
