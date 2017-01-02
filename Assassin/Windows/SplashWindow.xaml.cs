using System;
using System.Windows;
using System.Windows.Threading;

namespace Assassin
{
    /// <summary>Interaction logic for SplashWindow.xaml</summary>
    public partial class SplashWindow
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private TimeSpan _startTime;
        private readonly TimeSpan _endTime = new TimeSpan(0, 0, 2);
        private readonly TimeSpan _oneSecond = new TimeSpan(0, 0, 1);

        public SplashWindow(Window owner)
        {
            Owner = owner;
            InitializeComponent();
            _timer.Tick += timer_Tick;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
        }

        private void timer_Tick(object sender, EventArgs eventArgs)
        {
            if (_startTime < _endTime)
                _startTime += _oneSecond;
            else
            {
                _timer.Stop();
                this.Close();
            }
        }
    }
}