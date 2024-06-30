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
    /// Логика взаимодействия для HeHeLinks.xaml
    /// </summary>
    public partial class HeHeLinks : Page
    {

        string filename = AppDomain.CurrentDomain.BaseDirectory + "../../../source/HeHeLinks.xml";
        ObservableCollection<classes.HeHeLinks> links = new ObservableCollection<classes.HeHeLinks>();
        classes.HeHeLinks selected;

        public HeHeLinks()
        {
            InitializeComponent();
            try
            {
                links = ImportFromXml();
                lw1.ItemsSource = links;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ObservableCollection<classes.HeHeLinks> ImportFromXml()
        {

            var ObservableCollection = new ObservableCollection<classes.HeHeLinks>();
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    classes.HeHeLinks link = new classes.HeHeLinks();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                    {
                        link.Name = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "desc")
                    {
                        link.Desc = reader.ReadElementContentAsString();
                    }

                    if (!String.IsNullOrEmpty(link.Name))
                        ObservableCollection.Add(link);
                }
            }

            return ObservableCollection;
        }

        void ExportHeHeLinksToXml(ObservableCollection<classes.HeHeLinks> ObservableCollection)
        {
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("links");

                foreach (var note in ObservableCollection)
                {
                    writer.WriteStartElement("link");
                    writer.WriteElementString("name", note.Name);
                    writer.WriteElementString("desc", note.Desc);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        void OverwriteXmlFile()
        {
            File.WriteAllText(filename, "");
            ExportHeHeLinksToXml(links);
        }

        void UpdateListViewItemsSource()
        {
            lw1.ItemsSource = null;
            lw1.ItemsSource = links;
        }

        int GetIndexOfSelected()
        {
            int i = 0;
            for (i = 0; i < links.Count; i++)
            {
                if (selected == links[i])
                    break;
            }
            return i;
        }

        void CreateNewlinks()
        {
            selected = new classes.HeHeLinks();
            selected.Name = "Ссылка";
            selected.Desc = "Desc";
        }

        void SetDataContextAndAddItemToList(Control editTb)
        {
            editTb.DataContext = selected;
            links.Add(selected);
        }

        void CheckSelectedOnNullAndSetValue(Control editTb)
        {
            if (selected == null)
                selected = editTb.DataContext as classes.HeHeLinks;
        }

        private void GoByLink_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var editTb = sender as Button;
                selected = editTb.DataContext as classes.HeHeLinks;
                Process.Start(new ProcessStartInfo(selected.Name) { UseShellExecute = true });
            }
            catch { MessageBox.Show("Недействительная ссылка.");}
            
        }

        private void LinkAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateNewlinks();
            links.Add(selected);
            OverwriteXmlFile();
            UpdateListViewItemsSource();
        }

        private void LinkDelete_Click(object sender, RoutedEventArgs e)
        {
            var toDelete = lw1.SelectedItem as classes.HeHeLinks;
            links.Remove(toDelete);
            OverwriteXmlFile();
            UpdateListViewItemsSource();
        }

        private void btName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;
                selected = editTb.DataContext as classes.HeHeLinks;

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    links[i].Name = editTb.Text;
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
                    links[i].Desc = editTb.Text;
                }
                else
                {
                    CreateNewlinks();
                    selected.Desc = editTb.Text;
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
