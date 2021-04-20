using System;
using System.Threading;
using RaspberryCam.Camera;
using RaspberryCam.IO;

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

                JoystickReader reader = new JoystickReader();

                reader.ReadJoystick(
                    cts.Token,
                    () =>
                    {
                        camera.TakePicture().Wait(cts.Token);
                        Console.WriteLine("Taken the picture.. I hope..");
                    }
                        )            ;


                Console.WriteLine("Press the button to take a picture");
                Console.ReadLine();
                cts.Cancel();
            }


        }

    }
}
