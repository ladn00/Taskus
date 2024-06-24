using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
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
using System.Xml.Serialization;
using Taskus.classes;

namespace Taskus.Pages
{
    /// <summary>
    /// Логика взаимодействия для StopsPage.xaml
    /// </summary>
    public partial class StopsPage : Page
    {
        string filename = AppDomain.CurrentDomain.BaseDirectory + "../../../source/Stops.xml";
        List<Stops> stops = new List<Stops>();
        Stops selected;

        public StopsPage()
        {
            InitializeComponent();

            try
            {
                stops = ImportFromXml();
                dg1.ItemsSource = stops;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        List<Stops> ImportFromXml()
        {

            var list = new List<Stops>();
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    Stops stop = new Stops();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                    {
                        stop.Name = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "desc")
                    {
                        stop.Desc = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "link")
                    {
                        stop.Link = reader.ReadElementContentAsString();
                    }

                    if (!String.IsNullOrEmpty(stop.Name))
                        list.Add(stop);
                }
            }

            return list;
        }

        void ExportPlansToXml(List<Stops> list)
        {
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("stops");

                foreach (var stop in list)
                {
                    writer.WriteStartElement("stop");
                    writer.WriteElementString("name", stop.Name);
                    writer.WriteElementString("desc", stop.Desc);
                    writer.WriteElementString("link", stop.Link);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        void OverwriteXmlFile()
        {
            File.WriteAllText(filename, "");
            ExportPlansToXml(stops);
        }

        void UpdateDatagridItemsSource()
        {
            dg1.ItemsSource = null;
            dg1.ItemsSource = stops;
        }

        int GetIndexOfSelected()
        {
            int i = 0;
            for (i = 0; i < stops.Count; i++)
            {
                if (selected == stops[i])
                    break;
            }
            return i;
        }

        void CreateNewStops()
        {
            selected = new Stops();
            selected.Name = "-";
        }

        void SetDataContextAndAddItemToList(Control editTb)
        {
            editTb.DataContext = selected;
            stops.Add(selected);
        }

        void CheckSelectedOnNullAndSetValue(Control editTb)
        {
            if(selected == null)
                selected = editTb.DataContext as Stops;
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;
                selected = editTb.DataContext as Stops;

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    stops[i].Name = editTb.Text;
                }
                else
                {
                    CreateNewStops();
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
                    stops[i].Desc = editTb.Text;
                }
                else
                {
                    CreateNewStops();
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

        private void tbLink_Changed(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;

                CheckSelectedOnNullAndSetValue(editTb);

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    stops[i].Link = editTb.Text;
                }
                else
                {
                    CreateNewStops();
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

        private void StopDelete_Click(object sender, RoutedEventArgs e)
        {
            var editTb = sender as Button;
            var selected = editTb.DataContext as Stops;
            stops.Remove(selected);
            OverwriteXmlFile();
            UpdateDatagridItemsSource();
        }

        private void StopsAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateNewStops();
            stops.Add(selected);
            OverwriteXmlFile();
            UpdateDatagridItemsSource();
        }
    }
}
