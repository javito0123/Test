using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace STIGDocument
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string xx = "test";
            string inputFile = txtInputDocu.Text;
            string outputFile = txtOutputDocu.Text;
            ReadAndWriteTextFile(inputFile, outputFile);
            int count = CountGroupID(inputFile);
            txtGroupIDCount.Text = count.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string inputFile = textBox2.Text;
            string outputFile = textBox1.Text;
            GenerateXMLFile(inputFile, outputFile);
            MessageBox.Show("Done");
        }

        private void ReadAndWriteTextFile(string inputFile, string outputFile)
        {
            List<string> titles = new List<string>();
            titles.Add("Group ID (Vulid):");
            titles.Add("Group Title:");
            titles.Add("Rule ID:");
            titles.Add("Severity:");
            titles.Add("Rule Version (STIG-ID):");
            titles.Add("Rule Title:");
            titles.Add("Vulnerability Discussion:");
            titles.Add("Responsibility:");
            titles.Add("IAControls:");
            titles.Add("Check Content:");
            titles.Add("Fix Text:");
            //
            //
            StreamReader re = File.OpenText(inputFile);
            StreamWriter ro = new StreamWriter(outputFile);
            string input = null;
            string outputLine = "";
            bool lineBreak = false;
            while ((input = re.ReadLine()) != null)
            {
                if (input.Length > 0)
                {
                    if (input.StartsWith(txtBreakingString.Text))
                    {
                        ro.WriteLine(outputLine);
                        outputLine = "";
                        lineBreak = true;
                        continue;
                    }
                    bool found = false;
                    foreach (string title in titles)
                    {
                        if (input.StartsWith(title))
                        {
                            if (lineBreak)
                            {
                                lineBreak = false;
                            }
                            else
                            {
                                outputLine = outputLine + "|";
                            }
                            outputLine = outputLine + input.Substring(input.IndexOf(":") + 1, input.Length - input.IndexOf(":") - 1);
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        continue;
                    }
                    outputLine = outputLine + input;
                }
            }
            re.Close();
            ro.Close();
            MessageBox.Show("Done");
        }

        private int CountGroupID(string inputFile)
        {
            StreamReader re = File.OpenText(inputFile);
            string input = null;
            int count = 0;
            while ((input = re.ReadLine()) != null)
            {
                if (input.Length > 0 && input.StartsWith("Group ID (Vulid):"))
                {
                    count++;
                }
            }
            re.Close();
            return count;
        }


        private void GenerateXMLFile(string inputFile, string outputFile)
        {
            List<string> tabs = new List<string>();
            tabs.Add("GroupId");
            tabs.Add("GroupTitle");
            tabs.Add("RuleId");
            tabs.Add("Severity");
            tabs.Add("RuleVersion");
            tabs.Add("RuleTitle");
            tabs.Add("Where");
            tabs.Add("Applied");
            tabs.Add("Type");
            tabs.Add("Value");
            tabs.Add("Ignore");
            tabs.Add("IgnoreReason");
            //
            //
            StreamReader re = File.OpenText(inputFile);
            StreamWriter ro = new StreamWriter(outputFile);
            ro.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            ro.WriteLine("<Groups>");
            string input = null;
            while ((input = re.ReadLine()) != null)
            {
                ro.WriteLine("  <Group>");
                int columnsCount = 0;
                string[] columns = input.Split('\t');
                foreach (string column in columns)
                {
                    ro.WriteLine("    <" + tabs[columnsCount] + ">" + column + "</" + tabs[columnsCount] + ">");
                    columnsCount++;
                }
                //
                for (int i = 7; i <= 11; i++)
                {
                    ro.WriteLine("    <" + tabs[i] + ">" + "</" + tabs[i] + ">");
                }
                ro.WriteLine("  </Group>");
            }
            ro.WriteLine("</Groups>");
            re.Close();
            ro.Close();
        }
    }
}
