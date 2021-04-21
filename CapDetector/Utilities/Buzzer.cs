using System;
using System.Device.Gpio;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class Buzzer : IDisposable
    {
        private GpioController _controller;

        public Buzzer()
        {
            _controller = GpioControllerFactory.GetController();
            _controller.OpenPin(Constants.PIN_BUZZER, System.Device.Gpio.PinMode.Output);
        }
        public void Buzz()
        {
            Task.Run(
                () =>
                {
                    _controller.Write(Constants.PIN_BUZZER, PinValue.High);
                    Task.Delay(1000).Wait();
                    _controller.Write(Constants.PIN_BUZZER, PinValue.Low);
                });

        }

            public void Dispose()
            {
                // I know this is not a good example of using IDisposable,
                // but hey, it's a demo.. I can do whatever I want ;-)
                // Dennis

                _controller.ClosePin(Constants.PIN_BUZZER);
            }

    }
}
