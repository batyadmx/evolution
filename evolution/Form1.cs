using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace evolution
{
    public partial class Form1 : Form
    {
        Car car;
        Checkpoint checkpoint;
        public Form1()
        {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            checkpoint = new Checkpoint(new Point(Width / 2, 50), 40, 40);
            car = new Car(new Point(Width / 2, Height - 100), 50, 30);
            InitPaint();
            InitControl();
            var timer = new Timer();
            timer.Interval = 4;
            timer.Tick += (x, e) => { 
                Invalidate(); 
                car.Update(Width, Height);
                if (car.IsDead)
                   timer.Stop();
            };
            timer.Start();
        }

        public void InitPaint()
        { 
            Paint += (x, e) =>
            {
                var cpImg = Image.FromFile(Environment.CurrentDirectory + @"\checkpoint.png");
                e.Graphics.DrawString(Width + " " + Height, new Font("Aerial", 16), Brushes.Black, 15, 15);
                e.Graphics.DrawImage(cpImg, checkpoint.Pos.X - checkpoint.Width / 2, checkpoint.Pos.Y - checkpoint.Height / 2);
                var carimg = Image.FromFile(Environment.CurrentDirectory + @"\car.png");              
                e.Graphics.TranslateTransform(car.Origin.X, car.Origin.Y);              
                e.Graphics.RotateTransform((float)car.angle * 57);            
                e.Graphics.DrawImage(carimg, -25, -15);               
            };
        }

        public void InitControl()
        {
            var pressed = false;
            var keysPressed = new List<Keys>();
            KeyDown += (x, e) => {
                if (!keysPressed.Contains(e.KeyData))
                    keysPressed.Add(e.KeyData);
            };
            KeyUp += (x, e) => keysPressed.Remove(e.KeyData);

            var timer = new Timer();
            timer.Interval = 16;
            timer.Tick += (x, e) =>
            {
                if (!keysPressed.Contains(Keys.W) || !keysPressed.Contains(Keys.S))
                    car.Break();
                pressed = keysPressed.Count != 0;
                if (pressed)
                {
                    foreach (var key in keysPressed)
                    {
                        if (key == Keys.W)
                            car.AddVel(false);
                        if (key == Keys.S)
                            car.AddVel(true);
                        if (key == Keys.A && car.vel != 0)
                            car.Rotate(true, car.vel < 0);
                        if (key == Keys.D && car.vel != 0)
                            car.Rotate(false, car.vel < 0);
                    }
                }
            };
            timer.Start();
        }

        public class Car
        {
            public bool IsDead;
            private float maxVelocity;
            public PointF Origin;
            public float vel;
            public double angle;
            public int Length;
            public int Width;

            public Car(PointF pos, int width, int length)
            {
                this.Origin = pos;
                vel = 0;
                angle = -Math.PI / 2;
                maxVelocity = 7;
                Width = width;
                Length = length;
            }

            public void Update(int w, int h)
            {
                Origin.X += (float)Math.Cos(angle) * vel;
                Origin.Y += (float)Math.Sin(angle) * vel;
                IsInside(w, h);
            }

            public void AddVel(bool back)
            {
                if (vel <= maxVelocity && !back)
                {
                    vel += 1f;
                }
                else if (vel >= -maxVelocity && back)
                {
                    vel -= 1f;
                }
            }

            public void Rotate(bool left, bool back)
            {
                if (back)
                {
                    angle += left ? 5f / 57 : -5f / 57;
                }
                else
                    angle += left ? -5f / 57 : 5f / 57;
                angle %= 2 * Math.PI;
            }

            public void Break()
            {
                if (vel > 0)
                    vel -= 0.25f;
                else if (vel < 0)
                    vel += 0.25f;
            }

            public PointF[] GetVertexes()
            {
                var res = new List<PointF>();
                var temp = new int[] { -1, 1 };
                foreach(var el in temp)
                {
                    foreach(var el2 in temp)
                    {
                        var x1 = (Width / 2 - 4) * el * Math.Cos(angle) + (Length / 2 - 4) * el2 * -Math.Sin(angle);
                        var y1 = (Width / 2 - 4) * el * Math.Sin(angle) + (Length / 2 - 4) * el2 * Math.Cos(angle);
                        res.Add(new PointF((float)x1 + Origin.X, (float)y1 + Origin.Y));
                    }
                }
                return res.ToArray();
            }

            public void IsInside(int w, int h)
            {
                foreach (var p in GetVertexes())
                    if (!Geometry.IsInside(p, w, h))
                        IsDead = true;
            }
        }

        public static class Geometry
        {
            public static void ColDet()
            {

            }

            public static bool IsInside(PointF p, int width, int height)
            {
                return p.X >= 0 && p.X < width && p.Y >= 0 && p.Y < height;
            }
        }

        public class Checkpoint
        {
            public Point Pos;
            public int Width;
            public int Height;

            public Checkpoint(Point pos, int width, int height)
            {
                Pos = pos;
                Width = width;
                Height = height;
            }
        }
    }
}
