using System;
using System.Data;
using System.Linq;
using NovaCenter.DAL;
using NovaCenter.DTO;

namespace NovaCenter.BUS
{
    internal sealed class StudyService
    {
        public static readonly string[] RegistrationStates = { "Đang học","Hoàn thành","Đã hủy" };
        private readonly UserSession session;
        private readonly StudyRepository enrollment = new StudyRepository();

        public StudyService(UserSession session) { this.session = session; }
        private void Access(bool admin = false) => AuthService.Guard(session,admin);

        public DataTable Registrations(int classId = 0) { Access(); return enrollment.List(classId); }

        public DataTable Results(int classId = 0) { Access(); return enrollment.Results(classId); }

        public DataTable Report() { Access(); return enrollment.Report(); }

        public int Enroll(int studentId, int classId)
        {
            Access();
            if (studentId <= 0 || classId <= 0) throw new ArgumentException("Chọn học viên và lớp.");
            return enrollment.Enroll(studentId,classId);
        }

        public void ChangeStatus(int id, int classId, string status)
        {
            Access();
            if (!RegistrationStates.Contains(status)) throw new ArgumentException("Chọn trạng thái đăng ký.");
            enrollment.ChangeStatus(id,classId,status);
        }

        public void SaveResult(int id, decimal progress, decimal final, string note)
        {
            Access();
            if (id <= 0) throw new ArgumentException("Chọn học viên trong bảng kết quả.");
            if (progress < 0 || progress > 10 || final < 0 || final > 10)
                throw new ArgumentException("Điểm phải từ 0 đến 10.");
            note = (note ?? "").Trim();
            if (note.Length > 200) throw new ArgumentException("Ghi chú tối đa 200 ký tự.");
            enrollment.SaveResult(id,progress,final,note);
        }

        public void DeleteResult(int id) { Access(true); enrollment.DeleteResult(id); }
    }
}
