   M S B A M L   `   `   `  ����C  ?WmsPrism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullU/clr-namespace:WmsPrism.Views.StoreBillIofLading!WmsPrism.Views.StoreBillIofLading  5      ;    5WmsPrism.Views.StoreBillIofLading.BillofLadingDetails   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Alocal/clr-namespace:WmsPrism.Views.StoreBillIofLading     5      S OPrism.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59Z VPrism.DryIoc.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/   5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  True5      $	��900��5	      $	��600��6+    � 	Resources 5
         ContentStyle �� 
TargetType �� Property �� Value��%�   &	        �� 5   
   " ��5      .�5      ��  � ��$ Center=�5      �� 5       � ��$ Center=�62   5      5      .��5      �  � RowDefinitions 5   
   �� 5      6   �� 5      $��50 �6   6*   .+�5   
   �� -   5         ��Name$ LodingBorder��5      $��0��6(    J� Opacity$	 0.8Q�65   ���#333333 K� ZIndex$
 9999��6X   $y�	Collapsed=�6m   ._�5      �� $��1��6   	 �� Orientation$	 
Horizontal=�6'   $��Stretch=�6@   $��Center=�6\   .+�5      � -   5        
 �Name$	
 bar��5      $��60��5      $��60��5       % !MaterialDesignCircularProgressBar#���  5      ���#1E90FF � IsIndeterminate$
 True��5        � Value$ 0Q�5!      5"      5#      &   System.Windows.Controls.DataGrid  5&   
      ColumnWidth5  /System.Windows.Controls.DataGridLengthConverter$ * 5'      $	��500��5(         AutoGenerateColumns$ False��5)         CanUserAddRows$ False��5*      ��6*   �� *BillOfLadingDetailsDto+   ColumnHeaderStyle 5,      �� 5-      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5.      ��  � ��$ 0q�68   �� 5/       � ��$ Center=�6:   �� 50       � ��$ Center=�6C   �� 51       � �� �	#FFF2F2F263   �� 52       � �� �	#FF66666663   �� 53       � ��$
 Bold"�63   �� 54       � ��$ 35��6/   55         RowStyle 58      �� 59      )  #System.Windows.Controls.DataGridRow"  6   .�5:      ��  � ��$ 0q�68   �� 5;       � ��$ 40��6/   �� 5<       � ��
 �����63   �� 5=       � �� �	#FF66666663   �� 5>       � ��$ Center=�6<   �� 5?       � ��$ Center=�6C    �� Triggers 5A      P� 5B       P� Property �@J�IsMouseOver P� Value$
 True��69   . �5C      ��  � ��
 �����6;   5D      P� 5E       �@ 
IsSelected$
 True��68   . �5F      ��  � ��
 �����6;   5G      5I         Columns 5M      0  *System.Windows.Controls.DataGridTextColumn  5O      ,  &System.Windows.Controls.DataGridColumn   Header$ 提单编号��6%   1  +System.Windows.Controls.DataGridBoundColumn   ElementStyle# [  63      
IsReadOnly$
 True��6`      Binding 6r   �� *	Bill_id+  5P      $ 	标签码��6%   # [  64      Width$	 180 6a   $
 True��6m    6�   �� *	BarCode+  5Q      $ 	面单号��6%   # [  64   $
 True��6a    6t   �� *
Waybill_no+  5R      $ 重量��6%   # [  65   $
 True��6c    6u   �� *Weight+  5S      $ 标签总数��6%   # [  63   $
 True��6`    6r   �� *Num+  5T      $ 进仓时间��6%   # [  63   $	 190 6a   $
 True��6m    6   �� *
IntimeData+5Z      5�      5�      