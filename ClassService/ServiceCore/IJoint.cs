using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace ServiceCore
{
    public interface IJoint
    {
        SkeletonPoint Position { get; }
    }
}
