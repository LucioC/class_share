using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceCore;

namespace KinectPowerPointControl
{
    public interface IKinectService : IWindowThreadControl
    {
        void setMode(PRESENTATION_MODE mode);
    }
}
