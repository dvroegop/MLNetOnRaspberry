using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class Buzzer
    {
        public void Buzz()
        {
            var controller = GpioControllerFactory.GetController();

            Task.Run(
                () =>
                {
                    controller.OpenPin(Constants.PIN_BUZZER, System.Device.Gpio.PinMode.Output);

                    controller.Write(Constants.PIN_BUZZER, PinValue.High);
                    Task.Delay(1000).Wait();
                    controller.Write(Constants.PIN_BUZZER, PinValue.Low);

                    controller.ClosePin(Constants.PIN_BUZZER);

                });

        }
    }
}
