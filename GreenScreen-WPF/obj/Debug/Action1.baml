   M S B A M L   `   `   `  ����  GreenScreen-WPF5    /Microsoft.Samples.Kinect.GreenScreen.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  
flash in 1��5   	   $	��735��6   $	��770��6)    � 	Resources 5         MediumGreyBrush �� Color  KinectPurpleBrush  KinectBlueBrush �� 
TargetType �� Property �� Value  SnapshotButton �� 
TargetType �� Orientation ��Name	 �� TextAlignment
 ��Name �� Triggers P� Property P� Value �� 
TargetName  SquareCheckBox �Name��%w
  &	        &	 #     &	 F     '�� i     &	 �     7  7  &	 �    7  7  �� 5   
   $ 	#ff6e6e6e��62   �� 5   
   $ 	#ff52318f��64   �� 5	   
   $ 	#ff00BCF2��62   �� 5
   
   " ��6   .�5      ��  �@J�SnapsToDevicePixels . 64   5      �� 5   
   " ��6   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      � .+�5      �� $ 
Horizontal=�6)   
������ .+�5   "   �� $ SnapText��6,   #��z��6>   $
	 Left=�6_   $��Center=�6u   8��  $��15(�6�   � 5   "   $��9,0,0,0q�6'   .+�5   &   �� $
 
SnapNormal��6,   $��Images\SnapNormal.png��6@   $
��None=�6_   $��Center=�6n   �� 5   &   $
 	SnapHover��6,   $��Images\SnapHover.png��6?   $
��None=�6]   $��Center=�6l   $y�	Collapsed=�6�   5   #   5      5       5      P� 5       �@J�IsMouseOver . . �5   "   �� $ 
SnapNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5   "   $ 	SnapHover��6O    � y�$ Visible=�6?   6O   �� 5   "   8  $ SnapText��6h    � ��5      5!      5#      5$      �� 5&   
   " ��6   .�5'      ��  � �� 5(      �� 5)      " ��6&   .R�5*      � .+�5+      �� $ 
Horizontal=�6)   
������ .+�5,   "   � $ SquareCheckBoxChecked��6'   .+�5-   &   �� $
 CheckedNormal��6,   $��Images\CheckedNormal.png��6C   $
��None=�6e   $��Center=�6t   �� 5.   &   $
 CheckedHover��6,   $��Images\CheckedHover.png��6B   $
��None=�6c   $��Center=�6r   $y�	Collapsed=�6�   5/   #   � 50   "   $ SquareCheckBoxUnchecked��6'   $y�	Collapsed=�6H   .+�51   &   �� $
 UncheckedNormal��6,   $ ��Images\UncheckedNormal.png��6E   $
��None=�6i   $��Center=�6x   �� 52   &   $
 UncheckedHover��6,   $��Images\UncheckedHover.png��6D   $
��None=�6g   $��Center=�6v   $y�	Collapsed=�6�   53   #   �� 54   "   $ SquareCheckBoxText��6,   #��z��6H   $
	 Left=�6i   $��Center=�6   8��  $��15(�6�   $��9,0,0,0q�6�   55      56       57      P� 58       �@d�	IsChecked$ falseY�6;   . �59   "   �� $ SquareCheckBoxChecked��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5:   "   $ SquareCheckBoxUnchecked��6O    � y�$ Visible=�6?   6O   5;      P� 5<       �@J�IsMouseOver . . �5=   "   �� $ CheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5>   "   $ UncheckedNormal��6Q    � y�$ 	Collapsed=�6?   6Q   �� 5?   "   $ CheckedHover��6O    � y�$ Visible=�6?   6O   �� 5@   "   $ UncheckedHover��6O    � y�$ Visible=�6?   6O   �� 5A   "   8  $ SquareCheckBoxText��6h    � ��5B      5D      5F      5G      5H      .��5J      � -   5        $��
layoutGrid��5J      $��	10 0 10 0q�6    � RowDefinitions 5K   
   �� 5L      $
��Auto �6   �� 5M      $��* �6   �� 5N      $
��Auto �6   �� 5O      $
��Auto �6   .+�5Q   
   ]� $��	0,0,0,574q�6   ���#00BCF2$��1��6<   .+�5R      �� $��Images\Status.png��6   $
��None=�6/   $��Center=�6>   $��0 0 0 5q�6[    J� RenderTransformOrigin$ 1.342,0.554&�6l   5S      � 5T   
   $��1��6   $��0 0 0 20q�6   .+�5U      �� -   5        $
 Backdrop��5U      $��Images\Background.png��6'   $��
0,46,0,-46q�6F   � 5V      -   5        $ 	FlashGrid��5V      $��10,106,409,112q�6&   $
��Left=�6>   $��Center=�6Y   �� 5W      -   5        $
 MaskedColor��5W      $��442,106,27,112q�6*   $��Right=�6B   $��Center=�6^   5X      �� 5Y   
   -   5        $��	statusBar��5Y   ?   $��3��6   $��Stretch=�6!   $��Bottom=�6P   
�������#��[  6~   .@�5Z      �� $��0 0 0 10q�6   .��5[      �� -   5        $��statusBarText��5[      $��-1 0 0 0q�61   JHPress 'Screenshot' to save a screenshot to your 'My Pictures' directory.6�   5\      5]      �� 5^   
   $
��Left=�6   $��	75,10,0,0q�60   $	��Top=�6D   �� 5`   
   $��Right=�6   $��
423,10,0,0q�60   $	��Top=�6D   �� 5a   
   $��RETURN��6   $
��Left=�6#   $	��Top=�6>   $��86��6V   $��8,8,0,0q�5b      $��74��6"   $ 0.854,0.225&�6.   $��1��6S   ��5d   
   �� 5e       �� ImageSource$2 ,/GreenScreen-WPF;component/Images/Status.png��6   5g      5h      5i      