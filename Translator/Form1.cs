using System;
using System.Collections;
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
        SortOrder sort = SortOrder.None;
        public Form1()
        {
            InitializeComponent();
            this.listView.Columns[0].Width = listView.Width / 2;
            this.listView.Columns[1].Width = listView.Width / 2;

            foreach (FontFamily font in System.Drawing.FontFamily.Families)
            {
                toolStripComboBox.Items.Add(font.Name);
            }
            toolStripComboBox.SelectedIndex = toolStripComboBox.FindString("Calibri");
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
                                        listView.Columns[0].Width = listView.Width/2;
                                        listView.Columns[1].Width = listView.Width / 2;
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
                        sw.WriteLine(listView.Items[i].SubItems[0].Text + " " + listView.Items[i].SubItems[1].Text);
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

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void listView_Resize(object sender, EventArgs e)
        {
            if (listView.Columns.Count >= 2)
            {
                listView.Columns[0].Width = listView.Width / 2;
                listView.Columns[1].Width = listView.Width / 2;
            }
        }

        private void listView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                foreach( ListViewItem item in listView.SelectedItems)
                {
                   listView.Items.Remove(item);
                }
            }
        }

        private void listView_ColumnClick(object sender, ColumnClickEventArgs e)
        {

            sort = sort == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;

            if (sort == SortOrder.Ascending)
            {
                this.listView.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Ascending);
            }
            else
            {
                this.listView.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Descending);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InputDialog inputDialog = new InputDialog();
            if(inputDialog.Show("Add a new word", listView.Columns[0].Text, listView.Columns[1].Text) == DialogResult.OK)
            {
                ListViewItem li = new ListViewItem();
                li.Text = inputDialog.source;
                li.SubItems.Add(inputDialog.target);
                listView.Items.Add(li);
            }
            //add string validation
        }

        private void toolStripButtonBold_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Font.Bold)
            {
                inputTextBox.Font = new Font(inputTextBox.Font, FontStyle.Bold ^ inputTextBox.Font.Style);
                toolStripButtonBold.Checked = false;
            }
            else
            {
                inputTextBox.Font = new Font(inputTextBox.Font, FontStyle.Bold | inputTextBox.Font.Style);
                toolStripButtonBold.Checked = true;
            }
            setColor();
        }

        private void toolStripButtonItalic_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Font.Italic)
            {
                inputTextBox.Font = new Font(inputTextBox.Font, FontStyle.Italic ^ inputTextBox.Font.Style);
                toolStripButtonItalic.Checked = false;
            }
            else
            {
                inputTextBox.Font = new Font(inputTextBox.Font, FontStyle.Italic | inputTextBox.Font.Style);
                toolStripButtonItalic.Checked = true;
            }
            setColor();
        }

        private void toolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            if (inputTextBox.Font.Underline)
            {
                inputTextBox.Font = new Font(inputTextBox.Font, FontStyle.Underline ^ inputTextBox.Font.Style);
                toolStripButtonUnderline.Checked = false;
            }
            else
            {
                inputTextBox.Font = new Font(inputTextBox.Font, FontStyle.Underline | inputTextBox.Font.Style);
                toolStripButtonUnderline.Checked = true;
            }
            setColor();
        }

        private void toolStripButtonFontColor_Click(object sender, EventArgs e)
        {
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                setColor();
            }
        }


        private void toolStripButtonBgColor_Click(object sender, EventArgs e)
        {
            if (colorDialogBg.ShowDialog() == DialogResult.OK)
            {
                inputTextBox.BackColor = colorDialogBg.Color;
            }
        }
        private void setColor()
        {
            int start = inputTextBox.SelectionStart;
            int lenght = inputTextBox.SelectionLength;
            inputTextBox.SelectAll();
            inputTextBox.SelectionColor = colorDialog.Color;
            inputTextBox.Select(start, lenght);
        }

        private void toolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
           inputTextBox.Font = new Font(toolStripComboBox.Text, 12, inputTextBox.Font.Style);
        }
    }

    class ListViewItemComparer : IComparer
    {
        private int col;
        public SortOrder so;
        public ListViewItemComparer()
        {
            col = 0;
            so = SortOrder.Ascending;
        }
        public ListViewItemComparer(int column, SortOrder order)
        {
            col = column;
            so = order;
        }
        public int Compare(object x, object y)
        {
            if (so == SortOrder.Ascending)
            {
                return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
            else
            {
                return -String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
            }
        }
    }
}


