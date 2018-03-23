using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        
        private void loadMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            Regex r = new Regex(@"^[a-zA-Z]+ [a-zA-Z]+$");
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            StreamReader sr = new StreamReader(myStream);

                            //Read the file line by line and add the lines to a collection
                            string line;
                            bool addedCollmuns = false;

                            //Clear previous data
                            listView.Columns.Clear();
                            listView.Items.Clear();

                            while ((line = sr.ReadLine()) != null)
                            {
                                line.Trim();
                                if (r.IsMatch(line)) {
                                    string[] words = line.Split();
                                    if (addedCollmuns)
                                    {
                                        ListViewItem li = new ListViewItem();
                                        li.Text = words[0];
                                        li.SubItems.Add(words[1]);
                                        listView.Items.Add(li);
                                    }
                                    else
                                    {
                                        listView.Columns.Add(words[0]);
                                        listView.Columns.Add(words[1]);
                                        addedCollmuns = true;
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void exportMenuItem_Click(object sender, EventArgs e)
        {
            //TODO: Add Exception handling

            if (saveFileDialog.ShowDialog() == DialogResult.OK && saveFileDialog.FileName != "" && listView.Columns.Count>=2)
            {
                using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                {
                    sw.WriteLine(listView.Columns[0].Text + " " + listView.Columns[1].Text);
                    for (int i =0; i<listView.Items.Count; i++)
                    {
                        sw.WriteLine(listView.Items[i].SubItems[1].Text + " " + listView.Items[i].SubItems[1].Text);
                    }
                    sw.Close();
                }
            }
        }

        private void translateButton_Click(object sender, EventArgs e)
        {
            string input = inputTextBox.Text;
            for (int i = 0; i < listView.Items.Count; i++)
            {
                input = Regex.Replace(input, listView.Items[i].SubItems[0].Text, listView.Items[i].SubItems[1].Text, RegexOptions.IgnoreCase);
            }

            //TODO: Make the output word start with a capital letter if the source word started with one. This is not required by the task nor is it implemented in the example app

            outputTextBox.Text = input;

            //Make everything red
            outputTextBox.SelectAll();
            outputTextBox.SelectionColor = Color.Red;
            
            //Make translated words black
            for (int i = 0; i < listView.Items.Count; i++)
            {
                ChangeColor(listView.Items[i].SubItems[1].Text,Color.Black,0);
            }

            //Make all puncuation and numbers black as well
            for(char c = ' '; c < 'A'; c++)
            {
                ChangeColor(c+"", Color.Black, 0);
            }
            for (char c = '['; c < 'a'; c++)
            {
                ChangeColor(c + "", Color.Black, 0);
            }
        }

        private void ChangeColor(string word, Color color, int startIndex)
        {
            if (this.outputTextBox.Text.Contains(word))
            {
                int index = -1;
                int selectStart = this.outputTextBox.SelectionStart;

                while ((index = this.outputTextBox.Text.IndexOf(word, (index + 1))) != -1)
                {
                    this.outputTextBox.Select((index + startIndex), word.Length);
                    this.outputTextBox.SelectionColor = color;
                    this.outputTextBox.Select(selectStart, 0);
                    this.outputTextBox.SelectionColor = Color.Black;
                }
            }
        }

    }
}
