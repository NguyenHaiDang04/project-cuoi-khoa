using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using NovaCenter.BUS;
using NovaCenter.DTO;
using static NovaCenter.GUI.Ui;

namespace NovaCenter.GUI
{
    internal partial class MainForm : Form
    {
        private readonly UserSession session;
        private readonly StudentService studentService;
        private readonly TrainingService trainingService;
        private readonly StudyService studyService;
        private readonly AuthService auth = new AuthService();
        private bool loading = true;
        private int studentId,courseId,classId,registrationId,registrationClassId,resultId;
        private DataTable students,courses,classes;
        public bool RequestedLogout { get; private set; }

        public MainForm(UserSession session)
        {
            this.session = session;
            studentService = new StudentService(session);
            trainingService = new TrainingService(session);
            studyService = new StudyService(session);
            InitializeComponent();
        }

        private void MainFormLoad(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            lblUser.Text = "NOVA  |  " + session.FullName + "  |  " + (session.IsAdmin ? "Quản trị viên" : "Nhân viên");
            cboGender.Items.AddRange(new[] { "Nam","Nữ","Khác" });
            cboClassStatus.Items.AddRange(TrainingService.ClassStates);
            cboEnrollmentStatus.Items.AddRange(StudyService.RegistrationStates);
            dtBirth.MaxDate = DateTime.Today.AddDays(-1);
            btnDeleteStudent.Enabled = btnDeleteCourse.Enabled = btnDeleteClass.Enabled = btnDeleteResult.Enabled = session.IsAdmin;
            LoadAll();
            Notice(lblMessage,"Chọn tab để quản lý học viên và hoạt động đào tạo.");
        });

        private void LoadAll()
        {
            loading = true;
            try
            {
                students = studentService.Students();
                courses = trainingService.Courses();
                classes = trainingService.Classes();
                LoadStudentGrid();
                Grid(gridCourses,courses,"Id","Mã khóa","Tên khóa","Số giờ","Học phí (đ)","Hoạt động");
                Grid(gridClasses,classes,"Id","Mã lớp","Khóa Id","Khóa học","Giảng viên","Bắt đầu","Kết thúc",
                    "Sĩ số tối đa","Đã đăng ký","Trạng thái");
                gridClasses.Columns["TenKhoa"].FillWeight = 155;
                Bind(cboCourse,courses.Copy(),"TenKhoa");

                DataTable choices = students.Copy();
                choices.Columns.Add("LuaChon",typeof(string));
                foreach (DataRow row in choices.Rows) row["LuaChon"] = row["MaHV"] + " - " + row["HoTen"];
                Bind(cboStudent,choices,"LuaChon");
                Bind(cboEnrollClass,classes.Copy(),"MaLop");
                Bind(cboEnrollmentFilter,ClassChoices(),"MaLop",true);
                Bind(cboResultFilter,ClassChoices(),"MaLop",true);
                ClearStudent();
                ClearCourse();
                ClearClass();
                LoadEnrollments();
                LoadResults();
                LoadReport();
                errors.Clear();
            }
            finally { loading = false; }
        }

        private DataTable ClassChoices()
        {
            var result = new DataTable();
            result.Columns.Add("Id",typeof(int));
            result.Columns.Add("MaLop",typeof(string));
            foreach (DataRow row in classes.Rows) result.Rows.Add(row["Id"],row["MaLop"]);
            return result;
        }

        private void LoadStudentGrid()
        {
            DataTable rows = studentService.Students(txtSearch.Text);
            Grid(gridStudents,rows,"Id","Mã HV","Họ tên","Ngày sinh","Giới tính","Điện thoại","Email","Địa chỉ","Hoạt động");
            gridStudents.Columns["HoTen"].FillWeight = 170;
            gridStudents.Columns["Email"].FillWeight = 155;
        }

        private void ReloadClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            LoadAll();
            Notice(lblMessage,"Đã tải lại dữ liệu.");
        });

        private void LogoutClick(object sender, EventArgs e)
        {
            RequestedLogout = true;
            Close();
        }

        private void ExportTable(DataTable source, string filename)
        {
            if (source == null) throw new ArgumentException("Chưa có dữ liệu để xuất.");
            using (var dialog = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv",FileName = filename })
            {
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                var table = source.Copy();
                foreach (DataColumn column in table.Columns.Cast<DataColumn>().ToArray())
                    if (column.ColumnName == "Id" || column.ColumnName.EndsWith("Id")) table.Columns.Remove(column);
                CsvExporter.Save(table,dialog.FileName);
                Notice(lblMessage,"Đã xuất CSV " + table.Rows.Count + " dòng.");
            }
        }

        private void ExportStudentsClick(object sender, EventArgs e) => Run(lblMessage,
            () => ExportTable((DataTable)gridStudents.DataSource,"HocVien_Nova.csv"));
        private void ExportResultsClick(object sender, EventArgs e) => Run(lblMessage,
            () => ExportTable((DataTable)gridResults.DataSource,"BangDiem_Nova.csv"));
        private void ExportReportClick(object sender, EventArgs e) => Run(lblMessage,
            () => ExportTable((DataTable)gridReport.DataSource,"ThongKe_Nova.csv"));
    }
}
