   M S B A M L   `   `   `  ����H  DVridge.Kinect, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:Vridge.KinectVridge.Kinect  5   	       Vridge.Kinect.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:Vridge.Kinect     5   	     � Title$  Vridge.Kinect��5   	   $��207.85��6   $��452.642��6/    � 
ResizeMode$ NoResize=�6?   .��5	      � $��0,0,4,0q�6    � RowDefinitions 5
   
   �� 5      $��1* �6   �� 5      $��20 �6    � ColumnDefinitions 5   
   �� 5      .+�5   
   �� $��	5,5,197,0q�6   $��0��6(   .+�5      ��  �� Orientation$ 
Horizontal=�6   $��
0, 0, 0, 5q�62   .+�5      �� $
��Left=�6   $��Kinect Type��67   $��Center=�6J   $	��100��6e   �� 5      -   5         ��Name$ KinectTypeCombo��5   9   $	��115��6    �� SelectedIndex$ 0��6'   .@�5      �� .��6#   
Xbox One6-   �� 5      .��6#   
Xbox 3606-   5      5      �� 5      -   5         ��Name$ SendLeftPosition��5   E   $��Send Left Position��6    d� 	IsChecked$
 TrueY�64   6_   �� 5      -   5        $ SendLeftRotation��5   E   $��Send Left Rotation��6   $
 TrueY�64   6_   �� 5      -   5        $ SendRightPosition��5   F   $��Send Right Position��6   $
 TrueY�65   6a   �� 5      -   5        $ SendRightRotation��5   F   $��Send Right Rotation��6   $
 TrueY�65   6a   �� 5      -   5        $ DisableHeadPosition��5   @   $��Upright Hands��6   $
 TrueY�6/   6]   �� 5      $ 
Horizontal=�6   $��5q�63   .+�5      �� -   5        $��Connect��5      
	 � Tag$	 Connect��6+   $	��100��69   $��0, 0, 15, 0q�6E   6u   �� 5       -   5        $��Close��5       $	 Close��6)   $	��100��65   6A   5!      5"      �� 5#   
   $��5q�6   $��1��6   $��1��6)   ._�5$      �� 5&      5'      �� 5(   
   $ 
Horizontal=�6   $��2��6.   
�������.+�5)      �� -	   5        
 ��Name$
 ConnectionStatus��5)   -   $��Not Connected��6   $��Right=�6G   5*      �� 5+   
   $
��Left=�6   $	��143��6,   $��
246,10,0,0q�69   $	��Top=�6M   $	��187��6e   5,      5-      