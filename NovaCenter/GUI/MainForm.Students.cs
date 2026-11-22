using System;
using System.Data;
using System.Windows.Forms;
using NovaCenter.DTO;
using static NovaCenter.GUI.Ui;

namespace NovaCenter.GUI
{
    internal partial class MainForm
    {
        private void ClearStudent()
        {
            gridStudents.CurrentCell = null;
            studentId = 0;
            txtStudentCode.Clear();
            txtStudentName.Clear();
            dtBirth.Value = DateTime.Today.AddYears(-20);
            cboGender.SelectedIndex = 0;
            txtPhone.Clear();
            txtEmail.Clear();
            txtAddress.Clear();
            chkStudentActive.Checked = true;
        }

        private void StudentSelected(object sender, EventArgs e)
        {
            if (loading) return;
            DataRow row = Row(gridStudents);
            if (row == null) return;
            studentId = (int)row["Id"];
            txtStudentCode.Text = (string)row["MaHV"];
            txtStudentName.Text = (string)row["HoTen"];
            dtBirth.Value = (DateTime)row["NgaySinh"];
            cboGender.SelectedItem = row["GioiTinh"];
            txtPhone.Text = (string)row["DienThoai"];
            txtEmail.Text = (string)row["Email"];
            txtAddress.Text = (string)row["DiaChi"];
            chkStudentActive.Checked = (bool)row["HoatDong"];
        }

        private void SearchClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            loading = true;
            try { LoadStudentGrid(); ClearStudent(); }
            finally { loading = false; }
            Notice(lblMessage,"Tìm thấy " + gridStudents.Rows.Count + " học viên.");
        });

        private void NewStudentClick(object sender, EventArgs e) { errors.Clear(); ClearStudent(); }

        private void SaveStudentClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!Required(errors,txtStudentCode,txtStudentName,txtPhone)) return;
            int id = studentService.SaveStudent(new Student
            {
                Id = studentId,Code = txtStudentCode.Text,Name = txtStudentName.Text,BirthDate = dtBirth.Value,
                Gender = cboGender.Text,Phone = txtPhone.Text,Email = txtEmail.Text,Address = txtAddress.Text,
                Active = chkStudentActive.Checked
            });
            LoadAll();
            Notice(lblMessage,"Đã lưu học viên #" + id + ".");
        });

        private void DeleteStudentClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!ConfirmDelete(this,studentId)) return;
            studentService.DeleteStudent(studentId);
            LoadAll();
            Notice(lblMessage,"Đã xóa học viên.");
        });
    }
}
