using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace review_classifier
{
    public partial class ResultForm : Form
    {
        string row;
        List<string> categories = new List<string>(),
            apps = new List<string>(),
            reviews = new List<string>(),
            res = new List<string>();

        public ResultForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enabled = false;

            listBox1.Visible = true;

            SendRequest("getApps", comboBox1.Text);

            if (row.Contains("Error"))
            {
                MessageBox.Show(row);
                return;
            }

            dynamic stu = JsonConvert.DeserializeObject(row);
            foreach (var item in stu)
            {
                apps.Add(item.ToString());
                listBox1.Items.Add(item.ToString());
            }

            listBox1.SelectedIndex = 0;

            SendRequest("getReviews", comboBox1.Text + " 20");
            button2.Enabled = true;

            if (row.Contains("Error"))
            {
                MessageBox.Show(row);
                return;
            }

            dynamic stuf = JsonConvert.DeserializeObject(row);
            foreach (var item in stuf)
                reviews.Add(item.ToString());

            //TODO: запись в бд

            Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Enabled = false;

            StartPython(
            row,
            "d", "api_train");

            MessageBox.Show(res[0]);

            Enabled = true;
        }

        private void ResultForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StartForm form = new StartForm();
            form.Show();
            Hide();
        }

        private void ResultForm_Shown(object sender, EventArgs e)
        {
            SendRequest("getCategories", "");

            if (row.Contains("Error"))
            {
                MessageBox.Show(row);
                return;
            }

            dynamic stuf = JsonConvert.DeserializeObject(row);
            foreach (var item in stuf)
            {
                categories.Add(item.ToString());
                comboBox1.Items.Add(item);
            }

            comboBox1.SelectedIndex = 0;
        }

        private void SendRequest(string filename, string arguments)
        {
            string result;
            ProcessStartInfo start = new ProcessStartInfo();

            string curDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = Directory.GetParent(curDir);
            DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
            start.FileName = @"C:\Program Files\nodejs\node.exe";
            string path = directoryInfo2.FullName + @"\scrapper\" + filename + ".js";

            start.Arguments = string.Format("{0} {1}", path, arguments);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
                using (StreamReader reader = process.StandardOutput)
                    result = reader.ReadToEnd();
            // node 1.js 1 2 3 4 - запуск 1.js с аргументами 1, 2 , 3,4

            dynamic stuff = JsonConvert.DeserializeObject(result);

            if (stuff.data == null)
                row = "Error: " + stuff.error.ToString();
            else row = stuff.data.ToString();
        }

        private void StartPython(string row, string letter, string file)
        {
            listBox1.Items.Clear();

            ProcessStartInfo start = new ProcessStartInfo();

            string curDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = Directory.GetParent(curDir);
            DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
            start.FileName = directoryInfo2.FullName + @"\analytics\venv\Scripts\python.exe";
            string path = directoryInfo2.FullName + @"\analytics\" + file + ".py";

            start.Arguments = string.Format("{0} -{1}", path, letter);
            // -c - строка из текстового поля
            // -р - путь к файлу абсолютный
            // -d - данные от скрипта Димы
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardInput = true;
            string result;
            using (Process process = Process.Start(start))
            {
                StreamWriter sq = process.StandardInput;
                char[] charsToTrim = { '\n', ' ', '\r' };
                row = row.Replace("\r", String.Empty);
                row = row.Replace("\n", String.Empty);
                sq.WriteLine(row);
                using (StreamReader reader = process.StandardOutput)
                    result = reader.ReadToEnd();
            }

            dynamic stuff = JsonConvert.DeserializeObject(result);

            if (stuff.data == null)
                res.Add("Error: " + stuff.error.ToString());
            else res.Add(stuff.data.ToString());
        }
    }
}
