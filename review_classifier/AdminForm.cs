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

        private void button2_Click(object sender, EventArgs e)
        {
            Enabled = false;
            
            res = new List<string>();
            StartPython(textBox1.Text, "c", "api_evaluate");

            if (res[0].Contains("Error"))
                MessageBox.Show(res[0]);
            else
                listBox1.Items.Add(res[0]);

            Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            res = new List<string>();
            openFileDialog1.Filter = "Text files(*.csv)|*.csv";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName; // полный путь

            Enabled = false;

            StartPython(filename, "p", "api_evaluate");

            if (res[0].Contains("Error"))
                MessageBox.Show(res[0]);
            else
            {
                dynamic stuff = JsonConvert.DeserializeObject(res[0]);
                res.Clear();

                for (int i = 0; i < stuff.Count; i++)
                    res.Add(stuff[i].ToString());

                //if (res[0].Contains("Error"))
                //    MessageBox.Show(res[0]);
                //else
                for (int i = 0; i < res.Count; i++)
                    listBox1.Items.Add(res[i]);
            }

            Enabled = true;
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

            start.Arguments = string.Format("{0} -{2} \"{1}\"", path, row, letter);
            // -c - строка из текстового поля
            // -р - путь к файлу абсолютный
            // -d - данные от скрипта Димы
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            string result;
            using (Process process = Process.Start(start))
                using (StreamReader reader = process.StandardOutput)
                    result = reader.ReadToEnd();

            dynamic stuff = JsonConvert.DeserializeObject(result);

            if (stuff.data == null)
                res.Add("Error: " + stuff.error.ToString());
            else res.Add(stuff.data.ToString());
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StartForm form = new StartForm();
            form.Show();
            Hide();
        }
    }
}
