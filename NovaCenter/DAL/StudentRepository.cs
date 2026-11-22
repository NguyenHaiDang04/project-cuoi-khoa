using System;
using System.Data;
using NovaCenter.DTO;
using static NovaCenter.DAL.Db;

namespace NovaCenter.DAL
{
    internal sealed class StudentRepository
    {
        public DataTable Students(string search) => Read(@"
            SELECT Id,MaHV,HoTen,NgaySinh,GioiTinh,DienThoai,Email,DiaChi,HoatDong
            FROM dbo.HocVien
            WHERE @q=N'' OR CHARINDEX(@q,MaHV)>0 OR CHARINDEX(@q,HoTen)>0 OR CHARINDEX(@q,DienThoai)>0
            ORDER BY MaHV", P("@q",search));

        public int SaveStudent(Student s)
        {
            var args = new[] { P("@id",s.Id), P("@ma",s.Code), P("@ten",s.Name),
                P("@ngay",s.BirthDate.Date), P("@gt",s.Gender), P("@dt",s.Phone),
                P("@email",s.Email), P("@dc",s.Address), P("@active",s.Active) };
            if (s.Id == 0)
                return Identity(@"INSERT dbo.HocVien(MaHV,HoTen,NgaySinh,GioiTinh,DienThoai,Email,DiaChi,HoatDong)
                    OUTPUT INSERTED.Id VALUES(@ma,@ten,@ngay,@gt,@dt,@email,@dc,@active)", args);
            if (Execute(@"UPDATE dbo.HocVien SET MaHV=@ma,HoTen=@ten,NgaySinh=@ngay,GioiTinh=@gt,
                DienThoai=@dt,Email=@email,DiaChi=@dc,HoatDong=@active WHERE Id=@id",args) != 1)
                throw new InvalidOperationException("Học viên không còn tồn tại.");
            return s.Id;
        }

        public void DeleteStudent(int id)
        {
            if (Execute("DELETE dbo.HocVien WHERE Id=@id",P("@id",id)) != 1)
                throw new InvalidOperationException("Học viên không còn tồn tại.");
        }
    }
}
