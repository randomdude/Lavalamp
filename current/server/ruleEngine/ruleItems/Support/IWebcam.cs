using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ruleEngine.ruleItems.Support
{
    using System.Drawing;

    public interface IWebcam : IDisposable
    {
        Image Capture();
    }
}
