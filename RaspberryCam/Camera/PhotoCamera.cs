using MMALSharp;
using MMALSharp.Common;
using MMALSharp.Handlers;
using System;
using System.Threading.Tasks;

namespace RaspberryCam.Camera
{
    class PhotoCamera : IAsyncDisposable
    {
        #region Private members
        private MMALCamera _camera;
        private ImageStreamCaptureHandler _imgCaptureHandler;
        #endregion


        #region Housekeeping


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
            await _camera.TakePicture(_imgCaptureHandler, MMALEncoding.JPEG, MMALEncoding.I420);
        }
        #endregion

        #region Private members
        private void Initialize()
        {


            if (_camera == null)
            {
                MMALCameraConfig.StillResolution = new MMALSharp.Common.Utility.Resolution(640, 480);
                _camera = MMALCamera.Instance;
            }

            if (_imgCaptureHandler == null)
            {
                _imgCaptureHandler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg");
            }
        }
        #endregion
    }
}
