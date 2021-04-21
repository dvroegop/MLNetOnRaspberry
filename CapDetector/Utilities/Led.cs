using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class Led : IDisposable
    {

        private GpioController _controller;
        private int _pinNumber;
        private readonly LedColor color;

        public Led(LedColor color)
        {
            this.color = color;
            _controller = GpioControllerFactory.GetController();

            _pinNumber = color switch
            {
                LedColor.Red => Constants.PIN_LEDRED,
                LedColor.Green => Constants.PIN_LEDGREEN,
                LedColor.Blue => Constants.PIN_LEDBLUE,
                _ => throw new NotImplementedException(),
            };

            _controller.OpenPin(_pinNumber, PinMode.Output);
        }

        public void Off()
        {
            _controller.Write(_pinNumber, PinValue.Low);
        }


        public void On()
        {
            _controller.Write(_pinNumber, PinValue.High);
        }

        public void Flash(int delay = 100, int counter = 1)
        {
            Task.Run(() =>
            {
                for (int i = 0; i < counter; i++)
                {
                    On();
                    Task.Delay(delay).Wait();
                    Off();
                    Task.Delay(delay).Wait();
                }
            });
        }

        public void Dispose()
        {
            // I know this is not a good example of using IDisposable,
            // but hey, it's a demo.. I can do whatever I want ;-)
            // Dennis

            _controller.ClosePin(_pinNumber);
        }

        public enum LedColor
        {
            Red,
            Green,
            Blue
        }
        public enum Mode
        {
            On,
            Off
        }

    }
}
