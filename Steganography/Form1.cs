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
            openFileDialog.Filter = "Image Files(*.BMP)|*.bmp";
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
            if (pbImage.Image != null)
            {
                Bitmap bitmap = new Bitmap(pbImage.Image);

                if (!tbText.Text.Equals(""))
                {
                    string text = tbText.Text.Normalize();
                    int length = text.Length;
                    int iText = 0;
                    if (length <= bitmap.Height * bitmap.Width)
                    {
                        for (int i = 0; i < bitmap.Height; i++)
                        {
                            for (int j = 0; j < bitmap.Width; j++)
                            {
                                if (length > iText)
                                {
                                    Color color1 = bitmap.GetPixel(i, j);
                                    Color color2 = Color.FromArgb(color1.A, color1.R, color1.G, (int)text[iText]);
                                    bitmap.SetPixel(i, j, color2);
                                    iText++;
                                }
                                else
                                {
                                    Color color1 = bitmap.GetPixel(i, j);
                                    Color color2 = Color.FromArgb(color1.A, 13, color1.G, color1.B);
                                    bitmap.SetPixel(i, j, color2);
                                    pbImage.Image = (Image)bitmap;
                                    return;
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
            else
            {
                MessageBox.Show("Вы не выбрали изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            tbText.Text = "";

            if (pbImage.Image != null)
            {
                Bitmap bitmap = new Bitmap(pbImage.Image);

                for (int i = 0; i < bitmap.Height; i++)
                {
                    for (int j = 0; j < bitmap.Width; j++)
                    {
                        if (bitmap.GetPixel(i, j).R == 13)
                        {
                            return;
                        }
                        else
                        {
                            Color color = bitmap.GetPixel(i, j);
                            tbText.Text += ((char)color.B).ToString();
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (pbImage.Image != null)
            {
                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Title = "Сохранить картинку как...";
                savedialog.OverwritePrompt = true;
                savedialog.CheckPathExists = true;
                savedialog.Filter = "Image Files(*.BMP)|*.bmp";
                savedialog.ShowHelp = true;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        pbImage.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали изображение", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
