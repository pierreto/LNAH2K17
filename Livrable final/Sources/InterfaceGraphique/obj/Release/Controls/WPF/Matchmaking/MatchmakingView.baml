   M S B A M L   `   `   `  ���� M  IInterfaceGraphique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulli9clr-namespace:InterfaceGraphique.Controls.WPF.Matchmaking+InterfaceGraphique.Controls.WPF.Matchmaking  Q-clr-namespace:InterfaceGraphique.Controls.WPFInterfaceGraphique.Controls.WPF  c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35qPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.InteractivitySystem.Windows.Interactivity O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem I)clr-namespace:InterfaceGraphique.ControlsInterfaceGraphique.Controls  g8clr-namespace:InterfaceGraphique.Controls.WPF.Converters*InterfaceGraphique.Controls.WPF.Converters  A    ;InterfaceGraphique.Controls.WPF.Matchmaking.MatchmakingView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     Klocal9clr-namespace:InterfaceGraphique.Controls.WPF.Matchmaking     =wpf-clr-namespace:InterfaceGraphique.Controls.WPF     ^iPclr-namespace:System.Windows.Interactivity;assembly=System.Windows.Interactivity     9system&clr-namespace:System;assembly=mscorlib     >Controls)clr-namespace:InterfaceGraphique.Controls     Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  Fconvert8clr-namespace:InterfaceGraphique.Controls.WPF.Converters    � 	Resources  6   0InterfaceGraphique.Controls.WPF.ViewModelLocator   	VMLocator �� MergedDictionaries �� Source��  �� $> 8/InterfaceGraphique;component/Resource/appresources.xaml?�%   &	          .��� ���	#FF373A3E � DataContext ��  �� Path$ MatchmakingViewModel� �� Source# [   � RowDefinitions �� $��* �.+��  �� $	��10* ��� $	��15* ��� $	��25* ��� $	��50* �.+��� $��0��$
��Left=�{��� *
EnabledMap+���� *MainMenuCommand+��!  FontAwesome.WPF.FontAwesome     Icon$ 	ArrowLeft=��� $��1�� �� Orientation$ 
Horizontal=�$��Center=�$��Center=�.+��� $ Vertical=�$��0 0 20 0q�.+��� $��Center=�$��60��$��60������ *OpponentPicture+	 �� 	Converter	 E   ?InterfaceGraphique.Controls.WPF.Converters.Base64ImageConverter  �� $��Center=�$
��Wrap=�$��15(�
�������$
��Bold"����� *OpponentName+�� $��Center=�$��Center=�$
��Wrap=�$��VS��
�������$��45(�$
��Bold"��� $ Vertical=�$��20 0 0 0q�.+��� $��Center=�$��60��$��60������ *PlayerPicture+	   �� $��Center=�$
��Wrap=�$��15(�
�������$
��Bold"����� *
PlayerName+�� $��2��$��Center=�.+��� $��Center=�$��Temps avant la partie :��$��18(�
��������� $��Center=�$	��100(�
�������$
��Bold"����� *RemainingTime+�� $��3��$��Center=�.+��� $��Cartes��$��20(�
��������� -   $��mapList��$	��450������ *MapsAvailable+
 �� SelectedItem
 �� *SelectedMap+{��� *
EnabledMap+���� .R��� $ 
Horizontal=�.+��� $��0��$��35��$��35��$��10 0 0 0q����� *Icon+	   �� $��1��
�������$��18(�$��15 0 0 0q�$��Center=����� *	MapName+�� $	��200��$	��200��$��0 20 0 0q����� *
ImageSrc+	   ]� $��Center=�$��Center=�y��� *IsWaitingForOpponent+���� *ActualHeight+ �� RelativeSource �� *FindAncestor+ �� AncestorType" >����� *ActualWidth+ �� *FindAncestor+" >�����  �� 
TargetType" ]�.���  �� Property � �� �� Value �#cccccc��  �@J�Opacity$	 0.8Q�.+��� $��Center=�$��Center=�.+�"  FontAwesome.WPF.ImageAwesome     Icon$ CircleOutlineNotch=�  DarkSilverBrush   
Foreground# [    Spin .    SpinDuration$ 4Q�$	��200���� $��Center=�$��25(�$
��Bold"�$��40��En attente d'un joueur�� $��40��$	��100��
���  ��
���  ������ *Cancel+.���� $
��Bold"�	Annuler� $��Center=�$��Center=�$	��200��$	��400��
�������y��� *OpponentLeftMsg+ �� $	��70* ��� $	��30* �.+��� $��0��$X��RVotre adversaire a quitté. Veuillez patientez, nous vous en cherchons un nouveau.��$
��Wrap=� �� TextAlignment$ Center=�$��Center=�$��Center=�
�������$��25(�$��20 10 20 10q��� $��1��$��Center=�$��Center=����� *HidePopupCommand+.��Ok