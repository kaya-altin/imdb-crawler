   M S B A M L   `   `   `  ????F  BImdbCrawler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:ImdbCrawlerImdbCrawler  5   	       ImdbCrawler.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:ImdbCrawler     5   	   -   5          ? Title$  ImdbCrawler??5   	   $??233.6??6   $??476.4??6,    ? WindowStartupLocation$ CenterScreen=?6:    ? 
ResizeMode$ NoResize=?6_    ? Icon$ IMDb.ico??6u   .??5	      ?  ? ColumnDefinitions 5
   
   ?? 5      $	??17* ?6   ?? 5      $
??101* ?6   .+?5   
   ?? -   5         ??Name$ btnStart??5      $??Başlat??6#   $
??Left=?64   $??288.9,29,0,0q?6O   $	??Top=?6e   $??75??6}   $??1??6?   ?? 5   
   -   5         ??Name$ label??5      $??Başlangıç Id??6   $
??Left=?66   $??	23,23,0,0q?6Q   $	??Top=?6d   $??2??6|   ?? 5   
   -   5         ??Name$ txtImdbIdStart??5      $
??Left=?6*   $??23??6E   $??48.4,26,0,0q?6Q    ?? TextWrapping$
 Wrap=?6f   $??1??6z   $	??Top=?6?   $	??120??6?   $??1??6?   ? 5   
   -   5        	 ?Name$	 progressBar??5      $
??Left=?6+   $??10??6F   $??
30,172,0,0q?6R   $	??Top=?6f   $	??402??6~   $??2??6?   ?? 5   
   -   5        $ 
label_Copy??5      $??	Bitiş Id??6$   $
??Left=?67   $??	23,51,0,0q?6R   $	??Top=?6e   $??2??6}   ?? 5   
   -   5        $ txtImdbIdEnd??5      $
??Left=?6(   $??23??6C   $??48.4,54,0,0q?6O   $
 Wrap=?6d   $??9999999??6x   $	??Top=?6?   $	??120??6?   $??1??6?   ?? 5   
   -   5        $ lblState??5      $
??Left=?6"   $??
30,126,0,0q?6=   $	??Top=?6Q   $	??402??6i   $??2??6u   5      5      