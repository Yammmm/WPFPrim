   M S B A M L   `   `   `  ����C  ?WmsPrism, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:WmsPrism.ViewModelsWmsPrism.ViewModels  5	   	       WmsPrism.Views.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   b ^MaterialDesignThemes.Wpf, Version=4.0.0.2422, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5   	   S OPrism.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59Z	 VPrism.DryIoc.Wpf, Version=8.0.0.1909, Culture=neutral, PublicKeyToken=40ee6c3a2184dc59&prismhttp://prismlibrary.com/  	 5   	   _
 [Microsoft.Xaml.Behaviors, Version=1.1.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a3i+http://schemas.microsoft.com/xaml/behaviors 
 5   	   .domain!clr-namespace:WmsPrism.ViewModels 
 5	   	   6dragablz'http://dragablz.net/winfx/xaml/dragablz 
 5   	   !  Prism.Mvvm.ViewModelLocator    AutoWireViewModel  True5   	    � Title$ 仓库系统��5   	   $
��1440��5   	   $	��900��5   	   $
��1020��5   	   $	��850��5   	      MaterialDesignPaper#���   5   	    � 
ResizeMode$ CanResizeWithGrip=�5   	     MaterialDesignBody#���  5   	    � Icon$ /Image/Logiico.ico��5   	    � WindowState$ 	Maximized=�5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5       �� 
TargetType �� Property	 �� Value  MaterialDesignTreeViewItem
 �� BasedOn�� 5   
   %/  '��       'Q� �     7  �� 5      " ��6   .�5      ��  � ��
	 ���� 6/   �� 5       � ��$	 0q�64   �� 5       � ��$	 Center=�6?   �� 5       � ��$	 Center=�6?   5      �� 5       " Q�6   8
   .�5!      ��  �@Q�
IsExpanded$
	 True��6/   5"      5$      .��5&      �  � ColumnDefinitions 5(   
   �� 5)      $	��300 �6   �� 5*      .+�5,   
   �  K� ZIndex$ 1��6   ��5-      �� 5.       �� ImageSource$ /Image/background.jpg��6    j� Stretch$ UniformToFill=�6A    �� Opacity$	 0.5Q�6Y    � RowDefinitions 51      �� 52      $��90 �6    �� 53      .+�56      �� $��0 0 0 0q�6   ���	#FF2196F3���#E3E4E8$��0 0 0 0q�6U   �� 57      $��Center=�6    �� Orientation$ 
Horizontal=�66   $��	-20 0 0 0q�6O   .+�58      �� $��/Image/Logiico.ico��6   $��50��64   �� 5:      $��0,0,0,0q�5;      $��Center=�5<      $��Center=�5=      $��22(�5>      $
��Bold"�5?      
�������$��新禾供应链��5A      5B      5C      � 5E      ���#E0E0E0 J� Opacity$	 0.8Q�6)   $��2��67   $	��220��6D    5F      �� 5G      $��* �6$   �� 5H      $
��Auto �6$   .+�5J      R� -   5         R�Name$ treeview��5J   (   $��0��6   $	��220��5K   D   ��6   �� *MovieCategories+* 
 $Microsoft.Xaml.Behaviors.Interaction   Triggers 5L      + 
 %Microsoft.Xaml.Behaviors.EventTrigger  5M         	EventName$ SelectedItemChanged��6)   * 
 $Microsoft.Xaml.Behaviors.TriggerBase   Actions. 5N      2 
 ,Microsoft.Xaml.Behaviors.InvokeCommandAction     Command 64   ��  �� Path$  SelectedItemChangedCommand�   CommandParameter 5O   ,   ��  �� ElementName$ treeview��$ SelectedItem�5P       5R       �� DataType'   !WmsPrism.ViewModels.MovieCategory �� ItemsSource   WmsPrism.ViewModels.Movie��%_  (��       5T   !   *, +)(�� �     5]   (   *, +)�� 5S      "  5T   !    5U   !   �� *Movies+.R�5W      �� $��15(�5X   %   
���   �$	��3,2q�5[   %   ��5Z   %   �� *Name+5[   %   5\      �� 5]      "  6(   .R�5_      �� $	��3 2q�5a   %   $��14(�5b   %   
���   ���5`   %   �� *Name+5c   %   5e      5f      5n      5o      5p      � 5s   
   $��1��6   ���#F5F6FA 5t      �� 5u      $
��auto �6    �� 5v      $
��auto �6    �� 5w      .+�5y      �� $��0��6   $ 1��6%   $��0 0 0 0q�67    J� Effect 5z      3  -System.Windows.Media.Effects.DropShadowEffect  5{         Color$ #D1D1D1��6'      
BlurRadius$ 5Q�67       ShadowDepth$  5Q�6G   !   Opacity$! 1Q�6W   5}      (	  "MaterialDesignThemes.Wpf.ColorZone	  5~      $��10,30,20,10q�5      $	��Top=�5�      +
  %MaterialDesignThemes.Wpf.ShadowAssist" 
  ShadowDepth$" Depth0=�5�      # 	  Mode$# 
PrimaryMid=�5�      .��5�      �  5�      �� 5�      $
��auto �6(   �� 5�      �� 5�      .+�5�      �� $��	0 -20 0 0q�6"   $��Right=�65   $ 
Horizontal=�6Q   .+�5�      ��  ! MaterialDesignFlatMidBgButton#��[ 5�   !   $ � ToolTip$$ 用户信息��5�   !   .��5�      �� $ 
Horizontal=�6)   .+�5�   "   '  !MaterialDesignThemes.Wpf.PackIcon  $��20��5�   )   $��20��5�   )   $��Center=�5�   )   %   Kind$
% User=�5�   )   5�      5�      �� 5�      #��[ 5�   !   $$ 消息��5�   !   .��5�      �� $ 
Horizontal=�6)   .+�5�   "     $��20��5�   )   $��20��5�   )   $��Center=�5�   )   $% EmailEditOutline=�5�   )   5�      5�      �� 5�      #��[ 5�   !   $$ 更多��5�   !   .��5�      �� $ 
Horizontal=�6)   .+�5�   "     $��20��5�   )   $��20��5�   )   $��Center=�5�   )   $% FormatAlignJustify=�5�   )   5�      5�      5�      �� 5�      -   5        $��itemsControl��5�   $   ��68   �� *ModuleGroups+��5�      �� 5�      .R�5�   "   �� $��0 25 0 0q�6-   $��3��6?   $	��850��6L   $��25��6X   $ 
Horizontal=�6e   $
��Left=�6~   5�      ��5�      �� 5�      .R�5�   "   #  MaterialDesignThemes.Wpf.Chip  $��0 0 0 0q�5�   %   $��25��5�   &   &   IsDeletable$
& True��5�   &   $$ 关闭当前页��5�   &   ��5�   &   �� *Tite+��5�   &   �� ' �� RelativeSource' �� ( �� Mode$( FindAncestor=�) �� AncestorType) ItemsControl$! DataContext.OpenPageCommand���5�   &   �� *Tite+*   DeleteCommand* 5�   &   �� ' �� $( FindAncestor=�) ItemsControl$" DataContext.ClosePageCommand�+   DeleteCommandParameter+ 5�   &   �� *Tite+5�   8   5�      5�      5�      5�      �� 5�      $��3��6   $	��Top=�6+   $
��Left=�6D   $
��1200��5�      $	��800��6+   $	��900��68   $	��700��6G   $��25q�5�      !  Prism.Regions.RegionManager,   
RegionName$, ContentRegion��6+   5�      5�      5�      