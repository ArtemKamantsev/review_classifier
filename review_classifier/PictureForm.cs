using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace review_classifier
{
    public partial class PictureForm : Form
    {
        string ImageT;

        public PictureForm(string text)
        {
            InitializeComponent();
            ImageT = text;
        }

        private void PictureForm_Shown(object sender, EventArgs e)
        {
            ShowMyImage(ImageT);
        }

        public void ShowMyImage(string file)
        {
            Image MyImage = Base64ToImage(file);
            pictureBox1.Image = MyImage;
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

        }
    }
}
