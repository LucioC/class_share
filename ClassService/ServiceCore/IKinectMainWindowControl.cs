﻿using System;
namespace ServiceCore
{
    public interface IKinectMainWindowControl : IWindowThreadControl
    {
        string FilesFolder { get; set; }
    }
}