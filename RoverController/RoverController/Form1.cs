using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using MjpegProcessor;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;

namespace RoverController
{
    public partial class Form1 : Form
    {
        MjpegDecoder m_mjpeg;
        TcpClient tcp;
        NetworkStream nStream;
        String receivedMessage;
        byte[] readBuffer;
        Boolean rightSliderMoving;
        Boolean leftSliderMoving;
        int rightX;
        int rightY;
        int leftX;
        int leftY;
        TaskFactory taskFac;
        
        public Form1()
        {
            InitializeComponent();
            rightSliderMoving = false;
            leftSliderMoving = false;
            m_mjpeg = new MjpegDecoder();
            m_mjpeg.FrameReady += mjpeg_FrameReady;
            receivedMessage = "";
            readBuffer = new byte[256];
            tcp = new TcpClient();
            taskFac = new TaskFactory();

            pbRightSlider.MouseDown += new MouseEventHandler(PbRightSlider_MouseDown);
            pbRightSlider.MouseMove += new MouseEventHandler(PbRightSlider_MouseMove);
            pbRightSlider.MouseUp += new MouseEventHandler(PbRightSlider_MouseUp);
            pbLeftSlider.MouseDown += new MouseEventHandler(PbLeftSlider_MouseDown);
            pbLeftSlider.MouseMove += new MouseEventHandler(PbLeftSlider_MouseMove);
            pbLeftSlider.MouseUp += new MouseEventHandler(PbLeftSlider_MouseUp);
            

        }

        private void PbRightSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                rightX = e.X;
                rightY = e.Y;
                rightSliderMoving = true;
            }
        }

        private void PbRightSlider_MouseMove(object sender, MouseEventArgs e)
        {
            taskFac.StartNew(() =>
            {
                if (rightSliderMoving)
                {
                    PictureBox thisPB = (PictureBox)sender;
                    thisPB.Location = new Point(thisPB.Location.X, thisPB.Location.Y - (rightY - e.Y));
                }
            });
        }

        private void PbRightSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if(rightSliderMoving)
            {
                rightSliderMoving = false;
            }
        }

        private void PbLeftSlider_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                leftX = e.X;
                leftY = e.Y;
                leftSliderMoving = true;
            }
        }

        private void PbLeftSlider_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftSliderMoving)
            {
                PictureBox thisPB = (PictureBox)sender;
                thisPB.Location = new Point(thisPB.Location.X, thisPB.Location.Y - (leftY - e.Y));
            }
        }

        private void PbLeftSlider_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftSliderMoving)
            {
                leftSliderMoving = false;
            }
        }

        #region MJPEG
        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            pbStream.Image = e.Bitmap;
        }
        #endregion

        #region Buttons/Messages
        private void btnTCPTest_Click(object sender, EventArgs e)
        {
            try
            {
                tcp.Connect("192.168.0.7", 8081);
                nStream = tcp.GetStream();
                TaskFactory tf = new TaskFactory();
                tf.StartNew(() => ControlComs());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
            }
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            BeginWriting("Shutdown");
            //BeginReading(); I want to read (synchronously?) to verify that the pi got the message and is shutting down
        }

        private void btnLEDToggle_Click(object sender, EventArgs e)
        {
            if (btnLEDToggle.BackColor == Color.Lime)
            {
                BeginWriting("LEDOff");
                btnLEDToggle.BackColor = Color.Gainsboro;
            }
            else
            {
                BeginWriting("LEDOn");
                btnLEDToggle.BackColor = Color.Lime;
            }
        }

        private void btnIRLEDToggle_Click(object sender, EventArgs e)
        {
            if(btnIRLEDToggle.BackColor == Color.Lime)
            {
                BeginWriting("IRLEDOff");
                btnIRLEDToggle.BackColor = Color.Gainsboro;
            } else
            {
                BeginWriting("IRLEDOn");
                btnIRLEDToggle.BackColor = Color.Lime;
            }
        }

        private void btnVideoToggle_Click(object sender, EventArgs e)
        {
            if (btnVideoToggle.BackColor == Color.Lime)
            {
                m_mjpeg.StopStream();
                btnVideoToggle.BackColor = Color.Gainsboro;
            } else
            {
                m_mjpeg.ParseStream(new Uri("http://192.168.0.7:8080/stream/video.mjpeg"));
                btnVideoToggle.BackColor = Color.Lime;
            }
        }

        private void ControlComs()
        {
            while(true)
            {
                BeginWriting("Testing.");
                Thread.Sleep(2000);
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
            
            if(nStream.DataAvailable)
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
                    writeBuffer = Encoding.ASCII.GetBytes(str);
                    nStream.BeginWrite(writeBuffer, 0, writeBuffer.Length, new AsyncCallback(EndWriting), nStream);
                }
            } catch (Exception ex)
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
