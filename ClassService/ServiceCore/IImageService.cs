﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public interface IImageService: IWindowThreadControl
    {
        void SetFilePath(String fileName);
        String GetImageFilePath();
        void SendCommand(String command);
    }
}
