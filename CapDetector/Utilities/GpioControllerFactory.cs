using System;
using System.Device.Gpio;

namespace CapDetector.Utilities
{
    static class GpioControllerFactory
    {

        private static GpioController _controller;

        public static GpioController GetController()
        {
            if (_controller == null)
            {
                _controller = new GpioController();
            }
            return _controller;


        }

    }
}
