   M S B A M L   `   `   `  ����J  FWpfApplication1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:HostelManagmentHostelManagment  5   	       HostelManagment.Dashboard   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:HostelManagment     5   	     � Title$  	Dashboard��5   	   $	��300��6   $��578.606��6(   .��5	      � $��0,0,2,0q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   o� $��Stretch=�6   $	��Top=�6-   ���	#FFF99797$��1,0,3,0q�6\   .@�5      m� -   5        $��	Dashboard��5      6*   m� 5      $��Members��6   .@�5      m� -   5        $��List Member��5      60   m� 5      -   5        $��
Add Member��5      6/   5      m� 5      -   5        $��Par Days Count��5      6/   5      &   System.Windows.Controls.DataGrid  5   
   -   5        $��DemoDataGrid��5       $��1��6      AutoGenerateColumns .  $��
1,32,0,-22q�6P      
IsReadOnly .    Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$ SL��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ 20 6H   1  +System.Windows.Controls.DataGridBoundColumn   Binding 61   �� *Id+6H     5      $
 Name��6%   $ 60 6R    63   �� *
MemberName+6R     5      $ Total��6%   $ 30 6R    64   �� *	TotalCost+6R     5      $
 Paid��6%   $ 30 6P    63   �� *
PaidCost+6P     5       $	 Due��6%   $ 40 6N    62   �� *	DueCost+6N     5!      $ 
Total Mill��6%   $ 60 6W    69   �� *	TotalMill+6W     5"      $ 
Fixed Cost��6%   $ 67 6W    69   �� *	FixedCost+6W     5#      $
 Date��6%   $	 110 6U    63   �� *FormattedDate+6U   4  .System.Windows.Controls.DataGridTemplateColumn  5&      $ Actions��6)   $	 150 6:      CellTemplate 5'      �� 5(      .R�5)      �� 	 �� Orientation$	 
Horizontal=�6)   .+�5*   "   �� -   5        $
��Edit��5*   )   $��60��68   $	��5,0q�6C   

 � Tag
 6i   �� *Id+�� 5+   "   -   5        $��Delete��5+   )   $��60��6:   $	��5,0q�6E   
 6m   �� *Id+5,      5-      5/      51      �� 52   
   -   5         ��Name$ button��52      $��Add A New Summary��6!   $
��Left=�6=   $��	434,7,0,0q�6X   $��1��6k   $	��Top=�6x   $	��125��6�   ���	#FFFDFDFD���	#FFF353536�   54      55      