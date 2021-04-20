using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace RaspberryCam.IO
{
    class JoystickReader
    {
        private const int PIN_BUTTON = 26;
        public void ReadJoystick(CancellationToken cancellationToken, Action callback)
        {



            GpioController gpioController = new GpioController();
            gpioController.OpenPin(PIN_BUTTON, PinMode.Input);
            var lastValue = PinValue.Low;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Run(
                async () =>
                {
                    while(!cancellationToken.IsCancellationRequested)
                    {
                        var readValue = gpioController.Read(PIN_BUTTON);
                        
                        if(readValue != lastValue)
                        {
                            if(readValue == PinValue.High)
                            {
                                // We clicked...
                                Console.WriteLine("Calling the callback.");
                                callback?.Invoke();
                            }

                            lastValue = readValue;
                        }
                        await Task.Delay(10, cancellationToken);
                    }
                },cancellationToken);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }
    }
}
