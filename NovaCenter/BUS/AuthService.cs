using System;
using System.Data;
using NovaCenter.DAL;
using NovaCenter.DTO;

namespace NovaCenter.BUS
{
    internal sealed class AuthService
    {
        private readonly AccountRepository repository = new AccountRepository();

        public void SeedDemoAccounts()
        {
            Seed("admin","Quản trị viên Nova","Admin","Admin@123");
            Seed("nhanvien","Nhân viên đào tạo","Staff","Nova@123");
        }

        private void Seed(string username, string name, string role, string password)
        {
            byte[] salt = PasswordHasher.NewSalt();
            repository.Seed(username,name,role,PasswordHasher.Hash(password,salt),salt);
        }

        public UserSession Login(string username, string password)
        {
            username = (username ?? "").Trim();
            if (username.Length == 0 || string.IsNullOrEmpty(password))
                throw new ArgumentException("Nhập tên đăng nhập và mật khẩu.");
            DataTable result = repository.ByUsername(username);
            if (result.Rows.Count != 1) return null;
            DataRow user = result.Rows[0];
            if (!(bool)user["HoatDong"] || !PasswordHasher.Verify(password,(byte[])user["Salt"],(byte[])user["MatKhauHash"]))
                return null;
            return new UserSession((int)user["Id"],(string)user["TenDangNhap"],(string)user["HoTen"],(string)user["VaiTro"]);
        }

        public static void Guard(UserSession session, bool admin = false)
        {
            if (session == null) throw new UnauthorizedAccessException("Bạn cần đăng nhập.");
            DataTable result = new AccountRepository().ById(session.Id);
            if (result.Rows.Count == 0 || !(bool)result.Rows[0]["HoatDong"]
                || (string)result.Rows[0]["VaiTro"] != session.Role)
                throw new UnauthorizedAccessException("Phiên đã hết hiệu lực. Vui lòng đăng nhập lại.");
            if (admin && !session.IsAdmin)
                throw new UnauthorizedAccessException("Chức năng này dành cho quản trị viên.");
        }

        public void ChangePassword(UserSession session, string oldPassword, string newPassword)
        {
            Guard(session);
            CheckPassword(newPassword);
            DataRow user = repository.ById(session.Id).Rows[0];
            byte[] oldHash = (byte[])user["MatKhauHash"];
            if (!PasswordHasher.Verify(oldPassword,(byte[])user["Salt"],oldHash))
                throw new ArgumentException("Mật khẩu hiện tại không đúng.");
            if (oldPassword == newPassword) throw new ArgumentException("Mật khẩu mới phải khác mật khẩu cũ.");
            byte[] salt = PasswordHasher.NewSalt();
            if (!repository.ChangePassword(session.Id,oldHash,PasswordHasher.Hash(newPassword,salt),salt))
                throw new InvalidOperationException("Tài khoản vừa thay đổi. Hãy đăng nhập lại.");
        }

        private static void CheckPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 8 || password.Length > 100)
                throw new ArgumentException("Mật khẩu phải có từ 8 đến 100 ký tự.");
        }
    }
}
