using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;



namespace CommandLineRunUserInterface
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        

        private void button1_Click(object sender, EventArgs e)
        {   //Assigns user input to the corresponding variables
            userName = textBox1.Text;
            password = textBox2.Text;
            ftpSite = textBox3.Text;
            ftpOutputUserName = textBox6.Text;
            ftpOutputPassword = textBox7.Text; ;
            ftpOutputSite = textBox8.Text;
            commandExecutable = textBox9.Text;
            string stringInputFolder = localPath + @"\Input\";
            string stringProcessedFolder = localPath + @"\Processed\";
            if (!textBox10.Text.Equals(""))
            {
                localPath = textBox10.Text;
            }
            if (!(Directory.Exists(stringInputFolder)))
                
            {

                Directory.CreateDirectory(stringInputFolder);
            }

            
            Process pdfComp = new Process();
            //Creates an array of elements for each line in input file
            FTPInfo fileNames = new FTPInfo();
            fileNames.DownloadFileNames(ftpSite, userName, password, fileNameList, stringInputFolder);
            fileNames.GetFileLocations();
            Console.WriteLine(fileNameList);

            var messageDialog = "Download Compete";
            MessageBox.Show(messageDialog);

            //Loops through the array to take different input file and process them out to the output file. Output the the content of the lines of the input file and indicates when it is finished. 
            foreach (string line in fileNameList)
            {
               if(!(line.Equals("")))
                {
                Console.WriteLine(line);
                string inFile = line;
                string newOutFile;
                //Replace from Input Folder to a possibe Output Folder
                //Replace the output file location to possible Drive Wanted
                string fileLocation = line.Substring(0, 2);
                if (!(textBox5.Text == ""))
                {
                    Console.WriteLine(fileLocation);
                    newOutFile = line.Replace(fileLocation, textBox5.Text);
                    Console.WriteLine(newOutFile);
                }
                else
                {
                    newOutFile = line;
                }
                //Executes Command Line
                if(!(textBox4.Text.Equals("")))
                {
                pdfComp.StartInfo.UseShellExecute = false;
                pdfComp.StartInfo.RedirectStandardOutput = true;
                pdfComp.StartInfo.RedirectStandardError = true;
                pdfComp.StartInfo.FileName = @commandExecutable; pdfComp.StartInfo.Arguments = string.Format(textBox4.Text + "-in {0} -out {1}", inFile, newOutFile);
                pdfComp.Start();
                string o = pdfComp.StandardOutput.ReadToEnd();
                Console.WriteLine(o);
                Console.WriteLine("Done");
                pdfComp.WaitForExit();
                }
                }
            }

            var messageDialogTwo = "Processing Complete";
            MessageBox.Show(messageDialogTwo);
            //Moves content of the input folder to processed folder
           Directory.Move(stringInputFolder, stringProcessedFolder);
            if (!(Directory.Exists(stringInputFolder)))
            {
                Directory.CreateDirectory(stringInputFolder);
            }
            UploadToFTPClass.recursiveDirectory(stringProcessedFolder, ftpOutputSite, ftpOutputUserName, ftpOutputPassword);
         

            var messageDialogThree = "Upload Complete";
            MessageBox.Show(messageDialogThree);

            Console.ReadLine();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

       


     
        

        
       

       
    }
}
