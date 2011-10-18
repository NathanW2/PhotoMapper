using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using log4net;
using PhotoMapper.Core;
using VEarth.Locations;
using System.Reflection;

namespace PhotoMapper
{
    public partial class PhotoMapperUI : Form
    {
        /// <summary>
        /// Get the logger for this class.
        /// </summary>
        private static readonly ILog log = Logging.GetLog(typeof(PhotoMapperUI));

        public delegate void Action();
        BackgroundWorker maploader = new BackgroundWorker();
        private bool MapIsLoaded;
        BackgroundWorker processworker;

        List<String> filestoProcess;
        // Create a list of locations to search for
        List<SearchLocation> locations = new List<SearchLocation>();

        public PhotoMapperUI()
        {
            InitializeComponent();

            this.Text = "Photo Mapper : Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.settingsBindingSource.DataSource = Properties.Settings.Default;

            this.Shown += Form1_Shown;
            this.FormClosing += Form1_FormClosing;
            this.fileListBox.DisplayMember = "Name";
            this.fileListBox.MultiColumn = true;

            if (!Properties.Settings.Default.LoadMap)
                this.Height = this.MinimumSize.Height;

            this.aboutBox.Text = About.AboutString;
        }

        /// <summary>
        /// The currently selected input path.
        /// </summary>
        private string SelectedImportPath { get; set; }

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
            SearchLocation a = new SearchLocation
                                   {
                                       Longitude = picture.GPSLongitude,
                                       Latitude = picture.GPSLatitude,
                                       PushPinDescription =
                                           String.Format("<IMG SRC=\"{0}\" ALT=\"{0}\" WIDTH=200 HEIGHT=200>",
                                                         picture.FileName),
                                       PushPinLayer = "Photos",
                                       PushPinTitle = picture.Name
                                   };
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
                this.filePicker.InitialDirectory = this.SelectedImportPath;
                this.filePicker.Filter = "JPEG Images|*.jpg";

                if (filePicker.ShowDialog() == DialogResult.OK)
                {
                    this.SelectedImportPath = Directory.GetParent(filePicker.FileNames[0]).FullName;

                    if (this.sameAsInputCheck.Checked)
                        this.outPathText.Text = this.SelectedImportPath;

                    return new List<String>(filePicker.FileNames);
                }
            }

            if (this.folderRadioButton.Checked)
            {
                FolderBrowserDialog folderpicker = new FolderBrowserDialog {SelectedPath = this.SelectedImportPath};
                if (folderpicker.ShowDialog() == DialogResult.OK)
                {
                    this.SelectedImportPath = folderpicker.SelectedPath;

                    if (this.sameAsInputCheck.Checked)
                        this.outPathText.Text = this.SelectedImportPath;

                    return new List<String>() { folderpicker.SelectedPath };
                }
            }

            return null;
        }

        BackgroundWorker pictureloader;
        private bool cancelprocess;

        private void SelectButtonClick(object sender, EventArgs e)
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
            pictureloader = new BackgroundWorker
                                {
                                    WorkerSupportsCancellation = true, 
                                    WorkerReportsProgress = true
                                };

            pictureloader.DoWork += PictureloaderDoWork;
            pictureloader.RunWorkerCompleted += PictureloaderRunWorkerCompleted;
            pictureloader.ProgressChanged += ProgressChanged;

            pictureloader.RunWorkerAsync(filestoProcess);
        }

        void PictureloaderRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        void PictureloaderDoWork(object sender, DoWorkEventArgs e)
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

        private void SelectoutfolderButtonClick(object sender, EventArgs e)
        {
            FolderBrowserDialog folderpicker = new FolderBrowserDialog {SelectedPath = SelectedImportPath};
            if (folderpicker.ShowDialog() == DialogResult.OK)
            {
                this.SelectedImportPath = folderpicker.SelectedPath;
                Properties.Settings.Default.OutPath = folderpicker.SelectedPath;
            }
        }


        private void Process_Click(object sender, EventArgs e)
        {
            // If the check box is empty and there are no files to process, we don't need to process anything.
            if (this.fileListBox.CheckedItems.Count == 0 && (this.filestoProcess == null || this.filestoProcess.Count == 0))
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

            processworker = new BackgroundWorker {WorkerReportsProgress = true};

            processworker.DoWork += WorkerDoWork;
            processworker.RunWorkerCompleted += WorkerRunWorkerCompleted;
            processworker.ProgressChanged += ProgressChanged;

            processworker.RunWorkerAsync(finalfilestoprocess);
        }

        void ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Invoke(new Action(() => PrintStatus((string)e.UserState + "\n")));
        }

        void WorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        void WorkerDoWork(object sender, DoWorkEventArgs e)
        {
            processworker.ReportProgress(0,"Creating files....");

            List<Picture> pictures = (List<Picture>)e.Argument;

            ImageProcessor process = new ImageProcessor();
            process.ProgessUpdated += ProcessProgessUpdated;

            string path = this.outPathText.Text;
            string name = this.outFileName.Text;

            ImageProcessor.FileFormat flag = ImageProcessor.FileFormat.None;

            if (this.outTab.Checked && this.outMIF.Checked)
                flag = ImageProcessor.FileFormat.MIF | ImageProcessor.FileFormat.TAB;
            else if (!this.outTab.Checked && this.outMIF.Checked)
                flag = ImageProcessor.FileFormat.MIF;
            else if (this.outTab.Checked && !this.outMIF.Checked)
                flag = ImageProcessor.FileFormat.TAB;


            process.ProcessPictures(path,name,pictures,flag);

            // Clear the list of files.
            this.Invoke(new Action(this.fileListBox.Items.Clear));
        }

        void ProcessProgessUpdated(string msg)
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
                this.Invoke(new Action(this.SetButtonBasedOnCurrentOperation));
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

        private void FolderRadioButtonCheckedChanged(object sender, EventArgs e)
        {
            this.SetButtonBasedOnCurrentOperation();
        }

        private void PhotoMapper_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            AboutBox1 about = new AboutBox1();
            about.ShowDialog(this);
            hlpevent.Handled = true;
        }

        private void SameAsInputCheckCheckedChanged(object sender, EventArgs e)
        {
            if (this.sameAsInputCheck.Checked)
                this.outPathText.Text = this.SelectedImportPath;

            this.outPathText.ReadOnly = this.sameAsInputCheck.Checked;
        }
    }
}
