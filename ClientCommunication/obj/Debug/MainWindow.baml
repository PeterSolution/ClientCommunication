   M S B A M L   `   `   `  ����N  JClientCommunication, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null9!clr-namespace:ClientCommunicationClientCommunication  5   	   $    ClientCommunication.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   3local!clr-namespace:ClientCommunication     5   	     � Title$  
MainWindow��5   	   $	��450��6   $	��800��6)   .��5
      � -   5         �Name$ chatbox��5
      $
��Auto=�6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� -   5         ��Name$ 
ListClient��5   '   $y�Visible=�6   $	��200��6;   $��0,0,0,64q�6G   $
��Auto=�6Y   $��Right=�6�   �� 5   
   -   5         ��Name$ btngora��5      $��Button��6"   $
��Left=�63   $��580,197,0,0q�6N   $	��Top=�6c   6{   �� 5   
   -   5         ��Name$ 
LoginPanel��5      $��Center=�5      $��Center=�5      $	��300��5      .+�5      �� 
������� �� CornerRadius$ 8��5       �� Padding$ 20q�5         MaterialDropShadow J� Effect# �   5      ._�5      �� .+�5      �� -   5        	 ��Name$	 backWelcome��5       $��Witaj znowu��65   $��24(�5      $��SemiBold"�5      $��Center=�5      $��0,0,0,20q�5      �� 5!      -   5        
 ��Name$
 l1Nazwa��5!      $��Nazwa uzytkownika��6-   $��0,0,0,5q�6I   �� 5"      -   5         ��Name$ LoginBox��5"      $��40��5#      $
��10,0q�5$      $��Center=�5%      $��0,0,0,15q�5&      �� 5(      -   5        $
 L2Haslo��5(      $��Haslo��6-   $��0,0,0,5q�6=   G� 5)      -	   5         G�Name$ HasloBox��5)   "   $��40��5*       $
��10,0q�5+       $��Center=�5,       $��0,0,0,20q�5-       �� 5/      -
   5        $ 	Logowanie��5/      $��Sign In��50      $��40��51      ���#1877F2
�������54      55      56      57      �� 58   
   -   5        $
 
zalogowano��58      $�� ��6$   $
��Left=�6/   $	��Top=�6J   �� 59   
   -   5         ��Name$ chatbox1��59      $
��Chat��6%   $��140,0,200,75q�63   $y�Hidden=�6I   .��5:      �� -   5         ��Name$ 
ChatCanvas��5:      5;      5=      � 5>   
   -   5        $ SendGrid��5>      $��1��6!   $��0��6.   $��2��6>    � RowDefinitions 5?      �� 5@      $
��Auto �6    �� 5A      $
��Auto �6    .+�5D      �� -   5        $ sendtextbox��5D       �� TextWrapping$
 Wrap=�5E      $��Bottom=�5F      $��70��5G      $y�Hidden=�5H      $��140,364,0,0q�5I      $��Stretch=�5J      �� 5L      -   5        $ 	Wyslijbtn��5L      $��Wyslij��5M      $y�Hidden=�5N      $��58��5P      $	��113��5Q      $
��Left=�5R      $��Bottom=�5S      $��0,0,0,0q�5T      �� 5U      -   5         ��Name$ MenuOpen��5U      $y�Hidden=�6&   $
��Left=�6:   $��38��6U   $��0,36,0,0q�6a   $	��Top=�6s   $��56��6�   $��/Resources/menuicon.png��6�   $
��Fill=�6�   6�   �� 5V      -   5        $
 logoutlabel��5V      $��Wyloguj sie��6)   $
��Left=�6?   $��0,73,0,0q�6Z   $	��Top=�6l   $y�Hidden=�6�   6�   �� 5W      -   5        $
 closechatlabel��5W      $��Zamknij chat��6,   $
��Left=�6C   $��	0,104,0,0q�6^   $	��Top=�6q   $y�Hidden=�6�   6�   �� 5X      -   5        $
 Newchat��5X      $��Stworz nowy chat��6%   $
��Left=�6@   $��	0,144,0,0q�6[   $	��Top=�6n   $y�Hidden=�6�   6�   �� 5Y      -   5        $
 
userslabel��5Y      $7��1Wybierz uzytkownika z jakim chcesz rozpaczac chat��6(   $y�Hidden=�6d   $
��Left=�6x   $��	519,0,0,0q�6�   $	��Top=�6�   5[      5\      5]      