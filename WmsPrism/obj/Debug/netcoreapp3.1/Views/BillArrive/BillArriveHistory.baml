   M S B A M L   `   `   `  ����C  ?WmsPrism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:WmsPrism.Views.BillArriveWmsPrism.Views.BillArrive  5      1    +WmsPrism.Views.BillArrive.BillArriveHistory   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      _ [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors  5      3local'clr-namespace:WmsPrism.Views.BillArrive  5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      S	 OPrism.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59Z
 VPrism.DryIoc.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/ 	 
 5      ! 	 Prism.Mvvm.ViewModelLocator    AutoWireViewModel  True5       � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $f `pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Button.xaml?�6%   �� 5      $j dpack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.DialogHost.xaml?�6%   %    5      .��5      )  #MaterialDesignThemes.Wpf.DialogHost  -   5        .��5   
   �� 
������� �� CornerRadius$ 10��6&   ._�5      �  � RowDefinitions 5      �� 5      $��40 �6$   �� 5       $
��auto �6$   �� 5!      �� 5"      $
��auto �6$   .+�5%      � $��1��6    5&      �� 5'      $
��auto �6(   �� 5(      .+�5+      �� $	��750��6   $	��500��6)   
�������$ 10��6J   ._�5-      �� $��	20,80,0,0q�6%    �� Orientation$ Vertical=�68   .+�5.      �� $��30 0 0 0q�6)   $	��450��6;   .+�5/   "   �� -   5        $��TiteText��52      $��26(�50      $
��Bold"�51       �� TextAlignment$ Center=�53      $��提单运抵��54      $��	-50 0 0 0q�55      �� 56   "   $��5,20,0,0q�6-   $ 
Horizontal=�6?   57   #   �� 59   "   $ 
Horizontal=�6.   5:   #   �� 5<   "   $��5,20,0,0q�6-   $ 
Horizontal=�6?   5=   #   �� 5?   "   -   5        $��
BillComBox��5?   9   $��15(�6+   $	��450��6K   $��30��6W    �� SelectedValuePath$	 Key��5@   %   	 �� DisplayMemberPath$	 Value��6=   )  #MaterialDesignThemes.Wpf.HintAssist
   Hint$
 	提单号��5A   +   $��0 0 0 0q�5C   &   ��5?   c   �� *SelGroupList+ �� SelectedValue 5B   &   �� *Group+ �� UpdateSourceTrigger$ PropertyChanged=�*  $Microsoft.Xaml.Behaviors.Interaction   Triggers 5E   &   +  %Microsoft.Xaml.Behaviors.EventTrigger  5F   *      	EventName$ SelectionChanged��69   *  $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5G   .   2  ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 6D   ��  �� Path$ ComboxChangedCommand�   CommandParameter 5H   ,   ��  �� ElementName$ 
BillComBox��5I   +   5K   #   �� 5M   "   -   5         ��Name$ 	ActualTxt��5O   )   $��1��5M   +   $
 实物包装数量��5N      $��15(�5O       %  !MaterialDesignFloatingHintTextBox#��[  5P      ��5Q      �� *
Actual_num+�� 5S   "   -   5        $ 	WeightTxt��5U   )   $��1��5S   +   $
 实物重量(kg)��5T      $��15(�5U      #��[  5V      ��5W      �� *Actual_weight+�� 5Y   "   -   5        $ PlateTxt��5[   )   $��1��5Y   +   $
 运抵车牌��5Z      $��15(�5[      #��[  5\      ��5]      �� *
Plate_no+�� 5`   "   $ 
Horizontal=�6-   $��0 25 0 0q�6F   .+�5c   &   �� $ 
Horizontal=�61   .+�5d   *   �� $
 运抵时间��6H   $	��200��6o   $��15(�6{   ��62   �� *Date+6{   �� 5e   *   $��10 0 0 0q�5f   )   $��选择��5g   )      OpenDialogCommand#��Z 5h   )   $��25��6j   ��5k   .   � 5l   2   $��-1q�67    5m   6   �� 5n   :   $��* �6H   �� 5o   :   $
��Auto �6H   .+�5r   6   �� $��0��5s   9   $ 
Horizontal=�5t   9   .+�5u   :   &   System.Windows.Controls.Calendar  -   5          Name$ CombinedCalendar��5w   A    � Language$ zh-CN�5v   A   $��
-1 -4 -1 0q�5x   A   $	  MaterialDesignThemes.Wpf.Clock	  5y   :   -   5         	 Name$ CombinedClock��5z   =    	  DisplayAutomation$ CycleWithSeconds=�5{   =    	  	Is24Hours$
 True��5|   =   5}   7   �� 5   6   $��1��5�   B   $��8q�5�   B   $ 
Horizontal=�5�   B   .+�5�   :   ��   MaterialDesignFlatButton#���  5�   B      CloseDialogCommand#��Z 5�   B   $��0��5�   B   $��CANCEL��5�   B   �� 5�   :   #���  5�   B   #��Z 5�   B   $��1��5�   B   $��OK��5�   B   5�   7   5�   3   5�   +   5�   '   5�   #   �� 5�   "   $��0 15 0 0q�6F   ���#B22222$
��Left=�6o   ��6,   �� *
ErrorMsg+6�   �� 5�   "   $��提交��6)   $��Center=�6�   $��15q�6�   $��90��6�   $��30��6�   {�66   �� *
IsCancel+��6U   �� *AddWMS_bill_arrive_history+6�   5�      5�      5�      5�      5�      5�      5�      5�      