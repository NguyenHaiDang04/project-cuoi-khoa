using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using NovaCenter.GUI;
using NovaCenter.Tests;

namespace NovaCenter
{
    internal static class Program
    {
        [STAThread]
        private static int Main(string[] args)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("vi-VN");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Control.CheckForIllegalCrossThreadCalls = true;
            if (args.Length == 2 && args[0] == "--verify")
                return Verification.Run(args[1]);

            using (var login = new LoginForm())
            {
                login.LoginSucceeded += (sender,e) =>
                {
                    login.Hide();
                    using (var main = new MainForm(login.Session))
                    {
                        main.ShowDialog();
                        if (main.RequestedLogout) login.Show();
                        else login.Close();
                    }
                };
                Application.Run(login);
            }
            return 0;
        }
    }
}
