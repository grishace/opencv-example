using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.IO;

namespace opencvw
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(40);
            timer.Tick += (s, e) => Tick();
            timer.Start();
        }

        private BitmapImage BitmapToImageSource(System.Drawing.Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.DecodePixelWidth = (int) img.Width;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private void Tick ()
        {
            var app = Application.Current as App;
            using (var frame = new OpenCvSharp.Mat())
            {
                var success = app.capture.Read(frame);
                if (success)
                {
                    var f = app.face.DetectMultiScale(frame);
                    foreach (var r in f) {
                        OpenCvSharp.Cv2.Rectangle(frame, r, OpenCvSharp.Scalar.Green, 3);
                    }

                    if (f.Length > 0)
                    {
                        var s = app.smile.DetectMultiScale(frame, 1.9, 30, OpenCvSharp.HaarDetectionType.FindBiggestObject);
                        foreach (var r in s) {
                            OpenCvSharp.Cv2.Rectangle(frame, r, OpenCvSharp.Scalar.Red, 3);
                        }
                    }

                    this.Background = f.Length > 0 ? Brushes.White : Brushes.Green;

                    using (var bmp = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(frame))
                    {
                        img.Source = BitmapToImageSource(bmp);
                    }
                }
                else
                {
                    this.Background = Brushes.Red;
                }
            }
        }
    }
}
