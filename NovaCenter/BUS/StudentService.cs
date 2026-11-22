using System;
using System.Data;
using System.Linq;
using NovaCenter.DAL;
using NovaCenter.DTO;
using static NovaCenter.BUS.Validation;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace NovaCenter.BUS
{
    internal sealed class StudentService
    {
        private readonly UserSession session;
        private readonly StudentRepository catalog = new StudentRepository();

        public StudentService(UserSession session) { this.session = session; }
        private void Access(bool admin = false) => AuthService.Guard(session,admin);

        public DataTable Students(string search = "") { Access(); return catalog.Students(search.Trim()); }

        public int SaveStudent(Student s)
        {
            Access();
            s.Code = Code(s.Code);
            s.Name = Required(s.Name,100,"họ tên");
            if (s.BirthDate.Date < new DateTime(1900,1,1) || s.BirthDate.Date >= DateTime.Today)
                throw new ArgumentException("Ngày sinh phải từ năm 1900 và trước hôm nay.");
            if (!new[] {"Nam","Nữ","Khác"}.Contains(s.Gender)) throw new ArgumentException("Chọn giới tính.");
            s.Phone = (s.Phone ?? "").Trim();
            if (!Regex.IsMatch(s.Phone, @"^\+?[0-9]{8,15}$"))
                throw new ArgumentException("Điện thoại có từ 8 đến 15 chữ số, có thể bắt đầu bằng dấu +.");
            s.Email = (s.Email ?? "").Trim();
            s.Address = (s.Address ?? "").Trim();
            if (s.Email.Length > 100 || s.Address.Length > 200)
                throw new ArgumentException("Email tối đa 100 ký tự, địa chỉ tối đa 200 ký tự.");
            if (s.Email.Length > 0)
            {
                try
                {
                    if (new MailAddress(s.Email).Address != s.Email) throw new FormatException();
                }
                catch (FormatException) { throw new ArgumentException("Địa chỉ email chưa hợp lệ."); }
            }
            return catalog.SaveStudent(s);
        }

        public void DeleteStudent(int id) { Access(true); catalog.DeleteStudent(id); }
    }
}
