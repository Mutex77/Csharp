using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Threading;
using MjpegProcessor;
using System.Net.Sockets;

namespace RoverControllerWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MjpegDecoder m_mjpeg;
        TcpClient tcp;
        NetworkStream nStream;
        String receivedMessage;
        byte[] readBuffer;
        TouchDevice tdRightSlider;
        TouchDevice tdLeftSlider;
        Point ptRightLast;
        Point ptLeftLast;
        double sliderHome;
        CancellationTokenSource tokensource;
        CancellationToken ct;
        TaskFactory tf;
        Task coms;

        public MainWindow()
        {
            InitializeComponent();
            m_mjpeg = new MjpegDecoder();
            m_mjpeg.FrameReady += mjpeg_FrameReady;
            receivedMessage = "";
            readBuffer = new byte[256];
            tcp = new TcpClient();
            tf = new TaskFactory();

        }

        #region MotionJPEG
        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            imgStream.Source = e.BitmapImage;
            imgStream.InvalidateVisual();
        }
        #endregion

        #region Track and Slider methods
        private void rightSlider_TouchDown(object sender, TouchEventArgs e)
        {
            e.TouchDevice.Capture(rightSlider);

            if(tdRightSlider == null)
            {
                tdRightSlider = e.TouchDevice;
                ptRightLast = tdRightSlider.GetTouchPoint(imgRightTrack).Position;
                Canvas.SetTop(rightSlider, Canvas.GetTop(imgRightTrack) + (ptRightLast.Y - (rightSlider.ActualHeight/2)));
            }

            e.Handled = true;
        }

        private void rightSlider_TouchMove(object sender, TouchEventArgs e)
        {
            if(e.TouchDevice == tdRightSlider)
            {
                Point currentTouchPoint = tdRightSlider.GetTouchPoint(imgRightTrack).Position;
                if (currentTouchPoint.Y > 0 && currentTouchPoint.Y < imgRightTrack.ActualHeight)
                {
                    double deltaY = currentTouchPoint.Y - ptRightLast.Y;

                    double newTop = Canvas.GetTop(rightSlider) + deltaY;

                    Canvas.SetTop(rightSlider, newTop);

                    ptRightLast = currentTouchPoint;
                }
                e.Handled = true;
            }
        }

        private void rightSlider_TouchUp(object sender, TouchEventArgs e)
        {
            if(e.TouchDevice == tdRightSlider)
            {
                tdRightSlider = null;
            }
            RightSliderReturnHome();
            e.Handled = true;
        }

        private void leftSlider_TouchDown(object sender, TouchEventArgs e)
        {
            e.TouchDevice.Capture(leftSlider);

            if (tdLeftSlider == null)
            {
                tdLeftSlider = e.TouchDevice;
                ptLeftLast = tdLeftSlider.GetTouchPoint(imgLeftTrack).Position;
                Canvas.SetTop(leftSlider, Canvas.GetTop(imgLeftTrack) + (ptLeftLast.Y - (leftSlider.ActualHeight/2)));
            }

            e.Handled = true;
        }

        private void leftSlider_TouchMove(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == tdLeftSlider)
            {
                Point currentTouchPoint = tdLeftSlider.GetTouchPoint(imgLeftTrack).Position;
                if (currentTouchPoint.Y > 0 && currentTouchPoint.Y < imgLeftTrack.ActualHeight)
                {
                    double deltaY = currentTouchPoint.Y - ptLeftLast.Y;

                    double newTop = Canvas.GetTop(leftSlider) + deltaY;
                    
                    Canvas.SetTop(leftSlider, newTop);

                    ptLeftLast = currentTouchPoint;
                }
                e.Handled = true;
            }
        }

        private void leftSlider_TouchUp(object sender, TouchEventArgs e)
        {
            if (e.TouchDevice == tdLeftSlider)
            {
                tdLeftSlider = null;
            }

            LeftSliderReturnHome();

            e.Handled = true;
        }

        private void RightSliderReturnHome()
        {
            sliderHome = (canvasRight.ActualHeight / 2) - (rightSlider.ActualHeight / 2);

            Canvas.SetTop(rightSlider, sliderHome);
        }

        private void LeftSliderReturnHome()
        {
            sliderHome = (canvasLeft.ActualHeight / 2) - (leftSlider.ActualHeight / 2);

            Canvas.SetTop(leftSlider, sliderHome);
        }

        private void CenterTracks()
        {
            double center = (canvasLeft.ActualHeight / 2) - (imgLeftTrack.ActualHeight / 2);
            Canvas.SetTop(imgLeftTrack, center);
            Canvas.SetTop(imgRightTrack, center);
        }

        private void RoverControllerWindow_ContentRendered(object sender, EventArgs e)
        {
            LeftSliderReturnHome();
            RightSliderReturnHome();
            CenterTracks();
        }

        private void RoverControllerWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LeftSliderReturnHome();
            RightSliderReturnHome();
            CenterTracks();
        }
        #endregion

        #region Button Events
        

        private void btnTCPConnect_Click(object sender, TouchEventArgs e)
        {
            try
            {
                tcp.Connect("192.168.42.1", 8081);
                nStream = tcp.GetStream();
                btnTCPConnect.Background = Brushes.Lime;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
            }
        }

        private void btnMotors_Click(object sender, TouchEventArgs e)
        {
            if(btnMotors.Background == Brushes.Lime)
            {
                tokensource.Cancel();

                try
                {
                    coms.Wait();
                }
                catch(TaskCanceledException ex)
                {
                    Debug.WriteLine("Task cancelled exception: {0}", ex.Task.Id);
                }
                finally
                {
                    tokensource.Dispose();
                }

                btnMotors.Background = Brushes.Gainsboro;
            }
            else
            {
                tokensource = new CancellationTokenSource();
                ct = tokensource.Token;
                coms = tf.StartNew(() => ControlComs(ct), ct);
                btnMotors.Background = Brushes.Lime;
            }
        }

        private void btnIRLED_Click(object sender, TouchEventArgs e)
        {
            if (btnIRLED.Background == Brushes.Lime)
            {
                BeginWriting("IRLEDOff");
                btnIRLED.Background = Brushes.Gainsboro;
            }
            else
            {
                BeginWriting("IRLEDOn");
                btnIRLED.Background = Brushes.Lime;
            }
        }
        
        private void btnLED_Click(object sender, TouchEventArgs e)
        {
            if (btnLED.Background == Brushes.Lime)
            {
                BeginWriting("LEDOff");
                btnLED.Background = Brushes.Gainsboro;
            }
            else
            {
                BeginWriting("LEDOn");
                btnLED.Background = Brushes.Lime;
            }
        }
        
        private void btnVideo_Click(object sender, TouchEventArgs e)
        {
            if (btnVideo.Background == Brushes.Lime)
            {
                m_mjpeg.StopStream();
                btnVideo.Background = Brushes.Gainsboro;
            }
            else
            {
                m_mjpeg.ParseStream(new Uri("http://192.168.42.1:8080/stream/video.mjpeg"));
                btnVideo.Background = Brushes.Lime;
            }
        }
        
        private void btnShutdown_Click(object sender, TouchEventArgs e)
        {
            BeginWriting("Shutdown");
        }
        #endregion

        #region Motor Communications Task
        private void ControlComs(CancellationToken cancelToken)
        {
            while (!cancelToken.IsCancellationRequested)
            {
                this.Dispatcher.Invoke((Action)(() => 
                {
                    double leftZero;
                    double rightZero;
                    double leftThrottle;
                    double rightThrottle;

                    leftZero = Canvas.GetTop(imgLeftTrack) + (imgLeftTrack.ActualHeight / 2);
                    rightZero = Canvas.GetTop(imgRightTrack) + (imgRightTrack.ActualHeight / 2);

                    leftThrottle = leftZero - Canvas.GetTop(leftSlider) - (leftSlider.ActualHeight / 2);
                    rightThrottle = rightZero - Canvas.GetTop(rightSlider) - (rightSlider.ActualHeight / 2);

                    if (leftThrottle > 270)
                        leftThrottle = 270;
                    else if (leftThrottle < -270)
                        leftThrottle = -270;

                    if (rightThrottle > 270)
                        rightThrottle = 270;
                    else if (rightThrottle < -270)
                        rightThrottle = -270;
                    
                    Debug.WriteLine(leftThrottle + ", " + rightThrottle);
                    BeginWriting("@" + (int)leftThrottle + "@" + (int)rightThrottle);
                }));
                
                Thread.Sleep(150);
            }
        }
        #endregion

        #region READ/WRITE
        public void BeginReading()
        {
            if (nStream.CanRead)
            {
                nStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(EndReading), nStream);
            }
            else
            {
                Debug.WriteLine("Cannot read from this network stream.");
            }
        }

        public void EndReading(IAsyncResult ar)
        {
            int numBytesRead = nStream.EndRead(ar);

            if (numBytesRead != 0)
                receivedMessage = string.Concat(receivedMessage, Encoding.ASCII.GetString(readBuffer, 0, numBytesRead));

            if (nStream.DataAvailable)
            {
                nStream.BeginRead(readBuffer, 0, readBuffer.Length, new AsyncCallback(EndReading), nStream);
                return;
            }

            Debug.WriteLine(receivedMessage);
            receivedMessage = "";
        }

        public void BeginWriting(string str)
        {
            try
            {
                if (nStream.CanWrite)
                {
                    byte[] writeBuffer;
                    writeBuffer = Encoding.ASCII.GetBytes(str + "#");
                    nStream.BeginWrite(writeBuffer, 0, writeBuffer.Length, new AsyncCallback(EndWriting), nStream);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
            }
        }

        public void EndWriting(IAsyncResult ar)
        {
            nStream.EndWrite(ar);
        }
        #endregion
    }
}
