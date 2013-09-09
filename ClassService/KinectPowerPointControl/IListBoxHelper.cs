using System;
namespace KinectPowerPointControl
{
    public interface IListBoxHelper
    {
        void AddItemToList(Microsoft.Speech.Recognition.RecognitionResult speech, System.Windows.Controls.ListBox list, System.Windows.Media.Color color);
        void AddToList(System.Windows.Controls.ListBox list, System.Windows.Controls.ListBoxItem item);
        System.Windows.Controls.ListBoxItem CreateDefaultListBoxItem();
        int Max { get; set; }
    }
}
