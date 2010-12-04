using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace GameObjectCreator
{
    public partial class EditAttributes : Form
    {
        private XElement attribute;

        public EditAttributes(ref XElement attribute)
        {
            this.attribute = attribute;
            InitializeComponent();

            dataGridView1.Rows.Clear();
            foreach (XAttribute xAttribute in attribute.Attributes().Where(s => s.Name != "AttributeName" && s.Name != "AttributeType"))
            {
                dataGridView1.Rows.Add(new object[] {xAttribute.Name, xAttribute.Value});
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            attribute.Attributes().Where(s => s.Name != "AttributeName" && s.Name != "AttributeType").Remove();

            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                if(!dataGridViewRow.IsNewRow)
                {
                    try
                    {
                        attribute.SetAttributeValue(dataGridViewRow.Cells[0].Value.ToString(), dataGridViewRow.Cells[1].Value);
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
            }

            Close();
        }
    }
}
