using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IClipboard
{
    public partial class ImageForm : MaterialForm
    {
        public ImageForm(Image image)
        {
            InitializeComponent();
            pb1.Image = image;
        }

        private void tssave_Click(object sender, EventArgs e)
        {
            Image img = pb1.Image;
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "图片文件|*.JPG|*.PNG|*.GIF|*.TIFF|*.BMP|*.ICO|";
            save.FileName ="IC"+DateTime.Now.ToFileTime().ToString() + "."+ ImageType.GetType(img);
            if (save.ShowDialog() == DialogResult.OK)
            {
                string path = save.FileName;
                img.Save(save.FileName);
            }
        }

        private void tscopy_Click(object sender, EventArgs e)
        {
            MainForm.ICopy = 1;
            Image img = pb1.Image;
            Clipboard.SetImage(img);
            MainForm.ICopy = 0;
        }
    }
}
