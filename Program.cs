using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.IO;

namespace desktoptocc
{
    class NamedColor
    {
        public string name;
        public Color color;

        public NamedColor(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            int tick = 0;

            List<NamedColor> colorMap = new List<NamedColor>();

            colorMap.Add(new NamedColor("0", Color.FromArgb(240, 240, 240)));
            colorMap.Add(new NamedColor("1", Color.FromArgb(242, 178, 51 )));
            colorMap.Add(new NamedColor("2", Color.FromArgb(229, 127, 216)));
            colorMap.Add(new NamedColor("3", Color.FromArgb(153, 178, 242)));
            colorMap.Add(new NamedColor("4", Color.FromArgb(222, 222, 108)));
            colorMap.Add(new NamedColor("5", Color.FromArgb(127, 204, 25 )));
            colorMap.Add(new NamedColor("6", Color.FromArgb(242, 178, 204)));
            colorMap.Add(new NamedColor("7", Color.FromArgb(76 , 76 , 76 )));
            colorMap.Add(new NamedColor("8", Color.FromArgb(153, 153, 153)));
            colorMap.Add(new NamedColor("9", Color.FromArgb(76 , 153, 178)));
            colorMap.Add(new NamedColor("a", Color.FromArgb(178, 102, 229)));
            colorMap.Add(new NamedColor("b", Color.FromArgb(51 , 102, 204)));
            colorMap.Add(new NamedColor("c", Color.FromArgb(127, 102, 76 )));
            colorMap.Add(new NamedColor("d", Color.FromArgb(87 , 166, 78 )));
            colorMap.Add(new NamedColor("e", Color.FromArgb(204, 76 , 76 )));
            colorMap.Add(new NamedColor("f", Color.FromArgb(17 , 17 , 17 )));

            

            int screenX = 0;
            int screenY = 0;
            int screenWidth = 1920;
            int screenHeight = 1080;

            int resizeWidth = 328;
            int resizeHeight = 162;

            int frameX = 164;
            int frameY = 81;
            int frameWidth = 164;
            int frameHeight = 81;

            int sleepFor = 1000/60;

            string outPutPath = "./frame.txt";

            while (true)
            {
                Bitmap ss = GetSreenshot(screenX, screenY, screenWidth, screenHeight);

                if (resizeWidth > 0 && resizeHeight > 0)
                {
                    ss = ResizeImage(ss, new Size(resizeWidth, resizeHeight));
                }
                
                string result = "";
                Console.Title = tick.ToString();
                int lastY = 0;
                for (int y = 0; y < frameHeight; y++)
                {
                    if (lastY != y)
                    {
                        result += "\n";
                        lastY = y;
                    }
                    for (int x = 0; x < frameWidth; x++)
                    {
                        NamedColor color = NearestColor(ss.GetPixel(frameX + x, frameY + y), colorMap);
                        result += color.name;
                    }
                }

                File.WriteAllText(outPutPath, result);

                if (sleepFor > 0) Thread.Sleep(sleepFor);

                tick++;
                if (tick > 20)
                {
                    tick = 0;
                    GC.Collect();
                };

            }

            Bitmap GetSreenshot(int x, int y, int w, int h)
            {
                Bitmap bm = new Bitmap(w, h);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(x, y, 0, 0, bm.Size);
                return bm;
            }

            Bitmap ResizeImage(Bitmap imgToResize, Size size)
            {
                try
                {
                    Bitmap b = new Bitmap(size.Width, size.Height);
                    using (Graphics g = Graphics.FromImage((Image)b))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
                        g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
                    }
                    return b;
                }
                catch
                {
                    Console.WriteLine("Bitmap could not be resized");
                    return imgToResize;
                }
            }


            NamedColor NearestColor(Color needle, List<NamedColor> colors) {
                NamedColor result = new NamedColor("TEMP", Color.Transparent);

                double distanceSq;
                double minDistanceSq = double.MaxValue;
                NamedColor currentColor;
                for (int i = 0; i < colors.Count; i++)
                {
                    currentColor = colors[i];

                    distanceSq = 
                        Math.Pow(needle.R - currentColor.color.R, 2) +
                        Math.Pow(needle.G - currentColor.color.G, 2) +
                        Math.Pow(needle.B - currentColor.color.B, 2);

                    if (distanceSq < minDistanceSq)
                    {
                        minDistanceSq = distanceSq;
                        result = currentColor;
                    }
                }

                return result;
            }

        }


    }
}
