   M S B A M L   `   `   `  ���� M  IInterfaceGraphique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulla5clr-namespace:InterfaceGraphique.Controls.WPF.Friends'InterfaceGraphique.Controls.WPF.Friends  g8clr-namespace:InterfaceGraphique.Controls.WPF.Converters*InterfaceGraphique.Controls.WPF.Converters  Q-clr-namespace:InterfaceGraphique.Controls.WPFInterfaceGraphique.Controls.WPF  @    :InterfaceGraphique.Controls.WPF.Friends.FriendListItemView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Glocal5clr-namespace:InterfaceGraphique.Controls.WPF.Friends     Lconvert8clr-namespace:InterfaceGraphique.Controls.WPF.Converters     Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  7wpf-clr-namespace:InterfaceGraphique.Controls.WPF  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity    � 	Resources  6   0InterfaceGraphique.Controls.WPF.ViewModelLocator   	VMLocator  ContentStyle �� 
TargetType �� Property �� Value �� 
TargetType ��Name  DarkGrayBrush  UltraDarkGrayBrush J� InputBindings d� MouseAction �� Command.  (System.Windows.Interactivity.Interaction	   Triggers/  )System.Windows.Interactivity.EventTrigger
   	EventName.  (System.Windows.Interactivity.TriggerBase   Actions6  0System.Windows.Interactivity.InvokeCommandAction   Command �Name � ColumnDefinitions K� ZIndex �� CornerRadius �� 	ConverterM   GInterfaceGraphique.Controls.WPF.Converters.BooleanToVisibilityConverter �� ConverterParameter �� Padding �� RadiusX �� RadiusY �� RelativeSource �� BitmapScalingMode �� ImageSourceE   ?InterfaceGraphique.Controls.WPF.Converters.Base64ImageConverter �� Padding �� AncestorType �� Orientation  FontAwesome.WPF.Awesome   Content  DeleteBrush  AddBrush �� Triggers �� Value �� Binding  �� 
TargetName! :� RoutedEvent  UltraDarkGrayColor	" �� To  DarkGrayColor# �� From$ g� Duration% �� 
TargetName& �� TargetProperty' P� Property( P� Value) �� MergedDictionaries* �� Source�� ) �� $>* 8/InterfaceGraphique;component/Resource/appresources.xaml?�%�	  &	        &	      7  7  7  7  7  7  7  7  7  7  7    �� " ��.���  � �� �� " ��.R��� $ 
background��8��  8�� $��0,0,0,0q� d� $ 	LeftClicke� �� *GoToProfileCommand+	   $
 
MouseEnter��.    �� *ClickFriendCommand+  $
 
MouseLeave��.    �� *MouseOverCommand+._�� $ 	container��
������  �� $
��Auto ��� $��2* ��� $��3* �.+��� 
�������$ 1��$��0,0,3,10q�$��15��$��15��$ 15��$��Bottom=�$��Right=�$��5q�
���2�2�y��� *CanShowOnline+   $
 True���� 
�������$ 1��$��0,0,3,10q�$��15��$��15��$ 15��$��Bottom=�$��Right=�$��5q����	#FF2C2B2By��� *CanShowOffline+   $
 True���� $��0��$
 8 15q�
�������$ 8��._��� $��40��$ 8��$ 8������ *ActualHeight+ �� *Self+���� $ HighQuality=� �� *ProfilePicture+   �� $��1��$ 	15 0 15 0q�._��� $��Center=�.+��� $ 0 0 0 2q�
�������$��Medium"�$��18(�$��CharacterEllipsis=����� *DataContext.Username+ �� *FindAncestor+" >��� $��2��$ 
Horizontal=�$	��3pxq�$��Right=�y��� *DataContext.CurrentFriend+ �� *FindAncestor+" >�   $
 True��.+��� $ Gamepad=�$��0 0 5 0q�$��0q����� *PlayGameCommand+y��� *CanSendPlay+   $
 True���� $ Comment=�$��0 0 5 0q�$��0q�8�� ���� *ChatWithCommand+y��� *CanShowOnline+   $
 True���� $ Trash=�$��0 0 5 0q�$��0q�8�� ���� *RemoveFriendCommand+�� $��2��$ 
Horizontal=�$	��3pxq�$��Right=�y��� *DataContext.AddingFriend+ �� *FindAncestor+" >�   $
 True��.+��� $
 Plus=�$��0 0 5 0q�$��0q�8�� ���� *SendFriendRequestCommand+�� $��2��$ 
Horizontal=�$	��3pxq�$��Right=�y��� *DataContext.RequestedFriend+ �� *FindAncestor+" >�   $
 True��.+��� $ Check=�$��0 0 5 0q�$��0q�8�� ���� *AcceptFriendRequestCommand+�� $ Times=�$��0 0 5 0q�$��0q�8�� ���� *RefuseFriendRequestCommand+ �� $
 True�� �� *DataContext.IsSelected+ �� *FindAncestor+" >�.a��� $  
background��8   � ��:� $! 
MouseEnter��.[��� .���� .���� 8"  8# 	 $$ 0:0:0.3D�$% 	container��$& Background.Color�:� $! 
MouseLeave��.[��� .���� .���� 8# 
 $" TransparentX�$$ 0:0:0.0D�$% 	container��$& Background.Color�P� ' � {�( .  . ��� $  	container�� � ��
 � � �.���� #��[ 