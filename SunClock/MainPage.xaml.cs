/*using Astro;

namespace SunClock
{
    public partial class MainPage : ContentPage
    {
        private Location? location;
        private Observer observer;
        private TimeSpan _timeAdjustment;
        private bool isExact = true;
        private bool isTimeReady = false;

        public MainPage()
        {
            InitializeComponent();
            GetGeolocation();
        }

        private async void GetGeolocation()
        {
            ClockLabel.Text = "Получение геоданных...";
            location = await new GeolocationService().GetCurrentLocationAsync();
            if (location != null)
            {
                observer = new Observer(location.Latitude, location.Longitude, location.Altitude ?? 0.0);
                InitializeTimeAdjustment();
            }
            else
            {
                ClockLabel.Text = "Не удалось получить местоположение";
                //_timeAdjustment = TimeSpan.Zero;
                return;
            }
        }

        private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (ExactTimeRadioButton.IsChecked)
            {
                isExact = true;
            }
            else if (AverageTimeRadioButton.IsChecked)
            {
                isExact = false; 
            }

            if (isTimeReady)
            {
                UpdateClock();
            }
        }

        private void InitializeTimeAdjustment()
        {
            DateTime checkDate = DateTime.Today;
            var utcSpan = DateTime.Now - DateTime.UtcNow;
            var sunNoon1 = Astronomy.SearchHourAngle(Body.Sun, observer, 0, new AstroTime(checkDate));
            DateTime noonToday = sunNoon1.time.ToUtcDateTime().Add(utcSpan);
            TimeSpan timeSpan = noonToday - checkDate.AddHours(12);
            int roundedSeconds = (int)Math.Round(timeSpan.TotalSeconds);
            _timeAdjustment = TimeSpan.FromSeconds(roundedSeconds);

            isTimeReady = true;
            StartClock();
        }

        private void StartClock()
        {
            if (!isTimeReady)
            {
                return;
            }

            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateClock();
                return true; // Повторяем таймер каждую секунду
            });
        }

        private void UpdateClock()
        {
            if (!isTimeReady)
            {
                return;
            }

            DateTime adjustedTime;
            if (isExact)
            {
                adjustedTime = DateTime.Now - _timeAdjustment;
            }
            else
            {
                adjustedTime = DateTime.UtcNow.AddHours(location.Longitude / 15.0);
            }

            ClockLabel.Text = adjustedTime.ToString("HH:mm:ss");
        }


    }
}*/

using Astro;

namespace SunClock
{
    public partial class MainPage : ContentPage
    {
        private Location? location;
        private Observer observer;
        private TimeSpan _timeAdjustment;
        private bool isExact = true;
        private bool isTimeReady = false;

        public MainPage()
        {
            InitializeComponent();
            GetGeolocation();
        }

        private async void GetGeolocation()
        {
            ClockLabel.Text = "Получение геоданных...";
            location = await new GeolocationService().GetCurrentLocationAsync();
            if (location != null)
            {
                observer = new Observer(location.Latitude, location.Longitude, location.Altitude ?? 0.0);
                InitializeTimeAdjustment();
            }
            else
            {
                ClockLabel.Text = "Не удалось получить местоположение";
                return;
            }
        }

        private void OnRadioButtonCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            isExact = ExactTimeRadioButton.IsChecked;

            if (isTimeReady) UpdateClock();
        }

        private void InitializeTimeAdjustment()
        {
            DateTime now = DateTime.Now;
            DateTime checkDate = DateTime.Today;
            TimeSpan utcOffset = now - DateTime.UtcNow;

            var sunNoon = Astronomy.SearchHourAngle(Body.Sun, observer, 0, new AstroTime(checkDate));
            DateTime noonToday = sunNoon.time.ToUtcDateTime().Add(utcOffset);

            _timeAdjustment = noonToday - checkDate.AddHours(12);
            _timeAdjustment = TimeSpan.FromSeconds(Math.Round(_timeAdjustment.TotalSeconds));

            isTimeReady = true;
            StartClock();
        }

        private void StartClock()
        {
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                UpdateClock();
                return true; // Повторяем таймер каждую секунду
            });
        }

        private void UpdateClock()
        {
            if (!isTimeReady) return;

            DateTime adjustedTime = isExact
                ? DateTime.Now - _timeAdjustment
                : DateTime.UtcNow.AddHours(location.Longitude / 15.0);

            ClockLabel.Text = adjustedTime.ToString("HH:mm:ss");
        }
    }
}