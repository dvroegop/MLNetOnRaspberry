using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class ButtonReader
    {
        public void ReadButton(CancellationToken cancellationToken, Action callback)
        {
            GpioController gpioController = GpioControllerFactory.GetController();

            gpioController.OpenPin(Constants.PIN_BUTTON, PinMode.Input);
            var lastValue = PinValue.Low;

            Task.Run(
                () =>
                {
                    while(!cancellationToken.IsCancellationRequested)
                    {
                        var readValue = gpioController.Read(Constants.PIN_BUTTON);
                        
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
                        Task.Delay(10, cancellationToken).Wait();
                    }
                },cancellationToken);
        }
    }
}
