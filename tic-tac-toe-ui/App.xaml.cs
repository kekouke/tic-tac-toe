using System;
using System.Windows;

namespace tic_tac_toe_ui
{
    public partial class App : Application
    {
        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length > 0 && args[0] == "-console") {
                new CMD.UI.ConsoleWindow().Run();
            }
            else {
                var app = new App();
                app.InitializeComponent();
                app.Run(new MainWindow());
            }
        }
    }
}
