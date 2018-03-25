using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator
{
    public partial class InputDialog : Form
    {
        public string source { get; set; }
        public string target { get; set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        public DialogResult Show(string title, string lang1, string lang2)
        {
            Text = title;
            soureLabel.Text = lang1;
            targetLabel.Text = lang2;
            return (ShowDialog());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.source = textBox1.Text;
            this.target = textBox2.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
