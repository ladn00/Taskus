using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Taskus.classes;

namespace Taskus.Pages
{
    /// <summary>
    /// Логика взаимодействия для NotesPage.xaml
    /// </summary>
    public partial class NotesPage : Page
    {
        string filename = AppDomain.CurrentDomain.BaseDirectory + "../../../source/Notes.xml";
        ObservableCollection<Notes> notes = new ObservableCollection<Notes>();
        Notes selected;

        public NotesPage()
        {
            InitializeComponent();
            try
            {
                notes = ImportFromXml();
                notes.GroupBy(x => x.ToPin);
                dg1.ItemsSource = notes;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        ObservableCollection<Notes> ImportFromXml()
        {

            var ObservableCollection = new ObservableCollection<Notes>();
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    Notes note = new Notes();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                    {
                        note.Name = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "desc")
                    {
                        note.Desc = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "topin")
                    {
                        note.ToPin = bool.Parse(reader.ReadElementContentAsString());
                    }

                    if (!String.IsNullOrEmpty(note.Name))
                        ObservableCollection.Add(note);
                }
            }

            return ObservableCollection;
        }

        void ExportNotesToXml(ObservableCollection<Notes> ObservableCollection)
        {
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("notes");

                foreach (var note in ObservableCollection)
                {
                    writer.WriteStartElement("note");
                    writer.WriteElementString("name", note.Name);
                    writer.WriteElementString("desc", note.Desc);
                    writer.WriteElementString("topin", note.ToPin.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        void OverwriteXmlFile()
        {
            File.WriteAllText(filename, "");
            ExportNotesToXml(notes);
        }

        void UpdateDatagridItemsSource()
        {
            dg1.ItemsSource = null;
            dg1.ItemsSource = notes;
        }

        int GetIndexOfSelected()
        {
            int i = 0;
            for (i = 0; i < notes.Count; i++)
            {
                if (selected == notes[i])
                    break;
            }
            return i;
        }

        void CreateNewNotes()
        {
            selected = new Notes();
            selected.Name = "-";
        }

        void SetDataContextAndAddItemToList(Control editTb)
        {
            editTb.DataContext = selected;
            notes.Add(selected);
        }

        void CheckSelectedOnNullAndSetValue(Control editTb)
        {
            if (selected == null)
                selected = editTb.DataContext as Notes;
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;
                selected = editTb.DataContext as Notes;

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    notes[i].Name = editTb.Text;
                }
                else
                {
                    CreateNewNotes();
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
                    notes[i].Desc = editTb.Text;
                }
                else {
                    CreateNewNotes();
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

        private void rbToPin_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var editTb = sender as CheckBox;

                CheckSelectedOnNullAndSetValue(editTb);

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    notes[i].ToPin = editTb.IsChecked.Value;
                }
                else
                {
                    CreateNewNotes();
                    selected.ToPin = editTb.IsChecked.Value;
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

        private void NoteDelete_Click(object sender, RoutedEventArgs e)
        {
            var editTb = sender as Button;
            var selected = editTb.DataContext as Notes;
            notes.Remove(selected);
            OverwriteXmlFile();
            notes.GroupBy(x=>x.ToPin);
            UpdateDatagridItemsSource();
        }

        private void NotesAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateNewNotes();
            notes.Add(selected);
            OverwriteXmlFile();
            UpdateDatagridItemsSource();
        }
    }
}
