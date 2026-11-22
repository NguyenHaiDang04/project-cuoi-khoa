using System.Drawing;
using System.Windows.Forms;

namespace NovaCenter.GUI
{
    partial class LoginForm
    {
        private Label title;
        private Label lblStatus;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private CheckBox chkShowPassword;
        private Button btnLogin;
        private Button btnRetry;
        private Button btnExit;
        private ErrorProvider errors;

        protected override void Dispose(bool disposing)
        {
            if (disposing) errors?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            title = new Label();
            lblStatus = new Label();
            txtUsername = new TextBox();
            txtPassword = new TextBox();
            chkShowPassword = new CheckBox();
            btnLogin = new Button();
            btnRetry = new Button();
            btnExit = new Button();
            errors = new ErrorProvider();
            var usernameLabel = new Label();
            var passwordLabel = new Label();
            SuspendLayout();

            title.Text = "TRUNG TÂM TIN HỌC NOVA";
            title.Font = new Font("Segoe UI",14F,FontStyle.Bold);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Location = new Point(22,23);
            title.Size = new Size(470,44);

            usernameLabel.Text = "Tên đăng nhập";
            usernameLabel.Location = new Point(39,106);
            usernameLabel.Size = new Size(116,25);
            txtUsername.Name = "txtUsername";
            txtUsername.Location = new Point(165,102);
            txtUsername.Size = new Size(280,27);
            txtUsername.MaxLength = 50;

            passwordLabel.Text = "Mật khẩu";
            passwordLabel.Location = new Point(39,153);
            passwordLabel.Size = new Size(116,25);
            txtPassword.Name = "txtPassword";
            txtPassword.Location = new Point(165,149);
            txtPassword.Size = new Size(280,27);
            txtPassword.MaxLength = 100;
            txtPassword.UseSystemPasswordChar = true;

            chkShowPassword.Name = "chkShowPassword";
            chkShowPassword.Text = "Hiện mật khẩu";
            chkShowPassword.Location = new Point(165,190);
            chkShowPassword.Size = new Size(230,27);
            chkShowPassword.CheckedChanged += this.ShowPasswordChanged;

            btnLogin.Name = "btnLogin";
            btnLogin.Text = "Đăng nhập";
            btnLogin.Location = new Point(165,239);
            btnLogin.Size = new Size(131,34);
            btnLogin.Enabled = false;
            btnLogin.Click += this.LoginClick;
            btnExit.Name = "btnExit";
            btnExit.Text = "Thoát";
            btnExit.Location = new Point(314,239);
            btnExit.Size = new Size(131,34);
            btnExit.Click += (sender,e) => Close();

            btnRetry.Name = "btnRetry";
            btnRetry.Text = "Kết nối lại";
            btnRetry.Location = new Point(165,288);
            btnRetry.Size = new Size(280,31);
            btnRetry.Visible = false;
            btnRetry.Click += this.RetryClick;

            lblStatus.Name = "lblStatus";
            lblStatus.Location = new Point(28,337);
            lblStatus.Size = new Size(456,62);
            lblStatus.TextAlign = ContentAlignment.MiddleCenter;
            lblStatus.Text = "Đang kết nối SQL Server...";
            errors.ContainerControl = this;
            errors.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            AcceptButton = btnLogin;
            CancelButton = btnExit;
            AutoScaleMode = AutoScaleMode.Font;
            Font = new Font("Segoe UI",9F);
            AutoScaleDimensions = new SizeF(7F,15F);
            ClientSize = new Size(514,416);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Nova - Đăng nhập";
            Controls.AddRange(new Control[] {title,usernameLabel,passwordLabel,txtUsername,txtPassword,
                chkShowPassword,btnLogin,btnExit,btnRetry,lblStatus});
            Shown += this.FormShown;
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
