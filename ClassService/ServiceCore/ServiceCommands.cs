using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServiceCore
{
    public delegate void CommandExecutor(String command, String param);
    public class ServiceCommands
    {
        public const string CLOSE_PRESENTATION = "closepresentation";
        public const string CLOSE_IMAGE = "closeimage";
        public const string NEXT_SLIDE = "nextslide";
        public const string PREVIOUS_SLIDE = "previousslide";
        public const string IMAGE_ZOOM = "imagezoom";
        public const string IMAGE_MOVE_X = "imagemovex";
        public const string IMAGE_MOVE_Y = "imagemovey";
        public const string IMAGE_ROTATE = "imagerotate";
        public const string IMAGE_SET_VISIBLE_PART = "visiblepart";
    }
}
