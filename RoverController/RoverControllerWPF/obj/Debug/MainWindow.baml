   M S B A M L   `   `   `  ����M  IRoverControllerWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:RoverControllerWPFRoverControllerWPF  5   	   #    RoverControllerWPF.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:RoverControllerWPF     5   	   -   5          �Name$  RoverControllerWindow��5   	    � Title$ Rover Controller��5   	   $
��1280��6"   $	��960��6/   
���iii� � Icon$ sliderButton_140.png��5	   �    � WindowState$ 	Maximized=�6�   $
��1280��6�   $	��956��6�   .��5      � -   5        $��mainGrid��5       � ShowGridLines .  � ColumnDefinitions 5   
   �� 5      $	��160 �6   �� 5      $��* �6   �� 5      $	��160 �6    � RowDefinitions 5   
   �� 5      $��10 �6   �� 5      $
��700* �6   �� 5      $	��100 �6   .+�5   
   �� -   5        $��canvasRight��5      $��Stretch=�5      $��Stretch=�6-   $��1��5      $��2��6   .+�5      �� -   5        $��imgRightTrack��5      $	��140��5      $	��600��6    
 �� Top$ 10��5       �� Right$ 10��6$   $��Stretch=�5      $��Stretch=�60   $��Uniform=�6N   $��
tracks.png��5      5      �� 5!      -   5        $��rightSlider��5!      $	��140��5"      $	��140��6!   $	 300��5#      $ 10��6&   ��5(      �� 5)      	 �� ImageSource$	 sliderButton_140.png��6!   
 j� Stretch$

 Fill=�6D   5+      5,      �� 5-   
   -   5        $��
canvasLeft��5-      $��Stretch=�5.      $��Stretch=�6-   $��1��5/      $��0��6   .+�52      �� -   5        $��imgLeftTrack��52      $	��140��53      $	��600��6    $ 10��54       �� Left$ 10��6$   $��Stretch=�55      $��Stretch=�60   $��Uniform=�6N   $��
tracks.png��56      57      �� 59      -   5         ��Name$ 
leftSlider��59      $	��140��5:      $	��140��6   $	 300��5;      $ 10��6   ��5@      �� 5A      $	 sliderButton_140.png��6!   5C      5E      �� 5G   
   -	   5        $��
stkButtons��5G       �� Orientation$ 
Horizontal=�5H      $��0,0,0,0q�5I      $��Center=�5J      $��Bottom=�62   $��2��5K      $��1��6"   .+�5N      �� -
   5         ��Name$ btnTCPConnect��5N      $��Connect��6,   $	��150��6>   $��90��6J   $��5,0,5,5q�6V   $��24(�6g   
�������6�   �� 5O      -   5        $ 	btnMotors��5O      $��Motors��6(   $	��150��69   $��90��6E   $��5,0,5,5q�6Q   $��24(�6b   
�������6�   �� 5P      -   5        $ btnIRLED��5P      $��IRLED��6'   $	��150��67   $��90��6C   $��5,0,5,5q�6O   $��24(�6`   
�������6�   �� 5Q      -   5        $ btnLED��5Q      $	��LED��6%   $	��150��63   $��90��6?   $��5,0,5,5q�6K   $��24(�6\   
�������6�   �� 5R      -   5        $ btnVideo��5R      $��Video��6'   $	��150��67   $��90��6C   $��5,0,5,5q�6O   $��24(�6`   
�������6�   �� 5S      -   5        $ btnShutdown��5S      $��Shutdown��6*   $	��150��6=   $��90��6I   $��5,0,5,5q�6U   $��24(�6f   
�������6�   5T      �� 5V   
   -   5         ��Name$ 	imgStream��5V      $��1��5W      $��1��6   $��Stretch=�5X      $��Stretch=�6,   $
��Fill=�6J   5[      5\      