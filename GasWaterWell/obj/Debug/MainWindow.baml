   M S B A M L   `   `   `  ����G  CGasWaterWell, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:GasWaterWellGasWaterWell  5   	       GasWaterWell.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:GasWaterWell     5   	   -   5          � Title$&   气水两相法产能分析-GWDA��5   	   $��796.263��5	   	   $��1175.468��6    � WindowStartupLocation$ CenterScreen=�5
   	    � WindowState$ 	Maximized=�5   "    � 	Resources 5      ��%    5#      .��5$      ]� .+�5%   
   o� $	��Top=�6   $
��30px��6$   .@�5&      m� $��_文件��6   .@�5'      m� -   5        $��CreateProject��5'   '   $��新建��6   6<   m� 5(      -   5        $��OpenProject��5(   '   $��打开��6   6:   �� 5)      m� 5*      -   5        $��CloseProject��5*   '   $��退出��6   6;   5+      m� 5,      $��_视图��6   5-      m� 5.      -   5        $��_设置��5.      5/      m� 50      $��帮助��6   51      52      � 53   
   $	��Top=�6   .+�54      c� -   5         c�Name$ toolBar��54      $	��Top=�6(   $��Stretch=�6>   $��Stretch=�6\    55       �� 
TargetType �� Property �� Value��%d   'a�       �� 56      " a�6   .�57      ��  �@��Orientation$ Vertical=�68   58      59      .@�5;      ]� .+�5<      a�  �� Orientation$ 
Horizontal=�6#   .+�5=      �� -   5        $��新建项目��5=   "   61   �� 5>      -   5        $��打开项目��5>   "   61   5?      a� 5@      $ 
Horizontal=�6#   .+�5A      �� -	   5        $��	新建井��5A   "   60   �� 5B      -
   5        $��打开气水相渗��5B   "   63   5C      �� 5D      -   5        $��Connect_Button��5D      $��Right=�63   $��Right=�6J   $��83��6f   ���	#FFFF572B
�������$��25��6�   .��6�   	未连接6�   5E      5F      5G      � 5H   
   $	��Top=�6   	 � ColumnDefinitions	 5I      �� 5J      $��1* �6#   �� 5K      $��4 �6#   �� 5L      $��4* �6#   $	��700��6/   
 � RowDefinitions
 5N      �� 5O      $��* �6    6-   �� 5P      $��30 �6    .+�5R      � $��5q�6   $��0��6   
 5S      �� 5T      $��4* �6$   62   �� 5U      $��4 �6$   61   �� 5V      $��3* �6$   62   .+�5X      R� -   5        $��proSourceTree��5X   2   $
��Left=�6   ��5Z      �� 5[       �� ItemsSource 64   ��  �� XPath$ *��.R�5\      �� ��6(   �� $ @Name��
 � Tag 6E   �� $	 @Id��6a   5]       5_         	RightMenu��%!   &	        �� 5`      5a      5b      ��5c      �� 5d      " Q�6     �� 	Resources 5e       �� Color��%�  (��       5f   2   * SystemColors.HighlightBrushKey+)(�� $     5g   2   *$"SystemColors.HighlightTextBrushKey+)(�� C     5h   2   *1/SystemColors.InactiveSelectionHighlightBrushKey+)(�� g     5i   2   *53SystemColors.InactiveSelectionHighlightTextBrushKey+)�� 5f   "   $ 
DodgerBlue��6d   �� 5g   "   $ White��6h   �� 5h   "   $ 
DodgerBlue��6u   �� 5i   "   $ White��6y   5j      .�5k      ��  �@Q�
IsExpanded . 6;   -   5        5p      5r      �� 5t      $��2��6   $��
0,10,0,-14q�6'   $��1q�6;   
�������$	��Top=�6f   $	��287��6~   ._�5u      �� $��	5,5,5,-14q�6!   $	��280��64   $	��Top=�6A   .+�5v      9� -   5        $��wellProperty��5v   #   
����������	#FFF1EDED$��Stretch=�6e    9� ExpandDirection$
 Down=�6�   $��Stretch=�6�    J� SnapsToDevicePixels . ��5w      �� 5x   "   $ 
Horizontal=�6-   $	��242��6G   .+�5y   &   �� $��属性��60   $��14(�6;   $
��Bold"�6I   $��Stretch=�6\   $��Stretch=�6z   5z   #   ��5|      � 5}   "   ���#FFF$	��106��69   $��0,0,1,0q�6F   	 5~   &   �� 5   *   $	��33* �6;   6I   �� 5�   *   $	��34* �6;   6I   
 5�   &   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   .+�5�   &   �� $��13,5,-55,-9q�60   $��0��6E   $��0��6R   	名称：6g   �� 5�   &   -   5        $��wellName��5�   0   $��	48,5,41,5q�6@   $��0��6S   $��0��6`   6r   �� 5�   &   $��13,5,-55,-9q�60   $��1��6E   $��0��6R   	代码：6g   �� 5�   &   -   5        $��wellCode��5�   0   $��	48,5,41,5q�6@   $��1��6S   $��2��6`   6v   �� 5�   &   $��13,5,-55,-9q�60   $��2��6E   $��0��6R   pvt：6h   �� 5�   &   -   5        $��pvtName��5�   0   $��	48,5,41,5q�6?   $��2��6R   $��2��6_   6u   �� 5�   &   $��13,5,-55,-9q�60   $��3��6E   $��0��6R   所在地：6h   �� 5�   &   -   5        $��wellAddress��5�   0   $��72,7,4,3q�6C   $��3��6U   $��2��6b   6x   �� 5�   &   $��13,5,-55,-9q�60   $��4��6E   $��0��6R   	概况：6g   �� 5�   &   -   5        $��
wellDetail��5�   0   $��72,7,4,3q�6B   $��4��6T   $��2��6a   6w   5�   #   5�      9� 5�      
����������	#FFF1EDED$��Stretch=�6Q   $
 Down=�6m   $��Stretch=�6�    . ��5�      �� 5�   "   $��1q�6)   ���	#FFF1EDED$	��242��6U   ._�5�   &   �� $��	PVT计算��60   $��14(�6=   $
��Bold"�6K   $	��5,0q�6]   5�   #   ��5�      � 5�   "   ���#FFF$��35��69   	 5�   &   �� 5�   *   $	��33* �6;   6I   �� 5�   *   $	��33* �6;   6I   �� 5�   *   $	��33* �6;   6I   .+�5�   &   �� -   5        $��0��5�   -   $��5,5,5,5q�6=   ���	#FFF1EAEA.��6~   天然气性质6�   �� 5�   &   -   5        $��1��5�   -   $��5,5,5,5q�6=   ���	#FFF1EAEA.��6�   岩石性质6�   �� 5�   &   -   5        $��2��5�   -   $��5,5,5,5q�6=   ���	#FFF1EAEA.��6�   地层水性质6�   5�   #   5�      9� 5�      -   5        $��
wellParams��5�   #   
����������	#FFF1EDED$��Stretch=�6c   $
 Down=�6   $��Stretch=�6�    . ��5�      �� 5�   "   $ 
Horizontal=�6-   $	��242��6G   .+�5�   &   �� $��基础参数��60   $��14(�6=   $
��Bold"�6K   $
��Left=�6^   $��Stretch=�6y   5�   #   ��5�      � 5�   "   ���#FFF$	��200��69   $��0,0,1,0q�6F   	 5�   &   �� 5�   *   $	��33* �6;   6I   �� 5�   *   $	��34* �6;   6I   
 5�   &   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   �� 5�   *   $
��auto �68   $��24��6F   .+�5�   &   �� $��13,5,5,5q�60   $��0��6B   $��0��6O   	名称：6d   �� 5�   &   -   5        $��	wellName1��5�   0   $��	48,5,41,5q�6A   $��0��6T   $��0��6a   6s   �� 5�   &   -   5        $��	modifyBtn��5�   -   $��0,1,5,5q�6?   $��0��6P   $��1��6]   $��Right=�6m   ���	#FFF1EAEA{�.  .��6�   修改6�   �� 5�   &   $��13,5,-45,-9q�60   $��1��6E   $��0��6R   气井表皮系数：6k   �� 5�   &   -   5        $��params_s��5�   0   $��	104,5,0,5q�6@   $��1��6S   $��Stretch=�6`   $��2��6~   6�   �� 5�   &   $��13,5,-55,-9q�60   $��2��6E   $��0��6R   非达西渗流系数：6l   �� 5�   &   -   5        $��params_d��5�   0   $��	110,3,1,3q�6@   $��2��6S   $��2��6`   6v   �� 5�   &   $��13,5,-55,-9q�60   $��3��6E   $��0��6R   井控半径：6i   �� 5�   &   -   5        $��	params_re��5�   0   $��74,3,1,3q�6A   $��3��6S   $��2��6`   6v   �� 5�   &   $��13,5,-55,-9q�60   $��4��6E   $��0��6R   井筒半径：6i   �� 5�   &   -   5        $��	params_rw��5�   0   $��72,7,1,5q�6A   $��4��6S   $��2��6`   6v   �� 5�   &   $��13,5,-55,-9q�60   $��5��6E   $��0��6R   气体密度：6i   �� 5�   &   -   5        $��params_rhogsc��5�   0   $��72,7,4,3q�6E   $��5��6W   $��2��6d   6z   �� 5�   &   $��13,5,-55,-9q�60   $��6��6E   $��0��6R   水密度：6h   �� 5�   &   -   5        $��params_rhowsc��5�   0   $��72,7,4,3q�6E   $��6��6W   $��2��6d   6z   �� 5�   &   $��13,5,-55,-9q�60   $��7��6E   $��0��6R   初始压力：6i   �� 5�   &   -    5        $��	params_pe��5�   0   $��72,7,4,3q�6A   $��7��6S   $��2��6`   6v   �� 5�   &   -!   5        $��well_id��5�   0   $��72,7,4,3q�6?   $��8��6Q   $��2��6^   $y�Hidden=�6r   6�   5�   #   5�      5�      5�      5�      �� 5�      $��1��6   $��4��6+   $��Stretch=�65   �� 5�      -"   5         ��Name$ mainTab��5�      $��2��6*   $	 100��6:   
���   �$��0,0,0,20q�6W   $��2��6i    5�       �� 
TargetType ��Name �� Triggers P� Property P� Value �� 
TargetName��%  '��       �� 5�      " ��6   .�5�      ��  � �� 5�      �� 5�   "   " ��62   .R�5�   &   � $��Panel��6+   .+�5   *   �� $ ContentSite��6;   $��Center=�5  )   $��Center=�5  )   $��Header��5  )   $
��10,2q�5  )   5  '    5  &   P� 5  *    �@��
IsSelected . . �5  .   �� $ Panel��65    � ��
 ��·�6^   5
  +   P� 5  *    �@��
IsSelected .  . �5  .   �� $ Panel��65    � ��
 �����6^   5  +   5  #   5     5     5     z�5     V� 5     .~�5     �� �� 5     �� 5     T� 5      T� X$ -2Q�6-   5     5     � 5     $��1��6   $��3��6!   .+�5     �� $��0,10,0,0q�6   5     5     5     5     