   M S B A M L   `   `   `  ���� M  IInterfaceGraphique, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullQ-clr-namespace:InterfaceGraphique.Controls.WPFInterfaceGraphique.Controls.WPF  g8clr-namespace:InterfaceGraphique.Controls.WPF.Converters*InterfaceGraphique.Controls.WPF.Converters  7    1InterfaceGraphique.Controls.WPF.Signup.SignupView   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     =wpf-clr-namespace:InterfaceGraphique.Controls.WPF     Lconvert8clr-namespace:InterfaceGraphique.Controls.WPF.Converters     Z VFontAwesome.WPF, Version=4.7.0.37774, Culture=neutral, PublicKeyToken=0758b07a11a4f466-fa$http://schemas.fontawesome.io/icons/    � 	Resources  6   0InterfaceGraphique.Controls.WPF.ViewModelLocator   	VMLocator �� MergedDictionaries �� Source��  �� $> 8/InterfaceGraphique;component/Resource/appresources.xaml?�%   &	          .��� {��� *
NotLoading+.+�� $��Center=�$��Center=�$��3��$��3�� K� ZIndex$ 10��$��20q�y��� *	Loading+ �� 	Converter M   GInterfaceGraphique.Controls.WPF.Converters.BooleanToVisibilityConverter   �� ConverterParameter$
 True��.+�"  FontAwesome.WPF.ImageAwesome     Icon$ CircleOutlineNotch=�$��48��$��48��  DarkSilverBrush   
Foreground# [    Spin �� *	Loading+�� 	 �� Padding$	 10q����	#FF373A3E._�� .+��� $��Center=�$	��900��
 � DataContext
 ��  �� Path$ SignupViewModel� �� Source# [  .+��  � ColumnDefinitions �� $��* ��� $��2* �.+��� $��Center=� �� TextAlignment$ Right=�$��0��$
��Wrap=�$��Nom d'usager�� �� Padding$ 0 0 10 0q�$��24(�$
��Bold"�  TextColorBrush#��[ $��0,0,0,10q��� $��1��$	 5q� �� CornerRadius$ 5��$��1q�
������� J� SnapsToDevicePixels .   
InputBrush#��[ $��0,0,0,10q�$	��400��$
��Left=�$��45��._��� -    ��Name$ Username��
������ $��0q�$	��400��$��24(�$
��Bold"����� *
Username+ �� UpdateSourceTrigger$ PropertyChanged=�{��� *InputsEnabled+�� $��Center=�$��0,0,0,10q�$
��Wrap=�$	��Top=�
���  ��$��18(����� *UsernameErrMsg+y��� *UsernameErrMsg+ L  FInterfaceGraphique.Controls.WPF.Converters.StringToVisibilityConverter  $
 True���  �� $��* ��� $��2* �.+��� $��Center=�$ Right=�$��0��$
��Wrap=�$	��Nom��$ 0 0 10 0q�$��24(�$
��Bold"�#��[ $��0,0,0,10q��� $��1��$	 5q�$ 5��$��1q�
������� . #��[ $��0,0,0,10q�$	��400��$
��Left=�$��45��._��� -   $
 Name��
������ $��0q�$	��400��$��24(�$
��Bold"����� *Name+$ PropertyChanged=�{��� *InputsEnabled+�� $��Center=�$��0,0,0,10q�$
��Wrap=�$	��Top=�
���  ��$��18(����� *
NameErrMsg+y��� *
NameErrMsg+   $
 True���  �� $��* ��� $��2* �.+��� $��Center=�$ Right=�$��0��$
��Wrap=�$��Email��$ 0 0 10 0q�$��24(�$
��Bold"�#��[ $��0,0,0,10q��� $��1��$	 5q�$ 5��$��1q�
������� . #��[ $��0,0,0,10q�$	��400��$
��Left=�$��45��._��� -   $ Email��
������ $��0q�$	��400��$��24(�$
��Bold"����� *Email+$ PropertyChanged=�{��� *InputsEnabled+�� $��Center=�$��0,0,0,10q�$
��Wrap=�$	��Top=�
���  ��$��18(����� *EmailErrMsg+y��� *EmailErrMsg+   $
 True���  �� $��* ��� $��2* �.+��� $��Center=�$ Right=�$��0��$
��Wrap=�$��Mot de passe��$ 0 0 10 0q�$��24(�$
��Bold"�#��[ $��0,0,0,10q��� $��1��$��1��$	 5q�$ 5��$��1q�
������� . #��[ $��0,0,0,10q�$	��400��$
��Left=�$��45��._�G� -    G�Name$ Password��4   .InterfaceGraphique.Controls.WPF.PasswordHelper   Attach . 
������ $��0q� G� 	MaxLength$ 16��$��24(�$
��Bold"�   Password �� *
Password+$ PropertyChanged=� �� Mode$ TwoWay=�{��� *InputsEnabled+�� $��Center=�$��0,0,0,10q�$
��Wrap=�$	��Top=�
���  ��$��18(����� *PasswordErrMsg+y��� *PasswordErrMsg+   $
 True���  �� $��* ��� $��2* �.+��� $��Center=�$ Right=�$��0��$
��Wrap=�$��Confirmation mot de passe��$ 0 0 10 0q�$��24(�$
��Bold"�#��[ $��0,0,0,10q��� $��1��$��1��$	 5q�$ 5��$��1q�
������� . #��[ $��0,0,0,10q�$	��400��$
��Left=�$��45��._�G� -   $ ConfirmPassword�� . 
������ $��0q�$ 16��$��24(�$
��Bold"� �� *ConfirmPassword+$ PropertyChanged=�$ TwoWay=�{��� *InputsEnabled+�� $��Center=�$��0,0,0,10q�$
��Wrap=�$	��Top=�
���  ��$��18(����� *ConfirmPasswordErrMsg+y��� *ConfirmPasswordErrMsg+   $
 True���� $��Créer Compte��$��Center=�$��0,0,0,0q�$
��Bold"�$��24(�$��50��$	��Top=�$	��250�� �� 	IsDefault .   
GreenBrush#��[ $��0q����� *SignupCommand+{��� *InputsEnabled+