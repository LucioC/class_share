using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectPowerPointControl.Speech
{
    public class ImagePresentationGrammar: AbstractGrammar
    {
        public const String MOVE_RIGHT = "moveright";
        public const String MOVE_LEFT = "moveleft";
        public const String MOVE_UP = "moveup";
        public const String MOVE_DOWN = "movedown";
        public const String CLOSE_IMAGE = "closeimage";

        public const String ZOOM_IN = "zoomin";
        public const String ZOOM_OUT = "zoomout";

        public const String ROTATE_RIGHT = "rotateright";
        public const String ROTATE_LEFT = "rotateleft";

        public ImagePresentationGrammar()
        {
            words = new List<string>();
            dictionary = new Dictionary<string, IList<string>>();

            //FIXME could take sentences from a file?
            #region moves

            List<String> moveRight = new List<string>();
            moveRight.Add("right");
            moveRight.Add("move right");
            dictionary.Add(MOVE_RIGHT, moveRight);

            List<String> moveLeft = new List<string>();
            moveLeft.Add("left");
            moveLeft.Add("move left");
            dictionary.Add(MOVE_LEFT, moveLeft);

            List<String> moveUp = new List<string>();
            moveUp.Add("up");
            moveUp.Add("move up");
            dictionary.Add(MOVE_UP, moveUp);

            List<String> moveDown = new List<string>();
            moveDown.Add("down");
            moveDown.Add("move down");
            dictionary.Add(MOVE_DOWN, moveDown);

            #endregion

            #region zooms

            List<String> zoomIn = new List<string>();
            zoomIn.Add("zoom in");
            dictionary.Add(ZOOM_IN, zoomIn);

            List<String> zoomOut = new List<string>();
            zoomOut.Add("computer hide window");
            dictionary.Add(ZOOM_OUT, zoomOut);

            #endregion

            #region rotations

            List<String> rotateRight = new List<string>();
            rotateRight.Add("rotate right");
            dictionary.Add(ROTATE_RIGHT, rotateRight);

            List<String> rotateLeft = new List<string>();
            rotateLeft.Add("rotate left");
            dictionary.Add(ROTATE_LEFT, rotateLeft);

            #endregion
            
            List<String> closeImage = new List<string>();
            closeImage.Add("close image");
            closeImage.Add("close");
            dictionary.Add(CLOSE_IMAGE, closeImage);

            words.AddRange(closeImage);

            words.AddRange(moveRight);
            words.AddRange(moveLeft);
            words.AddRange(moveUp);
            words.AddRange(moveDown);

            words.AddRange(zoomIn);
            words.AddRange(zoomOut);

            words.AddRange(rotateRight);
            words.AddRange(rotateLeft);
        }
    }
}
