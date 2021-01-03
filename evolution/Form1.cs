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
        public Form1()
        {
            Width = 800;
            Height = 800;
            car = new Car(new Point(Width/2, Height/2));
            InitPaint();
            InitControl();
            InitializeComponent();
            var timer = new Timer();
            timer.Interval = 4;
            timer.Tick += (x, e) => { 
                Invalidate(); 
                car.Update();
            };
            timer.Start();
        }

        public void InitPaint()
        { 
            Paint += (x, e) =>
            {
                var img = Image.FromFile(@"C:\Users\Саня\source\repos\evolution\evolution\car.png");
                e.Graphics.DrawString(car.angle.ToString(), new Font("Arial", 16), Brushes.Black, 0, 0);
                e.Graphics.TranslateTransform(car.Pos.X, car.Pos.Y);
                e.Graphics.RotateTransform((float)car.angle * 57);
                e.Graphics.DrawImage(img, -25, -15);               
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
                            car.Rotate(true);
                        if (key == Keys.D && car.vel != 0)
                            car.Rotate(false);
                    }
                }
            };
            timer.Start();
        }

        public class Car
        {
            private float maxVelocity;
            public PointF Pos;
            public float vel;
            public double angle;
            private Point accel;

            public Car(PointF pos)
            {
                this.Pos = pos;
                accel = new Point();
                vel = 0;
                angle = 0;
                maxVelocity = 7;
            }

            public void Update()
            {
                Pos.X += (float)Math.Cos(angle) * vel;
                Pos.Y += (float)Math.Sin(angle) * vel;
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

            public void Rotate(bool left)
            {
                angle += left ? -5f / 57 : 5f / 57;
                angle %= 2 * Math.PI;
            }

            public void Break()
            {
                if (vel > 0)
                    vel -= 0.5f;
                else if (vel < 0)
                    vel += 0.5f;
            }
        }

        public static class Geometry
        {
            public static void IsCollided()
            {

            }
        }

        public class Checkpoint
        {
            Point pos;
        }
    }
}
