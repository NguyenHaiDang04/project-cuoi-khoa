using System;
using System.Data;
using NovaCenter.DTO;
using static NovaCenter.DAL.Db;

namespace NovaCenter.DAL
{
    internal sealed class TrainingRepository
    {
        public DataTable Courses() => Read("SELECT Id,MaKhoa,TenKhoa,SoGio,HocPhi,HoatDong FROM dbo.KhoaHoc ORDER BY MaKhoa");

        public int SaveCourse(Course c)
        {
            var args = new[] { P("@id",c.Id),P("@ma",c.Code),P("@ten",c.Name),
                P("@gio",c.Hours),Money("@phi",c.Fee),P("@active",c.Active) };
            if (c.Id == 0)
                return Identity(@"INSERT dbo.KhoaHoc(MaKhoa,TenKhoa,SoGio,HocPhi,HoatDong)
                    OUTPUT INSERTED.Id VALUES(@ma,@ten,@gio,@phi,@active)",args);
            if (Execute(@"UPDATE dbo.KhoaHoc SET MaKhoa=@ma,TenKhoa=@ten,SoGio=@gio,HocPhi=@phi,
                HoatDong=@active WHERE Id=@id",args) != 1)
                throw new InvalidOperationException("Khóa học không còn tồn tại.");
            return c.Id;
        }

        public void DeleteCourse(int id)
        {
            if (Execute("DELETE dbo.KhoaHoc WHERE Id=@id",P("@id",id)) != 1)
                throw new InvalidOperationException("Khóa học không còn tồn tại.");
        }

        public DataTable Classes() => Read(@"
            SELECT l.Id,l.MaLop,l.KhoaHocId,k.TenKhoa,l.GiangVien,l.NgayBatDau,l.NgayKetThuc,
                l.SiSoToiDa,(SELECT COUNT(*) FROM dbo.DangKy d WHERE d.LopHocId=l.Id AND d.TrangThai<>N'Đã hủy') AS DaDangKy,
                l.TrangThai
            FROM dbo.LopHoc l JOIN dbo.KhoaHoc k ON l.KhoaHocId=k.Id ORDER BY l.MaLop");

        public int SaveClass(ClassRoom c)
        {
            return Transaction((connection, transaction) =>
            {
                if (c.Id > 0)
                {
                    int course;
                    using (var command = Command(connection,transaction, @"
                        SELECT KhoaHocId FROM dbo.LopHoc WITH(UPDLOCK,HOLDLOCK) WHERE Id=@id",P("@id",c.Id)))
                    {
                        object value = command.ExecuteScalar();
                        if (value == null) throw new InvalidOperationException("Lớp không còn tồn tại.");
                        course = (int)value;
                    }
                    int count;
                    using (var command = Command(connection,transaction, @"
                        SELECT COUNT(*) FROM dbo.DangKy WHERE LopHocId=@id AND TrangThai<>N'Đã hủy'",P("@id",c.Id)))
                        count = (int)command.ExecuteScalar();
                    if (count > c.Capacity)
                        throw new InvalidOperationException("Sĩ số tối đa nhỏ hơn số học viên đã đăng ký.");
                    using (var command = Command(connection,transaction,
                        "SELECT COUNT(*) FROM dbo.DangKy WHERE LopHocId=@id",P("@id",c.Id)))
                    {
                        if (course != c.CourseId && (int)command.ExecuteScalar() > 0)
                            throw new InvalidOperationException("Lớp đã có đăng ký nên không thể đổi khóa học.");
                    }
                }

                using (var command = Command(connection,transaction,
                    "SELECT COUNT(*) FROM dbo.KhoaHoc WHERE Id=@id AND HoatDong=1",P("@id",c.CourseId)))
                    if ((int)command.ExecuteScalar() != 1)
                        throw new InvalidOperationException("Khóa học đã ngừng hoạt động hoặc bị xóa.");

                var args = new[] { P("@id",c.Id),P("@ma",c.Code),P("@khoa",c.CourseId),
                    P("@gv",c.Teacher),P("@bd",c.StartDate.Date),P("@kt",c.EndDate.Date),
                    P("@ss",c.Capacity),P("@tt",c.Status) };
                string sql = c.Id == 0
                    ? @"INSERT dbo.LopHoc(MaLop,KhoaHocId,GiangVien,NgayBatDau,NgayKetThuc,SiSoToiDa,TrangThai)
                        OUTPUT INSERTED.Id VALUES(@ma,@khoa,@gv,@bd,@kt,@ss,@tt)"
                    : @"UPDATE dbo.LopHoc SET MaLop=@ma,KhoaHocId=@khoa,GiangVien=@gv,NgayBatDau=@bd,
                        NgayKetThuc=@kt,SiSoToiDa=@ss,TrangThai=@tt WHERE Id=@id; SELECT @id;";
                using (var command = Command(connection,transaction,sql,args))
                    return (int)command.ExecuteScalar();
            });
        }

        public void DeleteClass(int id)
        {
            if (Execute("DELETE dbo.LopHoc WHERE Id=@id",P("@id",id)) != 1)
                throw new InvalidOperationException("Lớp học không còn tồn tại.");
        }
    }
}
