using System;
using System.Windows.Input;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using SwiftExcel;
using System.Globalization;

namespace WindowsFormsApp5
{
    public partial class Form1 : Form
    {
        int x1, y1, c = 0, c1 = 0, c2 = 0;
        public Form1()
        {
            InitializeComponent();
        }
        public static bool Near(int a, int b, int c)
        {
            return (a - c <= b) && (a + c >= b);
        }
        public static Bitmap Contrast(Bitmap sourceBitmap, int threshold)
        {
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];

            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);

            sourceBitmap.UnlockBits(sourceData);

            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);

            double red = 0;
            double green = 0;
            double blue = 0;

            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                red = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;

                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;

                blue = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;

                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }

                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }

                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }

                pixelBuffer[k] = (byte)red;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)blue;
            }

            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);

            return resultBitmap;
        }
        private void TrcThreshold_ValueChanged(object sender, EventArgs e)
        {
            Bitmap img1 = new Bitmap(pictureBox1.Image);
            lblContrastValue.Text = trcThreshold.Value.ToString();
            pictureBox1.Image = Contrast(img1, trcThreshold.Value);
        }
        public static int HtmlToInt(string a)
        {
            if (a.Length < 1 || a.Equals("#") || a.Length > 7) return 0;
            string b = a;
            if (a[0] == '#' && a.Length > 1)
            {
                b = a.Substring(1, a.Length - 1);
            }
            return Convert.ToInt32(b, 16);
        }

        public static bool ClrNear(int a, int b, int c)
        {
            return ((((a >> 16) - c <= (b >> 16)) && ((a >> 16) + c >= (b >> 16))) &&
                  (((a % 0x10000 >> 8) - c <= (b % 0x10000 >> 8)) && ((a % 0x10000 >> 8) + c >= (b % 0x10000 >> 8))) &&
                  (((a % 0x100) - c <= (b % 0x100)) && ((a % 0x100) + c >= (b % 0x100))));
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Загрузка изображения",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "png",
                Filter = "Images (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                pictureBox1.Image = Image.FromFile(openFileDialog1.FileName);
            }
        }
        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            Bitmap img1 = new Bitmap(pictureBox1.Image);
            x1 = e.Location.X;
            y1 = e.Location.Y;
            try
            {
                Color color1 = ((Bitmap)pictureBox1.Image).GetPixel(e.Location.X, e.Location.Y);
                label2.Text = "(" + e.Location.X + ", " + e.Location.Y + "): " + ColorTranslator.ToHtml(color1);
            }
            catch
            {

            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            Bitmap img1 = new Bitmap(pictureBox1.Image);
            int[,,,] clrarr = new int[img1.Width, img1.Height, 8, 3];
            int[,] points = new int[300000, 3];
            int k = 0;
            int[] cl = new int[3];
            for (int y = 4; (y <= (img1.Height - 5)); y += 1)
            {
                for (int x = 4; (x <= (img1.Width - 5)); x += 1)
                {
                    Color inv = img1.GetPixel(x - 3, y - 3);
                    clrarr[x, y, 0, 0] = inv.R;
                    clrarr[x, y, 0, 1] = inv.G;
                    clrarr[x, y, 0, 2] = inv.B;

                    inv = img1.GetPixel(x - 4, y);
                    clrarr[x, y, 1, 0] = inv.R;
                    clrarr[x, y, 1, 1] = inv.G;
                    clrarr[x, y, 1, 2] = inv.B;

                    inv = img1.GetPixel(x - 3, y + 3);
                    clrarr[x, y, 2, 0] = inv.R;
                    clrarr[x, y, 2, 1] = inv.G;
                    clrarr[x, y, 2, 2] = inv.B;

                    inv = img1.GetPixel(x, y + 4);
                    clrarr[x, y, 3, 0] = inv.R;
                    clrarr[x, y, 3, 1] = inv.G;
                    clrarr[x, y, 3, 2] = inv.B;

                    inv = img1.GetPixel(x + 3, y + 3);
                    clrarr[x, y, 4, 0] = inv.R;
                    clrarr[x, y, 4, 1] = inv.G;
                    clrarr[x, y, 4, 2] = inv.B;

                    inv = img1.GetPixel(x + 4, y);
                    clrarr[x, y, 5, 0] = inv.R;
                    clrarr[x, y, 5, 1] = inv.G;
                    clrarr[x, y, 5, 2] = inv.B;

                    inv = img1.GetPixel(x + 3, y - 3);
                    clrarr[x, y, 6, 0] = inv.R;
                    clrarr[x, y, 6, 1] = inv.G;
                    clrarr[x, y, 6, 2] = inv.B;

                    inv = img1.GetPixel(x, y - 4);
                    clrarr[x, y, 7, 0] = inv.R;
                    clrarr[x, y, 7, 1] = inv.G;
                    clrarr[x, y, 7, 2] = inv.B;

                    bool flag = true;
                    for (int a = 0; a < 8; a++)
                    {
                        if ((clrarr[x, y, a, 0] > 220) && (clrarr[x, y, a, 1] > 220) && (clrarr[x, y, a, 2] > 220))
                        {
                            flag = false;
                        }
                        else
                        {
                            cl[0] = cl[0] + clrarr[x, y, a, 0];
                            cl[1] = cl[1] + clrarr[x, y, a, 1];
                            cl[2] = cl[2] + clrarr[x, y, a, 2];

                            for (int b = 0; b < 8; b++)
                            {
                                if ((true) && (!Near(clrarr[x, y, a, 0], clrarr[x, y, b, 0], 1)))
                                {
                                    flag = false;
                                }
                            }
                        }
                    }
                    cl[0] = cl[0] / 8;
                    cl[1] = cl[1] / 8;
                    cl[2] = cl[2] / 8;
                    if ((flag == true))
                    {
                        points[k, 1] = x;
                        points[k, 2] = y;
                        k++;
                    }
                }
            }

            for (int j = 0; j < k; j++)
            {
                cl[0] = 0;
                cl[1] = 0;
                cl[2] = 0;

                int x = points[j, 1];
                int y = points[j, 2];

                Color inv = img1.GetPixel(x - 3, y - 3);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                inv = img1.GetPixel(x - 4, y);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                inv = img1.GetPixel(x - 3, y + 3);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                inv = img1.GetPixel(x, y + 4);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                inv = img1.GetPixel(x + 3, y + 3);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                inv = img1.GetPixel(x + 4, y);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                inv = img1.GetPixel(x + 3, y - 3);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                inv = img1.GetPixel(x, y - 4);
                cl[0] = cl[0] + inv.R;
                cl[1] = cl[1] + inv.G;
                cl[2] = cl[2] + inv.B;

                cl[0] = cl[0] / 8;
                cl[1] = cl[1] / 8;
                cl[2] = cl[2] / 8;
                dataGridView1.Rows.Add(ColorTranslator.ToHtml(Color.FromArgb(cl[0], cl[1], cl[2])), points[j, 1], points[j, 2]);
            }

            for (int j = 0; j < k; j++)
            {
                bool flag = true;
                for (int v = 0; v < dataGridView2.Rows.Count && v < dataGridView1.Rows.Count; v++)
                {
                    if ((Near(Convert.ToInt32(dataGridView1.Rows[j].Cells[2].Value), Convert.ToInt32(dataGridView2.Rows[v].Cells[2].Value), 4))
                    && (Near(Convert.ToInt32(dataGridView1.Rows[j].Cells[1].Value), Convert.ToInt32(dataGridView2.Rows[v].Cells[1].Value), 4)))
                    {
                        flag = false;
                    }
                }
                if (flag == true)
                {
                    dataGridView2.Rows.Add((dataGridView1.Rows[j].Cells[0].Value), (dataGridView1.Rows[j].Cells[1].Value), (dataGridView1.Rows[j].Cells[2].Value));
                }
            }
            int n = 0, m = 0;
            for (int p = 0; p < dataGridView2.Rows.Count - 1; p++)
            {
                int min = p;
                for (int h = p; h < dataGridView2.Rows.Count - 1; h++)
                {
                    if (HtmlToInt(dataGridView2.Rows[min].Cells[0].Value.ToString()) > HtmlToInt(dataGridView2.Rows[h].Cells[0].Value.ToString()))
                    {
                        min = h;
                    }


                }
                string[] temp = new string[3];
                temp[0] = dataGridView2.Rows[p].Cells[0].Value.ToString();
                temp[1] = dataGridView2.Rows[p].Cells[1].Value.ToString();
                temp[2] = dataGridView2.Rows[p].Cells[2].Value.ToString();

                dataGridView2.Rows[p].Cells[0].Value = dataGridView2.Rows[min].Cells[0].Value.ToString();
                dataGridView2.Rows[p].Cells[1].Value = dataGridView2.Rows[min].Cells[1].Value.ToString();
                dataGridView2.Rows[p].Cells[2].Value = dataGridView2.Rows[min].Cells[2].Value.ToString();
                dataGridView2.Rows[min].Cells[0].Value = temp[0].ToString();
                dataGridView2.Rows[min].Cells[1].Value = temp[1].ToString();
                dataGridView2.Rows[min].Cells[2].Value = temp[2].ToString();
            }
            dataGridView3.Rows.Add((dataGridView2.Rows[0].Cells[0].Value), (dataGridView2.Rows[0].Cells[1].Value), (dataGridView2.Rows[0].Cells[2].Value));
            string[] zeros = new string[dataGridView2.Rows.Count / 4];
            for (int p = 0; p < dataGridView2.Rows.Count/4; p++)
            {
                    dataGridView3.Columns.Add("Color" + p, "Color" + p);
                    dataGridView3.Columns.Add("X" + p, "X" + p);
                    dataGridView3.Columns.Add("Y" + p, "Y" + p);
            }

            for (int p = 0; p < dataGridView2.Rows.Count; p++)
            {
                dataGridView3.Rows.Add(zeros);
            }
            for (int p = 0; p < dataGridView2.Rows.Count - 1; p++)
            {
                if (!ClrNear(HtmlToInt(dataGridView3.Rows[0].Cells[3 * n].Value.ToString()), HtmlToInt(dataGridView2.Rows[p].Cells[0].Value.ToString()), Convert.ToInt32(numericUpDown1.Value)))
                {
                    n++;
                    m = 0;
                } else
                {
                    if (m > c1) c1 = m;
                    if (n > c2) c2 = n;
                    m++;
                }
                dataGridView3.Rows[m].Cells[n * 3].Value = dataGridView2.Rows[p].Cells[0].Value;
                dataGridView3.Rows[m].Cells[n * 3 + 1].Value = dataGridView2.Rows[p].Cells[1].Value;
                dataGridView3.Rows[m].Cells[n * 3 + 2].Value = dataGridView2.Rows[p].Cells[2].Value;
            }

            for (int p = 0; p <= c2; p++)
            {
                dataGridView4.Columns.Add("X" + p, "X" + p);
                dataGridView4.Columns.Add("Y" + p, "Y" + p);
            }

            for (int p = 0; p <= c1; p++)
            {
                dataGridView4.Rows.Add(zeros);
            }
            for (int rp = 0; rp < dataGridView4.Columns.Count; rp+=2)
            {
                for (int p = 0; p < dataGridView4.Rows.Count; p++)
                {
                    if (rp == 0 && p == 0)
                    {
                        p++;
                    }
                    if (rp == 0 || rp == 1)
                    {
                        dataGridView4.Rows[p-1].Cells[rp].Value = dataGridView3.Rows[p].Cells[rp + 1 + rp / 2].Value;
                        dataGridView4.Rows[p-1].Cells[rp + 1].Value = dataGridView3.Rows[p].Cells[rp + 2 + rp / 2].Value;
                    }
                        else
                    {
                        dataGridView4.Rows[p].Cells[rp].Value = dataGridView3.Rows[p].Cells[rp + 1 + rp / 2].Value;
                        dataGridView4.Rows[p].Cells[rp + 1].Value = dataGridView3.Rows[p].Cells[rp + 2 + rp / 2].Value;
                    }
                }
            }

            pictureBox1.Image = img1;
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            Bitmap img1 = new Bitmap(pictureBox1.Image);
            for (int t = 1; t <= 2; t++)
                for (int y = 2; (y <= (img1.Height - 4)); y += 3)
                {
                    for (int x = 2; (x <= (img1.Width - 4)); x += 3)
                    {
                        Color inv = img1.GetPixel(x, y);
                        img1.SetPixel(x, y, inv);
                        img1.SetPixel(x, y + 1, inv);
                        img1.SetPixel(x + 1, y, inv);
                        img1.SetPixel(x, y - 1, inv);
                        img1.SetPixel(x - 1, y, inv);
                        img1.SetPixel(x + 1, y + 1, inv);
                        img1.SetPixel(x - 1, y - 1, inv);
                        img1.SetPixel(x + 1, y - 1, inv);
                        img1.SetPixel(x - 1, y + 1, inv);
                    }
                }
            pictureBox1.Image = img1;
        }
        private void DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Button10_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Hand;
            pictureBox1.Cursor = Cursors.Hand;
        }

        private void PictureBox1_Click_1(object sender, EventArgs e)
        {
            if (Cursor.Current == Cursors.Hand)
            {
                Cursor.Current = Cursors.Default;
                pictureBox1.Cursor = Cursors.Default;
                textBox8.Text = x1.ToString();
                textBox9.Text = y1.ToString();
            }
        }

        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                numericUpDown1.Hexadecimal = true;
            else
                numericUpDown1.Hexadecimal = false;
        }

        private void TextBox15_TextChanged(object sender, EventArgs e)
        {
            TextBox12_TextChanged(sender, e);
        }

        private void TextBox12_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text.Length < 1)
            {
                textBox12.Text = "0";
            }
            if (textBox13.Text.Length < 1)
            {
                textBox13.Text = "0";
            }
            if (textBox14.Text.Length < 1)
            {
                textBox14.Text = "0";
            }
            if (textBox15.Text.Length < 1)
            {
                textBox15.Text = "0";
            }
            if (textBox16.Text.Length < 1)
            {
                textBox16.Text = "0";
            }
            if (textBox17.Text.Length < 1)
            {
                textBox17.Text = "0";
            }
            try
            {
                double x = Convert.ToDouble(textBox12.Text);
                double y = Convert.ToDouble(textBox13.Text);
                double x2 = Convert.ToDouble(textBox14.Text);
                double y2 = Convert.ToDouble(textBox15.Text);
                double x3 = Convert.ToDouble(textBox16.Text);
                double y3 = Convert.ToDouble(textBox17.Text);
            }
            catch
            {
                try
                {
                    char a = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    textBox12.Text = textBox12.Text.Replace(".", ",");
                    textBox13.Text = textBox13.Text.Replace(".", ",");
                    textBox14.Text = textBox14.Text.Replace(".", ",");
                    textBox15.Text = textBox15.Text.Replace(".", ",");
                    textBox16.Text = textBox16.Text.Replace(".", ",");
                    textBox17.Text = textBox17.Text.Replace(".", ",");
                    double x = Convert.ToDouble(textBox12.Text);
                    double y = Convert.ToDouble(textBox13.Text);
                    double x2 = Convert.ToDouble(textBox14.Text);
                    double y2 = Convert.ToDouble(textBox15.Text);
                    double x3 = Convert.ToDouble(textBox16.Text);
                    double y3 = Convert.ToDouble(textBox17.Text);
                }
                catch
                {
                    MessageBox.Show("Ошибка! Таких дробных значений не бывает", "Ошибка!");
                    return;
                }
            }
            double x4 = (pictureBox1.Image.Size.Width - Convert.ToDouble(textBox16.Text)) / (Convert.ToDouble(textBox15.Text) - Convert.ToDouble(textBox12.Text));
            double y4 = (pictureBox1.Image.Size.Height - Convert.ToDouble(textBox17.Text)) / (Convert.ToDouble(textBox14.Text) - Convert.ToDouble(textBox13.Text));
            textBox7.Text = x4.ToString();
            textBox6.Text = y4.ToString();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            Button5_Click(sender, e);
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "(*.xlsx)|*.xlsx";
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var ew = new ExcelWriter(saveFileDialog1.FileName))
                {
                    try
                    {
                        double x = Convert.ToDouble(textBox7.Text);
                        double y = Convert.ToDouble(textBox6.Text);
                        double x2 = Convert.ToDouble(textBox8.Text);
                        double y2 = Convert.ToDouble(textBox9.Text);
                    }
                    catch
                    {
                        try
                        {
                            char a = Convert.ToChar(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                            textBox6.Text = textBox6.Text.Replace(".", ",");
                            textBox7.Text = textBox7.Text.Replace(".", ",");
                            textBox8.Text = textBox8.Text.Replace(".", ",");
                            textBox9.Text = textBox9.Text.Replace(".", ",");
                            double x = Convert.ToDouble(textBox7.Text);
                            double y = Convert.ToDouble(textBox6.Text);
                            double x2 = Convert.ToDouble(textBox8.Text);
                            double y2 = Convert.ToDouble(textBox9.Text);
                        }
                        catch
                        {
                            MessageBox.Show("Ошибка! Таких дробных значений не бывает", "Ошибка!");
                            return;
                        }
                    }
                    for (var row = 1; row <= dataGridView4.Rows.Count; row++)
                    {
                        for (var col = 1; col <= dataGridView4.Columns.Count; col++)
                        {
                            double x = Convert.ToDouble(textBox7.Text);
                            double y = Convert.ToDouble(textBox6.Text);
                            double x2 = Convert.ToDouble(textBox8.Text);
                            double y2 = Convert.ToDouble(textBox9.Text);
                            if (dataGridView4.Rows[row - 1].Cells[col - 1].Value != null)
                            {
                                if (col % 2 == 1)
                                    ew.Write((Math.Round((Convert.ToDouble(dataGridView4.Rows[row - 1].Cells[col - 1].Value) - x2) / x, 2)).ToString(), col, row);
                                else
                                    ew.Write((Math.Round((y2 - Convert.ToDouble(dataGridView4.Rows[row - 1].Cells[col - 1].Value)) / y, 2)).ToString(), col, row);
                            }
                        }
                    }
                }
            }
        }

        private void TextBox13_TextChanged(object sender, EventArgs e)
        {
            TextBox12_TextChanged(sender, e);
        }

        private void TextBox14_TextChanged(object sender, EventArgs e)
        {
            TextBox12_TextChanged(sender, e);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
