using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl
{
    public class ServiceCommandToKeyEvent
    {
        public void ProcessMoveRight()
        {
            //Window go left image go right
            System.Windows.Forms.SendKeys.SendWait("{Left}");
        }

        public void ProcessMoveLeft()
        {
            //window go right image go left
            System.Windows.Forms.SendKeys.SendWait("{Right}");
        }

        public void ProcessMoveUp()
        {
            //Window go down image go up
            System.Windows.Forms.SendKeys.SendWait("{Down}");
        }

        public void ProcessMoveDown()
        {
            //window go up image go down
            System.Windows.Forms.SendKeys.SendWait("{UP}");
        }

        public void ProcessRotateRight()
        {
            System.Windows.Forms.SendKeys.SendWait("{END}");
        }

        public void ProcessRotateLeft()
        {
            System.Windows.Forms.SendKeys.SendWait("{HOME}");
        }

        public void ProcessZoomOut()
        {
            System.Windows.Forms.SendKeys.SendWait("{PGDN}");
        }

        public void ProcessZoomIn()
        {
            System.Windows.Forms.SendKeys.SendWait("{PGUP}");
        }

        public void ProcessNextSlide()
        {
            System.Windows.Forms.SendKeys.SendWait("{Right}");
        }

        public void ProcessPreviousSlide()
        {
            System.Windows.Forms.SendKeys.SendWait("{Left}");
        }
    }
}
