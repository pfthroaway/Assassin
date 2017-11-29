using Assassin.Classes;
using Assassin.Pages.Admin;
using Assassin.Pages.Help;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Assassin.Pages
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        #region ScaleValue Depdency Property

        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            MainWindow mainPage = o as MainWindow;
            return mainPage?.OnCoerceScaleValue((double)value) ?? value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainWindow = o as MainWindow;
            mainWindow?.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
                return 1.0f;

            return Math.Max(0.1, value);
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {
        }

        public double ScaleValue
        {
            get => (double)GetValue(ScaleValueProperty);
            set => SetValue(ScaleValueProperty, value);
        }

        #endregion ScaleValue Depdency Property

        /// <summary>Calculates the scale for the Page.</summary>
        internal void CalculateScale()
        {
            double yScale = ActualHeight / GameState.CurrentPageHeight;
            double xScale = ActualWidth / GameState.CurrentPageWidth;
            double value = Math.Min(xScale, yScale) * 0.8;
            if (value > 3)
                value = 3;
            else if (value < 1)
                value = 1;
            ScaleValue = (double)OnCoerceScaleValue(WindowMain, value);
        }

        #region Click Methods

        private void MnuFileExit_Click(object sender, RoutedEventArgs e) => GameState.GoBack();

        private void MnuAdmin_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new AdminLoginPage());

        private void MnuHelpManual_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new ManualPage());

        private void MnuHelpAbout_Click(object sender, RoutedEventArgs e) => GameState.Navigate(new AboutPage());

        #endregion Click Methods

        #region Window-Manipulation Methods

        public MainWindow() => InitializeComponent();

        private void MainFrame_OnSizeChanged(object sender, SizeChangedEventArgs e) => CalculateScale();

        private async void WindowMain_Loaded(object sender, RoutedEventArgs e)
        {
            GameState.MainWindow = this;
            new SplashWindow(this).Show();
            await Task.Factory.StartNew(GameState.LoadAll);
        }

        #endregion Window-Manipulation Methods
    }
}