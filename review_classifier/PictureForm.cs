using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace review_classifier
{
    public partial class PictureForm : Form
    {
        string ImageT, TextT;
        bool flag = false;

        public PictureForm(string text)
        {
            InitializeComponent();
            ImageT = text;
        }
        
        public PictureForm( string file, string text)
        {
            InitializeComponent();
            ImageT = file;
            TextT = text;
            flag = true;
        }

        private void PictureForm_Shown(object sender, EventArgs e)
        {
            if (flag)
                ShowMyImageAndText(ImageT, TextT);
            else
                ShowMyImage(ImageT);
        }

        public void ShowMyImage(string file)
        {
            if (file != null && file != "")
            {
                Image MyImage = Base64ToImage(file);
                pictureBox1.Image = MyImage;
            }
            else
                ShowImage();
        }

        public Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public void ShowMyImageAndText(string file, string text)
        {
            pictureBox1.Height = 357;
            if (file != null && file != "")
            {
                Image image = Base64ToImage(file);
                pictureBox1.Image = image;
            }
            else
                ShowImage();
            string [] Texts = text.Split('\n');
            for (int i = 0; i < Texts.Length; i++)
            {
                textBox1.Text += Texts[i];
                if (i != Texts.Length - 1)
                {
                    textBox1.Text += Environment.NewLine;
                }
            }
            textBox1.Visible = true;
        }

        private void ShowImage()
        {
            string curDir = Directory.GetCurrentDirectory();
            DirectoryInfo directoryInfo = Directory.GetParent(curDir);
            DirectoryInfo directoryInfo2 = Directory.GetParent(directoryInfo.FullName);
            string path = directoryInfo2.FullName + @"\analytics\models\model_image.png";

            pictureBox1.Image = Image.FromFile(path);
        }
    }
}
