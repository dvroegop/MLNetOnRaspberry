using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class ButtonReader: IDisposable
    {

        private Action<bool> _callBack;
        private bool _isPressed;
        private readonly GpioController _controller;

        public ButtonReader(Action<bool> callback)
        {
            _callBack = callback;

            _controller = GpioControllerFactory.GetController();

            _controller.OpenPin(Constants.PIN_BUTTON, PinMode.Input);

        }
        public bool IsPressed
        {
            get
            {
                if (_isPressed)
                {
                    _isPressed = false;
                    Console.WriteLine("Calling the callback after button press..");
                    _callBack?.Invoke(true);
                    return true;
                }
                return false;
            }
        }

        public void Dispose()
        {
            // I know this is not a good example of using IDisposable,
            // but hey, it's a demo.. I can do whatever I want ;-)
            // Dennis

            _controller.ClosePin(Constants.PIN_BUTTON);
        }

        public void ReadButton(CancellationToken cancellationToken)
        {            
            PinValue lastValue = PinValue.Low;

            Task.Run(
                () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        PinValue readValue = _controller.Read(Constants.PIN_BUTTON);

                        if (readValue != lastValue)
                        {
                            if (readValue == PinValue.High)
                            {
                                // We clicked...
                                 _callBack?.Invoke(true);
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
