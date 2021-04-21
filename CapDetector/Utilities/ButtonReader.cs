using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class ButtonReader
    {

        private Action<bool> _callBack;
        private bool _isPressed;

        public bool IsPressed
        {
            get
            {
                if (_isPressed)
                {
                    _isPressed = false;
                    _callBack?.Invoke(true);
                    return true;
                }
                return false;
            }
        }

        public void ReadButton(CancellationToken cancellationToken, Action<bool> callback)
        {
            _callBack = callback;
            GpioController gpioController = GpioControllerFactory.GetController();

            gpioController.OpenPin(Constants.PIN_BUTTON, PinMode.Input);
            PinValue lastValue = PinValue.Low;

            Task.Run(
                () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        PinValue readValue = gpioController.Read(Constants.PIN_BUTTON);

                        if (readValue != lastValue)
                        {
                            if (readValue == PinValue.High)
                            {
                                // We clicked...
                                Console.WriteLine("Calling the callback.");
                                _isPressed = true;
                            }

                            lastValue = readValue;
                        }
                        Task.Delay(10, cancellationToken).Wait();
                    }
                }, cancellationToken);
        }

    }
}
