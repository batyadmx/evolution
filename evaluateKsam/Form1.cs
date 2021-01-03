using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace evaluateKsam
{
    public class Border
    {
        public Image BorderPic;
        public int X;
        public int Y;
        public int SizeX;
        public int SizeY;
        private readonly string _path;
        
        public Border(int x, int y)
        {
            _path = AppDomain.CurrentDomain.BaseDirectory;
            BorderPic = new Bitmap(_path + "\\sprites2\\border.png");
            X = x;
            Y = y;
            SizeX = 50;
            SizeY = 800;
        }
    }
    
    public class Car
    {
        public Image CarPic;
        public float X;
        public float Y;
        public int Size;
        public float Speed;
        private readonly string _path;
        
        public Car(int x, int y)
        {
            _path = AppDomain.CurrentDomain.BaseDirectory;
            CarPic = new Bitmap(_path + "\\sprites2\\car.png");
            X = x;
            Y = y;
            Size = 50;
            Speed = 0.01f;
        }

        public AsDemo()
        {
            if (X => )
        }
    }
    
    public partial class Form1 : Form
    {
        private Car car;
        private Border border1;
        private Border border2;
        private float speed;
        private float angle;

        public Form1()
        {
            FormBorderStyle = FormBorderStyle.Fixed3D;
            MaximizeBox = false;
            InitializeComponent();
            Init();
            Invalidate();
        }

        public void Init()
        {
            car = new Car(400, 750);
            border1 = new Border(250, 0);
            border2 = new Border(550, 0);

            speed = 0;
            angle = 0.0f;
            var angleVal = 0.0f;
            var rand = new Random();
            
            var timer1 = new Timer();
            timer1.Interval = 10;
            timer1.Start();
            timer1.Tick += (sender, e) =>
            {
                speed += car.Speed;
                var angleRad = (Math.PI / 180) * angle;
                var g = Math.Sqrt(speed * speed + speed * speed);
                car.Y -= (float)(Math.Cos(angleRad) * g);
                car.X += (float)(Math.Sin(angleRad) * g);
                angle += angleVal;
                var k = rand.Next(0, 3);
                switch (k)
                {
                    case 0:
                        angleVal += 0.1f;
                        break;
                    case 1:
                        angleVal += -0.1f;
                        break;
                }

                Invalidate();
            };
            Paint += (sender, args) =>
            {
                args.Graphics.TranslateTransform(car.X, car.Y);
                args.Graphics.RotateTransform(angle);
                args.Graphics.TranslateTransform(-car.X, -car.Y);
                args.Graphics.DrawImage(car.CarPic, car.X, car.Y, car.Size, car.Size);
            };
        }
        
        private void OnPaint(object sender, PaintEventArgs e)
        {
            
            e.Graphics.DrawImage(border1.BorderPic, border1.X, border1.Y, border1.SizeX, border1.SizeY);
            e.Graphics.DrawImage(border2.BorderPic, border2.X, border2.Y, border2.SizeX, border2.SizeY);
            
        }
    }
}