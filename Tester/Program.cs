using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace Tester
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            GpioController gpioController = new GpioController();
            gpioController.OpenPin(26, PinMode.Input);
            bool isOn = false;
            var lastValue = PinValue.Low;

            while (true)
            {
                var value = gpioController.Read(26);
                if(value != lastValue)
                {
                    Console.WriteLine($"Value is {value}");
                    lastValue = value;
                }
                Task.Delay(10).Wait();
            }
            //while (true)
            //{
            //    gpioController.Write(26, isOn ? PinValue.Low : PinValue.High);

            //    Task.Delay(500).Wait();
            //    isOn = !isOn;
            //}

            Console.WriteLine("Closing");
        }
    }
}
