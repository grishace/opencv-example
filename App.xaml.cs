using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using OpenCvSharp;

namespace opencvw
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public VideoCapture capture { get; private set; }
        public CascadeClassifier face { get; private set; }
        public CascadeClassifier smile { get; private set; }

        override protected void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            capture = new VideoCapture(0, VideoCaptureAPIs.DSHOW);
            face = new CascadeClassifier("haarcascade_frontalface_alt2.xml");
            smile = new CascadeClassifier("haarcascade_smile.xml");
        }

        override protected void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            if (capture != null) {
                capture.Release();
            }
        }
    }
}
