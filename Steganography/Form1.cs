using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace Steganography
{
    public partial class fmMain : Form
    {
        public fmMain()
        {
            InitializeComponent();
        }

        private void pbImage1_Click(object sender, EventArgs e)
        {
            pbImage1.Image = (Image)openImage();
            //MessageBox.Show(((int)"А"[0]).ToString());



            //byte[] byteArray = new byte[(int)Math.Ceiling((double)a.Length / 8)];
            //a.CopyTo(byteArray, 0);
            //var str = Encoding.Unicode.GetString(byteArray);
        }

        private void pbImage2_Click(object sender, EventArgs e)
        {
            pbImage2.Image = (Image)openImage();
        }

        Bitmap openImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP)|*.bmp";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bitmap = new Bitmap(openFileDialog.FileName);
                    return bitmap;
                }
                catch
                {
                    MessageBox.Show("Невозможно загрузить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return null;
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (pbImage1.Image != null)
            {
                Bitmap bitmap = new Bitmap(pbImage1.Image);

                if (!tbText.Text.Equals(""))
                {
                    string text = tbText.Text.Normalize();
                    byte[] strBytes = Encoding.Unicode.GetBytes(text);
                    string s = BitConverter.ToString(strBytes, 0);
                    BitArray bitArray = new BitArray(strBytes);
                    string bitString = "";
                    for (int i = 0; i < bitArray.Count; i++)
                    {
                        bitString += Convert.ToInt32(bitArray.Get(i)).ToString();
                    }
                    byte[] byteArray = Enumerable.Range(0, bitString.Length / 8).
                                    Select(pos => Convert.ToByte(bitString.Substring(pos * 8, 8), 2)).ToArray();
                    var str = Encoding.Unicode.GetString(byteArray);
                    strBytes = Encoding.Unicode.GetBytes(str);
                    s = BitConverter.ToString(strBytes, 0);
                    bitArray = new BitArray(strBytes);

                    int length = bitArray.Length;
                    int index = 0;
                    if (length <= bitmap.Height * bitmap.Width)
                    {
                        for (int i = 0; i < bitmap.Width; i++)
                        {
                            for (int j = 0; j < bitmap.Height; j++)
                            {
                                if (length > index)
                                {
                                    Color color1 = bitmap.GetPixel(i, j);
                                    int r = 0, g = 0, b = 0;
                                    try
                                    {
                                        r = (color1.R == 255 && Convert.ToInt32(bitArray.Get(index)) == 1) ? 254 : color1.R + Convert.ToInt32(bitArray.Get(index));
                                        g = (color1.G == 255 && Convert.ToInt32(bitArray.Get(index + 1)) == 1) ? 254 : color1.G + Convert.ToInt32(bitArray.Get(index + 1));
                                        b = (color1.B == 255 && Convert.ToInt32(bitArray.Get(index + 2)) == 1) ? 254 : color1.B + Convert.ToInt32(bitArray.Get(index + 2));
                                    }
                                    catch
                                    {
                                        if (index - length == 1) { b = color1.B >= 2 ? color1.B - 2 : color1.B + 2; }
                                        if (index - length == 2) { g = color1.G >= 2 ? color1.G - 2 : color1.G + 2; }
                                    }
                                    Color color2 = Color.FromArgb(color1.A, r, g, b);
                                    bitmap.SetPixel(i, j, color2);
                                    index += 3;
                                }
                                else
                                {
                                    Color color1 = bitmap.GetPixel(i, j);
                                    Color color2 = Color.FromArgb(color1.A, (color1.R >= 2 ? color1.R - 2 : color1.R + 2), color1.G, color1.B);
                                    bitmap.SetPixel(i, j, color2);
                                    pbImage2.Image = (Image)bitmap;
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

            if (pbImage1.Image != null && pbImage2.Image != null)
            {

                Bitmap bitmap1 = new Bitmap(pbImage1.Image);
                Bitmap bitmap2 = new Bitmap(pbImage2.Image);
                string bitString = "";

                ArrayList arrayList = new ArrayList();

                if (bitmap1.Width == bitmap2.Width && bitmap1.Height == bitmap2.Height)
                {
                    for (int i = 0; i < bitmap1.Width; i++)
                    {
                        for (int j = 0; j < bitmap1.Height; j++)
                        {
                            if (Math.Abs(bitmap1.GetPixel(i, j).R - bitmap2.GetPixel(i, j).R) == 2 || Math.Abs(bitmap1.GetPixel(i, j).G - bitmap2.GetPixel(i, j).G) == 2 || Math.Abs(bitmap1.GetPixel(i, j).B - bitmap2.GetPixel(i, j).B) == 2)
                            {
                                byte[] byteArray = Enumerable.Range(0, bitString.Length / 8).
                                    Select(pos => Convert.ToByte(bitString.Substring(pos * 8, 8), 2)).ToArray();
                                var str = Encoding.Unicode.GetString(byteArray);
                                tbText.Text = str;
                                return;
                            }
                            else
                            {
                                int r = Math.Abs(bitmap1.GetPixel(i, j).R - bitmap2.GetPixel(i, j).R);
                                int g = Math.Abs(bitmap1.GetPixel(i, j).G - bitmap2.GetPixel(i, j).G);
                                int b = Math.Abs(bitmap1.GetPixel(i, j).B - bitmap2.GetPixel(i, j).B);

                                bitString += r.ToString() + g.ToString() + b.ToString();
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Размеры изображений не совпадают", "Ошибка",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (pbImage2.Image != null)
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
                        pbImage2.Image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
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
