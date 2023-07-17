CREATE TABLE Phongban (
   MaPB char(2) PRIMARY KEY,
   TenPB nvarchar(30)
)

CREATE TABLE Nhanvien (
   MaNV char(6) PRIMARY KEY,
   TenNV nvarchar(20),
   Ngaysinh datetime,
   MaPB char(2) FOREIGN KEY REFERENCES Phongban(MaPB)
)

INSERT INTO Phongban VALUES ('P1', N'Phòng Kế Toán')
INSERT INTO Phongban VALUES ('P2', N'Phòng Nhân Sự')

INSERT INTO Nhanvien VALUES ('NV001', N'Nguyễn Văn A', '1980-01-01', 'P1') 
INSERT INTO Nhanvien VALUES ('NV002', N'Trần Thị B', '1985-03-05', 'P1')
INSERT INTO Nhanvien VALUES ('NV003', N'Lê Văn C', '1990-08-15', 'P2')