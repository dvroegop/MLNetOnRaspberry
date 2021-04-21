using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;
using CapDetector.Utilities;
using MLNetOnRaspberryML.Model;

namespace CapDetector
{
    class Program
    {

        static Buzzer _buzzer;
        static Led _ledBlue;
        static Led _ledGreen;
        static Led _ledRed;

        static void HandleButtonPress()
        {
            _ledBlue.On();
            TestML();
            _ledBlue.Off();
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Setting up the button handler");

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            ButtonReader buttonReader = new ButtonReader();
            buttonReader.ReadButton(cancellationTokenSource.Token, null);

            //var gpioController = GpioControllerFactory.GetController();
            //gpioController.OpenPin(Constants.PIN_BUTTON, System.Device.Gpio.PinMode.Input);

            _ledRed = new Led(Led.LedColor.Red);
            _ledGreen = new Led(Led.LedColor.Green);
            _ledBlue = new Led(Led.LedColor.Blue);

            _buzzer = new Buzzer();

            // Do the test run
            _ledRed.Flash();
            _ledGreen.Flash();
            _ledBlue.Flash();
            _buzzer.Buzz();

            Console.WriteLine("Ready to get going. Press CTRL-C to stop the program.");

            try
            {
                while(true)
                {
                    _ledBlue.On();
                    //var simulateFriday = gpioController.Read(Constants.PIN_BUTTON) == PinValue.High;
                    var simulateFriday = buttonReader.IsPressed;

                    Console.WriteLine($"\nSimulate is {simulateFriday}");

                    TestML(simulateFriday);
                    _ledBlue.Off();

                    Task.Delay(2000).Wait();
                }
            } finally
            {
                cancellationTokenSource.Cancel();

                cancellationTokenSource.Dispose();
            }
        }
        private static void TestML(bool simulateLatFriday = false)
        {
            Console.WriteLine("Start test");
            // Create single instance of sample data from first line of dataset for model input
            ModelInput sampleData;
            if (simulateLatFriday)
            {
                sampleData = new ModelInput { Time = @"17:00", DayOfWeek = 5F, DidTeamWin = 1F, };
            }
            else
            {
                string workTime = DateTime.Now.AddHours(-4).ToString("HH:mm");

                sampleData = new ModelInput { Time = workTime, DayOfWeek = 1F, DidTeamWin = 0F, };
            }
            // Make a single prediction on the sample data and print results
            ModelOutput predictionResult = ConsumeModel.Predict(sampleData);

            Console.WriteLine($"Result {simulateLatFriday} has {predictionResult.Score}.");

            if (predictionResult.Score < 10.0f)
            {
                Console.WriteLine("Fail");
                _buzzer.Buzz();
                _ledRed.Flash();
            }
            else
            {
                Console.WriteLine("Pass");
                _ledGreen.Flash();
            }
        }

    }
}
