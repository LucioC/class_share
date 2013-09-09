using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using Microsoft.Speech.Recognition;
using System.Windows.Media;

namespace KinectPowerPointControl
{
    public class ListBoxHelper : KinectPowerPointControl.IListBoxHelper
    {
        public int Max { get; set; }

        public ListBoxHelper()
        {
            Max = 3;
        }

        public ListBoxItem CreateDefaultListBoxItem()
        {
            ListBoxItem item = new ListBoxItem();
            item.FontSize = 28;
            return item;
        }

        public void AddToList(ListBox list, ListBoxItem item)
        {
            list.Items.Add(item);

            if (list.Items.Count > Max) list.Items.RemoveAt(0);
        }

        public void AddItemToList(RecognitionResult speech, ListBox list, Color color)
        {
            ListBoxItem item = CreateDefaultListBoxItem();
            item.Content = "S:" + speech.Text + " C:" + speech.Confidence;
            item.Foreground = new SolidColorBrush(color);
            AddToList(list, item);
        }
    }
}
