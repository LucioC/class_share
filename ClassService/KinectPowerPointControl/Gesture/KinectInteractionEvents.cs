using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Kinect.Toolkit.Interaction;
using CommonUtils;

namespace KinectPowerPointControl.Gesture
{
    public class KinectInteractionEvents
    {
        private UserInfo[] userInfos;

        public void AllocateUserInfos(int size)
        {
            this.userInfos = new UserInfo[size];
        }

        int pressState = 0;

        /// <summary>
        /// Event handler for InteractionStream's InteractionFrameReady event
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        [SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "KinectSensor", Justification = "KinectSensor is valid in a debug string"),
         SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "KinectRegion", Justification = "KinectRegion is valid in a debug string"),
         SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "BeginInvoke", Justification = "BeginInvoke is valid in a debug string"),
         SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Diagnostics.Debugger.Log(System.Int32,System.String,System.String)", Justification = "Debugger-only messages.  Will never be localized.")]
        public void InteractionFrameReady(object sender, InteractionFrameReadyEventArgs e)
        {
            // Check for a null userInfos since we may still get posted events
            // from the stream after we have unregistered our event handler and
            // deleted our buffers.
            if (this.userInfos == null)
            {
                return;
            }

            UserInfo[] localUserInfos = null;
            long timestamp = 0;

            using (InteractionFrame interactionFrame = e.OpenInteractionFrame())
            {
                if (interactionFrame != null)
                {
                    // Copy interaction frame data so we can dispose interaction frame
                    // right away, even if data processing/event handling takes a while.
                    interactionFrame.CopyInteractionDataTo(this.userInfos);
                    timestamp = interactionFrame.Timestamp;
                    localUserInfos = this.userInfos;
                }
            }

            if (localUserInfos != null)
            {
                for (int userIndex = 0; userIndex < localUserInfos.Length; ++userIndex)
                {
                    var user = localUserInfos[userIndex];
                    foreach (var handPointer in user.HandPointers)
                    {
                        if (handPointer.IsPressed)
                        {   
                            //Do not repeat output
                            if(pressState != 2)
                                Output.Debug("KinectControl", "Hand Pressed " + handPointer.PressExtent.ToString());
                            
                            pressState = 2;
                        }
                        else
                            if (handPointer.PressExtent > 0.5d)
                            {
                                //Do not repeat output
                                if(pressState != 1)
                                    Output.Debug("KinectControl", "Hand Almost Pressed " + handPointer.PressExtent.ToString());

                                pressState = 1;
                            }


                        if (handPointer.HandEventType == InteractionHandEventType.Grip)
                        {
                            Output.Debug("KinectControl", "Hand Grip");
                        }
                    }
                }
            }
        }
    }
}
