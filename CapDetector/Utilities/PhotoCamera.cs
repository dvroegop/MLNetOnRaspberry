using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CapDetector.Utilities
{
    class PhotoCamera
    {
        #region Private members
        private MMALCamera _camera;
        private bool _firstRun = true;
        #endregion


        #region Public methods
        public async Task TakePicture(Action beforePictureCallback, Action<string> afterPictureCallback)
        {
            Initialize();
            using (var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg"))
            {

                beforePictureCallback?.Invoke();
                // We need to wait for 2 seconds the first time this is called, to give the camera the time to adjust 
                if (_firstRun)
                {
                    await Task.Delay(2000);
                    _firstRun = false;
                }

                await _camera.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
                var fileName = imgCaptureHandler.GetFilename();
                afterPictureCallback?.Invoke(fileName);
                System.IO.File.Delete(fileName);
            }
        }
        #endregion

        #region Private members
        private void Initialize()
        {


            if (_camera == null)
            {
                Console.WriteLine("Initializing camera");
                MMALCameraConfig.StillResolution = new MMALSharp.Common.Utility.Resolution(640, 480);
                MMALCameraConfig.Flips = MMALSharp.Native.MMAL_PARAM_MIRROR_T.MMAL_PARAM_MIRROR_VERTICAL;

                _camera = MMALCamera.Instance;
            }
        }
        #endregion
    }
}
