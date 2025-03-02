using System.Timers;

namespace lab1._1
{
    public class TrafficLightController
    {
        private enum LightState {Off, Red, RedYellow, Green, Yellow , WaitYellow }
        private LightState currentState = LightState.Off;
        private int timeLeft;
        private readonly System.Timers.Timer timer;

        private readonly BoxView red1, yellow1, green1, pedRed1, pedGreen1;
        private readonly Label timerLabel1;

        private readonly BoxView red2, yellow2, green2, pedRed2, pedGreen2;
        private readonly Label timerLabel2;

        private readonly Button forceYellowButton;
        private readonly Button forceYellowButton2;

        public TrafficLightController(
            BoxView red1, BoxView yellow1, BoxView green1, Label timerLabel1, BoxView pedRed1, BoxView pedGreen1,
            BoxView red2, BoxView yellow2, BoxView green2, Label timerLabel2, BoxView pedRed2, BoxView pedGreen2,
            Button forceYellowButton, Button forceYellowButton2)
        {
            this.red1 = red1;
            this.yellow1 = yellow1;
            this.green1 = green1;
            this.timerLabel1 = timerLabel1;
            this.pedRed1 = pedRed1;
            this.pedGreen1 = pedGreen1;

            this.red2 = red2;
            this.yellow2 = yellow2;
            this.green2 = green2;
            this.timerLabel2 = timerLabel2;
            this.pedRed2 = pedRed2;
            this.pedGreen2 = pedGreen2;

            this.forceYellowButton = forceYellowButton;
            this.forceYellowButton2 = forceYellowButton2;

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
        }

        public void Start()
        {
            SetLightState(LightState.Off, 3);
            timer.Start();
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                UpdateUI();
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
                    case LightState.WaitYellow:
                        SetLightState(LightState.Yellow, 2);
                        break;
                }
            }
        }

        private void SetLightState(LightState state, int duration)
        {
            currentState = state;
            timeLeft = duration;
            UpdateUI();

            forceYellowButton.Dispatcher.Dispatch(() =>
            {
                // Разблокируем кнопку, когда светофор становится красным
                if (state == LightState.Red)
                {
                    forceYellowButton.Text = "Нажмите";
                    forceYellowButton.IsEnabled = true;
                    forceYellowButton2.Text = "Нажмите";
                    forceYellowButton2.IsEnabled = true;
                }
            });
        }

        private void UpdateUI()
        {
            void UpdateTrafficLight(BoxView red, BoxView yellow, BoxView green, Label timerLabel, BoxView pedRed, BoxView pedGreen)
            {
                red.Color = (currentState == LightState.Red || currentState == LightState.RedYellow) ? Colors.Red : Colors.Gray;
                yellow.Color = (currentState == LightState.Yellow || currentState == LightState.RedYellow) ? Colors.Yellow : Colors.Gray;
                green.Color = (currentState == LightState.Green || currentState == LightState.WaitYellow) ? Colors.Green : Colors.Gray;

                pedRed.Color = (currentState == LightState.Green || currentState == LightState.Yellow || currentState == LightState.RedYellow || currentState == LightState.WaitYellow) ? Colors.Red : Colors.Gray;
                pedGreen.Color = (currentState == LightState.Red) ? Colors.Green : Colors.Gray;

                timerLabel.Dispatcher.Dispatch(() => timerLabel.Text = timeLeft.ToString());
            }

            UpdateTrafficLight(red1, yellow1, green1, timerLabel1, pedRed1, pedGreen1);
            UpdateTrafficLight(red2, yellow2, green2, timerLabel2, pedRed2, pedGreen2);
        }

        public void ForceYellow()
        {
            if (currentState == LightState.Green)
            {
                forceYellowButton.Text = "Ожидайте";
                forceYellowButton.IsEnabled = false;
                forceYellowButton2.Text = "Ожидайте";
                forceYellowButton2.IsEnabled = false;
                SetLightState(LightState.WaitYellow, 3);
            }
        }
    }
}
