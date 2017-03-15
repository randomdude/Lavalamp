
namespace ruleEngine.ruleItems
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    using ruleEngine.pinDataTypes;
    using ruleEngine.ruleItems.Support;

  //  [ToolboxRule("Web camera facial detection")]
    public class ruleItemWebcam : ruleItemBase, IDisposable
    {


        private detectionOptions _detectionOptions = new detectionOptions();

        private IWebcam _camera;
        private bool _disposed;

        public override string typedName
        {
            get
            {
                return "WebCam";
            }
        }

        //private ImageViewer _viewer;

        private const string Harrpath = @"haarcascade_frontalface_default.xml";

        public ruleItemWebcam() : this(null)
        {
            /*if (GpuInvoke.HasCuda)
                _cascade = new GpuCascadeClassifier(Harrpath);
            else//*/

        }

        public ruleItemWebcam(IFactory factory)
        {
            if (null == factory)
                factory = new SystemImplementationFactory();
            _camera = factory.GetInstance<IWebcam>();
        }

    public override string ruleName()
        {
            return "Web camera facial detection";
        }

        public override Dictionary<string, pin> getPinInfo()
        {
            var info = base.getPinInfo();

            info.Add(
                "trigger",
                new pin()
                    {
                        name = "trigger",
                        description = "set to check the camera",
                        direction = pinDirection.input,
                        valueType = typeof(pinDataBool)
                    });
            info.Add(
                "output",
                new pin()
                    {
                        name = "output",
                        description = "is present",
                        direction = pinDirection.output,
                        valueType = typeof(pinDataBool)
                    });
            return info;
        }


        public override IFormOptions setupOptions()
        {
            return _detectionOptions;
        }

        public override void evaluate()
        {
            if (!pinInfo["trigger"].value.asBoolean())
                return;
        /*    if (_viewer == null)
            {
                _viewer = new ImageViewer();
                _viewer.Show();
            } */
             Image capture = _camera.Capture();
             if (null != capture)
             {
                 //todo facial recognition
             }
          
     /*       {
                using (Image<Bgr, byte> nextFrame = webcam.QueryFrame())
                {
                    if (nextFrame == null)
                        return;
                    Rectangle[] faces;
     
                    using (Image<Gray, byte> grayframe = nextFrame.Convert<Gray, byte>())
                    {
                        /*if (GpuInvoke.HasCuda)
                        {

                            using (var gpuImage = new GpuImage<Gray, byte>(grayframe))
                            {
                                lock (_cascade)
                                {
                                    var gpucascade = this._cascade as GpuCascadeClassifier;
                                    faces = gpucascade.DetectMultiScale(
                                        gpuImage, 1.4, 4, new Size(nextFrame.Width / 8, nextFrame.Height / 8));
                                }
                            }
                        }
                        else
                        {
                            lock (_cascade)
                            {
                                var cpuCascade = this._cascade as CascadeClassifier;
                                faces = cpuCascade.DetectMultiScale(
                                    grayframe, 1.4, 4, new Size(nextFrame.Width / 8, nextFrame.Height / 8), Size.Empty);
                            }
                        }

                        if (_detectionOptions.Anyone && faces.Any())
                            onRequestNewTimelineEvent(
                                new timelineEventArgs(new pinDataBool(true, this, pinInfo["output"])));
                        else if (faces.Any() && _detectionOptions.Users.Any())
                        {
                            FaceRecognizer recognizer = new EigenFaceRecognizer(0, 10);
                            List<int> seen = new List<int>();
                            foreach (var rect  in faces)
                            {
                                bool shouldBeIn;
                                int userId = recognizer.Predict(grayframe.GetSubRect(rect)).Label;
                                if (
                                    _detectionOptions.Users.TryGetValue(userId, out shouldBeIn))
                                {
                                    if (!shouldBeIn)
                                    {
                                        onRequestNewTimelineEvent(
                                            new timelineEventArgs(new pinDataBool(false, this, pinInfo["output"])));
                                        return;
                                    }
                                    seen.Add(userId);
                                }
                                
                            }
                            onRequestNewTimelineEvent(
                                            new timelineEventArgs(new pinDataBool(seen.Count == _detectionOptions.Users.Count && 
                                                                                  seen.Intersect(_detectionOptions.Users.Keys).Count() == seen.Count
                                                                                  , this,pinInfo["output"])));
                        }
                        else
                        {
                            onRequestNewTimelineEvent(
                                new timelineEventArgs(new pinDataBool(false, this, pinInfo["output"])));
                        }
                        foreach (var rectangle in faces)
                        {
                            nextFrame.Draw(rectangle, new Bgr(0, double.MaxValue, 0), 3);
                        }
                    }
                    lock(_viewer)
                        this._viewer.Image = nextFrame;
                }
            }*/

        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // If you need thread safety, use a lock around these  
            // operations, as well as in your methods that use the resource. 
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_camera != null)
                        _camera.Dispose();
                }

                // Indicate that the instance has been disposed.
                _disposed = true;
            }
        }

    }


    public class detectionOptions : BaseOptions
    {
        public bool Anyone { get; set; }

        public Dictionary<int,bool> Users { get; set; }
    }

}
