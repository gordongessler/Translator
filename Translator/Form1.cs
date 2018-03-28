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
        Regex r = new Regex(@"^[a-zA-Z]+ [a-zA-Z]+$");
        Regex r2 = new Regex(@"^[a-zA-Z]+$");
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
                                    bool skip = false;
                                    string[] words = line.Split();
                                    foreach(ListViewItem l in listView.Items)
                                    {
                                        if (l.Text == words[0])
                                            skip = true;
                                    }
                                    if (skip) continue; //dont add duplicates
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
            StringComparer stringComparer = StringComparer.CurrentCultureIgnoreCase;
            outputTextBox.Text = "";
            string[] lines = inputTextBox.Text.Split('\n');
            for ( int j = 0; j<lines.Count();j++)
            {
                string s = lines[j];
                if(j!=0)outputTextBox.AppendText("\n");
                string[] words = s.Split(' ');
                for (int i = 0; i < words.Count(); i++)
                {
                    if(i!=0) outputTextBox.AppendText(" ");
                    bool added = false;
                    foreach (ListViewItem l in listView.Items)
                    {
                        if (stringComparer.Equals(l.Text, words[i].Trim()))
                        {
                            added = true;
                            string newWord = Regex.Replace(words[i],l.Text, l.SubItems[1].Text,RegexOptions.IgnoreCase);
                            outputTextBox.SelectionStart = outputTextBox.Text.Length;
                            var oldcolor = outputTextBox.SelectionColor;
                            outputTextBox.SelectionColor = Color.Black;
                            outputTextBox.AppendText(newWord);
                            outputTextBox.SelectionColor = oldcolor;

                        }
                    }
                    if (!added)
                    {
                        outputTextBox.SelectionStart = outputTextBox.Text.Length;
                        var oldcolor = outputTextBox.SelectionColor;
                        outputTextBox.SelectionColor = Color.Red;
                        outputTextBox.AppendText(words[i]);
                        outputTextBox.SelectionColor = oldcolor;
                    }
                }
                outputTextBox.DeselectAll();
            }
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
            if(inputDialog.Show("Add a new word", listView.Columns[0].Text, listView.Columns[1].Text,"") == DialogResult.OK)
            {
                ListViewItem li = new ListViewItem();
                li.Text = inputDialog.source;
                li.SubItems.Add(inputDialog.target);
                listView.Items.Add(li);
            }
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
            setColor();
        }
        private void listView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listView_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                if (file.EndsWith(".txt")) { 
                StreamReader sr = new StreamReader(file);

                string line;
                bool addedCollmuns = false;

                //Clear previous data
                listView.Columns.Clear();
                listView.Items.Clear();
                while ((line = sr.ReadLine()) != null)
                {
                    line.Trim();
                    if (r.IsMatch(line))
                    {
                        bool skip = false;
                        string[] words = line.Split();
                        foreach (ListViewItem l in listView.Items)
                        {
                            if (l.Text == words[0])
                                skip = true;
                        }
                        if (skip) continue; //dont add duplicates
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
                            listView.Columns[0].Width = listView.Width / 2;
                            listView.Columns[1].Width = listView.Width / 2;
                            addedCollmuns = true;
                        }
                    }
                   }
                }
            }

        }

        private string WordUnderMouse(RichTextBox outputTextBox, int x, int y)
        {
            int pos = outputTextBox.GetCharIndexFromPosition(outputTextBox.PointToClient(Cursor.Position));
            if (pos <= 0) return "";

            string txt = outputTextBox.Text;

            int start_pos;
            for (start_pos = pos; start_pos > 0; start_pos--)
            {
                char ch = txt[start_pos];
                if (char.IsWhiteSpace(ch)) break;
            }


            int end_pos;
            for (end_pos = pos; end_pos < txt.Length; end_pos++)
            {
                char ch = txt[end_pos];
                if (char.IsWhiteSpace(ch)) break;
            }

            if (start_pos > end_pos) return "";
            return txt.Substring(start_pos, end_pos - start_pos);
           
        }

        private void outputTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    {
                        string word = WordUnderMouse(outputTextBox, Cursor.Position.X, Cursor.Position.Y).Trim();
                        if (String.IsNullOrWhiteSpace(word)) return;
                        foreach(ListViewItem l in listView.Items)
                        {
                            if (l.SubItems[1].Text == word) return;
                        }
                        contextMenuStrip1.Items.Clear();
                        contextMenuStrip1.Items.Add("ADD " + word);
                        contextMenuStrip1.Items[0].Click += addToolStripMenuItem_Click;
                        contextMenuStrip1.Show(outputTextBox, new Point(e.X, e.Y));
                        
                    }
                    break;
            }
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InputDialog inputDialog = new InputDialog();
            if (inputDialog.Show("Add a new word", listView.Columns[0].Text, listView.Columns[1].Text, contextMenuStrip1.Items[0].Text.Substring(3).Trim()) == DialogResult.OK)
            {
                ListViewItem li = new ListViewItem();
                li.Text = inputDialog.source;
                li.SubItems.Add(inputDialog.target);
                listView.Items.Add(li);
            }
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


