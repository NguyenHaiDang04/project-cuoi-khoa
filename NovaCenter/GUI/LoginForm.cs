using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;
using NovaCenter.BUS;
using NovaCenter.DAL;
using NovaCenter.DTO;
using static NovaCenter.GUI.Ui;

namespace NovaCenter.GUI
{
    internal partial class LoginForm : Form
    {
        private readonly AuthService auth = new AuthService();
        private bool busy;
        public UserSession Session { get; private set; }
        public event EventHandler LoginSucceeded;
        public Task ReadyTask { get; private set; } = Task.CompletedTask;
        public Task LoginTask { get; private set; } = Task.CompletedTask;

        public LoginForm() { InitializeComponent(); }

        private void FormShown(object sender, EventArgs e) { ReadyTask = InitializeAsync(); }
        private void RetryClick(object sender, EventArgs e) { ReadyTask = InitializeAsync(); }

        private async Task InitializeAsync()
        {
            if (busy) return;
            busy = true;
            btnLogin.Enabled = btnRetry.Enabled = false;
            try
            {
                Notice(lblStatus,"Đang kết nối SQL Server và chuẩn bị dữ liệu...");
                await DatabaseInitializer.EnsureAsync();
                await Task.Run(() => auth.SeedDemoAccounts());
                if (IsDisposed) return;
                btnRetry.Visible = false;
                btnLogin.Enabled = true;
                Notice(lblStatus,"Nhập tài khoản để đăng nhập.");
                txtUsername.Focus();
            }
            catch (Exception ex) when (ex is SqlException || ex is InvalidOperationException)
            {
                if (IsDisposed) return;
                btnRetry.Visible = btnRetry.Enabled = true;
                Notice(lblStatus,"Không kết nối được CSDL. Kiểm tra SQL Server và chuỗi kết nối trong App.config.",true);
            }
            finally { busy = false; }
        }

        private void ShowPasswordChanged(object sender, EventArgs e) =>
            txtPassword.UseSystemPasswordChar = !chkShowPassword.Checked;

        private void LoginClick(object sender, EventArgs e) { LoginTask = LoginAsync(); }

        private async Task LoginAsync()
        {
            if (busy || !Required(errors,txtUsername,txtPassword)) return;
            busy = true;
            btnLogin.Enabled = false;
            string username = txtUsername.Text, password = txtPassword.Text;
            try
            {
                Session = await Task.Run(() => auth.Login(username,password));
                if (IsDisposed) return;
                txtPassword.Clear();
                if (Session == null)
                {
                    Notice(lblStatus,"Tên đăng nhập, mật khẩu không đúng hoặc tài khoản đã khóa.",true);
                    txtPassword.Focus();
                }
                else
                {
                    Notice(lblStatus,"Đăng nhập thành công.");
                    LoginSucceeded?.Invoke(this,EventArgs.Empty);
                }
            }
            catch (SqlException)
            {
                if (!IsDisposed) Notice(lblStatus,"Mất kết nối SQL Server. Hãy thử lại.",true);
            }
            finally
            {
                busy = false;
                if (!IsDisposed) btnLogin.Enabled = true;
            }
        }
    }
}
