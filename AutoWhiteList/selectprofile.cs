using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoWhiteList
{
    public partial class selectprofile : Form
    {
        public selectprofile()
        {
            InitializeComponent();
        }

        private void selectprofile_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtPath.Text))
            {
                var list = Directory.GetDirectories(txtPath.Text,"*", SearchOption.TopDirectoryOnly).ToList();
                //MessageBox.Show(list.Count.ToString());
                for (int i = 0; i < list.Count; i++)
                {
                    dataGridView1.Rows.Add(i + 1, list[i].ToString());
                }
                //dataGridView1.DataSource = list;
            }
            
        }
    }
}
