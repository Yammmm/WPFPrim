   M S B A M L   `   `   `  ����C  ?WmsPrism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:WmsPrism.Views.BingCargoWmsPrism.Views.BingCargo  5      ,    &WmsPrism.Views.BingCargo.BingWayBillNo   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:WmsPrism.Views.BingCargo     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      S OPrism.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59Z	 VPrism.DryIoc.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/  	 5      _
 [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors 
 5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  True5
      .��5      �� 
������� �� CornerRadius$ 10��6!   ._�5   
   �  � RowDefinitions 5      �� 5      .+�5      � $��1��6   $��15 0 0 0q�6     5      �� 5      $��70 �6$   �� 5      $��60 �6$   �� 5      $
��auto �6$   .+�5      �� $��0��6    �� TextAlignment$ Center=�6*   $��Center=�6A   $��26(�6\   $
��Bold"�6j   $��标签绑定面单��6|   6�   �� 5      $��2��6   $
��Left=�6*   $��Center=�6E   $��5q�6`   .+�5      �� -   5         ��Name$ 
BillComBox��5$   1   $	��250��5      $��30��6+    �� SelectedValuePath$	 Key��5   #    �� DisplayMemberPath$ Value��6;      MaterialDesignComboBox#��[  5"   #   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 	提单号��5#   #   $��15(�5$   #   ��5   #   �� *SelGroupList+ �� SelectedValue 5    #   �� *Group+	 �� UpdateSourceTrigger$	 PropertyChanged=�* 
 $Microsoft.Xaml.Behaviors.Interaction
   Triggers
 5%      + 
 %Microsoft.Xaml.Behaviors.EventTrigger  5&         	EventName$ SelectionChanged��6-   * 
 $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5'   "   2 
 ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 68   ��  �� Path$ ComboxChangedCommand�   CommandParameter 5(   ,   ��  �� ElementName$ 
BillComBox��5)      5+      �� 5/      -   5        $��
BarCodeTxt��51   )   $	��250��5/      $ 标签条码��50      $��15(�51       % !MaterialDesignFloatingHintTextBox#��[ 53      ��52      �� *	BarCode+.��54       J� InputBindings 55      �� 56      
 �� Key$ Enter��6)    �� Command 65   �� $ BarCodeEnterCommd�.��5:   1   5<      �� 5=      -   5        $��
WayBillTxt��5?   )   $	��250��5=      $ 面单号码��5>      $��15(�5?      #��[ 5A      ��5B      �� *	WayBillNo+.��66    5D      .��5I   1   5J      �� 5L      $��30��65   $��0 10 0 0q�6A   ���#B22222$��Center=�6i   $��Center=�6�   ��6    �� *Msg+6�   �� 5N      $	��350��6!    �� Orientation$ 
Horizontal=�6-   $��Center=�6F   .+�5O      �� $��添加��6!   $��75 0 0 0q�6o   $��90��6�   $��30��6�    � Cursor$
 Hand��6�   ��6.   �� *AddRichTxtCommd+{�6R   �� *IsAdd+6�   �� 5P      $��删除��6!   $��15 0 0 0q�6r   $��90��6�   $��30��6�   $
 Hand��6�   ��6.   �� *DelRichTxtCommd+{�6R   �� *
IsCancel+6�   5Q      5R      �� 5T      $��2��6   $
��Left=�6*   $ Vertical=�6E   .+�5U      �� -   5        $��richTextBox��5W      $��1 1 1 1q�5V      $
��Left=�5X      $	��450��5Z      $��Center=�5[      $	��650��60   $��	379,0,0,0q�6<   . �5]      /� .T�5^      J� .*�6(   .*�5_   "   �� $��14(�6'   �65   �� *	RichTxt+6Q   5`      5a      5b      �� 5c      $��保存��6   $��Right=�6*   $��
10 15 10 0q�6�   $��90��6�   $��30��6�   $
 Hand��6�   ��6F   �� *	SaveBtn+{�6b   �� *
IsCancel+6�   5e      5f      5g      5h      5i      