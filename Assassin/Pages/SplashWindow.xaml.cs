using System;
using System.Windows;
using System.Windows.Threading;

namespace Assassin.Pages
{
    /// <summary>Interaction logic for SplashPage.xaml</summary>
    public partial class SplashWindow
    {
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private TimeSpan _startTime = new TimeSpan(0, 0, 0);
        private readonly TimeSpan _endTime = new TimeSpan(0, 0, 0, 0, 300);
        private readonly TimeSpan _tickTime = new TimeSpan(0, 0, 0, 0, 300);

        public SplashWindow(Window owner)
        {
            Owner = owner;
            InitializeComponent();
            _timer.Tick += Timer_Tick;
            _timer.Interval = _tickTime;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs eventArgs)
        {
            if (_startTime < _endTime)
                _startTime += _tickTime;
            else
            {
                _timer.Stop();
                Close();
            }
        }
    }
}