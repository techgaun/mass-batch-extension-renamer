using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace MassExtensionRenamer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string foldername = folderDialog.SelectedPath;
                txtPath.Text = foldername;
                //string[] files = Directory.GetFiles(foldername);
            }
        }

        private void rename_files()
        {
            string path = txtPath.Text.ToString();
            string input = txtInput.Text.ToString();
            string output = txtOutput.Text.ToString();

            try
            {
                List<string> fullPathFiles;
                String str = input.Substring(0, 2);
                if (input.Substring(0, 2) == "*." || input.Substring(0, 2) == "?.")
                {
                    fullPathFiles = Directory.GetFiles(path, input).ToList();
                }
                else
                {
                    var regexTest = new Func<string, bool>(p => Regex.IsMatch(p, input, RegexOptions.Compiled | RegexOptions.IgnoreCase));
                    fullPathFiles = Directory.GetFiles(path).Where(regexTest).ToList();
                }                
                if (fullPathFiles.Count == 0)
                {
                    lblResult.Text = "No such file(s) found!!!";
                    lblResult.ForeColor = System.Drawing.Color.Red;
                    lblResult.Show();
                    return;
                }
                foreach (string file in fullPathFiles)
                {
                    try
                    {
                        int counter = 0;
                        string new_filename = path + "\\" + Path.GetFileNameWithoutExtension(file);
                        string final_path;
                        while (File.Exists(new_filename + "." + output))
                        {
                            new_filename = new_filename + counter;
                        }
                        final_path = new_filename + "." + output;
                        File.Move(file, final_path);
                    }
                    catch (Exception ex)
                    {
                        lblResult.ForeColor = System.Drawing.Color.Red;
                        lblResult.Text = "Could not rename the file " + file;
                        lblResult.Show();
                    }
                }
                lblResult.Text = "Mass Extension Renaming Completed Successfully";
                lblResult.ForeColor = System.Drawing.Color.Blue;
                lblResult.Show();
            }
            catch (Exception ex)
            { 
                //error
                lblResult.ForeColor = System.Drawing.Color.Red;
                lblResult.Text = "Could not read the directory";
                lblResult.Show();
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            DialogResult ask = MessageBox.Show("Should I start renaming files now?", "You sure about this?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (ask == DialogResult.Yes)
            {
                rename_files();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblInfo.Links.Add(0, 0, "http://www.techgaun.com");
        }

        private void lblInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.techgaun.com");
        }
    }
}
