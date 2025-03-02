namespace lab1._1
{
    public partial class MainPage : ContentPage
    {
        private TrafficLightController trafficLightController;

        public MainPage()
        {
            InitializeComponent();

            // Создаем контроллер и передаем в него элементы для обоих светофоров
            trafficLightController = new TrafficLightController(
                RedLight1, YellowLight1, GreenLight1, TimerLabel1, PedestrianRed1, PedestrianGreen1,
                RedLight2, YellowLight2, GreenLight2, TimerLabel2, PedestrianRed2, PedestrianGreen2,
                ForceYellowButton, ForceYellowButton2
            );

            // Запускаем светофор
            trafficLightController.Start();
        }

        private void OnForceYellowButtonClicked(object sender, EventArgs e)
        {
            trafficLightController.ForceYellow();
        }
    }
}
