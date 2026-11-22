using System;
using System.Data;
using System.Linq;
using static NovaCenter.GUI.Ui;

namespace NovaCenter.GUI
{
    internal partial class MainForm
    {
        private void LoadEnrollments()
        {
            Grid(gridEnrollments,studyService.Registrations(Id(cboEnrollmentFilter)),"Id","HV Id","Lớp Id","Mã HV",
                "Họ tên","Lớp","Khóa học","Ngày đăng ký","Học phí (đ)","Trạng thái");
            gridEnrollments.Columns["HoTen"].FillWeight = 140;
            gridEnrollments.CurrentCell = null;
            registrationId = registrationClassId = 0;
            cboEnrollmentStatus.SelectedIndex = 0;
        }

        private void LoadResults()
        {
            Grid(gridResults,studyService.Results(Id(cboResultFilter)),"ĐK Id","Lớp Id","Lớp","Mã HV","Họ tên",
                "Trạng thái","Quá trình","Cuối kỳ","Điểm TB","Xếp loại","Ghi chú");
            gridResults.Columns["HoTen"].FillWeight = 150;
            gridResults.CurrentCell = null;
            resultId = 0;
            numProgress.Value = numFinal.Value = 0;
            txtResultNote.Clear();
            lblSelectedResult.Text = "Chọn học viên trong bảng để nhập điểm.";
        }

        private void LoadReport()
        {
            DataTable rows = studyService.Report();
            Grid(gridReport,rows,"Lớp","Khóa học","Giảng viên","Sĩ số tối đa","Đã đăng ký",
                "Hoàn thành","Học phí đăng ký (đ)","Đã có điểm","Đạt");
            gridReport.Columns["TenKhoa"].FillWeight = 155;
            gridReport.Columns["TongHocPhi"].FillWeight = 145;
            lblSummary.Text = string.Format(
                "Học viên đang hoạt động: {0}     Khóa học: {1}     Lớp học: {2}\r\nĐăng ký còn hiệu lực: {3}     Tổng học phí đăng ký: {4:N2} đ",
                studentService.Students().Rows.Cast<DataRow>().Count(r => (bool)r["HoatDong"]),
                courses.Rows.Count,classes.Rows.Count,rows.Rows.Cast<DataRow>().Sum(r => (int)r["DangKy"]),
                rows.Rows.Cast<DataRow>().Sum(r => (decimal)r["TongHocPhi"]));
        }

        private void EnrollmentSelected(object sender, EventArgs e)
        {
            if (loading) return;
            DataRow row = Row(gridEnrollments);
            if (row == null) return;
            registrationId = (int)row["Id"];
            registrationClassId = (int)row["LopHocId"];
            cboStudent.SelectedValue = row["HocVienId"];
            cboEnrollClass.SelectedValue = row["LopHocId"];
            cboEnrollmentStatus.SelectedItem = row["TrangThai"];
        }

        private void ResultSelected(object sender, EventArgs e)
        {
            if (loading) return;
            DataRow row = Row(gridResults);
            if (row == null) return;
            resultId = (int)row["DangKyId"];
            numProgress.Value = row["DiemQuaTrinh"] == DBNull.Value ? 0 : (decimal)row["DiemQuaTrinh"];
            numFinal.Value = row["DiemCuoiKy"] == DBNull.Value ? 0 : (decimal)row["DiemCuoiKy"];
            txtResultNote.Text = Convert.ToString(row["GhiChu"]);
            lblSelectedResult.Text = row["MaHV"] + " - " + row["HoTen"] + "  |  Lớp " + row["MaLop"];
        }

        private void EnrollmentFilterChanged(object sender, EventArgs e)
        {
            if (!loading) Run(lblMessage, () =>
            {
                loading = true;
                try { LoadEnrollments(); }
                finally { loading = false; }
            });
        }

        private void ResultFilterChanged(object sender, EventArgs e)
        {
            if (!loading) Run(lblMessage, () =>
            {
                loading = true;
                try { LoadResults(); }
                finally { loading = false; }
            });
        }

        private void EnrollClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            int id = studyService.Enroll(Id(cboStudent),Id(cboEnrollClass));
            LoadAll();
            Notice(lblMessage,"Đã đăng ký học thành công, mã #" + id + ".");
        });

        private void EnrollmentStatusClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (registrationId <= 0) throw new ArgumentException("Chọn một dòng đăng ký.");
            studyService.ChangeStatus(registrationId,registrationClassId,cboEnrollmentStatus.Text);
            LoadAll();
            Notice(lblMessage,"Đã cập nhật trạng thái đăng ký.");
        });

        private void SaveResultClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            studyService.SaveResult(resultId,numProgress.Value,numFinal.Value,txtResultNote.Text);
            LoadAll();
            Notice(lblMessage,"Đã lưu điểm và tính điểm trung bình.");
        });

        private void DeleteResultClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!ConfirmDelete(this,resultId)) return;
            studyService.DeleteResult(resultId);
            LoadAll();
            Notice(lblMessage,"Đã xóa kết quả.");
        });

        private void ReportClick(object sender, EventArgs e) => Run(lblMessage,LoadReport);
    }
}
