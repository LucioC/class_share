using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Kinect;

namespace ServiceCore
{
    public interface ISkeleton
    {
        IJoint HandRight { get; }
        IJoint HandLeft { get; }
        IJoint AnkleLeft { get; }
        IJoint AnkleRight { get; }
        IJoint ElbowRight { get; }
        IJoint ElbowLeft { get; }
        IJoint Spine { get; }
        IJoint Head { get; }
        IJoint FootLeft { get; }
        IJoint FootRight { get; }
        IJoint HipCenter { get; }
        IJoint HipRight { get; }
        IJoint HipLeft { get; }
        IJoint KneeRight { get; }
        IJoint KneeLeft { get; }
        IJoint ShoulderCenter { get; }
        IJoint ShoulderRight { get; }
        IJoint ShoulderLeft { get; }
        IJoint WristLeft { get; }
        IJoint WristRight { get; }
    }
}
