using System;
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

            var buttonReader = new ButtonReader();
            buttonReader.ReadButton(cancellationTokenSource.Token, TestML);

            _ledRed = new Led(Led.LedColor.Red);
            _ledGreen = new Led(Led.LedColor.Green);
            _ledBlue = new Led(Led.LedColor.Blue);

            _buzzer = new Buzzer();

            // Do the test run
            _ledRed.Flash();
            _ledGreen.Flash();
            _ledBlue.Flash();

            _buzzer.Buzz();

            Console.WriteLine("Ready to get going. Press any key to stop the program.");

            try
            {
                while(!Console.KeyAvailable)
                {
                    _ledBlue.On();

                    TestML(false);
                    _ledBlue.Off();

                    Task.Delay(2000).Wait();
                }
            } finally
            {
                cancellationTokenSource.Cancel();

                cancellationTokenSource.Dispose();
                _ledRed.Off();
                _ledGreen.Off();
                _ledBlue.Off();

                buttonReader.Dispose();
                _buzzer.Dispose();
                _ledRed.Dispose();
                _ledGreen.Dispose();
                _ledBlue.Dispose();
            }
        }
        private static void TestML(bool simulateLatFriday = false)
        {
            //if(simulateLatFriday)
            //    _ledGreen.Flash(500, 5);
            //else
            //    _ledRed.Flash(250, 10);

            ModelInput inputData;
            if (simulateLatFriday)
            {
                inputData = new ModelInput { Time = @"17:00", DayOfWeek = 5F, DidTeamWin = 1F, };
            }
            else
            {
                string workTime = DateTime.Now.AddHours(-4).ToString("HH:mm");

                inputData = new ModelInput { Time = workTime, DayOfWeek = 1F, DidTeamWin = 0F, };
            }            
            ModelOutput predictionResult = ConsumeModel.Predict(inputData);
            var workingDay = simulateLatFriday ? "Friday" : "a normal day";
            Console.WriteLine($"Result from working on {workingDay} has {predictionResult.Score} bolts per 5 minutes");

            if (predictionResult.Score < 10.0f)
            {
                Console.WriteLine("Fail");
                _buzzer.Buzz();
                _ledRed.Flash(100, 10);
            }
            else
            {
                Console.WriteLine("Pass");
                _ledGreen.Flash(200, 2);
            }
        }

    }
}
