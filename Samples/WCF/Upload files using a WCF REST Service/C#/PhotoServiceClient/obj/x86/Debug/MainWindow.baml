   M S B A M L   `   `   `  ����  PhotoServiceClient#    PhotoServiceClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$J  DPhotoService Client (Demo by Marco Minerva, marco.minerva@gmail.com)��5   	   $	��600��5   	   $	��800��6    � WindowStartupLocation$ CenterScreen=�6"   .��5      �  � RowDefinitions 5   
   �� 5	      $��* �6   6)   �� 5
      $
��Auto �6   6,   �� 5      $
��Auto �6   6,   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgPhotos��5       $��0��6      AutoGenerateColumns .     
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 200 63   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 6?   �� 
 �� Path$

 Name�6_     5      $ Description��6%   $	 400 6:   	 6F   �� $
 Description�6m     5      $ Taken On��6%   $	 150 67   	 6C   �� $
 
UploadedOn� �� StringFormat$ {0:MM/dd/yyyy hh:mm tt}��6�   5      �� 5   
   $��1��6   ��5      �� 5       �� Orientation$ Vertical=�6   .+�5      �� $��Selected photo��6    $
��Bold"�66   $��16(�6H   �� 5      -   5         ��Name$ imgPhoto��5      $	��320��6.   $	��240��6:   $��Uniform=�6G   6[   5      5      �� 5   
   $ 
Horizontal=�6   $��2��6.   $��Right=�6;   $��5q�6W   .+�5      �� -   5         ��Name$ btnGetPhotos��5      $��
Get photos��6+   $��5q�6[   6h   �� 5      -   5        $ btnNewPhoto��5      $��Upload a new photo...��6*   $��5q�6d   6q   �� 5      -   5        $ btnDeletePhoto��5      $��Delete selected photo��6-   $��5q�6j   6w   5       5!      5"      