using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace review_classifier
{
    public partial class ResultForm : Form
    {
        private readonly IMongoCollection<BsonDocument> m;

        string row;
        List<string> categories = new List<string>(),
            apps = new List<string>(),
            reviews = new List<string>(),
            res = new List<string>();

        public ResultForm()
        {
            InitializeComponent();

            m = new MongoClient("mongodb://localhost:27017").GetDatabase("first").GetCollection<BsonDocument>("cFirst");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enabled = false;

            SendRequest("getReviews", comboBox1.Text + " 100");
            button2.Enabled = true;

            if (row.Contains("Error"))
            {
                MessageBox.Show(row);
                return;
            }

            dynamic stuf = JsonConvert.DeserializeObject(row);
            foreach (var item in stuf)
                reviews.Add(item.ToString());

            var doc = new BsonDocument();
            var t = m.DeleteMany(doc).DeletedCount;
            SaveData(reviews);

            Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Enabled = false;

            string resultus = "[";
            var doc = m.Find(new BsonDocument()).ToList();
            for (int i = 0; i < doc.Count; i++)
            {
                resultus += doc[i].GetValue("data").ToString() == "" ? doc[i].GetValue("error").ToString() : doc[i].GetValue("data").ToString();
                if (i < doc.Count - 1)
                    resultus += ",";
            }
            resultus += "]";

            StartPython(
            resultus, "d", "api.py");

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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
            listBox1.Items.Clear();
            foreach (var item in stu)
            {
                apps.Add(item.ToString());
                listBox1.Items.Add(item.ToString());
            }

            listBox1.SelectedIndex = 0;

            Enabled = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            label1.Text = String.Format("Текущее значение: {0}", trackBar1.Value);
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
            int number = int.Parse(trackBar1.Value.ToString());
            string OneOfTwo = rbGini.Checked ? "gini" : "entropy";
            ProcessStartInfo start = new ProcessStartInfo();

            string curDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = Directory.GetParent(curDir);
            DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
            start.FileName = directoryInfo2.FullName + @"\analytics\venv\Scripts\python.exe";
            string path = directoryInfo2.FullName + @"\analytics\" + file + ".py";

            start.Arguments = 
                $"{path} -v train";
                //string.Format($"{0} -v train {{\"max_depth\":{1}, \"criterion\":{2}}}", path, number, OneOfTwo);
            // -c - строка из текстового поля
            // -р - путь к файлу абсолютный
            // -d - данные от скрипта Димы
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardInput = true;
            string result_t;
            using (Process process = Process.Start(start))
            {
                StreamWriter sq = process.StandardInput;
                sq.WriteLine($"{{\"max_depth\":{number}, \"criterion\":{OneOfTwo}}}");
                char[] charsToTrim = { '\n', ' ', '\r' };
                row = row.Replace("\r", String.Empty);
                row = row.Replace("\n", String.Empty);
                sq.WriteLine(row);
                using (StreamReader reader = process.StandardOutput)
                    result_t = reader.ReadToEnd();
            }

            dynamic stuff = JsonConvert.DeserializeObject(result_t);
        
            if (stuff.data == null)
                res.Add("Error: " + stuff.error.ToString());
            else
            {
                res.Add(stuff.result.ToString());
                if (stuff.image_base64 != null)
                {
                    PictureForm picture = new PictureForm(stuff.image_base64.ToString());
                    picture.Show();
                }
            }
        }

        private void SaveData(List<string> revs)
        {
            List<BsonDocument> q = new List<BsonDocument>();
            foreach (var item in revs)
            {
                var document = new BsonDocument
                {
                    { "data", item.Contains("Error, ") ? "": item },
                    { "error", item.Contains("Error, ") ? item.Replace("Error, ", string.Empty): "" }
                };
                q.Add(document);
            }
            m.InsertMany(q);
        }
    }
}
