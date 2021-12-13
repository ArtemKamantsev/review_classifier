using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace review_classifier
{
    public partial class AdminForm : Form
    {
        List<string> res;

        public AdminForm()
        {
            InitializeComponent();
        }

        // "[{\"text\": \"the worst app\", \"score\": 1},{\"text\": \"the best app\", \"score\": 5}]"

        private void button2_Click(object sender, EventArgs e)
        {
            res = new List<string>();
            StartPython(textBox1.Text, "c");

            MessageBox.Show(res[res.Count - 1]);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            res = new List<string>();
            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName; // полный путь

            StartPython(filename, "p");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //res = new List<string>();
            //StartPython(
            //    "[{\"text\": \"the worst app\", \"score\": 1},{\"text\": \"the best app\", \"score\": 5}]",
            //    "d");

            // TODO: Какой вывод?
        }

        private void StartPython(string row, string letter)
        {
            ProcessStartInfo start = new ProcessStartInfo();

            string curDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = Directory.GetParent(curDir);
            DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
            start.FileName = directoryInfo2.FullName + @"\analytics\venv\Scripts\python.exe";
            string path = directoryInfo2.FullName + @"\analytics\api_evaluate.py";

            start.Arguments = string.Format("{0} -{2} \"{1}\"", path, row, letter);
            // -c - строка
            // -р - путь к файлу абсолютный
            // train
            // -d - данные от скрипта Димы
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            string result;
            using (Process process = Process.Start(start))
                using (StreamReader reader = process.StandardOutput)
                    result = reader.ReadToEnd();

            dynamic stuff = JsonConvert.DeserializeObject(result);

            if (stuff.data != null) res.Add(stuff.data.ToString());
            else res.Add("Error: " + stuff.error.ToString());
        }
    }
}
//ProcessStartInfo start = new ProcessStartInfo();

//string curDir = Directory.GetCurrentDirectory();
//DirectoryInfo directoryInfo = Directory.GetParent(curDir);
//DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
//start.FileName = directoryInfo2.FullName + @"\analytics\venv\Scripts\python.exe";
//string path = directoryInfo2.FullName + @"\analytics\api_evaluate.py";

//start.Arguments = string.Format("{0} -c \"{1}\"", path, "worst app!");
//// -c - строка
//// -р - путь к файлу абсолютный

//// train
//// -d - данные от скрипта Димы
//start.UseShellExecute = false;
//start.RedirectStandardOutput = true;
//string result;
//using (Process process = Process.Start(start))
//{
//    using (StreamReader reader = process.StandardOutput)
//    {
//        result = reader.ReadToEnd();
//        //MessageBox.Show(result);
//    }
//}

////dynamic stuff = JsonConvert.DeserializeObject("{ 'Name': 'Jon Smith', 'Address': { 'City': 'New York', 'State': 'NY' }, 'Age': 42 }");
//dynamic stuff = JsonConvert.DeserializeObject(result);

//string name = stuff.data;
//string error = stuff.error;