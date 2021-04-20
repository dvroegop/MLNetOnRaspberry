using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RaspberryCam.Camera
{
    class PhotoCamera : IAsyncDisposable, IDisposable
    {
        #region Private members
        private MMALCamera _camera;
        private ImageStreamCaptureHandler _imgCaptureHandler;
        #endregion


        #region Housekeeping

        public void Dispose()
        {
            DisposeAsync(true).Wait();
            GC.SuppressFinalize(this);
        }


        protected async Task DisposeAsync(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_imgCaptureHandler != null)
                {
                    _imgCaptureHandler.Dispose();
                    _imgCaptureHandler = null;
                }
            }

            await Task.CompletedTask;
        }
        public async ValueTask DisposeAsync()
        {
            await DisposeAsync(true);
            GC.SuppressFinalize(this);
        }

        ~PhotoCamera()
        {
            DisposeAsync(false).Wait(1000);
        }
        #endregion

        #region Public methods
        public async Task TakePicture()
        {
            Initialize();
            using(var imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg"))
            {
                
                // We need to wait for 2 seconds, to give the camera the time to adjust 
                await Task.Delay(2000);

                await _camera.TakePicture(imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);                
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

        //    if (_imgCaptureHandler == null)
        //    {
        //        Console.WriteLine("Initializing capture handler");
        //        _imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg");
        //    }
        }
        #endregion
    }
}
