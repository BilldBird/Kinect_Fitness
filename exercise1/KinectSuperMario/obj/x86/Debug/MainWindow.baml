   M S B A M L   `   `   `  ����^  ZMicrosoft.Samples.Kinect.WpfViewers, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null�^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers#Microsoft.Samples.Kinect.WpfViewers  5   j    KinectSuperMario   KinectProject.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   mmy^clr-namespace:Microsoft.Samples.Kinect.WpfViewers;assembly=Microsoft.Samples.Kinect.WpfViewers     5   j   -   5          � Title$  KinectProject��5   	   $	��617��6   $
��1297��6,   .��5      � 
��� ��� � ColumnDefinitions 5   
   �� 5      $
��643* �6   �� 5	      $
��632* �6    � RowDefinitions 5   
   �� 5      $	��97* �6   �� 5      $
��382* �6   �� 5      $	��99* �6   .+�5   
   � -   5         �Name$ 	FlashGrid��5      $��1��6"   $��	92,0,47,0q�6/   5      �� 5   
   -   5        $��canvas1��5   1   $��1��6   $��1��6    $��0,0,89,2q�6@   .+�5      =   7Microsoft.Samples.Kinect.WpfViewers.KinectSensorChooser  -   5        $��kinectSensorChooser1��5   *   $	��296��6F   $	��232��6R    �� Left$ 14��6_   
 �� Top$ 48��6p   ;   5Microsoft.Samples.Kinect.WpfViewers.KinectColorViewer  5      -   5        $��kinectColorViewer1��5   5   $	��317��6(   $	��520��6O   $ 0��6[   $ 49��6k   �� 5      -   5        $��label1��5   N   $%��Skeleton checked! You can play!��6   $��43��6B   $	��327��6\   $��20(�6h   $ 6��6v   $ 0��6�   5      �� 5   
   -   5        $��label2��5   n   $��2��6   $��Center=�6   $��40(�6:   
�������$��There is��6[   $��
393,0,86,0q�6|   $	��164��6�   �� 5   
   -   5        $��label3��5   k   $��2��6   $��Center=�6   $��40(�6:   
�������$��left.��6[   $��	0,0,468,0q�6y   $	��164��6�   $��1��6�   �� 5   
   -	   5        $��label4��5   h   $��2��6   $��Center=�6   $��40(�6:   
�������$��30��6[   $��	563,0,0,0q�6v   $��80��6�   5      5      