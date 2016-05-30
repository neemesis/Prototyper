using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Prototyper {
    public partial class Form1 : Form {
        Image img;
        bool pictureBoxIsClicked;
        private Point MouseDownLocation;
        int mouseResizePosition; // 12 - top, 3 - right, 6 - bottom, 9 - left
        bool mouseClickedForResize;
        int pictureBoxSizeWidth;
        int pictureBoxSizeHeight;
        int zoomPanel = 1;


        public Form1() {
            InitializeComponent();
            pictureBoxIsClicked = false;
            mouseClickedForResize = false;
            WindowState = FormWindowState.Maximized;
        }

        private void btnLoad_Click(object sender, EventArgs e) {
            if (openFile.ShowDialog() == DialogResult.OK) {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = Image.FromFile(openFile.FileName);
                pictureBox.Height = (int) ((500.0 / pictureBox.Image.Width) * pictureBox.Image.Height * 1.0);
                pictureBox.Width = 500;
                
                pictureBox.Top = 20;
                pictureBox.Left = (panel.Width - 500) / 2;

                pictureBox.MouseDown += pictureBox_MouseDown;
                pictureBox.MouseUp += pictureBox_MouseUp;
                pictureBox.MouseMove += pictureBox_MouseMove;

                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox.ContextMenuStrip = imageRightClickMenu;

                panel.Controls.Add(pictureBox);
                pictureBox.BringToFront();
            }
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                MouseDownLocation = e.Location;
                pictureBoxSizeWidth = (sender as PictureBox).Width;
                pictureBoxSizeHeight = (sender as PictureBox).Height;
                if (mouseResizePosition == 0) {
                    pictureBoxIsClicked = true;
                    mouseClickedForResize = false;
                } else {
                    pictureBoxIsClicked = false;
                    mouseClickedForResize = true;
                }
            }
        }

        private void pictureBox_MouseUp(object sender, MouseEventArgs e) {
            MouseDownLocation = Point.Empty;
            pictureBoxIsClicked = false;
            mouseClickedForResize = false;
        }

        private void pictureBox_MouseMove(object sender, MouseEventArgs e) {
            PictureBox pictureBox = sender as PictureBox;
            checkResizer(pictureBox, e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                if (pictureBoxIsClicked) {
                    pictureBox.Left = e.X + pictureBox.Left - MouseDownLocation.X;
                    pictureBox.Top = e.Y + pictureBox.Top - MouseDownLocation.Y;
                } else if (mouseClickedForResize) {
                    int w = e.X - MouseDownLocation.X + pictureBoxSizeWidth;
                    if (w > 8)
                        pictureBox.Width = w;
                    int h = e.Y - MouseDownLocation.Y + pictureBoxSizeHeight;
                    if (h > 8)
                        pictureBox.Height = h;
                }

            }
        }

        private void checkResizer(PictureBox pb, MouseEventArgs e) {
            if (mouseClickedForResize || pictureBoxIsClicked)
                return;

            int left = pb.Left;
            int right = pb.Right;
            int top = pb.Top;
            int bottom = pb.Bottom;
            int x = e.X;
            int y = e.Y;

            if (x > right - left - 8 && y > bottom - top - 8) {
                Cursor.Current = Cursors.SizeNWSE;
                mouseResizePosition = 4;
            }
            /*
            if (x < 4) {
                mouseResizePosition = 9;
                Cursor.Current = Cursors.SizeWE;
            } else if (x > right - left - 3) {
                mouseResizePosition = 3;
                Cursor.Current = Cursors.SizeWE;
            } else if (y < 4) {
                mouseResizePosition = 12;
                Cursor.Current = Cursors.SizeNS;
            } else if (y > bottom - top - 3) {
                mouseResizePosition = 6;
                Cursor.Current = Cursors.SizeNS;
            } */else {
                mouseResizePosition = 0;
                Cursor.Current = Cursors.Default;
            }
        }

        private bool checkBor(int a, int b) {
            Debug.WriteLine(a + " : " + b);
            if (Math.Abs(a - b) < 4) {
                return true;
            }
            return false;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e) {
            PictureBox pb = (((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl as PictureBox);
            pb.Dispose();
        }

        private void bringToTopToolStripMenuItem_Click(object sender, EventArgs e) {
            PictureBox pb = (((sender as ToolStripItem).Owner as ContextMenuStrip).SourceControl as PictureBox);
            pb.BringToFront();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (Control c in panel.Controls) {
                c.Scale(new SizeF(1.2f, 1.2f));
            }
        }

        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e) {
            foreach (Control c in panel.Controls) {
                c.Scale(new SizeF(0.8f, 0.8f));
            }
        }
    }
}
