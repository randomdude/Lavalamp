using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ruleEngine.ruleItems.Support;
using DirectShowLib;
namespace WindowsSpecificRuleItems
{
    public class Webcam : IWebcam
    {
        private DsDevice camera = null;
        public Webcam()
        {
            findCaptureDevice();
        }

        private void findCaptureDevice()
        {
            var cameras = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (cameras.Length > 0)
                camera = cameras[0];
        }

        public void Dispose()
        {
            
            camera.Dispose();
        }

        public System.Drawing.Image Capture()
        {
            IGraphBuilder builder;
            IFilterGraph2 graph;
            IVideoWindow win;
            
          //  builder.Render()
            return null;
        }
    }
}
