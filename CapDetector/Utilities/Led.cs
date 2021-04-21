using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class Led
    {
        private readonly LedColor color;
        private GpioController _controller;
        private int _pinNumber;

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


        public void On()
        {
            _controller.Write(_pinNumber, PinValue.High);
        }

        public void Off()
        {
            _controller.Write(_pinNumber, PinValue.Low);
        }

        public void Flash(int delay=100) {
            _controller.Write(_pinNumber, PinValue.High);
            Task.Delay(delay).Wait();
            _controller.Write(_pinNumber, PinValue.Low);
        }
    }
}
