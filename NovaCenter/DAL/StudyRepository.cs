using System;
using System.Data;
using static NovaCenter.DAL.Db;

namespace NovaCenter.DAL
{
    internal sealed class StudyRepository
    {
        public DataTable List(int classId = 0) => Read(@"
            SELECT d.Id,d.HocVienId,d.LopHocId,h.MaHV,h.HoTen,l.MaLop,k.TenKhoa,
                d.NgayDangKy,d.HocPhi,d.TrangThai
            FROM dbo.DangKy d JOIN dbo.HocVien h ON d.HocVienId=h.Id
            JOIN dbo.LopHoc l ON d.LopHocId=l.Id JOIN dbo.KhoaHoc k ON l.KhoaHocId=k.Id
            WHERE @lop=0 OR l.Id=@lop ORDER BY d.Id DESC", P("@lop",classId));

        public int Enroll(int studentId, int classId)
        {
            return Transaction((connection, transaction) =>
            {
                decimal fee;
                int capacity;
                using (var command = Command(connection,transaction,@"
                    SELECT l.SiSoToiDa,k.HocPhi
                    FROM dbo.LopHoc l WITH(UPDLOCK,HOLDLOCK) JOIN dbo.KhoaHoc k ON l.KhoaHocId=k.Id
                    WHERE l.Id=@id AND l.TrangThai<>N'Kết thúc' AND k.HoatDong=1",P("@id",classId)))
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) throw new InvalidOperationException("Lớp đã kết thúc hoặc khóa học ngừng hoạt động.");
                    capacity = reader.GetInt32(0);
                    fee = reader.GetDecimal(1);
                }
                using (var command = Command(connection,transaction,
                    "SELECT COUNT(*) FROM dbo.HocVien WHERE Id=@id AND HoatDong=1",P("@id",studentId)))
                    if ((int)command.ExecuteScalar() != 1)
                        throw new InvalidOperationException("Học viên không còn hoạt động.");
                using (var command = Command(connection,transaction,
                    "SELECT COUNT(*) FROM dbo.DangKy WHERE HocVienId=@hv AND LopHocId=@lop",
                    P("@hv",studentId),P("@lop",classId)))
                    if ((int)command.ExecuteScalar() != 0)
                        throw new InvalidOperationException("Học viên đã có đăng ký trong lớp. Chọn dòng để cập nhật trạng thái.");
                using (var command = Command(connection,transaction,
                    "SELECT COUNT(*) FROM dbo.DangKy WHERE LopHocId=@lop AND TrangThai<>N'Đã hủy'",P("@lop",classId)))
                    if ((int)command.ExecuteScalar() >= capacity)
                        throw new InvalidOperationException("Lớp đã đủ sĩ số.");
                using (var command = Command(connection,transaction, @"
                    INSERT dbo.DangKy(HocVienId,LopHocId,HocPhi) OUTPUT INSERTED.Id VALUES(@hv,@lop,@phi)",
                    P("@hv",studentId),P("@lop",classId),Money("@phi",fee)))
                    return (int)command.ExecuteScalar();
            });
        }

        public void ChangeStatus(int registrationId, int classId, string status)
        {
            Transaction((connection, transaction) =>
            {
                int capacity;
                string classStatus;
                using (var command = Command(connection,transaction,@"
                    SELECT SiSoToiDa,TrangThai FROM dbo.LopHoc WITH(UPDLOCK,HOLDLOCK) WHERE Id=@id",P("@id",classId)))
                using (var reader = command.ExecuteReader())
                {
                    if (!reader.Read()) throw new InvalidOperationException("Lớp không còn tồn tại.");
                    capacity = reader.GetInt32(0);
                    classStatus = reader.GetString(1);
                }
                string old;
                using (var command = Command(connection,transaction,
                    "SELECT TrangThai FROM dbo.DangKy WITH(UPDLOCK,HOLDLOCK) WHERE Id=@id AND LopHocId=@lop",
                    P("@id",registrationId),P("@lop",classId)))
                {
                    old = command.ExecuteScalar() as string;
                    if (old == null) throw new InvalidOperationException("Đăng ký đã thay đổi. Hãy tải lại.");
                }

                if (old == "Đã hủy" && status != "Đã hủy")
                {
                    if (classStatus == "Kết thúc") throw new InvalidOperationException("Không thể khôi phục đăng ký của lớp đã kết thúc.");
                    using (var command = Command(connection,transaction,@"
                        SELECT COUNT(*) FROM dbo.DangKy d JOIN dbo.HocVien h ON d.HocVienId=h.Id
                        JOIN dbo.LopHoc l ON d.LopHocId=l.Id JOIN dbo.KhoaHoc k ON l.KhoaHocId=k.Id
                        WHERE d.Id=@id AND h.HoatDong=1 AND k.HoatDong=1",P("@id",registrationId)))
                        if ((int)command.ExecuteScalar() != 1)
                            throw new InvalidOperationException("Học viên hoặc khóa học đã ngừng hoạt động.");
                    using (var command = Command(connection,transaction,
                        "SELECT COUNT(*) FROM dbo.DangKy WHERE LopHocId=@lop AND TrangThai<>N'Đã hủy'",P("@lop",classId)))
                        if ((int)command.ExecuteScalar() >= capacity) throw new InvalidOperationException("Lớp đã đủ sĩ số.");
                }
                if (status == "Đã hủy")
                {
                    using (var command = Command(connection,transaction,
                        "SELECT COUNT(*) FROM dbo.KetQua WHERE DangKyId=@id",P("@id",registrationId)))
                        if ((int)command.ExecuteScalar() != 0)
                            throw new InvalidOperationException("Đăng ký đã có điểm. Xóa kết quả trước khi hủy.");
                }

                using (var command = Command(connection,transaction,
                    "UPDATE dbo.DangKy SET TrangThai=@tt WHERE Id=@id",P("@id",registrationId),P("@tt",status)))
                    return command.ExecuteNonQuery();
            });
        }

        public DataTable Results(int classId) => Read(@"
            SELECT d.Id AS DangKyId,l.Id AS LopHocId,l.MaLop,h.MaHV,h.HoTen,d.TrangThai,
                q.DiemQuaTrinh,q.DiemCuoiKy,q.DiemTB,
                CASE WHEN q.DangKyId IS NULL THEN N'Chưa có điểm'
                     WHEN q.DiemTB>=5 THEN N'Đạt' ELSE N'Chưa đạt' END AS XepLoai,q.GhiChu
            FROM dbo.DangKy d JOIN dbo.HocVien h ON d.HocVienId=h.Id
            JOIN dbo.LopHoc l ON d.LopHocId=l.Id LEFT JOIN dbo.KetQua q ON d.Id=q.DangKyId
            WHERE d.TrangThai<>N'Đã hủy' AND (@lop=0 OR d.LopHocId=@lop) ORDER BY l.MaLop,h.MaHV",P("@lop",classId));

        public void SaveResult(int id, decimal progress, decimal final, string note)
        {
            Transaction((connection, transaction) =>
            {
                using (var command = Command(connection,transaction, @"
                    SELECT TrangThai FROM dbo.DangKy WITH(UPDLOCK,HOLDLOCK) WHERE Id=@id",P("@id",id)))
                {
                    object status = command.ExecuteScalar();
                    if (status == null || (string)status == "Đã hủy")
                        throw new InvalidOperationException("Đăng ký không tồn tại hoặc đã hủy.");
                }
                using (var command = Command(connection,transaction, @"
                    IF EXISTS(SELECT 1 FROM dbo.KetQua WHERE DangKyId=@id)
                        UPDATE dbo.KetQua SET DiemQuaTrinh=@qt,DiemCuoiKy=@ck,GhiChu=@gc WHERE DangKyId=@id;
                    ELSE INSERT dbo.KetQua(DangKyId,DiemQuaTrinh,DiemCuoiKy,GhiChu) VALUES(@id,@qt,@ck,@gc);",
                    P("@id",id),Score("@qt",progress),Score("@ck",final),P("@gc",note)))
                    return command.ExecuteNonQuery();
            });
        }

        public void DeleteResult(int id)
        {
            if (Execute("DELETE dbo.KetQua WHERE DangKyId=@id",P("@id",id)) != 1)
                throw new InvalidOperationException("Học viên chưa có điểm.");
        }

        public DataTable Report() => Read(@"
            SELECT l.MaLop,k.TenKhoa,l.GiangVien,l.SiSoToiDa,
                COUNT(d.Id) AS DangKy,
                SUM(CASE WHEN d.TrangThai=N'Hoàn thành' THEN 1 ELSE 0 END) AS HoanThanh,
                COALESCE(SUM(d.HocPhi),0) AS TongHocPhi,
                COUNT(q.DangKyId) AS DaCoDiem,
                SUM(CASE WHEN q.DiemTB>=5 THEN 1 ELSE 0 END) AS Dat
            FROM dbo.LopHoc l JOIN dbo.KhoaHoc k ON l.KhoaHocId=k.Id
            LEFT JOIN dbo.DangKy d ON d.LopHocId=l.Id AND d.TrangThai<>N'Đã hủy'
            LEFT JOIN dbo.KetQua q ON q.DangKyId=d.Id
            GROUP BY l.Id,l.MaLop,k.TenKhoa,l.GiangVien,l.SiSoToiDa ORDER BY l.MaLop");
    }
}
