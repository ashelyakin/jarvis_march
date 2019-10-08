using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace JarvisMarch
{
    public partial class MainForm : Form
    {
        private readonly Pen _hullPen; 
        private readonly Pen _pointPen;
        private readonly Color _backGr;
        private Graphics _graphics;

        private readonly LinkedList<Point> _points;

        private bool _isDrawn;

        public MainForm()
        {
            InitializeComponent();

            _hullPen = new Pen(Color.Red);
            _hullPen.Width = 4;
            _pointPen = new Pen(Color.Black);
            _pointPen.Width = 3;
            _backGr = Color.White;

            _points = new LinkedList<Point>();
            _isDrawn = false;
        }

        private void ClearPictureBox()
        {
            using (var brush = new SolidBrush(_backGr))
                _graphics.FillRectangle
                    (brush, 0, 0, pictureBox.Width, pictureBox.Height);
            pictureBox.Invalidate();
        }

        private void RedrawPoints()
        {
            ClearPictureBox();
            foreach (var p in _points)
                _graphics.DrawRectangle(_pointPen, p.X, p.Y, 1, 1);
            pictureBox.Invalidate();
        }

        private void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            var point = new Point(e.X, e.Y);
            if (_points.Contains(point))
                return;

            _points.AddLast(point);

            _graphics.DrawRectangle(_pointPen, point.X, point.Y, 1, 1);
            pictureBox.Invalidate();
        }

        private void MainForm_Shown(object sender, System.EventArgs e)
        {
            pictureBox.BackColor = Color.White;
            pictureBox.Image = new Bitmap(pictureBox.Width, pictureBox.Height);

            _graphics = Graphics.FromImage(pictureBox.Image);
        }

        private void clearButton_Click(object sender, System.EventArgs e)
        {
            ClearPictureBox();
            _points.Clear();
            _isDrawn = false;
        }

        private void computeButton_Click(object sender, System.EventArgs e)
        {
            if (_points.Count < 3)
                return;

            if (_isDrawn)
                RedrawPoints();
            else _isDrawn = true;

            var hull = JarvisMarchLib.JarvisMarch.Calculate(_points);

            var node = hull.First;
            while (node.Next != null)
            {
                _graphics.DrawLine(_hullPen, node.Value, node.Next.Value);
                node = node.Next;
            }
            _graphics.DrawLine(_hullPen, node.Value, hull.First.Value);
            pictureBox.Invalidate();
        }
    }
}
