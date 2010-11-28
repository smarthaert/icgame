using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GameObjectCreator
{
    public enum EditState
    {
        Creating,
        Editing,
        None
    }

    public partial class Form1 : Form
    {
        private IOHandler ioHandler;
        private EditState editState;
        private int selectedToEdit;

        public Form1()
        {
            InitializeComponent();
            ioHandler = new IOHandler("..\\..\\..\\Content\\Resources\\GameObjectStats.xml");       //very, VERY creepy
            comboBox1.Items.AddRange(new object[] {"Vehicle", "Infantry", "Building", "StaticObject", "Civilian"});
            editState = EditState.None;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateListBox1();
        }

        private void UpdateListBox1()
        {
            listBox1.Items.Clear();
            foreach(XElement element in ioHandler.XDocument.Root.Elements())
            {
                listBox1.Items.Add(element.Attribute("Name").Value);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClearPanel1();
            editState = EditState.Creating;
            panel1.Enabled = true;
        }

        private void ClearPanel1()
        {
            dataGridView1.Rows.Clear();
            textBox1.Text = "";
            comboBox1.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ClearPanel1();
            panel1.Enabled = false;
            editState = EditState.None;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if(textBox1.Text == "" || comboBox1.Text == "")
                {
                    throw new Exception();
                }
                //TODO: Sprawdzanie obecności wymaganych pól

                XElement xElement= new XElement("GameObject");
                xElement.SetAttributeValue("Name", textBox1.Text);
                xElement.SetAttributeValue("Type", comboBox1.Text);
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        XElement el = new XElement("GameObjectAttribute");
                        el.SetAttributeValue("AttributeName", row.Cells[0].Value);
                        el.SetAttributeValue("AttributeType", row.Cells[1].Value);
                        el.Value = row.Cells[2].Value.ToString();

                        xElement.Add(el);
                    }
                }
                if (editState == EditState.Creating)
                {
                    ioHandler.XDocument.Root.Add(xElement);
                }
                else if (editState == EditState.Editing)
                {
                    ioHandler.XDocument.Root.Elements().ElementAt(selectedToEdit).ReplaceWith(xElement);
                }
                else
                {
                    throw new Exception();
                }
                
                ioHandler.WriteXMLFile();

                UpdateListBox1();
                ClearPanel1();

                editState = EditState.None;

                panel1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Insufficent data\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(listBox1.Items.Count > 0 && listBox1.SelectedIndex >=0)
            {
                selectedToEdit = listBox1.SelectedIndex;

                FillPanel1(selectedToEdit);
                editState = EditState.Editing;
                panel1.Enabled = true;
            }
        }

        private void FillPanel1(int index)
        {
            if (index >= 0)
            {
                textBox1.Text = ioHandler.XDocument.Root.Elements().ElementAt(index).Attribute("Name").Value;

                comboBox1.Text = ioHandler.XDocument.Root.Elements().ElementAt(index).Attribute("Type").Value;

                dataGridView1.Rows.Clear();
                foreach (
                    XElement xElement in
                        ioHandler.XDocument.Root.Elements().ElementAt(index).Elements("GameObjectAttribute"))
                {
                    dataGridView1.Rows.Add(new object[]
                                               {
                                                   xElement.Attribute("AttributeName").Value,
                                                   xElement.Attribute("AttributeType").Value,
                                                   xElement.Value
                                               });
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(editState == EditState.None)
            {
                FillPanel1(((ListBox)sender).SelectedIndex);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(editState == EditState.None && listBox1.Items.Count > 0 && listBox1.SelectedIndex >=0)
            {
                ioHandler.XDocument.Root.Elements().ElementAt(listBox1.SelectedIndex).Remove();
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                ClearPanel1();

                ioHandler.WriteXMLFile();
            }
        }
    }
}
