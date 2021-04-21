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

            var s = DateTime.Now.AddHours(-4).ToString("HH:mm");
            Console.WriteLine(s);
        }
    }
}
