using System;
using System.Data;
using System.Windows.Forms;
using NovaCenter.DTO;
using static NovaCenter.GUI.Ui;

namespace NovaCenter.GUI
{
    internal partial class MainForm
    {
        private void ClearCourse()
        {
            gridCourses.CurrentCell = null;
            courseId = 0;
            txtCourseCode.Clear();
            txtCourseName.Clear();
            numHours.Value = 30;
            numFee.Value = 0;
            chkCourseActive.Checked = true;
        }

        private void ClearClass()
        {
            gridClasses.CurrentCell = null;
            classId = 0;
            txtClassCode.Clear();
            txtTeacher.Clear();
            dtStart.Value = DateTime.Today;
            dtEnd.Value = DateTime.Today.AddDays(30);
            numCapacity.Value = 20;
            cboClassStatus.SelectedIndex = 0;
        }

        private void CourseSelected(object sender, EventArgs e)
        {
            if (loading) return;
            DataRow row = Row(gridCourses);
            if (row == null) return;
            courseId = (int)row["Id"];
            txtCourseCode.Text = (string)row["MaKhoa"];
            txtCourseName.Text = (string)row["TenKhoa"];
            numHours.Value = (int)row["SoGio"];
            numFee.Value = (decimal)row["HocPhi"];
            chkCourseActive.Checked = (bool)row["HoatDong"];
        }

        private void ClassSelected(object sender, EventArgs e)
        {
            if (loading) return;
            DataRow row = Row(gridClasses);
            if (row == null) return;
            classId = (int)row["Id"];
            txtClassCode.Text = (string)row["MaLop"];
            cboCourse.SelectedValue = row["KhoaHocId"];
            txtTeacher.Text = (string)row["GiangVien"];
            dtStart.Value = (DateTime)row["NgayBatDau"];
            dtEnd.Value = (DateTime)row["NgayKetThuc"];
            numCapacity.Value = (int)row["SiSoToiDa"];
            cboClassStatus.SelectedItem = row["TrangThai"];
        }

        private void NewCourseClick(object sender, EventArgs e) { errors.Clear(); ClearCourse(); }

        private void NewClassClick(object sender, EventArgs e) { errors.Clear(); ClearClass(); }

        private void SaveCourseClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!Required(errors,txtCourseCode,txtCourseName)) return;
            trainingService.SaveCourse(new Course
            {
                Id = courseId,Code = txtCourseCode.Text,Name = txtCourseName.Text,
                Hours = (int)numHours.Value,Fee = numFee.Value,Active = chkCourseActive.Checked
            });
            LoadAll();
            Notice(lblMessage,"Đã lưu khóa học.");
        });

        private void SaveClassClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!Required(errors,txtClassCode,txtTeacher)) return;
            trainingService.SaveClass(new ClassRoom
            {
                Id = classId,Code = txtClassCode.Text,CourseId = Id(cboCourse),Teacher = txtTeacher.Text,
                StartDate = dtStart.Value,EndDate = dtEnd.Value,Capacity = (int)numCapacity.Value,Status = cboClassStatus.Text
            });
            LoadAll();
            Notice(lblMessage,"Đã lưu lớp học.");
        });

        private void DeleteCourseClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!ConfirmDelete(this,courseId)) return;
            trainingService.DeleteCourse(courseId);
            LoadAll();
            Notice(lblMessage,"Đã xóa khóa học.");
        });

        private void DeleteClassClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!ConfirmDelete(this,classId)) return;
            trainingService.DeleteClass(classId);
            LoadAll();
            Notice(lblMessage,"Đã xóa lớp học.");
        });
    }
}
