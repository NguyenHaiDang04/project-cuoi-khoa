using System;
using System.Data;
using System.Linq;
using NovaCenter.DAL;
using NovaCenter.DTO;
using static NovaCenter.BUS.Validation;

namespace NovaCenter.BUS
{
    internal sealed class TrainingService
    {
        public static readonly string[] ClassStates = { "Mở","Đang học","Kết thúc" };
        private readonly UserSession session;
        private readonly TrainingRepository catalog = new TrainingRepository();

        public TrainingService(UserSession session) { this.session = session; }
        private void Access(bool admin = false) => AuthService.Guard(session,admin);

        public DataTable Courses() { Access(); return catalog.Courses(); }

        public DataTable Classes() { Access(); return catalog.Classes(); }

        public int SaveCourse(Course c)
        {
            Access();
            c.Code = Code(c.Code);
            c.Name = Required(c.Name,100,"tên khóa học");
            if (c.Hours < 1 || c.Hours > 2000) throw new ArgumentException("Thời lượng từ 1 đến 2000 giờ.");
            if (c.Fee < 0 || c.Fee > 999999999) throw new ArgumentException("Học phí không hợp lệ.");
            return catalog.SaveCourse(c);
        }

        public void DeleteCourse(int id) { Access(true); catalog.DeleteCourse(id); }

        public int SaveClass(ClassRoom c)
        {
            Access();
            c.Code = Code(c.Code);
            c.Teacher = Required(c.Teacher,100,"giảng viên");
            if (c.CourseId <= 0) throw new ArgumentException("Chọn khóa học.");
            if (c.EndDate.Date < c.StartDate.Date) throw new ArgumentException("Ngày kết thúc phải từ ngày bắt đầu trở đi.");
            if (c.Capacity < 1 || c.Capacity > 1000) throw new ArgumentException("Sĩ số tối đa từ 1 đến 1000.");
            if (!ClassStates.Contains(c.Status)) throw new ArgumentException("Chọn trạng thái lớp.");
            return catalog.SaveClass(c);
        }

        public void DeleteClass(int id) { Access(true); catalog.DeleteClass(id); }
    }
}
