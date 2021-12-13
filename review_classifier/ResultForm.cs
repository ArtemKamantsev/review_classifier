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
    public partial class ResultForm : Form
    {
        public ResultForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Visible = true;

            ProcessStartInfo start = new ProcessStartInfo();

            string curDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = Directory.GetParent(curDir);
            DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
            // TODO: нодовский интерпретатор
            start.FileName = @"C:\Program Files\nodejs\node.exe";
            string path = directoryInfo2.FullName + @"\scrapper\1.js";
            //start.FileName = path;

            start.Arguments = string.Format("{0} {1}", path, 12);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))// node 1.js 1 2 3 4 - запуск 1.js с аргументами 1, 2 , 3,4
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    MessageBox.Show(result);
                }
            }

            //using (FileStream fs = new FileStream("file.js", FileMode.Open))
            //{
            //    JintEngine js = JintEngine.Load(fs);
            //    object result = js.Run("return status;");
            //    Console.WriteLine(result);
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
