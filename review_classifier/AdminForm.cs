using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace review_classifier
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ProcessStartInfo start = new ProcessStartInfo();

            string curDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = Directory.GetParent(curDir);
            DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
            start.FileName = directoryInfo2.FullName + @"\analytics\venv\Scripts\python.exe";
            string path = directoryInfo2.FullName + @"\analytics\api_evaluate.py";

            start.Arguments = string.Format("{0} -c \"{1}\"", path, "worst app!");
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    MessageBox.Show(result);
                }
            }
        }
    }
}
//ProcessStartInfo start = new ProcessStartInfo();
//start.FileName = @"C:\Users\Andrey\Desktop\c++_projects\4_class\review_classifier\review_classifier\analytics\venv\Scripts\python.exe";

//string curDir = Directory.GetCurrentDirectory();
//DirectoryInfo directoryInfo = Directory.GetParent(curDir);
//DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
//string path = directoryInfo2.FullName + @"\analytics\api.py";

//start.Arguments = string.Format("{0} -c \"{1}\"", path, "worst app!");
//start.UseShellExecute = false;
//start.RedirectStandardOutput = true;
//using (Process process = Process.Start(start))
//{
//    using (StreamReader reader = process.StandardOutput)
//    {
//        string result = reader.ReadToEnd();
//        MessageBox.Show(result);
//    }
//}