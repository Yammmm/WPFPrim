   M S B A M L   `   `   `  ����C  ?WmsPrism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullC&clr-namespace:WmsPrism.Views.BingCargoWmsPrism.Views.BingCargo  5      )    #WmsPrism.Views.BingCargo.BingWeight   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      8local&clr-namespace:WmsPrism.Views.BingCargo     5      b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      S OPrism.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59Z	 VPrism.DryIoc.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/  	 5      _
 [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors 
 5      !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  True5
      .��5      �� 
������� �� CornerRadius$ 10��6"   ._�5   
   �  � RowDefinitions 5      �� 5      .+�5      � $��1��6   $��15 0 0 0q�6     5      �� 5      $��70 �6$   �� 5      $��60 �6$   �� 5      $
��auto �6$   .+�5      �� $��0��6    �� TextAlignment$ Center=�6*   $��Center=�6A   $��26(�6\   $
��Bold"�6j   $��采集重量��6|   6�   �� 5      $��2��6   $
��Left=�6*   $��Center=�6E   $��5q�6`   .+�5      �� -   5         ��Name$ 
BillComBox��5#   2   $	��250��5      $��30��6+    �� SelectedValuePath$	 Key��5   #    �� DisplayMemberPath$ Value��6;      MaterialDesignComboBox#��[  5!   #   )  #MaterialDesignThemes.Wpf.HintAssist   Hint$ 提单号码��5"   #   $��15(�5#   #   ��5   #   �� *SelGroupList+ �� SelectedValue 5   #   �� *Group+	 �� UpdateSourceTrigger$	 PropertyChanged=�* 
 $Microsoft.Xaml.Behaviors.Interaction
   Triggers
 5$      + 
 %Microsoft.Xaml.Behaviors.EventTrigger  5%         	EventName$ SelectionChanged��6-   * 
 $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5&   "   2 
 ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 68   ��  �� Path$ ComboxChangedCommand�   CommandParameter 5'   ,   ��  �� ElementName$ 
BillComBox��5(      5*      �� 5,      -   5        $��WeighTxt��5.   )   $	��250��5,      $ 采集重量(Kg)��5-      $��15(�5.       % !MaterialDesignFloatingHintTextBox#��[ 50      ��5/      �� *Weigh+.��51       J� InputBindings 52      �� 53      
 �� Key$ Enter��6)    �� Command 65   �� $ BarCodeEnterCommd�.��55   1   57      �� 59      -   5        $��
BarCodeTxt��5;   )   $	��250��59      $ 标签条码��5:      $��15(�5;      #��[ 5>      ��5=      �� *	BarCode+.��5?       5@      .��5C   1   5E      �� 5G      $��30��65   $��0 10 0 0q�6B   ���#B22222$��Center=�6j   $��Center=�6�   ��6    �� *Msg+6�   �� 5I      $	��350��6!    �� Orientation$ 
Horizontal=�6-   $��Center=�6F   .+�5J      �� $��添加��6!   $��75 0 0 0q�6r   $��90��6�   $��30��6�    � Cursor$
 Hand��6�   ��6.   �� *AddRichTxtCommd+{�6R   �� *
IsCancel+6�   �� 5K      $��删除��6!   $��15 0 0 0q�6r   $��90��6�   $��30��6�   $
 Hand��6�   ��6.   �� *DelRichTxtCommd+{�6R   �� *
IsCancel+6�   5L      5M      �� 5O      $��2��6   $
��Left=�6*   $ Vertical=�6E   .+�5P      �� -   5        $��richTextBox��5S      $��1 1 1 1q�5Q       �� 
IsReadOnly$
 True��5R      $
��Left=�5T      $	��450��5U      $��Center=�5V      $	��650��60   $��	379,0,0,0q�6<   . �5X      /� .T�5Y      J� .*�6(   .*�5Z   "   �� $��14(�6'   �65   �� *	RichTxt+6Q   5[      5\      5]      �� 5^      $��保存��6   $��Right=�6*   $��
10 15 10 0q�6�   $��90��6�   $��30��6�   $
 Hand��6�   ��6G   �� *	SaveBtn+{�6c   �� *
IsCancel+6�   5_      5`      5a      5b      5c      