using RaspberryCam.Camera;
using RaspberryCam.IO;
using RaspberryCam.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryCam
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            using (PhotoCamera camera = new PhotoCamera())
            {
                CancellationTokenSource cts = new CancellationTokenSource();

                var ledRed = new Led(Led.LedColor.Red);
                var ledGreen = new Led(Led.LedColor.Green);
                var ledBlue = new Led(Led.LedColor.Blue);

                ledRed.On();
                Task.Delay(100).Wait();
                ledRed.Off();
                ledGreen.On();
                Task.Delay(100).Wait();
                ledGreen.Off();
                ledBlue.On();
                Task.Delay(100).Wait();
                ledBlue.Off();

                var buzzer = new Buzzer();
                buzzer.Buzz();

                JoystickReader reader = new JoystickReader();

                reader.ReadJoystick(
                    cts.Token,
                    () =>
                    {
                        buzzer.Buzz();
                        camera.TakePicture().Wait(cts.Token);
                        Console.WriteLine("Taken the picture.. I hope..");
                        ledBlue.Flash();
                    }
                        );


                Console.WriteLine("Press the button to take a picture");
                Console.ReadLine();
                cts.Cancel();
            }


        }

    }
}
