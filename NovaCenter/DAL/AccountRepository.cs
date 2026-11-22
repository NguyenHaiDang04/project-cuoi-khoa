using System;
using System.Data;
using static NovaCenter.DAL.Db;

namespace NovaCenter.DAL
{
    internal sealed class AccountRepository
    {
        public DataTable ByUsername(string username) => Read(@"
            SELECT Id,TenDangNhap,HoTen,MatKhauHash,Salt,VaiTro,HoatDong
            FROM dbo.TaiKhoan WHERE TenDangNhap=@u",P("@u",username));

        public DataTable ById(int id) => Read(@"
            SELECT Id,TenDangNhap,HoTen,MatKhauHash,Salt,VaiTro,HoatDong
            FROM dbo.TaiKhoan WHERE Id=@id",P("@id",id));

        public void Seed(string username, string name, string role, byte[] hash, byte[] salt)
        {
            Transaction((connection, transaction) =>
            {
                using (var command = Command(connection,transaction,@"
                    IF NOT EXISTS(SELECT 1 FROM dbo.TaiKhoan WITH(UPDLOCK,HOLDLOCK) WHERE TenDangNhap=@u)
                        INSERT dbo.TaiKhoan(TenDangNhap,HoTen,VaiTro,MatKhauHash,Salt) VALUES(@u,@n,@r,@h,@s)",
                    P("@u",username),P("@n",name),P("@r",role),P("@h",hash),P("@s",salt)))
                    return command.ExecuteNonQuery();
            });
        }

        public bool ChangePassword(int id, byte[] oldHash, byte[] hash, byte[] salt) =>
            Execute(@"UPDATE dbo.TaiKhoan SET MatKhauHash=@h,Salt=@s
                WHERE Id=@id AND MatKhauHash=@old AND HoatDong=1",
                P("@id",id),P("@old",oldHash),P("@h",hash),P("@s",salt)) == 1;
    }
}
