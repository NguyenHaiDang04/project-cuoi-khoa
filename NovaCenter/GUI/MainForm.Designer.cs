using System.Drawing;
using System.Windows.Forms;

namespace NovaCenter.GUI
{
    partial class MainForm
    {
        private TabControl tabs;
        private Label lblMessage;
        private Label lblUser;
        private Button btnReload;
        private Button btnLogout;
        private ErrorProvider errors;
        private TabPage tabStudents;
        private TextBox txtSearch;
        private Button btnSearch;
        private Button btnExportStudents;
        private DataGridView gridStudents;
        private TextBox txtStudentCode;
        private TextBox txtStudentName;
        private DateTimePicker dtBirth;
        private ComboBox cboGender;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private TextBox txtAddress;
        private CheckBox chkStudentActive;
        private Button btnNewStudent;
        private Button btnSaveStudent;
        private Button btnDeleteStudent;
        private TabPage tabCourses;
        private DataGridView gridCourses;
        private TextBox txtCourseCode;
        private TextBox txtCourseName;
        private NumericUpDown numHours;
        private NumericUpDown numFee;
        private CheckBox chkCourseActive;
        private Button btnNewCourse;
        private Button btnSaveCourse;
        private Button btnDeleteCourse;
        private TabPage tabClasses;
        private DataGridView gridClasses;
        private TextBox txtClassCode;
        private ComboBox cboCourse;
        private NumericUpDown numCapacity;
        private TextBox txtTeacher;
        private ComboBox cboClassStatus;
        private DateTimePicker dtStart;
        private DateTimePicker dtEnd;
        private Button btnNewClass;
        private Button btnSaveClass;
        private Button btnDeleteClass;
        private TabPage tabEnrollments;
        private ComboBox cboEnrollmentFilter;
        private DataGridView gridEnrollments;
        private ComboBox cboStudent;
        private ComboBox cboEnrollClass;
        private Button btnEnroll;
        private ComboBox cboEnrollmentStatus;
        private Button btnEnrollmentStatus;
        private TabPage tabResults;
        private ComboBox cboResultFilter;
        private Button btnExportResults;
        private DataGridView gridResults;
        private Label lblSelectedResult;
        private NumericUpDown numProgress;
        private NumericUpDown numFinal;
        private TextBox txtResultNote;
        private Button btnSaveResult;
        private Button btnDeleteResult;
        private TabPage tabReports;
        private Label lblSummary;
        private Button btnReport;
        private Button btnExportReport;
        private DataGridView gridReport;
        private TabPage tabPassword;
        private TextBox txtOldPassword;
        private TextBox txtNewPassword;
        private TextBox txtConfirmPassword;
        private Button btnChangePassword;

        protected override void Dispose(bool disposing)
        {
            if (disposing) errors?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabs = new TabControl();
            this.lblMessage = new Label();
            this.lblUser = new Label();
            this.btnReload = new Button();
            this.btnLogout = new Button();
            this.errors = new ErrorProvider();
            this.tabStudents = new TabPage();
            var label1 = new Label();
            this.txtSearch = new TextBox();
            this.btnSearch = new Button();
            this.btnExportStudents = new Button();
            this.gridStudents = new DataGridView();
            var label2 = new Label();
            this.txtStudentCode = new TextBox();
            var label3 = new Label();
            this.txtStudentName = new TextBox();
            var label4 = new Label();
            this.dtBirth = new DateTimePicker();
            var label5 = new Label();
            this.cboGender = new ComboBox();
            var label6 = new Label();
            this.txtPhone = new TextBox();
            var label7 = new Label();
            this.txtEmail = new TextBox();
            var label8 = new Label();
            this.txtAddress = new TextBox();
            this.chkStudentActive = new CheckBox();
            this.btnNewStudent = new Button();
            this.btnSaveStudent = new Button();
            this.btnDeleteStudent = new Button();
            this.tabCourses = new TabPage();
            this.gridCourses = new DataGridView();
            var label9 = new Label();
            this.txtCourseCode = new TextBox();
            var label10 = new Label();
            this.txtCourseName = new TextBox();
            var label11 = new Label();
            this.numHours = new NumericUpDown();
            var label12 = new Label();
            this.numFee = new NumericUpDown();
            this.chkCourseActive = new CheckBox();
            this.btnNewCourse = new Button();
            this.btnSaveCourse = new Button();
            this.btnDeleteCourse = new Button();
            this.tabClasses = new TabPage();
            this.gridClasses = new DataGridView();
            var label13 = new Label();
            this.txtClassCode = new TextBox();
            var label14 = new Label();
            this.cboCourse = new ComboBox();
            var label15 = new Label();
            this.numCapacity = new NumericUpDown();
            var label16 = new Label();
            this.txtTeacher = new TextBox();
            var label17 = new Label();
            this.cboClassStatus = new ComboBox();
            var label18 = new Label();
            this.dtStart = new DateTimePicker();
            var label19 = new Label();
            this.dtEnd = new DateTimePicker();
            this.btnNewClass = new Button();
            this.btnSaveClass = new Button();
            this.btnDeleteClass = new Button();
            this.tabEnrollments = new TabPage();
            var label20 = new Label();
            this.cboEnrollmentFilter = new ComboBox();
            this.gridEnrollments = new DataGridView();
            var label21 = new Label();
            this.cboStudent = new ComboBox();
            var label22 = new Label();
            this.cboEnrollClass = new ComboBox();
            this.btnEnroll = new Button();
            var label23 = new Label();
            this.cboEnrollmentStatus = new ComboBox();
            this.btnEnrollmentStatus = new Button();
            var label24 = new Label();
            this.tabResults = new TabPage();
            var label25 = new Label();
            this.cboResultFilter = new ComboBox();
            this.btnExportResults = new Button();
            this.gridResults = new DataGridView();
            this.lblSelectedResult = new Label();
            var label26 = new Label();
            this.numProgress = new NumericUpDown();
            var label27 = new Label();
            this.numFinal = new NumericUpDown();
            var label28 = new Label();
            this.txtResultNote = new TextBox();
            this.btnSaveResult = new Button();
            this.btnDeleteResult = new Button();
            var label29 = new Label();
            this.tabReports = new TabPage();
            this.lblSummary = new Label();
            this.btnReport = new Button();
            this.btnExportReport = new Button();
            this.gridReport = new DataGridView();
            var label30 = new Label();
            this.tabPassword = new TabPage();
            var label36 = new Label();
            this.txtOldPassword = new TextBox();
            var label37 = new Label();
            this.txtNewPassword = new TextBox();
            var label38 = new Label();
            this.txtConfirmPassword = new TextBox();
            this.btnChangePassword = new Button();
            var label39 = new Label();
            this.SuspendLayout();
            this.tabs.Name = "tabs";
            this.tabs.Location = new Point(12, 65);
            this.tabs.Size = new Size(1136, 566);
            this.tabStudents.Name = "tabStudents";
            this.tabStudents.Text = "Học viên";
            this.tabStudents.UseVisualStyleBackColor = true;
            this.tabs.Controls.Add(this.tabStudents);
            label1.Name = "label1";
            label1.Location = new Point(12, 16);
            label1.Size = new Size(154, 24);
            label1.Text = "Tìm mã, tên, điện thoại";
            this.tabStudents.Controls.Add(label1);

            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Location = new Point(173, 12);
            this.txtSearch.Size = new Size(360, 27);
            this.txtSearch.MaxLength = 100;
            this.tabStudents.Controls.Add(this.txtSearch);

            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Location = new Point(546, 10);
            this.btnSearch.Size = new Size(100, 32);
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += this.SearchClick;
            this.tabStudents.Controls.Add(this.btnSearch);

            this.btnExportStudents.Name = "btnExportStudents";
            this.btnExportStudents.Location = new Point(658, 10);
            this.btnExportStudents.Size = new Size(110, 32);
            this.btnExportStudents.Text = "Xuất CSV";
            this.btnExportStudents.UseVisualStyleBackColor = true;
            this.btnExportStudents.Click += this.ExportStudentsClick;
            this.tabStudents.Controls.Add(this.btnExportStudents);

            this.gridStudents.Name = "gridStudents";
            this.gridStudents.Location = new Point(12, 55);
            this.gridStudents.Size = new Size(1080, 255);
            this.gridStudents.ReadOnly = true;
            this.gridStudents.AllowUserToAddRows = false;
            this.gridStudents.AllowUserToDeleteRows = false;
            this.gridStudents.MultiSelect = false;
            this.gridStudents.RowHeadersVisible = false;
            this.gridStudents.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridStudents.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridStudents.BackgroundColor = SystemColors.Window;
            this.gridStudents.CurrentCellChanged += this.StudentSelected;
            this.tabStudents.Controls.Add(this.gridStudents);

            label2.Name = "label2";
            label2.Location = new Point(12, 333);
            label2.Size = new Size(90, 24);
            label2.Text = "Mã học viên";
            this.tabStudents.Controls.Add(label2);

            this.txtStudentCode.Name = "txtStudentCode";
            this.txtStudentCode.Location = new Point(105, 329);
            this.txtStudentCode.Size = new Size(185, 27);
            this.txtStudentCode.MaxLength = 20;
            this.tabStudents.Controls.Add(this.txtStudentCode);

            label3.Name = "label3";
            label3.Location = new Point(316, 333);
            label3.Size = new Size(71, 24);
            label3.Text = "Họ tên";
            this.tabStudents.Controls.Add(label3);

            this.txtStudentName.Name = "txtStudentName";
            this.txtStudentName.Location = new Point(390, 329);
            this.txtStudentName.Size = new Size(300, 27);
            this.txtStudentName.MaxLength = 100;
            this.tabStudents.Controls.Add(this.txtStudentName);

            label4.Name = "label4";
            label4.Location = new Point(720, 333);
            label4.Size = new Size(78, 24);
            label4.Text = "Ngày sinh";
            this.tabStudents.Controls.Add(label4);

            this.dtBirth.Name = "dtBirth";
            this.dtBirth.Location = new Point(804, 329);
            this.dtBirth.Size = new Size(190, 27);
            this.dtBirth.Format = DateTimePickerFormat.Custom;
            this.dtBirth.CustomFormat = "dd/MM/yyyy";
            this.dtBirth.MinDate = new System.DateTime(1900,1,1);
            this.tabStudents.Controls.Add(this.dtBirth);

            label5.Name = "label5";
            label5.Location = new Point(12, 375);
            label5.Size = new Size(90, 24);
            label5.Text = "Giới tính";
            this.tabStudents.Controls.Add(label5);

            this.cboGender.Name = "cboGender";
            this.cboGender.Location = new Point(105, 371);
            this.cboGender.Size = new Size(185, 27);
            this.cboGender.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabStudents.Controls.Add(this.cboGender);

            label6.Name = "label6";
            label6.Location = new Point(316, 375);
            label6.Size = new Size(71, 24);
            label6.Text = "Điện thoại";
            this.tabStudents.Controls.Add(label6);

            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Location = new Point(390, 371);
            this.txtPhone.Size = new Size(300, 27);
            this.txtPhone.MaxLength = 16;
            this.tabStudents.Controls.Add(this.txtPhone);

            label7.Name = "label7";
            label7.Location = new Point(720, 375);
            label7.Size = new Size(78, 24);
            label7.Text = "Email";
            this.tabStudents.Controls.Add(label7);

            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Location = new Point(804, 371);
            this.txtEmail.Size = new Size(288, 27);
            this.txtEmail.MaxLength = 100;
            this.tabStudents.Controls.Add(this.txtEmail);

            label8.Name = "label8";
            label8.Location = new Point(12, 417);
            label8.Size = new Size(90, 24);
            label8.Text = "Địa chỉ";
            this.tabStudents.Controls.Add(label8);

            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Location = new Point(105, 413);
            this.txtAddress.Size = new Size(585, 27);
            this.txtAddress.MaxLength = 200;
            this.tabStudents.Controls.Add(this.txtAddress);

            this.chkStudentActive.Name = "chkStudentActive";
            this.chkStudentActive.Location = new Point(804, 413);
            this.chkStudentActive.Size = new Size(220, 26);
            this.chkStudentActive.Text = "Đang hoạt động";
            this.chkStudentActive.Checked = true;
            this.tabStudents.Controls.Add(this.chkStudentActive);

            this.btnNewStudent.Name = "btnNewStudent";
            this.btnNewStudent.Location = new Point(12, 474);
            this.btnNewStudent.Size = new Size(120, 32);
            this.btnNewStudent.Text = "Nhập mới";
            this.btnNewStudent.UseVisualStyleBackColor = true;
            this.btnNewStudent.Click += this.NewStudentClick;
            this.tabStudents.Controls.Add(this.btnNewStudent);

            this.btnSaveStudent.Name = "btnSaveStudent";
            this.btnSaveStudent.Location = new Point(145, 474);
            this.btnSaveStudent.Size = new Size(130, 32);
            this.btnSaveStudent.Text = "Lưu học viên";
            this.btnSaveStudent.UseVisualStyleBackColor = true;
            this.btnSaveStudent.Click += this.SaveStudentClick;
            this.tabStudents.Controls.Add(this.btnSaveStudent);

            this.btnDeleteStudent.Name = "btnDeleteStudent";
            this.btnDeleteStudent.Location = new Point(289, 474);
            this.btnDeleteStudent.Size = new Size(135, 32);
            this.btnDeleteStudent.Text = "Xóa học viên";
            this.btnDeleteStudent.UseVisualStyleBackColor = true;
            this.btnDeleteStudent.Click += this.DeleteStudentClick;
            this.tabStudents.Controls.Add(this.btnDeleteStudent);

            this.tabCourses.Name = "tabCourses";
            this.tabCourses.Text = "Khóa học";
            this.tabCourses.UseVisualStyleBackColor = true;
            this.tabs.Controls.Add(this.tabCourses);
            this.gridCourses.Name = "gridCourses";
            this.gridCourses.Location = new Point(12, 12);
            this.gridCourses.Size = new Size(1080, 300);
            this.gridCourses.ReadOnly = true;
            this.gridCourses.AllowUserToAddRows = false;
            this.gridCourses.AllowUserToDeleteRows = false;
            this.gridCourses.MultiSelect = false;
            this.gridCourses.RowHeadersVisible = false;
            this.gridCourses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridCourses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridCourses.BackgroundColor = SystemColors.Window;
            this.gridCourses.CurrentCellChanged += this.CourseSelected;
            this.tabCourses.Controls.Add(this.gridCourses);

            label9.Name = "label9";
            label9.Location = new Point(12, 341);
            label9.Size = new Size(90, 24);
            label9.Text = "Mã khóa";
            this.tabCourses.Controls.Add(label9);

            this.txtCourseCode.Name = "txtCourseCode";
            this.txtCourseCode.Location = new Point(106, 337);
            this.txtCourseCode.Size = new Size(204, 27);
            this.txtCourseCode.MaxLength = 20;
            this.tabCourses.Controls.Add(this.txtCourseCode);

            label10.Name = "label10";
            label10.Location = new Point(341, 341);
            label10.Size = new Size(80, 24);
            label10.Text = "Tên khóa";
            this.tabCourses.Controls.Add(label10);

            this.txtCourseName.Name = "txtCourseName";
            this.txtCourseName.Location = new Point(427, 337);
            this.txtCourseName.Size = new Size(665, 27);
            this.txtCourseName.MaxLength = 100;
            this.tabCourses.Controls.Add(this.txtCourseName);

            label11.Name = "label11";
            label11.Location = new Point(12, 385);
            label11.Size = new Size(90, 24);
            label11.Text = "Số giờ";
            this.tabCourses.Controls.Add(label11);

            this.numHours.Name = "numHours";
            this.numHours.Location = new Point(106, 381);
            this.numHours.Size = new Size(204, 27);
            this.numHours.Minimum = 1;
            this.numHours.Maximum = 2000;
            this.numHours.Value = 30;
            this.numHours.DecimalPlaces = 0;
            this.numHours.ThousandsSeparator = true;
            this.tabCourses.Controls.Add(this.numHours);

            label12.Name = "label12";
            label12.Location = new Point(341, 385);
            label12.Size = new Size(80, 24);
            label12.Text = "Học phí (đ)";
            this.tabCourses.Controls.Add(label12);

            this.numFee.Name = "numFee";
            this.numFee.Location = new Point(427, 381);
            this.numFee.Size = new Size(241, 27);
            this.numFee.Minimum = 0;
            this.numFee.Maximum = 999999999;
            this.numFee.Value = 0;
            this.numFee.DecimalPlaces = 2;
            this.numFee.ThousandsSeparator = true;
            this.tabCourses.Controls.Add(this.numFee);

            this.chkCourseActive.Name = "chkCourseActive";
            this.chkCourseActive.Location = new Point(720, 381);
            this.chkCourseActive.Size = new Size(240, 26);
            this.chkCourseActive.Text = "Đang hoạt động";
            this.chkCourseActive.Checked = true;
            this.tabCourses.Controls.Add(this.chkCourseActive);

            this.btnNewCourse.Name = "btnNewCourse";
            this.btnNewCourse.Location = new Point(12, 474);
            this.btnNewCourse.Size = new Size(120, 32);
            this.btnNewCourse.Text = "Nhập mới";
            this.btnNewCourse.UseVisualStyleBackColor = true;
            this.btnNewCourse.Click += this.NewCourseClick;
            this.tabCourses.Controls.Add(this.btnNewCourse);

            this.btnSaveCourse.Name = "btnSaveCourse";
            this.btnSaveCourse.Location = new Point(145, 474);
            this.btnSaveCourse.Size = new Size(135, 32);
            this.btnSaveCourse.Text = "Lưu khóa học";
            this.btnSaveCourse.UseVisualStyleBackColor = true;
            this.btnSaveCourse.Click += this.SaveCourseClick;
            this.tabCourses.Controls.Add(this.btnSaveCourse);

            this.btnDeleteCourse.Name = "btnDeleteCourse";
            this.btnDeleteCourse.Location = new Point(293, 474);
            this.btnDeleteCourse.Size = new Size(135, 32);
            this.btnDeleteCourse.Text = "Xóa khóa học";
            this.btnDeleteCourse.UseVisualStyleBackColor = true;
            this.btnDeleteCourse.Click += this.DeleteCourseClick;
            this.tabCourses.Controls.Add(this.btnDeleteCourse);

            this.tabClasses.Name = "tabClasses";
            this.tabClasses.Text = "Lớp học";
            this.tabClasses.UseVisualStyleBackColor = true;
            this.tabs.Controls.Add(this.tabClasses);
            this.gridClasses.Name = "gridClasses";
            this.gridClasses.Location = new Point(12, 12);
            this.gridClasses.Size = new Size(1080, 282);
            this.gridClasses.ReadOnly = true;
            this.gridClasses.AllowUserToAddRows = false;
            this.gridClasses.AllowUserToDeleteRows = false;
            this.gridClasses.MultiSelect = false;
            this.gridClasses.RowHeadersVisible = false;
            this.gridClasses.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridClasses.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridClasses.BackgroundColor = SystemColors.Window;
            this.gridClasses.CurrentCellChanged += this.ClassSelected;
            this.tabClasses.Controls.Add(this.gridClasses);

            label13.Name = "label13";
            label13.Location = new Point(12, 319);
            label13.Size = new Size(80, 24);
            label13.Text = "Mã lớp";
            this.tabClasses.Controls.Add(label13);

            this.txtClassCode.Name = "txtClassCode";
            this.txtClassCode.Location = new Point(96, 315);
            this.txtClassCode.Size = new Size(205, 27);
            this.txtClassCode.MaxLength = 20;
            this.tabClasses.Controls.Add(this.txtClassCode);

            label14.Name = "label14";
            label14.Location = new Point(327, 319);
            label14.Size = new Size(85, 24);
            label14.Text = "Khóa học";
            this.tabClasses.Controls.Add(label14);

            this.cboCourse.Name = "cboCourse";
            this.cboCourse.Location = new Point(416, 315);
            this.cboCourse.Size = new Size(302, 27);
            this.cboCourse.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabClasses.Controls.Add(this.cboCourse);

            label15.Name = "label15";
            label15.Location = new Point(743, 319);
            label15.Size = new Size(90, 24);
            label15.Text = "Sĩ số tối đa";
            this.tabClasses.Controls.Add(label15);

            this.numCapacity.Name = "numCapacity";
            this.numCapacity.Location = new Point(840, 315);
            this.numCapacity.Size = new Size(170, 27);
            this.numCapacity.Minimum = 1;
            this.numCapacity.Maximum = 1000;
            this.numCapacity.Value = 20;
            this.numCapacity.DecimalPlaces = 0;
            this.numCapacity.ThousandsSeparator = true;
            this.tabClasses.Controls.Add(this.numCapacity);

            label16.Name = "label16";
            label16.Location = new Point(12, 361);
            label16.Size = new Size(80, 24);
            label16.Text = "Giảng viên";
            this.tabClasses.Controls.Add(label16);

            this.txtTeacher.Name = "txtTeacher";
            this.txtTeacher.Location = new Point(96, 357);
            this.txtTeacher.Size = new Size(205, 27);
            this.txtTeacher.MaxLength = 100;
            this.tabClasses.Controls.Add(this.txtTeacher);

            label17.Name = "label17";
            label17.Location = new Point(327, 361);
            label17.Size = new Size(85, 24);
            label17.Text = "Trạng thái";
            this.tabClasses.Controls.Add(label17);

            this.cboClassStatus.Name = "cboClassStatus";
            this.cboClassStatus.Location = new Point(416, 357);
            this.cboClassStatus.Size = new Size(302, 27);
            this.cboClassStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabClasses.Controls.Add(this.cboClassStatus);

            label18.Name = "label18";
            label18.Location = new Point(12, 403);
            label18.Size = new Size(80, 24);
            label18.Text = "Bắt đầu";
            this.tabClasses.Controls.Add(label18);

            this.dtStart.Name = "dtStart";
            this.dtStart.Location = new Point(96, 399);
            this.dtStart.Size = new Size(205, 27);
            this.dtStart.Format = DateTimePickerFormat.Custom;
            this.dtStart.CustomFormat = "dd/MM/yyyy";
            this.dtStart.MinDate = new System.DateTime(1900,1,1);
            this.tabClasses.Controls.Add(this.dtStart);

            label19.Name = "label19";
            label19.Location = new Point(327, 403);
            label19.Size = new Size(85, 24);
            label19.Text = "Kết thúc";
            this.tabClasses.Controls.Add(label19);

            this.dtEnd.Name = "dtEnd";
            this.dtEnd.Location = new Point(416, 399);
            this.dtEnd.Size = new Size(205, 27);
            this.dtEnd.Format = DateTimePickerFormat.Custom;
            this.dtEnd.CustomFormat = "dd/MM/yyyy";
            this.dtEnd.MinDate = new System.DateTime(1900,1,1);
            this.tabClasses.Controls.Add(this.dtEnd);

            this.btnNewClass.Name = "btnNewClass";
            this.btnNewClass.Location = new Point(12, 474);
            this.btnNewClass.Size = new Size(120, 32);
            this.btnNewClass.Text = "Nhập mới";
            this.btnNewClass.UseVisualStyleBackColor = true;
            this.btnNewClass.Click += this.NewClassClick;
            this.tabClasses.Controls.Add(this.btnNewClass);

            this.btnSaveClass.Name = "btnSaveClass";
            this.btnSaveClass.Location = new Point(145, 474);
            this.btnSaveClass.Size = new Size(135, 32);
            this.btnSaveClass.Text = "Lưu lớp học";
            this.btnSaveClass.UseVisualStyleBackColor = true;
            this.btnSaveClass.Click += this.SaveClassClick;
            this.tabClasses.Controls.Add(this.btnSaveClass);

            this.btnDeleteClass.Name = "btnDeleteClass";
            this.btnDeleteClass.Location = new Point(293, 474);
            this.btnDeleteClass.Size = new Size(135, 32);
            this.btnDeleteClass.Text = "Xóa lớp học";
            this.btnDeleteClass.UseVisualStyleBackColor = true;
            this.btnDeleteClass.Click += this.DeleteClassClick;
            this.tabClasses.Controls.Add(this.btnDeleteClass);

            this.tabEnrollments.Name = "tabEnrollments";
            this.tabEnrollments.Text = "Đăng ký học";
            this.tabEnrollments.UseVisualStyleBackColor = true;
            this.tabs.Controls.Add(this.tabEnrollments);
            label20.Name = "label20";
            label20.Location = new Point(12, 16);
            label20.Size = new Size(92, 24);
            label20.Text = "Lọc theo lớp";
            this.tabEnrollments.Controls.Add(label20);

            this.cboEnrollmentFilter.Name = "cboEnrollmentFilter";
            this.cboEnrollmentFilter.Location = new Point(110, 12);
            this.cboEnrollmentFilter.Size = new Size(315, 27);
            this.cboEnrollmentFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboEnrollmentFilter.SelectedIndexChanged += this.EnrollmentFilterChanged;
            this.tabEnrollments.Controls.Add(this.cboEnrollmentFilter);

            this.gridEnrollments.Name = "gridEnrollments";
            this.gridEnrollments.Location = new Point(12, 55);
            this.gridEnrollments.Size = new Size(1080, 270);
            this.gridEnrollments.ReadOnly = true;
            this.gridEnrollments.AllowUserToAddRows = false;
            this.gridEnrollments.AllowUserToDeleteRows = false;
            this.gridEnrollments.MultiSelect = false;
            this.gridEnrollments.RowHeadersVisible = false;
            this.gridEnrollments.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridEnrollments.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridEnrollments.BackgroundColor = SystemColors.Window;
            this.gridEnrollments.CurrentCellChanged += this.EnrollmentSelected;
            this.tabEnrollments.Controls.Add(this.gridEnrollments);

            label21.Name = "label21";
            label21.Location = new Point(12, 351);
            label21.Size = new Size(80, 24);
            label21.Text = "Học viên";
            this.tabEnrollments.Controls.Add(label21);

            this.cboStudent.Name = "cboStudent";
            this.cboStudent.Location = new Point(96, 347);
            this.cboStudent.Size = new Size(411, 27);
            this.cboStudent.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabEnrollments.Controls.Add(this.cboStudent);

            label22.Name = "label22";
            label22.Location = new Point(541, 351);
            label22.Size = new Size(70, 24);
            label22.Text = "Lớp học";
            this.tabEnrollments.Controls.Add(label22);

            this.cboEnrollClass.Name = "cboEnrollClass";
            this.cboEnrollClass.Location = new Point(617, 347);
            this.cboEnrollClass.Size = new Size(370, 27);
            this.cboEnrollClass.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabEnrollments.Controls.Add(this.cboEnrollClass);

            this.btnEnroll.Name = "btnEnroll";
            this.btnEnroll.Location = new Point(12, 393);
            this.btnEnroll.Size = new Size(172, 32);
            this.btnEnroll.Text = "Đăng ký vào lớp";
            this.btnEnroll.UseVisualStyleBackColor = true;
            this.btnEnroll.Click += this.EnrollClick;
            this.tabEnrollments.Controls.Add(this.btnEnroll);

            label23.Name = "label23";
            label23.Location = new Point(12, 460);
            label23.Size = new Size(206, 24);
            label23.Text = "Trạng thái dòng đang chọn";
            this.tabEnrollments.Controls.Add(label23);

            this.cboEnrollmentStatus.Name = "cboEnrollmentStatus";
            this.cboEnrollmentStatus.Location = new Point(223, 456);
            this.cboEnrollmentStatus.Size = new Size(284, 27);
            this.cboEnrollmentStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            this.tabEnrollments.Controls.Add(this.cboEnrollmentStatus);

            this.btnEnrollmentStatus.Name = "btnEnrollmentStatus";
            this.btnEnrollmentStatus.Location = new Point(541, 453);
            this.btnEnrollmentStatus.Size = new Size(200, 32);
            this.btnEnrollmentStatus.Text = "Cập nhật trạng thái";
            this.btnEnrollmentStatus.UseVisualStyleBackColor = true;
            this.btnEnrollmentStatus.Click += this.EnrollmentStatusClick;
            this.tabEnrollments.Controls.Add(this.btnEnrollmentStatus);

            label24.Name = "label24";
            label24.Location = new Point(12, 513);
            label24.Size = new Size(1000, 24);
            label24.Text = "Học phí trong bảng là mức phí được lưu tại thời điểm đăng ký.";
            this.tabEnrollments.Controls.Add(label24);

            this.tabResults.Name = "tabResults";
            this.tabResults.Text = "Kết quả học tập";
            this.tabResults.UseVisualStyleBackColor = true;
            this.tabs.Controls.Add(this.tabResults);
            label25.Name = "label25";
            label25.Location = new Point(12, 16);
            label25.Size = new Size(92, 24);
            label25.Text = "Lọc theo lớp";
            this.tabResults.Controls.Add(label25);

            this.cboResultFilter.Name = "cboResultFilter";
            this.cboResultFilter.Location = new Point(110, 12);
            this.cboResultFilter.Size = new Size(315, 27);
            this.cboResultFilter.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboResultFilter.SelectedIndexChanged += this.ResultFilterChanged;
            this.tabResults.Controls.Add(this.cboResultFilter);

            this.btnExportResults.Name = "btnExportResults";
            this.btnExportResults.Location = new Point(447, 10);
            this.btnExportResults.Size = new Size(200, 32);
            this.btnExportResults.Text = "Xuất bảng điểm CSV";
            this.btnExportResults.UseVisualStyleBackColor = true;
            this.btnExportResults.Click += this.ExportResultsClick;
            this.tabResults.Controls.Add(this.btnExportResults);

            this.gridResults.Name = "gridResults";
            this.gridResults.Location = new Point(12, 55);
            this.gridResults.Size = new Size(1080, 300);
            this.gridResults.ReadOnly = true;
            this.gridResults.AllowUserToAddRows = false;
            this.gridResults.AllowUserToDeleteRows = false;
            this.gridResults.MultiSelect = false;
            this.gridResults.RowHeadersVisible = false;
            this.gridResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridResults.BackgroundColor = SystemColors.Window;
            this.gridResults.CurrentCellChanged += this.ResultSelected;
            this.tabResults.Controls.Add(this.gridResults);

            this.lblSelectedResult.Name = "lblSelectedResult";
            this.lblSelectedResult.Location = new Point(12, 371);
            this.lblSelectedResult.Size = new Size(1080, 26);
            this.tabResults.Controls.Add(this.lblSelectedResult);

            label26.Name = "label26";
            label26.Location = new Point(12, 417);
            label26.Size = new Size(110, 24);
            label26.Text = "Điểm quá trình";
            this.tabResults.Controls.Add(label26);

            this.numProgress.Name = "numProgress";
            this.numProgress.Location = new Point(128, 413);
            this.numProgress.Size = new Size(150, 27);
            this.numProgress.Minimum = 0;
            this.numProgress.Maximum = 10;
            this.numProgress.Value = 0;
            this.numProgress.DecimalPlaces = 2;
            this.numProgress.ThousandsSeparator = true;
            this.tabResults.Controls.Add(this.numProgress);

            label27.Name = "label27";
            label27.Location = new Point(306, 417);
            label27.Size = new Size(101, 24);
            label27.Text = "Điểm cuối kỳ";
            this.tabResults.Controls.Add(label27);

            this.numFinal.Name = "numFinal";
            this.numFinal.Location = new Point(413, 413);
            this.numFinal.Size = new Size(150, 27);
            this.numFinal.Minimum = 0;
            this.numFinal.Maximum = 10;
            this.numFinal.Value = 0;
            this.numFinal.DecimalPlaces = 2;
            this.numFinal.ThousandsSeparator = true;
            this.tabResults.Controls.Add(this.numFinal);

            label28.Name = "label28";
            label28.Location = new Point(592, 417);
            label28.Size = new Size(65, 24);
            label28.Text = "Ghi chú";
            this.tabResults.Controls.Add(label28);

            this.txtResultNote.Name = "txtResultNote";
            this.txtResultNote.Location = new Point(663, 413);
            this.txtResultNote.Size = new Size(429, 27);
            this.txtResultNote.MaxLength = 200;
            this.tabResults.Controls.Add(this.txtResultNote);

            this.btnSaveResult.Name = "btnSaveResult";
            this.btnSaveResult.Location = new Point(12, 474);
            this.btnSaveResult.Size = new Size(135, 32);
            this.btnSaveResult.Text = "Lưu kết quả";
            this.btnSaveResult.UseVisualStyleBackColor = true;
            this.btnSaveResult.Click += this.SaveResultClick;
            this.tabResults.Controls.Add(this.btnSaveResult);

            this.btnDeleteResult.Name = "btnDeleteResult";
            this.btnDeleteResult.Location = new Point(162, 474);
            this.btnDeleteResult.Size = new Size(135, 32);
            this.btnDeleteResult.Text = "Xóa kết quả";
            this.btnDeleteResult.UseVisualStyleBackColor = true;
            this.btnDeleteResult.Click += this.DeleteResultClick;
            this.tabResults.Controls.Add(this.btnDeleteResult);

            label29.Name = "label29";
            label29.Location = new Point(330, 479);
            label29.Size = new Size(760, 24);
            label29.Text = "Điểm TB = 40% điểm quá trình + 60% điểm cuối kỳ. Từ 5,00 trở lên là Đạt.";
            this.tabResults.Controls.Add(label29);

            this.tabReports.Name = "tabReports";
            this.tabReports.Text = "Thống kê";
            this.tabReports.UseVisualStyleBackColor = true;
            this.tabs.Controls.Add(this.tabReports);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Location = new Point(12, 16);
            this.lblSummary.Size = new Size(1080, 57);
            this.tabReports.Controls.Add(this.lblSummary);

            this.btnReport.Name = "btnReport";
            this.btnReport.Location = new Point(12, 84);
            this.btnReport.Size = new Size(173, 32);
            this.btnReport.Text = "Cập nhật thống kê";
            this.btnReport.UseVisualStyleBackColor = true;
            this.btnReport.Click += this.ReportClick;
            this.tabReports.Controls.Add(this.btnReport);

            this.btnExportReport.Name = "btnExportReport";
            this.btnExportReport.Location = new Point(200, 84);
            this.btnExportReport.Size = new Size(173, 32);
            this.btnExportReport.Text = "Xuất báo cáo CSV";
            this.btnExportReport.UseVisualStyleBackColor = true;
            this.btnExportReport.Click += this.ExportReportClick;
            this.tabReports.Controls.Add(this.btnExportReport);

            this.gridReport.Name = "gridReport";
            this.gridReport.Location = new Point(12, 134);
            this.gridReport.Size = new Size(1080, 364);
            this.gridReport.ReadOnly = true;
            this.gridReport.AllowUserToAddRows = false;
            this.gridReport.AllowUserToDeleteRows = false;
            this.gridReport.MultiSelect = false;
            this.gridReport.RowHeadersVisible = false;
            this.gridReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.gridReport.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.gridReport.BackgroundColor = SystemColors.Window;
            this.tabReports.Controls.Add(this.gridReport);

            label30.Name = "label30";
            label30.Location = new Point(12, 512);
            label30.Size = new Size(1080, 24);
            label30.Text = "Học phí đăng ký chưa bao gồm việc theo dõi thu tiền. Đăng ký đã hủy được loại khỏi thống kê.";
            this.tabReports.Controls.Add(label30);

            this.tabPassword.Name = "tabPassword";
            this.tabPassword.Text = "Đổi mật khẩu";
            this.tabPassword.UseVisualStyleBackColor = true;
            this.tabs.Controls.Add(this.tabPassword);
            label36.Name = "label36";
            label36.Location = new Point(35, 47);
            label36.Size = new Size(172, 24);
            label36.Text = "Mật khẩu hiện tại";
            this.tabPassword.Controls.Add(label36);

            this.txtOldPassword.Name = "txtOldPassword";
            this.txtOldPassword.Location = new Point(215, 43);
            this.txtOldPassword.Size = new Size(330, 27);
            this.txtOldPassword.MaxLength = 100;
            this.txtOldPassword.UseSystemPasswordChar = true;
            this.tabPassword.Controls.Add(this.txtOldPassword);

            label37.Name = "label37";
            label37.Location = new Point(35, 95);
            label37.Size = new Size(172, 24);
            label37.Text = "Mật khẩu mới";
            this.tabPassword.Controls.Add(label37);

            this.txtNewPassword.Name = "txtNewPassword";
            this.txtNewPassword.Location = new Point(215, 91);
            this.txtNewPassword.Size = new Size(330, 27);
            this.txtNewPassword.MaxLength = 100;
            this.txtNewPassword.UseSystemPasswordChar = true;
            this.tabPassword.Controls.Add(this.txtNewPassword);

            label38.Name = "label38";
            label38.Location = new Point(35, 143);
            label38.Size = new Size(172, 24);
            label38.Text = "Nhập lại mật khẩu mới";
            this.tabPassword.Controls.Add(label38);

            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.Location = new Point(215, 139);
            this.txtConfirmPassword.Size = new Size(330, 27);
            this.txtConfirmPassword.MaxLength = 100;
            this.txtConfirmPassword.UseSystemPasswordChar = true;
            this.tabPassword.Controls.Add(this.txtConfirmPassword);

            this.btnChangePassword.Name = "btnChangePassword";
            this.btnChangePassword.Location = new Point(215, 201);
            this.btnChangePassword.Size = new Size(160, 32);
            this.btnChangePassword.Text = "Đổi mật khẩu";
            this.btnChangePassword.UseVisualStyleBackColor = true;
            this.btnChangePassword.Click += this.ChangePasswordClick;
            this.tabPassword.Controls.Add(this.btnChangePassword);

            label39.Name = "label39";
            label39.Location = new Point(215, 259);
            label39.Size = new Size(600, 24);
            label39.Text = "Mật khẩu mới có từ 8 đến 100 ký tự.";
            this.tabPassword.Controls.Add(label39);

            this.lblUser.Location = new Point(16, 18);
            this.lblUser.Size = new Size(790, 32);
            this.lblUser.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblUser.TextAlign = ContentAlignment.MiddleLeft;

            this.btnReload.Text = "Tải lại dữ liệu";
            this.btnReload.Name = "btnReload";
            this.btnReload.Location = new Point(836, 19);
            this.btnReload.Size = new Size(146, 32);
            this.btnReload.Click += this.ReloadClick;

            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Location = new Point(996, 19);
            this.btnLogout.Size = new Size(146, 32);
            this.btnLogout.Click += this.LogoutClick;

            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Location = new Point(14, 641);
            this.lblMessage.Size = new Size(1131, 33);
            this.lblMessage.BorderStyle = BorderStyle.FixedSingle;
            this.lblMessage.Padding = new Padding(8, 0, 8, 0);
            this.lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            this.errors.ContainerControl = this;
            this.errors.BlinkStyle = ErrorBlinkStyle.NeverBlink;

            this.AutoScaleMode = AutoScaleMode.Font;
            this.Font = new Font("Segoe UI", 9F);
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.ClientSize = new Size(1160, 686);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "NOVA - Quản lý học viên Trung tâm Tin học";
            this.Controls.Add(this.tabs);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnLogout);
            this.Controls.Add(this.lblMessage);
            this.Load += this.MainFormLoad;
            this.ResumeLayout(false);
        }
    }
}
