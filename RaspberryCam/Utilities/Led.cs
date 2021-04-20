using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace RaspberryCam.Utilities
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
        }


        public void On()
        {
            _controller.Write(_pinNumber, PinValue.High);
        }

        public void Off()
        {
            _controller.Write(_pinNumber, PinValue.Low);
        }
    }
}
