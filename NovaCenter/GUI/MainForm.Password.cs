using System;
using static NovaCenter.GUI.Ui;

namespace NovaCenter.GUI
{
    internal partial class MainForm
    {
        private void ChangePasswordClick(object sender, EventArgs e) => Run(lblMessage, () =>
        {
            if (!Required(errors,txtOldPassword,txtNewPassword,txtConfirmPassword)) return;
            if (txtNewPassword.Text != txtConfirmPassword.Text)
                throw new ArgumentException("Mật khẩu nhập lại chưa khớp.");
            auth.ChangePassword(session,txtOldPassword.Text,txtNewPassword.Text);
            txtOldPassword.Clear();
            txtNewPassword.Clear();
            txtConfirmPassword.Clear();
            Notice(lblMessage,"Đã đổi mật khẩu.");
        });
    }
}
