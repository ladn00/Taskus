using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
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

namespace Taskus.Pages
{
    /// <summary>
    /// Логика взаимодействия для MusicPage.xaml
    /// </summary>
    public partial class MusicPage : Page
    {
        string filename = AppDomain.CurrentDomain.BaseDirectory + "../../../source/Music.xml";
        ObservableCollection<Music> music = new ObservableCollection<Music>();
        Music selected;

        public MusicPage()
        {
            InitializeComponent();

            try
            {
                music = ImportFromXml();
                lw1.ItemsSource = music;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ObservableCollection<Music> ImportFromXml()
        {

            var ObservableCollection = new ObservableCollection<Music>();
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    Music link = new Music();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                    {
                        link.Name = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "desc")
                    {
                        link.Desc = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "link")
                    {
                        link.Link = reader.ReadElementContentAsString();
                    }

                    if (!String.IsNullOrEmpty(link.Name))
                        ObservableCollection.Add(link);
                }
            }

            return ObservableCollection;
        }

        void ExportLinksToXml(ObservableCollection<Music> ObservableCollection)
        {
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("musics");

                foreach (var note in ObservableCollection)
                {
                    writer.WriteStartElement("music");
                    writer.WriteElementString("name", note.Name);
                    writer.WriteElementString("desc", note.Desc);
                    writer.WriteElementString("link", note.Link);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        void OverwriteXmlFile()
        {
            File.WriteAllText(filename, "");
            ExportLinksToXml(music);
        }

        void UpdateListViewItemsSource()
        {
            lw1.ItemsSource = null;
            lw1.ItemsSource = music;
        }

        int GetIndexOfSelected()
        {
            int i = 0;
            for (i = 0; i < music.Count; i++)
            {
                if (selected == music[i])
                    break;
            }
            return i;
        }

        void CreateNewlinks()
        {
            selected = new Music();
            selected.Name = "Name";
            selected.Desc = "Desc";
            selected.Link = "Link";
        }

        void SetDataContextAndAddItemToList(Control editTb)
        {
            editTb.DataContext = selected;
            music.Add(selected);
        }

        void CheckSelectedOnNullAndSetValue(Control editTb)
        {
            if (selected == null)
                selected = editTb.DataContext as Music;
        }

        private void GoByLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var editTb = sender as Button;
                selected = editTb.DataContext as Music;
                Process.Start(new ProcessStartInfo(selected.Link) { UseShellExecute = true });
            }
            catch { MessageBox.Show("Недействительная ссылка."); }

        }

        private void LinkAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateNewlinks();
            music.Add(selected);
            OverwriteXmlFile();
            UpdateListViewItemsSource();
        }

        private void LinkDelete_Click(object sender, RoutedEventArgs e)
        {
            var toDelete = lw1.SelectedItem as Music;
            music.Remove(toDelete);
            OverwriteXmlFile();
            UpdateListViewItemsSource();
        }

        private void btName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;
                selected = editTb.DataContext as Music;

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    music[i].Name = editTb.Text;
                }
                else
                {
                    CreateNewlinks();
                    selected.Name = editTb.Text;
                    SetDataContextAndAddItemToList(editTb);
                }
                OverwriteXmlFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;

                CheckSelectedOnNullAndSetValue(editTb);

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    music[i].Desc = editTb.Text;
                }
                else
                {
                    CreateNewlinks();
                    selected.Desc = editTb.Text;
                    SetDataContextAndAddItemToList(editTb);
                }

                OverwriteXmlFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tbLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;

                CheckSelectedOnNullAndSetValue(editTb);

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    music[i].Link = editTb.Text;
                }
                else
                {
                    CreateNewlinks();
                    selected.Link = editTb.Text;
                    SetDataContextAndAddItemToList(editTb);
                }
                selected = null;
                OverwriteXmlFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
