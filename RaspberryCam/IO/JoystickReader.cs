using Sense.Stick;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;

namespace RaspberryCam.IO
{
    class JoystickReader
    {
        public void ReadJoystick()
        {
            IObservable<bool> GetStatus(JoystickEvent joystickEvent)
            {
                IObservable<bool> result;
                switch (joystickEvent.State)
                {
                    case JoystickKeyState.Press:
                        result = Observable.Return<bool>(true);
                        break;

                    default:
                        result = Observable.Return<bool>(false);
                        break;
                }

                return result;
            }

            var d = Joystick.Events.Select(GetStatus);
            using (d)
            {
                Console.WriteLine("Press joystick to take a picture");
                Console.ReadLine();
            }
        }
    }
}
