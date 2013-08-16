using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace ServiceCore
{
    public interface ISkeleton
    {
        Joint HandRight { get; }
        Joint HandLeft { get; }
        Joint AnkleLeft { get; }
        Joint AnkleRight { get; }
        Joint ElbowRight { get; }
        Joint ElbowLeft { get; }
        Joint Spine { get; }
        Joint Head { get; }
        Joint FootLeft { get; }
        Joint FootRight { get; }
        Joint HipCenter { get; }
        Joint HipRight { get; }
        Joint HipLeft { get; }
        Joint KneeRight { get; }
        Joint KneeLeft { get; }
        Joint ShoulderCenter { get; }
        Joint ShoulderRight { get; }
        Joint ShoulderLeft { get; }
        Joint WristLeft { get; }
        Joint WristRight { get; }
    }
}
