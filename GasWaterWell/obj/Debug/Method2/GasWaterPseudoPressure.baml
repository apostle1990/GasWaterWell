   M S B A M L   `   `   `  ����G  CGasWaterWell, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:GasWaterWell.Method2GasWaterWell.Method2  5   	   1    +GasWaterWell.Method2.GasWaterPseudoPressure   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:GasWaterWell.Method2     5   	     � Title$5  /单点法二项式法-气水两相-拟压力法��5   	   $��409.1��6#   $	��800��62    � WindowStartupLocation$ CenterScreen=�6>   .��5	      � .+�5
   
   �� $��输入��6   $
��Left=�6   $��62��6:   $��	15,17,0,0q�6F   $	��Top=�6Y   $	��772��6q   .��5      � $��0,0,0,0q�6   .+�5      �� -   5         ��Name$ Product_Button��5      $��导入生产数据��61   $
��Left=�6B   $��	35,10,0,0q�6]   $	��Top=�6p   $	��156��6�   $��22��6�   6�   �� 5      -   5        $ 
Cal_Button��5      $��计算��6-   $
��Left=�6:   $��
595,10,0,0q�6U   $	��Top=�6i   $	��156��6�   $��22��6�   {�.  6�   5      5      �� 5   
   $��计算结果��6   $
��Left=�6!   $	��245��6<   $��	11,84,0,0q�6I   $	��Top=�6\   $	��772��6t   .��5      &   System.Windows.Controls.DataGrid  -   5          Name$ OutputDataGrid��5      $��Stretch=�6/   $��0,0,0,2q�6M   6^   5      �� 5   
   -   5        $ Output_Button��5      $��导出��6(   $
��Left=�65   $��
15,338,0,0q�6P   $	��Top=�6d   $��74��6|   {�.  $��19��6�   �� 5   
   -   5         ��Name$ Output_ComboBox��5      $
��Left=�6,   $��
95,338,0,0q�6G   $	��Top=�6[   $	��104��6s   $��21��6   .@�5      ��  �� 
IsSelected . $��导出Excel��6-   �� 5      $��	导出Csv��6   5      �� 5   
   -   5        $ Save_Button��5      $��保存��6&   $
��Left=�63   $��622,334,0,0q�6N   $	��Top=�6c   $��76��6{   $��19��6�   {�.  6�   �� 5   
   -   5        $��取消��5      $
��Left=�6   $��708,334,0,0q�69   $	��Top=�6N   $��74��6f   $��19��6�   5      5      