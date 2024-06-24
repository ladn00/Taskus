using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using Taskus.classes;
using Taskus.Pages;

namespace Taskus
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            frame1.NavigationService.Navigate(new Pages.Home());

            /*classes.Music[] plan = new classes.Music[] { new classes.Music { Name = "BRAT", Desc = "best 2024 electronic album PUMPIN THAT PUMPIN THAT PUMPIN THAT PUMPIN THAT PUMPIN THAT PUMPIN THAT", Link = "https://open.spotify.com/album/2lIZef4lzdvZkiiCzvPKj7?si=karCAtcxTvm_J9slmdYs6A" },
            new classes.Music { Name = @"yt", Desc = "desc 1", Link = "https://youtu.be/xJGDOtp84oY?si=mXlksXJHXXLzrUu1" } };
            string filename = AppDomain.CurrentDomain.BaseDirectory + "../../../source/Music.xml";
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("musics");

                foreach (var el in plan)
                {
                    writer.WriteStartElement("music");
                    writer.WriteElementString("name", el.Name);
                    writer.WriteElementString("desc", el.Desc);
                    writer.WriteElementString("link", el.Link);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }*/
        }

        private void GoToGitHub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/ladn00") { UseShellExecute = true });
        }

        private void rbPlan_Ckeched(object sender, RoutedEventArgs e)
        {
            frame1.NavigationService.Navigate(new Pages.PlanPage());
        }

        private void HotKeys(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                frame1.NavigationService.Navigate(new Home());
                ResetRbChecked();
            }
        }

        void ResetRbChecked()
        {
            rb1Menu.IsChecked = false;
            rb2Menu.IsChecked = false;
            rb3Menu.IsChecked = false;
            rb4Menu.IsChecked = false;
            rb5Menu.IsChecked = false;
            rb6Menu.IsChecked = false;
        }

        private void rbStops_Checked(object sender, RoutedEventArgs e)
        {
            frame1.NavigationService.Navigate(new Pages.StopsPage());
        }

        private void rbNotes_Checked(object sender, RoutedEventArgs e)
        {
            frame1.NavigationService.Navigate(new Pages.NotesPage());
        }

        private void rbLinks_Checked(object sender, RoutedEventArgs e)
        {
            frame1.NavigationService.Navigate(new Pages.LinksPage());
        }

        private void rbHeHeLinks_Checked(object sender, RoutedEventArgs e)
        {
            frame1.NavigationService.Navigate(new Pages.HeHeLinks());
        }

        private void rbMusic_Checked(object sender, RoutedEventArgs e)
        {
            frame1.NavigationService.Navigate(new Pages.MusicPage());
        }
    }
}
