   M S B A M L   `   `   `  ���� M  IInterfaceGraphique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullg8clr-namespace:InterfaceGraphique.Controls.WPF.Converters*InterfaceGraphique.Controls.WPF.Converters  k:clr-namespace:InterfaceGraphique.Controls.WPF.Chat.Channel,InterfaceGraphique.Controls.WPF.Chat.Channel  Q-clr-namespace:InterfaceGraphique.Controls.WPFInterfaceGraphique.Controls.WPF  >    8InterfaceGraphique.Controls.WPF.Chat.Channel.ChannelView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Lconvert8clr-namespace:InterfaceGraphique.Controls.WPF.Converters     Llocal:clr-namespace:InterfaceGraphique.Controls.WPF.Chat.Channel     Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  7wpf-clr-namespace:InterfaceGraphique.Controls.WPF    � 	Resources  6   0InterfaceGraphique.Controls.WPF.ViewModelLocator   	VMLocator �� MergedDictionaries �� Source��  �� $> 8/InterfaceGraphique;component/Resource/appresources.xaml?�%   &	          ����  �� 
TargetType" >�.R��� 
�������._��  � DataContext ��  �� Path$ ChannelViewModel� �� Source# [   � RowDefinitions �� $
��Auto ��� $��* �.+�� $��50��  SilverBrush#��[  � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��8q�	 �� Orientation$	 
Horizontal=��� $��8q�$	 
Horizontal=�$��2��.+��� $	 
Horizontal=�y��� *JoinChannelMenuOpen+.+��� $��Remove��  FontAwesome.WPF.Awesome
   Content$
 Trash=�$��0q�  DeleteBrush#��[ $��0,0,8,0q�y��� *ChannelSelected+ �� 	Converter M   GInterfaceGraphique.Controls.WPF.Converters.BooleanToVisibilityConverter   �� ConverterParameter$
 True������ *DeleteChannelCommand+�� $��CreateChannel��$

 Plus=�$��0q�  AddBrush#��[ $��0,0,8,0q����� *OpenAddPopupCommand+�� $��JoinChannel��$��0q�  DarkGrayBrush#��[ $��0,0,10,0q�
 �� *Arrow+���� *OpenJoinChannelCommand+��  ��Name$ addMenu�� K� ZIndex$ 1��$	��Top=�$��Right=�$��1��  UltraDarkGrayBrush#��[ $	��225��$
��Auto�� J� Opacity$	 1.0Q�$��2q�
������� �� CornerRadius$ 10��y��� *	IsOpenAdd+   $
 True�� J� Effect 3  -System.Windows.Media.Effects.DropShadowEffect     
BlurRadius$ 5Q�   Opacity$	 0.4Q�._��� $��15q�$��Center=�$
��Auto��.+��� $
 AngleUp=�$��Right=�$��	 0,0,0,15q�$��0q�#��[ ���� *CloseCreateChannelCommand+�  �Name$ 	InnerGrid�� �� $��* ��� $��2* �.+���  ��Name$ 	TextBlcok��$��Center=� �� TextAlignment$ Center=�$��0��$��0��$
��Wrap=�$	��Nom�� �� Padding$ 0 0 10 0q�$��20(�$��0,0,0,10q�
��������� $ AroundTextBox��$��0��$��1�� �� Padding$ 5q�$ 5��$��1q�
������� J� SnapsToDevicePixels . 
�������$��0,0,0,10q�._���  ��Name$ Names��
������ $��0q� �� 	MaxLength$ 16������ *Name+ �� UpdateSourceTrigger$ PropertyChanged=��� $��Center=�$��0,0,0,2q�$
��Wrap=�$	��Top=�
���  ��$��18(����� *ChannelErrMsg+y��� *ChannelErrMsg+ L  FInterfaceGraphique.Controls.WPF.Converters.StringToVisibilityConverter  $
 True���� ._��� $��0q�#��[ $��1��$	��110��$��Créer Canal��$��0,10,0,0q� �� 	IsDefault . ���� *CreateChannelCommand+�� $��1��$��0�� �� $ ChatListViewModel�# [  