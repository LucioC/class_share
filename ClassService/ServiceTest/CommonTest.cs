using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassService;
using Moq;
using ServiceCore;
using KinectPowerPointControl;
using Microsoft.Kinect;

namespace ServiceTest
{
    public class CommonTest
    {
        public static string GetFileResourcePath(string fileName)
        {
            String testLocalPath = AppDomain.CurrentDomain.BaseDirectory + "\\resources\\" + fileName;
            return testLocalPath;
        }

        public static void WaitForEventAsyncInvoke()
        {
            //Better way to wait for assynchronous event?
            System.Threading.Thread.Sleep(200);
        }

        public static void MockServiceMembers()
        {
            var presentationControlMock = new Mock<IPowerPointControl>();
            var imageControlMock = new Mock<IImageService>();
            var kinectControlMock = new Mock<IKinectService>();
            var fileManagerMock = new Mock<IServiceFileManager>();
            var mainWindowMock = new Mock<IKinectMainWindowControl>();

            Service.ImageForm = imageControlMock.Object;
            Service.KinectWindow = kinectControlMock.Object;
            Service.fileManager = fileManagerMock.Object;
            Service.mainWindow = mainWindowMock.Object;
            Service.PresentationControl = presentationControlMock.Object;
        }

        public static Service CreateAMockedService()
        {
            Service service = new Service();
            MockServiceMembers();
            return service;
        }

        public static SkeletonPoint CreateSkeletonPoint(float x, float y, float z)
        {
            SkeletonPoint point = new SkeletonPoint();
            point.X = x;
            point.Y = y;
            point.Z = z;
            return point;
        }

        public static IJoint CreateDummyJointWithSkeletonPoint(float x, float y, float z)
        {
            SkeletonPoint point = CreateSkeletonPoint(x, y, z);

            IJoint joint = new DummyJoint(point);
            return joint;
        }
    }
}
