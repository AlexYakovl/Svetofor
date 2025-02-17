using System.Timers;

namespace lab1._1
{
    public partial class MainPage : ContentPage
    {
        private enum LightState { Off, Red, RedYellow, Green, Yellow }
        private LightState currentState = LightState.Off;
        private int timeLeft;
        private readonly Label timerLabel;
        private readonly BoxView redLight, yellowLight, greenLight;
        private readonly BoxView pedestrianRed, pedestrianGreen;
        private readonly System.Timers.Timer timer;

        public MainPage()
        {
            timerLabel = new Label { FontSize = 24, HorizontalOptions = LayoutOptions.Center };

            // Автомобильный светофор
            redLight = new BoxView { Color = Colors.Gray, WidthRequest = 50, HeightRequest = 50, CornerRadius = 25 };
            yellowLight = new BoxView { Color = Colors.Gray, WidthRequest = 50, HeightRequest = 50, CornerRadius = 25 };
            greenLight = new BoxView { Color = Colors.Gray, WidthRequest = 50, HeightRequest = 50, CornerRadius = 25 };

            // Пешеходный светофор
            pedestrianRed = new BoxView { Color = Colors.Gray, WidthRequest = 50, HeightRequest = 50, CornerRadius = 25 };
            pedestrianGreen = new BoxView { Color = Colors.Gray, WidthRequest = 50, HeightRequest = 50, CornerRadius = 25 };

            StackLayout lights = new StackLayout
            {
                Children = { redLight, yellowLight, greenLight, timerLabel },
                Spacing = 10,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            StackLayout pedestrianLights = new StackLayout
            {
                Children = { pedestrianRed, pedestrianGreen },
                Spacing = 10,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            Content = new HorizontalStackLayout
            {
                Children = { lights, pedestrianLights },
                Spacing = 50,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            };

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Start();

            SetLightState(LightState.Off, 2);
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                Device.BeginInvokeOnMainThread(() => timerLabel.Text = timeLeft.ToString());
            }
            else
            {
                switch (currentState)
                {
                    case LightState.Off:
                        SetLightState(LightState.Red, 9);
                        break;
                    case LightState.Red:
                        SetLightState(LightState.RedYellow, 2);
                        break;
                    case LightState.RedYellow:
                        SetLightState(LightState.Green, 9);
                        break;
                    case LightState.Green:
                        SetLightState(LightState.Yellow, 2);
                        break;
                    case LightState.Yellow:
                        SetLightState(LightState.Red, 9);
                        break;
                }
            }
        }

        private void SetLightState(LightState state, int duration)
        {
            currentState = state;
            timeLeft = duration;

            // Автомобильный светофор
            redLight.Color = (state == LightState.Red || state == LightState.RedYellow) ? Colors.Red : Colors.Gray;
            yellowLight.Color = (state == LightState.Yellow || state == LightState.RedYellow) ? Colors.Yellow : Colors.Gray;
            greenLight.Color = state == LightState.Green ? Colors.Green : Colors.Gray;

            // Пешеходный светофор
            pedestrianRed.Color = (state == LightState.Green || state == LightState.Yellow || state == LightState.RedYellow ) ? Colors.Red : Colors.Gray;
            pedestrianGreen.Color = (state == LightState.Red) ? Colors.Green : Colors.Gray;

            Device.BeginInvokeOnMainThread(() => timerLabel.Text = timeLeft.ToString());
        }
    }
}
