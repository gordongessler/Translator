using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator
{
    public partial class InputDialog : Form
    {
        Regex r = new Regex(@"^[a-zA-Z]+$");
        public string source { get; set; }
        public string target { get; set; }

        public InputDialog()
        {
            InitializeComponent();
        }

        public DialogResult Show(string title, string lang1, string lang2, string inputWord)
        {
            Text = title;
            soureLabel.Text = lang1;
            targetLabel.Text = lang2;
            if(inputWord!="")
            {
                textBox1.Text = inputWord;
                textBox1.Enabled = false;
            }
            return (ShowDialog());
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (r.IsMatch(textBox1.Text)&&r.IsMatch(textBox2.Text))
            {
                this.source = textBox1.Text;
                this.target = textBox2.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Validation error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                errorProvider1.SetError(textBox1, "Cannot be empty!");
            }
            else if (r.IsMatch(textBox1.Text))
            {
                errorProvider1.SetError(textBox1, null);
            }
            else
            {
                errorProvider1.SetError(textBox1, "Only letters are allowed");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                errorProvider1.SetError(textBox2, "Cannot be empty!");
            }
            else if (r.IsMatch(textBox2.Text) || textBox2.Text == "")
            {
                errorProvider1.SetError(textBox2, null);
            }
            else
            {
                errorProvider1.SetError(textBox2, "Only letters are allowed");
            }
        }
    }
}
