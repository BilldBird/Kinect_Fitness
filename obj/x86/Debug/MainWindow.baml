   M S B A M L   `   `   `  ����P  LCoding4Fun.Kinect.Wpf, Version=1.5.0.0, Culture=neutral, PublicKeyToken=nullnKclr-namespace:Coding4Fun.Kinect.Wpf.Controls;assembly=Coding4Fun.Kinect.WpfCoding4Fun.Kinect.Wpf.Controls  5       KinectUserInterfaceDemo   KinectHubDemo.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   `ControlsKclr-namespace:Coding4Fun.Kinect.Wpf.Controls;assembly=Coding4Fun.Kinect.Wpf     5      -   5          � WindowState$  	Maximized=�5   	    � WindowStyle$ SingleBorderWindow=�5   	   
��� ��� � Title$ Kinect Hub Demo��5      $	��937��6   $
��1508��6(   .��5	      � -   5         �Name$ theGrid��5	      .+�5
      �� -   5        
������  � DataContext 5
   $   �� .+�5      0   *Coding4Fun.Kinect.Wpf.Controls.HoverButton  -   5          Name$ kinectButton��5   1   $��0q�6   $��0q�6%      	ImageSize$ 64Q�6G      ImageSource$ /Resources/Hand_Basic.png��5   !      ActiveImageSource$ /Resources/Hand_Basic.png��5   !   	   TimeInterval$
	 2000��5   !   
 �� Left$	
 525��66   
 �� Top$ 32��6H    K� ZIndex$
 1000��6X   �� 5      $��	0,0,12,12q�6   .+�5      �� -   5        $��button1��5   H   $
 45��6   $	 112��6*   $	��200��6;   $	��338��6W   $��Right=�6c   $��Bottom=�6�   ��5      �� 5       �� ImageSource$G A/KinectUserInterfaceDemo;component/Resources/images/exercise1.PNG��6%   5      �� 5      -   5        $��button2��5   I   $	
 455��6   $	 112��6+   $	��200��6<   $	��338��6X   $��Right=�6d   $��Bottom=�6�   ��5      �� 5      $G A/KinectUserInterfaceDemo;component/Resources/images/exercise2.PNG��6%   5      �� 5      -   5        $��button3��5   I   $	
 853��6   $	 112��6+   $	��200��6<   $	��338��6X   $��Right=�5      $��Bottom=�6&   ��5      �� 5      $G A/KinectUserInterfaceDemo;component/Resources/images/exercise3.PNG��6%   5      �� 5       -   5        $��button4��5    H   $
 45��6   $	 345��6*   $	��200��6;   $	��338��6W   $��Right=�5!      $��Bottom=�6&   ��5"      �� 5#      $G A/KinectUserInterfaceDemo;component/Resources/images/exercise4.PNG��6%   5%      �� 5&      -	   5        $��button5��5&   I   $	
 452��6   $	 345��6+   $	��200��6<   $	��338��6X   $��Right=�5'      $��Bottom=�6&   ��5(      �� 5)      $G A/KinectUserInterfaceDemo;component/Resources/images/exercise5.PNG��6%   5+      �� 5,      -
   5        $��button6��5,   I   $	
 850��6   $	 345��6+   $	��200��6<   $	��338��6X   $��Right=�5-      $��Bottom=�6&   ��5.      �� 5/      $G A/KinectUserInterfaceDemo;component/Resources/images/exercise6.PNG��6%   51      52      �� 54      -   5        $��
textBlock3��54   g   $
 48��6   $ 32��6)   $��46(�69   
�������$	��101��6Z   $��Kinect Fitness��6y   $	��448��6�   �� 56      -   5         ��Name$ textTip��56   E   $��24(�6   
�������$��35��69   $!��Tip: wave for kinect cursor��6V   $	��318��6y   $	
 903��6�   $	 574��6�   �� 57      -   5        $��
videoImage��57      $	��223��6&   $	��335��63   $	
 853��6?   $	 615��6Q   6d   � 59      -   5        $��progressBar1��59   J   $

 1269��6   $	 233��6-   $��19��6>   $	��184��6^   5F      �� 5G      -   5        $��button7��5G   Q   $

 1269��6   $	 100��6(   $�� ��69   $	��107��6D   $	��114��6`   6l   �� 5H      -   5        $��label1��5H   R   $

 1269��6   $	 204��6'   $	��Lv.��68   $��28��6F   $��69��6`   
�������$
��Bold"�6~   �� 5I      -   5        $��label2��5I   P   $

 1296��6   $	 204��6'   $��0��68   $��28��6D   $��41��6^   
�������$
��Bold"�6|   5L      5M      5O      