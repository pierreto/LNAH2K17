   M S B A M L   `   `   `  ���� M  IInterfaceGraphique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:InterfaceGraphique.Controls.WPFInterfaceGraphique.Controls.WPF  g8clr-namespace:InterfaceGraphique.Controls.WPF.Tournament*InterfaceGraphique.Controls.WPF.Tournament  I)clr-namespace:InterfaceGraphique.ControlsInterfaceGraphique.Controls  g8clr-namespace:InterfaceGraphique.Controls.WPF.Converters*InterfaceGraphique.Controls.WPF.Converters  ?    9InterfaceGraphique.Controls.WPF.Tournament.TournamentView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     =wpf-clr-namespace:InterfaceGraphique.Controls.WPF     @d2http://schemas.microsoft.com/expression/blend/2008     Jlocal8clr-namespace:InterfaceGraphique.Controls.WPF.Tournament     >Controls)clr-namespace:InterfaceGraphique.Controls     Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/  Fconvert8clr-namespace:InterfaceGraphique.Controls.WPF.Converters    � 	Resources  6   0InterfaceGraphique.Controls.WPF.ViewModelLocator   	VMLocator �� MergedDictionaries �� Source��  �� $> 8/InterfaceGraphique;component/Resource/appresources.xaml?�%   &	          .��� ���	#FF373A3E � DataContext ��  �� Path$ TournamentViewModel� �� Source# [  .+��  � RowDefinitions �� $	��10* ��� $	��30* ��� $	��60* �.+��� $��0��.+��� $
��Left=����� *Cancel+{��� *LeaveEnabled+��!  FontAwesome.WPF.FontAwesome     Icon$ 	ArrowLeft=�� $��1��$��20,0,0,0q� � ColumnDefinitions �� $��* ��� $��* �.+�� $��0��.+��� $��20,0,0,0q�.+��� $��Cartes��$��20(�
�������$��Center=��� -   $��mapList��$��1��$��0,10,0,0q�$	��400������ *MapsAvailable+	 �� SelectedItem	 �� *SelectedMap+{��� *EnabledMaps+���� .R��� 
 �� Orientation$
 
Horizontal=�.+��� $��0��$��35��$��35��$��10 0 0 0q����� *Icon+ �� 	Converter E   ?InterfaceGraphique.Controls.WPF.Converters.Base64ImageConverter  �� $��1��
�������$��18(�$��15 0 0 0q�$��Center=����� *	MapName+�� $	��200��$	��200��$��0 20 0 0q����� *
ImageSrc+   �� $��1��$��Center=�$��Center=�.+��� $��Center=�$(��"Temps avant le début du tournoi :��$
��Wrap=�$��18(�
��������� $��Center=�$	��100(�
�������$
��Bold"����� *RemainingTime+� $��2��$��Center=�$��Center=� �� $	��20* ��� $	��20* ��� $	��20* �.+�� $��0�� �� $��* ��� $��* ��� $��* ��� $��* �.+��� $��0��$��2q�
�������$��10 10 10 10q�$	��300��$	��100��._��� $��Center=�$��Center=�$
��Wrap=� �� TextAlignment$ Center=�
�������$��20(� �� Padding$ 10 10 10 10q����� *	Player1+�� $��1��$��2q�
�������$��10 10 10 10q�$	��300��$	��100��._��� $��Center=�$��Center=�$
��Wrap=�$ Center=�
�������$��20(�$ 10 10 10 10q����� *	Player2+�� $��2��$��2q�
�������$��10,10,10,10q�$	��300��$	��100��._��� $��Center=�$��Center=�$
��Wrap=�$ Center=�
�������$��20(�$ 10 10 10 10q����� *	Player3+�� $��3��$��2q�
�������$��10,10,10,10q�$	��300��$	��100��._��� $��Center=�$��Center=�$
��Wrap=�$ Center=�
�������$��20(�$ 10 10 10 10q����� *	Player4+� $��1�� �� $��* ��� $��* �.+��� $��0��$��2q�
�������$��10,10,10,10q�$��Center=�$	��300��$	��100��._��� $��Center=�$��20(�$
��Wrap=�$��Center=�$ Center=�
�������$ 10 10 10 10q����� *
SemiFinal1+�� $��1��$��2q�
�������$��10,10,10,10q�$��Center=�$	��300��$	��100��._��� $��Center=�$��20(�$
��Wrap=�$��Center=�$ Center=�
�������$ 10 10 10 10q����� *
SemiFinal2+� $��2�� �� $��* �.+��� $��0��$��2q�
�������$��10,10,10,10q�$��Center=�$��Center=�$	��300��$	��100��._��� $��Center=�$��20(�$
��Wrap=�$��Center=�$ Center=�
�������$ 10 10 10 10q����� *Winner+�� $	��400��$	��300��
����·�$��3q�y��� *IsEndOfTournament+._�� ���	#FF373A3E �� $	��20* ��� $	��40* �.+��� $��0��$
��Wrap=�
����·�$��25(�$
��Bold"����� *EndOfGameTitle+�� $��1��$
 Vertical=�.+��� $ Center=�
�������$��25(�$��0 10 0 0q����� *
WinnerName+]� $��Center=�$��Center=�y��� *	Loading+���� *ActualHeight+ �� RelativeSource �� *FindAncestor+ �� AncestorType" >����� *ActualWidth+ �� *FindAncestor+" >�����  �� 
TargetType" ]�.���  �� Property � �� �� Value �#cccccc��  �@J�Opacity$	 0.8Q�.+��� $��Center=�$��Center=�.+�"  FontAwesome.WPF.ImageAwesome     Icon$ CircleOutlineNotch=�  DarkSilverBrush   
Foreground# [    Spin .    SpinDuration$ 4Q�$	��200���� $��Center=�$��25(�$
��Bold"�$��40��En attente de joueurs joueur�� $��40��$	��100��
���  ��
���  ������ *Cancel+.���� $
��Bold"�	Annuler� $��Center=�$��Center=�$	��200��$	��400��
�������y��� *OpponentLeftMsg+ �� $	��70* ��� $	��30* �.+��� $��0��$]��WUn de vos adversaires a quitté. Veuillez patientez, nous vous en cherchons un nouveau.��$
��Wrap=�$ Center=�$��Center=�$��Center=�
�������$��25(�$��20 10 20 10q��� $��1��$��Center=�$��Center=����� *HidePopupCommand+.��Ok