using CapDetector.Utilities;
using System;
using System.Threading;

namespace CapDetector
{
    class Program
    {
        static PhotoCamera _camera;

        static Led _ledRed;
        static Led _ledGreen;
        static Led _ledBlue;

        static Buzzer _buzzer;

        static void Main(string[] args)
        {
            Console.WriteLine("Setting up the button handler");

            var cancellationTokenSource = new CancellationTokenSource();

            _camera = new PhotoCamera();

            var buttonReader = new ButtonReader();

            _ledRed = new Led(Led.LedColor.Red);
            _ledGreen = new Led(Led.LedColor.Green);
            _ledBlue = new Led(Led.LedColor.Blue);

            _buzzer = new Buzzer();

            // Do the test run
            _ledRed.Flash();
            _ledGreen.Flash();
            _ledBlue.Flash();
            _buzzer.Buzz();

            // Start reading the button.
            buttonReader.ReadButton(cancellationTokenSource.Token, HandleButtonPress);

            Console.WriteLine("Ready to get going. Press enter to stop the program.");
            Console.ReadLine();
            cancellationTokenSource.Cancel();

            cancellationTokenSource.Dispose();
        }
        static void HandleButtonPress()
        {
            _camera.TakePicture(
                () =>
                {
                    _ledBlue.On();
                },
                (fileName) =>
                {
                    _ledBlue.Off
                    return;
                }).Wait();
        }
    }
}
