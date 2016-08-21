using System;
using System.Threading;
using System.Windows;

namespace AlarmClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void updateTimerLabel(string t);

        private Thread threadTimer;
        System.Media.SoundPlayer player;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            string hours, minutes, seconds;
            
            if (threadTimer == null || !threadTimer.IsAlive)
            {
                hours = comboBox_Hours.Text;
                minutes = comboBox_Minutes.Text;
                seconds = comboBox_Seconds.Text;

                if (player != null)
                    player.Stop();
                
                threadTimer = new Thread(new ThreadStart(() => StartTimer(hours, minutes, seconds)));
                threadTimer.Start();
                while (!threadTimer.IsAlive);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            if (threadTimer != null && threadTimer.IsAlive)
            {
                threadTimer.Abort();
                threadTimer.Join();
            }
            label_Timer.Content = "00:00:00:0";

            if (player != null)
                player.Stop();
        }

        private void StartTimer(string hr, string min, string sec)
        {
            int hours, minutes, seconds;
            DateTime start, end;
            TimeSpan delta;
            
            hours = int.Parse(hr);
            minutes = int.Parse(min);
            seconds = int.Parse(sec);
            start = DateTime.Now;
            end = start.Add(new TimeSpan(hours, minutes, seconds));
            
            while(DateTime.Compare(DateTime.Now, end) < 0)
            {
                Thread.Sleep(100);
                delta = end - DateTime.Now;
                label_Timer.Dispatcher.Invoke(
                    new updateTimerLabel(this.UpdateTimer),
                    new object[] { delta.ToString(@"hh\:mm\:ss\:f") }
                    );
            }

            label_Timer.Dispatcher.Invoke(
                    new updateTimerLabel(this.UpdateTimer),
                    new object[] { "00:00:00:0" }
                    );
            playSound(@"Siren.wav");
        }

        private void UpdateTimer(string t)
        {
            if(threadTimer.IsAlive)
                label_Timer.Content = t;
        }

        private void playSound(string path)
        {
            player = new System.Media.SoundPlayer();
            player.SoundLocation = path;
            player.Load();
            player.Play();
        }
        
    }
}
