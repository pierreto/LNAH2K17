   M S B A M L   `   `   `  ���� M  IInterfaceGraphique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:InterfaceGraphique.Controls.WPFInterfaceGraphique.Controls.WPF  i9clr-namespace:InterfaceGraphique.Controls.WPF.UserProfile+InterfaceGraphique.Controls.WPF.UserProfile  g8clr-namespace:InterfaceGraphique.Controls.WPF.Converters*InterfaceGraphique.Controls.WPF.Converters  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity A    ;InterfaceGraphique.Controls.WPF.UserProfile.UserProfileView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     =wpf-clr-namespace:InterfaceGraphique.Controls.WPF     Klocal9clr-namespace:InterfaceGraphique.Controls.WPF.UserProfile     Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  Fconvert8clr-namespace:InterfaceGraphique.Controls.WPF.Converters  XiPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity  9t1http://schemas.telerik.com/2008/xaml/presentation    � 	Resources  6   0InterfaceGraphique.Controls.WPF.ViewModelLocator   	VMLocator  NoHover �� 
TargetType �� Property �� Value �� 
TargetType �� MergedDictionaries �� Source��  �� $> 8/InterfaceGraphique;component/Resource/appresources.xaml?�%p   &	        &	        �� " ��.���  � ��
 ���� ��  � �� �� " ��.R�� #��z��.+��� .��� {��� *
NotLoading+ � DataContext ��  �� Path$ UserProfileViewModel�	 �� Source#	 [  
 � ColumnDefinitions
 �� $	��80* ��� $	��20* �.+�� $��Center=�$��Center=�$��3��$��3�� K� ZIndex$ 10��$��20q�y��� *	Loading+ �� 	Converter M   GInterfaceGraphique.Controls.WPF.Converters.BooleanToVisibilityConverter   �� ConverterParameter$
 True��.+�"  FontAwesome.WPF.ImageAwesome     Icon$ CircleOutlineNotch=�$��48��$��48��  DarkSilverBrush   
Foreground# [    Spin �� *	Loading+� $��0�� � RowDefinitions �� $	��10* ��� $	��40* ��� $	��40* �.+�� $��0��
 �� $��* ��� $��* �.+��� $��0��$
��Left=����� *MainMenuCommand+��!  FontAwesome.WPF.FontAwesome     Icon$ 	ArrowLeft=�� $��1��
 �� $	��40* ��� $	��60* �.+��� #��[ $��0������ *ChangeProfilePictureCommand+.����  � Cursor$
 Hand������ *ProfilePicture+ E   ?InterfaceGraphique.Controls.WPF.Converters.Base64ImageConverter  � $��1��
 �� $��* ��� $��* �.+��� $��0��.+��� $��
Pseudonyme��$��14(�
�������$
��Bold"��� $��20(�
����·�$��0,5,0,0q����� *
UserName+�� $	��Nom��$��14(�
�������$
��Bold"�$��0,30,0,0q��� $��20(�
����·�$��0,5,0,0q����� *Name+�� $��Adresse Courriel��$��14(�
�������$
��Bold"�$��0,30,0,0q��� $��20(�
����·�$��0,5,0,0q����� *Email+�� $!��Date de création du compte��$��14(�
�������$
��Bold"�$��0,30,0,0q��� $��14(�
����·�$��0,5,0,0q����� *DateCreation+� $��1�� �� $��* ��� $��* ��� $��* �.+��� $��0�� �� Orientation$ Vertical=�$��Center=�$��Center=�.+��� $��Points��$��16(�
�������$
��Bold"��� $��Center=�$��35(�
����·�$
��Bold"�$��0,5,0,0q����� *
PointsNb+�� $��1��$ Vertical=�$��Center=�$��Center=�.+��� $��Parties Gagnées��$��16(�
�������$
��Bold"��� $��Center=�$��35(�
����·�$
��Bold"�$��0,5,0,0q����� *	GameWon+�� $��2��$ Vertical=�$��Center=�$��Center=�.+��� $��Tournois Gagnés��$��16(�
�������$
��Bold"��� $��Center=�$��35(�
����·�$
��Bold"�$��0,5,0,0q����� *TournamentWon+� $��2��$��0,20,0,0q� �� $��* �.+��� $��0������ *Achievements+���� .R�A�  A� Rows$ 3�� A� Columns$ 5������ .R�� $��Center=� �� $��* ��� $��* �.+��� $��0������ *
ImageUrl+�� $��1��$��14(�
�������$
��Bold"����� *Name+�� $��1��$��0��
�������$��2 0 0 0q�y��� *IsFriendProfile+._��� $��	10 25 0 0q�.+��� $��
Inventaire��
�������$��25(�$��0 0 0 10q�$��Stretch=� �� TextAlignment$ Center=��� -    ��Name$ 
storeItems������ *Items+���� .R��� $ 
Horizontal=�$��5 0 0 0q�.+���  d� 	IsChecked �� $ IsGameEnabled� �� Mode$ TwoWay=� � LayoutTransform ��  �� ScaleX$	 1.5Q� �� ScaleY$	 1.5Q�.  (System.Windows.Interactivity.Interaction   Triggers /  )System.Windows.Interactivity.EventTrigger     	EventName$ Checked��.  (System.Windows.Interactivity.TriggerBase    Actions.  6	  0System.Windows.Interactivity.InvokeCommandAction	  ! 	  Command! �� " �� ElementName$" 
storeItems��$ DataContext.SelectCommand�# 	  CommandParameter# ��   $ 	Unchecked��.  	  ! �� $" 
storeItems��$ DataContext.SelectCommand�# �� �� $��10 0 0 0q�$��40��$��40������ $ ImageUrl��� $��5 0 0 0q�
�������$��20(�$��Center=����� $
 Name�   $ MouseLeftButtonDown��.  	  ! �� $" 
storeItems��$  DataContext.ElementClicked�# �� 