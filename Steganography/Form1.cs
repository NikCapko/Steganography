using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Steganography
{
    public partial class fmMain : System.Windows.Forms.Form
    {
        public fmMain()
        {
            InitializeComponent();
        }

        private void pbImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.PNG)|*.bmp;*.jpg;*.png|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bitmap = new Bitmap(openFileDialog.FileName);
                    pbImage.Image = (Image)bitmap;
                }
                catch
                {
                    MessageBox.Show("Невозможно загрузить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (!tbText.Text.Equals(""))
            {
                string text = tbText.Text;
                int length = text.Length;
                Bitmap bitmap = new Bitmap(pbImage.Image);
                int iText = 0;
                if (length > bitmap.Height * bitmap.Width)
                {
                    for (int i = 0; i < bitmap.Width; i++)
                    {
                        for (int j = 0; j < bitmap.Height; j++)
                        {
                            if (length <= 0)
                            {
                                Color color1 = bitmap.GetPixel(i, j);
                                Color color2 = Color.FromArgb(color1.A, color1.R, color1.G, (int)text[iText]);
                                bitmap.SetPixel(i, j, color2);
                                iText++;
                                length--;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Слишком длинное сообщение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели сообщение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pbImage.Image != null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.JPG)|*.jpg|Image Files(*.BMP)|*.bmp|Image Files(*.PNG)|*.png|All files (*.*)|*.*";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pbImage.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
