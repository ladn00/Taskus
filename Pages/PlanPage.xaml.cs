using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PlanPage.xaml
    /// </summary>
    public partial class PlanPage : Page
    {
        string filename = AppDomain.CurrentDomain.BaseDirectory + "../../../source/Plan.xml";
        List<Plan> plans = new List<Plan>();
        Plan selected;

        public PlanPage()
        {
            InitializeComponent();
            
            try
            {
                plans = ImportFromXml();
                UpdateDatagridItemsSource();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        List<Plan> ImportFromXml()
        {

            var list = new List<Plan>();
            using (XmlReader reader = XmlReader.Create(filename))
            {
                while (reader.Read())
                {
                    Plan plan = new Plan();
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "name")
                    {
                        plan.Name = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "desc")
                    {
                        plan.Desc = reader.ReadElementContentAsString();
                    }
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == "isdone")
                    {
                        plan.IsDone = bool.Parse(reader.ReadElementContentAsString());
                    }

                    if (!String.IsNullOrEmpty(plan.Name))
                        list.Add(plan);
                }
            }

            return list;
        }

        void ExportPlansToXml(List<Plan> list)
        {
            using (XmlWriter writer = XmlWriter.Create(filename))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("plans");

                foreach (var plan in list)
                {
                    writer.WriteStartElement("plan");
                    writer.WriteElementString("name", plan.Name);
                    writer.WriteElementString("desc", plan.Desc);
                    writer.WriteElementString("isdone", plan.IsDone.ToString());
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        void OverwriteXmlFile()
        {
            File.WriteAllText(filename, "");
            ExportPlansToXml(plans);
        }

        void UpdateDatagridItemsSource()
        {
            dg1.ItemsSource = null;
            dg1.ItemsSource = plans;
        }

        int GetIndexOfSelected()
        {
            int i = 0;
            for (i = 0; i < plans.Count; i++)
            {
                if (selected == plans[i])
                    break;
            }
            return i;
        }

        void CreateNewplans()
        {
            selected = new Plan();
            selected.Name = "-";
        }

        void SetDataContextAndAddItemToList(Control editTb)
        {
            editTb.DataContext = selected;
            plans.Add(selected);
        }

        void CheckSelectedOnNullAndSetValue(Control editTb)
        {
            if (selected == null)
                selected = editTb.DataContext as Plan;
        }

        private void tbDesc_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;

                if(selected == null)
                    selected = editTb.DataContext as Plan;

                if(selected != null)
                {
                    int i = 0;
                    for(i = 0; i < plans.Count; i++)
                    {
                        if (selected == plans[i])
                            break;
                    }
                    plans[i].Desc = editTb.Text;
                }
                else
                {
                    selected = new Plan();
                    selected.Desc = editTb.Text;
                    selected.Name = "-";
                    editTb.DataContext = selected;
                    plans.Add(selected);
                }
                
                File.WriteAllText(filename, "");
                ExportPlansToXml(plans);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void tbName_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var editTb = sender as TextBox;
                selected = editTb.DataContext as Plan;

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    plans[i].Name = editTb.Text;
                }
                else
                {
                    CreateNewplans();
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

        private void rbStatus_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var editTb = sender as CheckBox;

                CheckSelectedOnNullAndSetValue(editTb);

                if (selected != null)
                {
                    int i = GetIndexOfSelected();
                    plans[i].IsDone = editTb.IsChecked.Value;
                }
                else
                {
                    CreateNewplans();
                    selected.IsDone = editTb.IsChecked.Value;
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

        private void PlanDelete_Click(object sender, RoutedEventArgs e)
        {
            var editTb = sender as Button;
            var selected = editTb.DataContext as Plan;
            plans.Remove(selected);
            OverwriteXmlFile();
            UpdateDatagridItemsSource();
        }

        private void PlanAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateNewplans();
            plans.Add(selected);
            OverwriteXmlFile();
            UpdateDatagridItemsSource();
        }
    }
}
