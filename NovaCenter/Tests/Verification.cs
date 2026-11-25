using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NovaCenter.BUS;
using NovaCenter.DAL;
using NovaCenter.DTO;
using NovaCenter.GUI;

namespace NovaCenter.Tests
{
    internal sealed class Verification
    {
        private readonly string output;
        private readonly List<string> log = new List<string>();
        private MainForm form;
        private StudentService studentService;
        private TrainingService trainingService;
        private StudyService studyService;
        private UserSession admin;

        private Verification(string output) { this.output = output; }

        public static int Run(string output)
        {
            Directory.CreateDirectory(output);
            var config = new SqlConnectionStringBuilder(Db.ConnectionString);
            if (config.DataSource != @"(localdb)\MSSQLLocalDB"
                || !config.InitialCatalog.StartsWith("Nova_Test_",StringComparison.Ordinal))
            {
                File.WriteAllText(Path.Combine(output,"verification.txt"),"FAIL: Verification requires an isolated Nova_Test_ LocalDB database.");
                return 1;
            }

            var test = new Verification(output);
            int result = 0;
            using (var host = new Form { ShowInTaskbar=false,Opacity=0,StartPosition=FormStartPosition.Manual,
                Location=new Point(-18000,-18000) })
            {
                host.Shown += (sender,e) => host.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        test.Scenario();
                        test.log.Add("RESULT: PASS (" + test.log.Count + " checks)");
                    }
                    catch (Exception ex)
                    {
                        test.log.Add("FAIL: " + ex);
                        if (test.form != null && !test.form.IsDisposed)
                            test.log.Add("Form: " + test.Find<Label>("lblMessage").Text);
                        result = 1;
                    }
                    finally
                    {
                        test.form?.Dispose();
                        File.WriteAllLines(Path.Combine(output,"verification.txt"),test.log,new UTF8Encoding(true));
                        host.Close();
                    }
                }));
                Application.Run(host);
            }
            return result;
        }

        private void Scenario()
        {
            var auth = new AuthService();
            DatabaseInitializer.Ensure();
            auth.SeedDemoAccounts();
            DatabaseInitializer.Ensure();
            auth.SeedDemoAccounts();
            Check(Scalar("SELECT COUNT(*) FROM dbo.HocVien") == 4 && Scalar("SELECT COUNT(*) FROM dbo.TaiKhoan") == 2,
                "Initialization can run twice without duplicate students or accounts.");

            using (var login = new LoginForm())
            {
                login.StartPosition = FormStartPosition.Manual;
                login.Location = new Point(-15000,-15000);
                login.ShowInTaskbar = false;
                login.Show();
                Application.DoEvents();
                Wait(login.ReadyTask);
                Button button = Get<Button>(login,"btnLogin");
                Check(button.Enabled && login.AcceptButton == button && login.CancelButton == Get<Button>(login,"btnExit"),
                    "Login initializes SQL, Enter is Login, Escape is Exit.");
                button.PerformClick();
                Application.DoEvents();
                var errors = (ErrorProvider)typeof(LoginForm).GetField("errors",BindingFlags.Instance|BindingFlags.NonPublic).GetValue(login);
                Check(errors.GetError(Get<TextBox>(login,"txtUsername")).Length > 0
                    && errors.GetError(Get<TextBox>(login,"txtPassword")).Length > 0, "ErrorProvider rejects both blank login fields.");
                Get<CheckBox>(login,"chkShowPassword").Checked = true;
                Check(!Get<TextBox>(login,"txtPassword").UseSystemPasswordChar,"Show password checkbox works.");
                Get<CheckBox>(login,"chkShowPassword").Checked = false;
                Get<TextBox>(login,"txtUsername").Text = "admin";
                Get<TextBox>(login,"txtPassword").Text = "incorrect";
                button.PerformClick();
                Wait(login.LoginTask);
                Check(login.Session == null && Get<Label>(login,"lblStatus").Text.Contains("không đúng"),
                    "Wrong password is rejected through the login button.");
                Capture(login,"01_login.png");
                bool succeeded = false;
                login.LoginSucceeded += (sender,e) => succeeded = true;
                Get<TextBox>(login,"txtPassword").Text = "Admin@123";
                button.PerformClick();
                Wait(login.LoginTask);
                admin = login.Session;
                Check(succeeded && admin?.IsAdmin == true,"Login button authenticates the seeded admin.");
                login.Close();
            }

            Check(auth.Login("admin' OR 1=1--","anything") == null,"SQL-like username does not bypass authentication.");
            studentService = new StudentService(admin);
            trainingService = new TrainingService(admin);
            studyService = new StudyService(admin);
            int studentCount = studentService.Students().Rows.Count, courseCount = trainingService.Courses().Rows.Count,
                classCount = trainingService.Classes().Rows.Count;
            Check(studentCount == 4 && courseCount == 3 && classCount == 3,
                "Seeded students, courses and classes can be read through BUS (" + studentCount + "/" + courseCount + "/" + classCount + ").");

            form = new MainForm(admin) { StartPosition = FormStartPosition.Manual,
                Location = new Point(-15000,-15000), ShowInTaskbar = false };
            form.Show();
            Application.DoEvents();
            Check(Find<DataGridView>("gridStudents").Rows.Count == 4
                && Find<TabControl>("tabs").TabPages.Count == 7,
                "Admin workspace loads the seven training and password tabs.");
            Capture(form,"02_students.png");

            Tab("tabStudents");
            Click("btnNewStudent");
            Set("txtStudentCode","HV005");
            Set("txtStudentName","Đặng Minh Khoa");
            Set("txtPhone","0901000005");
            Set("txtEmail","khoa@example.com");
            Find<DateTimePicker>("dtBirth").Value = new DateTime(2004,5,12);
            Click("btnSaveStudent");
            int student = FindId(studentService.Students(),"MaHV","HV005");
            Check(student > 0,"Create student from the WinForms editor.");
            Select("gridStudents","Id",student);
            Set("txtAddress","Quận Hà Đông");
            Click("btnSaveStudent");
            Check((string)studentService.Students().Rows.Cast<DataRow>().Single(r => (int)r["Id"] == student)["DiaChi"] == "Quận Hà Đông",
                "Edit existing student and reload persisted address.");
            Set("txtSearch","' OR 1=1--");
            Click("btnSearch");
            Check(Find<DataGridView>("gridStudents").Rows.Count == 0,"Student search uses literal parameters.");
            Set("txtSearch","Minh Khoa");
            Click("btnSearch");
            Check(Find<DataGridView>("gridStudents").Rows.Count == 1,"Student search finds Vietnamese names.");
            Set("txtSearch","");
            Click("btnSearch");

            Tab("tabCourses");
            Click("btnNewCourse");
            Set("txtCourseCode","PYTHON");
            Set("txtCourseName","Python cơ bản");
            Find<NumericUpDown>("numHours").Value = 40;
            Find<NumericUpDown>("numFee").Value = 2000000;
            Click("btnSaveCourse");
            int course = FindId(trainingService.Courses(),"MaKhoa","PYTHON");
            Check(course > 0 && trainingService.Courses().Rows.Count == 4,"Create course and tuition from the form.");
            Capture(form,"03_courses.png");

            Tab("tabClasses");
            Click("btnNewClass");
            Set("txtClassCode","PY-01");
            Set("txtTeacher","Cô Hương");
            Find<ComboBox>("cboCourse").SelectedValue = course;
            Find<NumericUpDown>("numCapacity").Value = 1;
            Find<DateTimePicker>("dtStart").Value = DateTime.Today;
            Find<DateTimePicker>("dtEnd").Value = DateTime.Today.AddDays(-1);
            Click("btnSaveClass");
            Check(trainingService.Classes().Rows.Count == 3 && Find<Label>("lblMessage").Text.Contains("Ngày kết thúc"),
                "Class end date before start date is rejected.");
            Find<DateTimePicker>("dtEnd").Value = DateTime.Today.AddDays(30);
            Click("btnSaveClass");
            int classId = FindId(trainingService.Classes(),"MaLop","PY-01");
            Check(classId > 0 && trainingService.Classes().Rows.Count == 4,"Create class with course, teacher, dates and capacity.");
            Capture(form,"04_classes.png");

            Tab("tabEnrollments");
            Find<ComboBox>("cboStudent").SelectedValue = student;
            Find<ComboBox>("cboEnrollClass").SelectedValue = classId;
            Click("btnEnroll");
            int registration = (int)studyService.Registrations(classId).Rows.Cast<DataRow>().Single()["Id"];
            Check(studyService.Registrations(classId).Rows.Count == 1,"Enroll student through the form.");
            Find<ComboBox>("cboStudent").SelectedValue = student;
            Find<ComboBox>("cboEnrollClass").SelectedValue = classId;
            Click("btnEnroll");
            Check(studyService.Registrations(classId).Rows.Count == 1
                && Find<Label>("lblMessage").Text.Contains("đã có đăng ký"),"Duplicate enrollment is rejected.");
            Find<ComboBox>("cboStudent").SelectedValue = 4;
            Click("btnEnroll");
            Check(studyService.Registrations(classId).Rows.Count == 1 && Find<Label>("lblMessage").Text.Contains("đủ sĩ số"),
                "Full class rejects another student without a partial insert.");
            Capture(form,"05_enrollment.png");
            var changedFee = new Course { Id=course,Code="PYTHON",Name="Python cơ bản",Hours=40,Fee=2500000,Active=true };
            trainingService.SaveCourse(changedFee);
            Check((decimal)studyService.Registrations(classId).Rows[0]["HocPhi"] == 2000000,
                "Course fee changes preserve the tuition snapshot of existing enrollments.");

            Select("gridEnrollments","Id",registration);
            Find<ComboBox>("cboEnrollmentStatus").SelectedItem = "Đã hủy";
            Click("btnEnrollmentStatus");
            Check((string)studyService.Registrations(classId).Rows[0]["TrangThai"] == "Đã hủy",
                "Cancel enrollment through the form and free class capacity.");
            Select("gridEnrollments","Id",registration);
            Find<ComboBox>("cboEnrollmentStatus").SelectedItem = "Đang học";
            Click("btnEnrollmentStatus");
            Check((string)studyService.Registrations(classId).Rows[0]["TrangThai"] == "Đang học",
                "Restore canceled enrollment when class still has room.");

            Tab("tabResults");
            Find<ComboBox>("cboResultFilter").SelectedValue = classId;
            Select("gridResults","DangKyId",registration);
            Find<NumericUpDown>("numProgress").Value = 8;
            Find<NumericUpDown>("numFinal").Value = 9;
            Set("txtResultNote","Hoàn thành bài thực hành");
            Click("btnSaveResult");
            DataRow score = studyService.Results(classId).Rows.Cast<DataRow>().Single();
            Check((decimal)score["DiemTB"] == 8.60m && (string)score["XepLoai"] == "Đạt",
                "Save scores through form, calculate 40/60 average 8.60 and pass classification.");
            Capture(form,"06_results.png");
            Reject<ArgumentException>(() => studyService.SaveResult(registration,11,5,""),"Scores above 10 are rejected by BUS.");
            Reject<InvalidOperationException>(() => studyService.ChangeStatus(registration,classId,"Đã hủy"),
                "Enrollment with scores cannot be canceled.");
            Reject<SqlException>(() => studentService.DeleteStudent(student),"Foreign keys protect students with existing enrollments.");

            Tab("tabEnrollments");
            Select("gridEnrollments","Id",registration);
            Find<ComboBox>("cboEnrollmentStatus").SelectedItem = "Hoàn thành";
            Click("btnEnrollmentStatus");
            DataRow report = studyService.Report().Rows.Cast<DataRow>().Single(r => (string)r["MaLop"] == "PY-01");
            Check((int)report["DangKy"] == 1 && (int)report["HoanThanh"] == 1
                && (int)report["Dat"] == 1 && (decimal)report["TongHocPhi"] == 2000000,
                "Report counts enrollments, completion, passing results and original tuition.");
            Tab("tabReports");
            Click("btnReport");
            Capture(form,"07_report.png");

            string csv = Path.Combine(output,"BaoCao_Nova.csv");
            CsvExporter.Save(studyService.Report(),csv);
            Check(File.ReadAllText(csv,Encoding.UTF8).Contains("Python cơ bản")
                && File.ReadAllBytes(csv).Take(3).SequenceEqual(new byte[]{239,187,191}),
                "CSV export preserves Vietnamese data and UTF-8 BOM for Excel.");
            var cells = new DataTable();
            cells.Columns.Add("Ten");
            cells.Rows.Add("=1+1");
            cells.Rows.Add("A,\"B\"");
            CsvExporter.Save(cells,Path.Combine(output,"CsvEdgeCases.csv"));
            string escaped = File.ReadAllText(Path.Combine(output,"CsvEdgeCases.csv"));
            Check(escaped.Contains("'=1+1") && escaped.Contains("\"A,\"\"B\"\"\""),
                "CSV quotes commas and quotation marks and neutralizes formula-like cells.");

            var staff = auth.Login("nhanvien","Nova@123");
            Reject<UnauthorizedAccessException>(() => new StudentService(staff).DeleteStudent(student),
                "Staff cannot delete records even when BUS is called directly.");
            using (var staffForm = new MainForm(staff) { StartPosition=FormStartPosition.Manual,
                Location=new Point(-15000,-15000),ShowInTaskbar=false })
            {
                staffForm.Show();
                Application.DoEvents();
                Check(Get<TabControl>(staffForm,"tabs").TabPages.Count == 7
                    && !Get<Button>(staffForm,"btnDeleteStudent").Enabled,
                    "Staff can open training tabs but delete is disabled.");
                staffForm.Close();
            }

            Tab("tabPassword");
            Set("txtOldPassword","Admin@123");
            Set("txtNewPassword","NewAdmin@123");
            Set("txtConfirmPassword","mismatch");
            Click("btnChangePassword");
            Check(Find<Label>("lblMessage").Text.Contains("chưa khớp"),"Password confirmation mismatch is rejected.");
            Set("txtConfirmPassword","NewAdmin@123");
            Click("btnChangePassword");
            Check(auth.Login("admin","Admin@123") == null && auth.Login("admin","NewAdmin@123") != null,
                "Change password works from form.");
            Capture(form,"08_password.png");
            auth.SeedDemoAccounts();
            Check(auth.Login("admin","NewAdmin@123") != null && auth.Login("admin","Admin@123") == null,
                "Restarting/seeding preserves changed account passwords.");

            studyService.DeleteResult(registration);
            Check(studyService.Results(classId).Rows[0]["DiemTB"] == DBNull.Value,"Admin can delete a stored result.");
            CheckCatalogRules(course,classId);
            CheckConcurrentCapacity(course);
            DatabaseInitializer.Ensure();
            Check(studentService.Students().Rows.Count == 5 && studyService.Registrations(classId).Rows.Count == 1,
                "Reinitialization preserves newly created students and enrollments.");
            form.Close();
        }

        private void CheckCatalogRules(int course, int classId)
        {
            var student = new Student
            {
                Code="HV-TEMP",Name="Học viên kiểm tra xóa",BirthDate=new DateTime(2004,1,1),
                Gender="Nam",Phone="0901000006",Email="",Address="",Active=true
            };
            student.Id = studentService.SaveStudent(student);
            student.Name = "Học viên đã cập nhật";
            studentService.SaveStudent(student);
            Check((string)studentService.Students("HV-TEMP").Rows[0]["HoTen"] == student.Name,
                "Student update persists through BUS and DAL.");
            studentService.DeleteStudent(student.Id);
            Check(studentService.Students("HV-TEMP").Rows.Count == 0,
                "Admin can delete a student without registrations.");

            var temporaryCourse = new Course
            {
                Code="TEMP",Name="Khóa kiểm tra",Hours=20,Fee=1000000,Active=true
            };
            temporaryCourse.Id = trainingService.SaveCourse(temporaryCourse);
            temporaryCourse.Name = "Khóa đã cập nhật";
            trainingService.SaveCourse(temporaryCourse);
            Check((string)trainingService.Courses().Rows.Cast<DataRow>()
                .Single(r => (int)r["Id"] == temporaryCourse.Id)["TenKhoa"] == temporaryCourse.Name,
                "Course update persists through BUS and DAL.");

            var temporaryClass = new ClassRoom
            {
                Code="TEMP-01",CourseId=temporaryCourse.Id,Teacher="Thầy Minh",
                StartDate=DateTime.Today,EndDate=DateTime.Today.AddDays(20),Capacity=10,Status="Mở"
            };
            temporaryClass.Id = trainingService.SaveClass(temporaryClass);
            temporaryClass.Capacity = 12;
            trainingService.SaveClass(temporaryClass);
            Check((int)trainingService.Classes().Rows.Cast<DataRow>()
                .Single(r => (int)r["Id"] == temporaryClass.Id)["SiSoToiDa"] == 12,
                "Class update persists through BUS and DAL.");
            trainingService.DeleteClass(temporaryClass.Id);
            trainingService.DeleteCourse(temporaryCourse.Id);
            Check(!trainingService.Classes().Rows.Cast<DataRow>().Any(r => (int)r["Id"] == temporaryClass.Id)
                && !trainingService.Courses().Rows.Cast<DataRow>().Any(r => (int)r["Id"] == temporaryCourse.Id),
                "Admin can delete a class without registrations and then its unused course.");

            var inactiveCourse = new Course
            {
                Id=course,Code="PYTHON",Name="Python cơ bản",Hours=40,Fee=2500000,Active=false
            };
            trainingService.SaveCourse(inactiveCourse);
            Tab("tabClasses");
            Click("btnReload");
            Select("gridClasses","Id",classId);
            Set("txtTeacher","Cô Hương (cập nhật)");
            Find<DateTimePicker>("dtEnd").Value = DateTime.Today.AddDays(40);
            Click("btnSaveClass");
            DataRow updated = trainingService.Classes().Rows.Cast<DataRow>().Single(r => (int)r["Id"] == classId);
            Check((string)updated["GiangVien"] == "Cô Hương (cập nhật)"
                && (DateTime)updated["NgayKetThuc"] == DateTime.Today.AddDays(40),
                "Existing class can update dates and teacher while its original course is inactive.");
            Reject<InvalidOperationException>(() => trainingService.SaveClass(new ClassRoom
            {
                Code="BLOCKED",CourseId=course,Teacher="Thầy Minh",StartDate=DateTime.Today,
                EndDate=DateTime.Today.AddDays(20),Capacity=10,Status="Mở"
            }),"Inactive course still rejects creation of a new class.");
            Reject<InvalidOperationException>(() => studyService.Enroll(4,classId),
                "Inactive course still rejects new enrollments in an existing class.");
            inactiveCourse.Active = true;
            trainingService.SaveCourse(inactiveCourse);
        }

        private void CheckConcurrentCapacity(int course)
        {
            int id = trainingService.SaveClass(new ClassRoom
            {
                Code="RACE-01",CourseId=course,Teacher="Thầy Minh",StartDate=DateTime.Today,
                EndDate=DateTime.Today.AddDays(30),Capacity=1,Status="Mở"
            });
            using (var start = new ManualResetEventSlim(false))
            {
                Func<int,bool> attempt = student =>
                {
                    start.Wait();
                    try { studyService.Enroll(student,id); return true; }
                    catch (InvalidOperationException) { return false; }
                };
                Task<bool> first = Task.Run(() => attempt(1)), second = Task.Run(() => attempt(2));
                start.Set();
                Task.WaitAll(first,second);
                Check(new[]{first.Result,second.Result}.Count(x => x) == 1 && studyService.Registrations(id).Rows.Count == 1,
                    "Concurrent enrollment for the last seat admits exactly one student.");
            }
        }

        private static int Scalar(string sql) => Convert.ToInt32(Db.Read(sql).Rows[0][0]);
        private static int FindId(DataTable table, string code, string value) =>
            (int)table.Rows.Cast<DataRow>().Single(r => (string)r[code] == value)["Id"];
        private static T Get<T>(Control owner, string name) where T : Control => (T)owner.Controls.Find(name,true).Single();
        private T Find<T>(string name) where T : Control => Get<T>(form,name);
        private void Set(string name, string value) => Find<TextBox>(name).Text = value;
        private void Tab(string name) { Find<TabControl>("tabs").SelectedTab=Find<TabPage>(name); Application.DoEvents(); }
        private void Click(string name) { Find<Button>(name).PerformClick(); Application.DoEvents(); }

        private void Select(string grid, string key, int id)
        {
            var view = Find<DataGridView>(grid);
            DataGridViewRow row = view.Rows.Cast<DataGridViewRow>().Single(r => (int)r.Cells[key].Value == id);
            view.CurrentCell = row.Cells.Cast<DataGridViewCell>().First(c => c.Visible);
            Application.DoEvents();
        }

        private static void Wait(Task task)
        {
            var watch = Stopwatch.StartNew();
            while (!task.IsCompleted && watch.Elapsed < TimeSpan.FromSeconds(60))
            {
                Application.DoEvents();
                Thread.Sleep(10);
            }
            if (!task.IsCompleted) throw new TimeoutException("UI task exceeded 60 seconds.");
            task.GetAwaiter().GetResult();
            Application.DoEvents();
        }

        private void Reject<T>(Action action, string description) where T : Exception
        {
            try { action(); }
            catch (T) { Check(true,description); return; }
            throw new InvalidOperationException(description);
        }

        private void Check(bool condition, string description)
        {
            if (!condition) throw new InvalidOperationException(description);
            log.Add("PASS: " + description);
        }

        private void Capture(Form window, string filename)
        {
            window.Refresh();
            Application.DoEvents();
            using (var image = new Bitmap(window.Width,window.Height))
            {
                window.DrawToBitmap(image,new Rectangle(Point.Empty,window.Size));
                image.Save(Path.Combine(output,filename),ImageFormat.Png);
            }
        }
    }
}
