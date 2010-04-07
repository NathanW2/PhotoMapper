using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using VEarth.Locations;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace PhotoMapper
{
    public partial class PhotoMapper : Form
    {
        public delegate void Action();
        BackgroundWorker maploader = new BackgroundWorker();
        private bool MapIsLoaded;

        List<String> filestoProcess;
        // Create a list of locations to search for
        List<SearchLocation> locations = new List<SearchLocation>();

        public PhotoMapper()
        {
            InitializeComponent();
            this.Text = "Photo Mapper : Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.settingsBindingSource.DataSource = Properties.Settings.Default;

            this.Shown += new EventHandler(Form1_Shown);
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.fileListBox.DisplayMember = "Name";
            this.fileListBox.MultiColumn = true;

            if (!Properties.Settings.Default.LoadMap)
            {
                this.Height = this.MinimumSize.Height;
            }
        }

        void Form1_Shown(object sender, EventArgs e)
        {
            this.LoadMap();
        }

        public void LoadMap()
        {
            if (Properties.Settings.Default.LoadMap)
            {
                this.ucVEarth.HTMLLocation = Path.Combine(Application.StartupPath, "VirtualEarth.html");
                this.ucVEarth.InitMap(); // Initialize the VEarth user control
                ucVEarth.VE_SetZoomLevel(15);
                this.MapIsLoaded = true;
            }
        }

        private void PlacePictureOnMap(Picture picture)
        {
            SearchLocation a = new SearchLocation();
            a.Longitude = picture.GPSLongitude;  // where to search for (address)
            a.Latitude = picture.GPSLatitude;
            a.PushPinDescription = String.Format("<IMG SRC=\"{0}\" ALT=\"{0}\" WIDTH=200 HEIGHT=200>", picture.FileName);
            a.PushPinLayer = "Photos";
            a.PushPinTitle = picture.Name;  // Title of the puspin
            locations.Add(a);
            ucVEarth.VE_AddPushPin(a);
            ucVEarth.VE_SetCenter(a);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        public List<String> SelectFilesOrFolder()
        {
            if (this.fileRadioButton.Checked)
            {
                this.filePicker.Filter = "JPEG Images|*.jpg";

                if (filePicker.ShowDialog() == DialogResult.OK)
                {
                    return new List<String>(filePicker.FileNames);
                }
            }

            if (this.folderRadioButton.Checked)
            {
                FolderBrowserDialog folderpicker = new FolderBrowserDialog();
                if (folderpicker.ShowDialog() == DialogResult.OK)
                {
                    return new List<String>() { folderpicker.SelectedPath };
                }
            }

            return null;
        }

        BackgroundWorker pictureloader;
        private bool cancelprocess;

        private void selectButton_Click(object sender, EventArgs e)
        {
            // If pictures are already being loaded we act as a cancel button.
            if (this.pictureloader != null && this.pictureloader.IsBusy)
            {
                this.cancelprocess = true;
                return;
            }

            List<String> selectedfiles = this.SelectFilesOrFolder();

            // If no files where selected just return.
            if (selectedfiles == null)
                return;

            //Check if the first path is a directory or a full file path.
            bool isDirectory = (File.GetAttributes(selectedfiles[0]) & FileAttributes.Directory) == FileAttributes.Directory;

            if (isDirectory)
            {

                filestoProcess = new List<String>(Directory.GetFiles(selectedfiles[0], "*.jpg"));
            }
            else
            {
                filestoProcess = selectedfiles;
            }

            this.ucVEarth.VE_AddShapeLayer("Photos", "Photos");
            pictureloader = new BackgroundWorker();
            pictureloader.WorkerSupportsCancellation = true;
            pictureloader.WorkerReportsProgress = true;

            pictureloader.DoWork += new DoWorkEventHandler(pictureloader_DoWork);
            pictureloader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(pictureloader_RunWorkerCompleted);
            pictureloader.ProgressChanged += ProgressChanged;

            pictureloader.RunWorkerAsync(filestoProcess);
        }

        void pictureloader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message + " : " + e.Error.StackTrace);
            }
            else
            {
                this.SetButtonBasedOnCurrentOperation();
                this.PrintStatus(String.Format("{0} pictures loaded and ready to process\n", this.filestoProcess.Count));
            }
        }

        void pictureloader_DoWork(object sender, DoWorkEventArgs e)
        {
            this.SetButtonBasedOnCurrentOperation();

            // If we don't need the list box or the map we can just return.
            if (Properties.Settings.Default.NoListBox && !Properties.Settings.Default.LoadMap)
                return;

            // If we need the map of or the list box we have to process them.
            if (!Properties.Settings.Default.NoListBox || Properties.Settings.Default.LoadMap)
            {
                foreach (var file in (List<String>)e.Argument)
                {

                    if (this.cancelprocess)
                    {
                        e.Cancel = true;
                        this.SetButtonBasedOnCurrentOperation();
                        return;
                    }

                    Picture pic = new Picture(file);

                    this.Invoke(new Action(() =>
                    {
                        if (!Properties.Settings.Default.NoListBox) this.fileListBox.Items.Add(pic, true);

                        if (this.MapIsLoaded)
                        {
                            if (pic.HasGPSInformation)
                            {
                                pictureloader.ReportProgress(0, "   Mapping picture: " + pic.Name);
                                this.PlacePictureOnMap(pic);
                            }
                            else
                            {
                                pictureloader.ReportProgress(0, "   Picture: " + pic.Name + " has no GPS information");
                            }
                        }
                    }));
                }
            }
        }

        private void selectoutfolderButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderpicker = new FolderBrowserDialog();
            if (folderpicker.ShowDialog() == DialogResult.OK)
            {
                Properties.Settings.Default.OutPath = folderpicker.SelectedPath;
            }
        }

        BackgroundWorker processworker;

        private void Process_Click(object sender, EventArgs e)
        {
            // If the check box is empty and there are no files to process, we don't need to process anything.
            if (this.fileListBox.CheckedItems.Count == 0 &&
                (this.filestoProcess == null || this.filestoProcess.Count == 0))
                return;

            // If the user hasn't selected anything to output throw up a error.
            if (!this.outMIF.Checked && !this.outTab.Checked)
            {
                MessageBox.Show("Please select a output format");
                return;
            }

            List<Picture> finalfilestoprocess;

            this.PrintStatus("Building list of photos to process\n");
            if (this.fileListBox.CheckedItems.Count > 0)
            {
                finalfilestoprocess = new List<Picture>();
                foreach (Picture item in this.fileListBox.CheckedItems)
                {
                    finalfilestoprocess.Add(item);
                }
            }
            else
            {
                finalfilestoprocess = new List<Picture>();
                foreach (string item in this.filestoProcess)
                {
                    finalfilestoprocess.Add(new Picture(item));
                }
            }
            this.PrintStatus("Building list of photos to process -> Complete\n");

            processworker = new BackgroundWorker();
            processworker.WorkerReportsProgress = true;

            processworker.DoWork += worker_DoWork;
            processworker.RunWorkerCompleted += worker_RunWorkerCompleted;
            processworker.ProgressChanged += ProgressChanged;

            processworker.RunWorkerAsync(finalfilestoprocess);
        }

        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Invoke(new Action(() =>
                {
                    PrintStatus((string)e.UserState + "\n");
                }));
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
                MessageBox.Show(e.Error.Message + " : " + e.Error.StackTrace);
            else
                this.PrintStatus("Complete!\n");
        }

        public void PrintStatus(string msg)
        {
            if (this.richTextBox1.InvokeRequired)
            {
                this.richTextBox1.Invoke(new Action(() =>
                    {
                        this.PrintStatus(msg);
                        return;
                    }));
            }

            this.richTextBox1.Text += msg;
            this.richTextBox1.SelectionLength = 0;
            this.richTextBox1.SelectionStart = this.richTextBox1.Text.Length;
            this.richTextBox1.ScrollToCaret();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            processworker.ReportProgress(0,"Creating files....");

            List<Picture> pictures = (List<Picture>)e.Argument;

            ImageProcessor process = new ImageProcessor();
            process.ProgessUpdated += new Action<string>(process_ProgessUpdated);

            string path = this.outPathText.Text;
            string name = this.outFileName.Text;

            ImageProcessor.FormatFlags flag = ImageProcessor.FormatFlags.None;

            if (this.outTab.Checked && this.outMIF.Checked)
                flag = ImageProcessor.FormatFlags.MIF | ImageProcessor.FormatFlags.TAB;
            else if (!this.outTab.Checked && this.outMIF.Checked)
                flag = ImageProcessor.FormatFlags.MIF;
            else if (this.outTab.Checked && !this.outMIF.Checked)
                flag = ImageProcessor.FormatFlags.TAB;


            process.ProcessPictures(path,name,pictures,flag);

            // Clear the list of files.
            this.Invoke(new Action(this.fileListBox.Items.Clear));
        }

        void process_ProgessUpdated(string msg)
        {
            if (this.processworker != null)
            {
                this.processworker.ReportProgress(10, msg);
            }
        }

        public void SetButtonBasedOnCurrentOperation()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                    {
                        this.SetButtonBasedOnCurrentOperation();
                    }));
            }

            if (this.pictureloader != null && this.pictureloader.IsBusy)
            {
                this.selectButton.Text = "Cancel...";
                return;
            }

            if (this.folderRadioButton.Checked)
            {
                this.selectButton.Text = "Select Folder";
                return;
            }

            if (this.fileRadioButton.Checked)
            {
                this.selectButton.Text = "Select Files";
                return;
            }
        }

        private void folderRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            this.SetButtonBasedOnCurrentOperation();
        }

        private void PhotoMapper_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog(this);
            hlpevent.Handled = true;
        }
    }
}
