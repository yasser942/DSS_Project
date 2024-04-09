using System;
using System.Windows.Forms;


namespace DSS_LastAssignment
{
    static class Program
    {
        
        [STAThread]
        static void Main()
        {
            // Enable visual styles for the application
            Application.EnableVisualStyles();
            // Set the compatibility of text rendering to false
            Application.SetCompatibleTextRenderingDefault(false);
            // Run the application with the main form
            Application.Run(new MainForm());
        }
    }
}
