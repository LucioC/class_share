using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public interface IThreadFileWindow: IWindowThreadControl
    {
        void SetFilePath(String fileName);
    }
}
