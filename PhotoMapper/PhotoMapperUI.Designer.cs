using PhotoMapper.Core;
namespace PhotoMapper
{
    partial class PhotoMapperUI
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.selectButton = new System.Windows.Forms.Button();
            this.processButton = new System.Windows.Forms.Button();
            this.outPathText = new System.Windows.Forms.TextBox();
            this.settingsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.selectoutfolderButton = new System.Windows.Forms.Button();
            this.outTab = new System.Windows.Forms.CheckBox();
            this.outMIF = new System.Windows.Forms.CheckBox();
            this.outFileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.filePicker = new System.Windows.Forms.OpenFileDialog();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.fileListBox = new System.Windows.Forms.CheckedListBox();
            this.ucVEarth = new VEarth.ucVEarth();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.fileRadioButton = new System.Windows.Forms.RadioButton();
            this.folderRadioButton = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.outputTab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sameAsInputCheck = new System.Windows.Forms.CheckBox();
            this.settingsTab = new System.Windows.Forms.TabPage();
            this.noListBoxCheckBox = new System.Windows.Forms.CheckBox();
            this.loadMapCheckBox = new System.Windows.Forms.CheckBox();
            this.aboutTabPage = new System.Windows.Forms.TabPage();
            this.aboutBox = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.outputTab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.settingsTab.SuspendLayout();
            this.aboutTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(12, 12);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(101, 26);
            this.selectButton.TabIndex = 0;
            this.selectButton.Text = "Select Folder";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.SelectButtonClick);
            // 
            // processButton
            // 
            this.processButton.Location = new System.Drawing.Point(296, 114);
            this.processButton.Name = "processButton";
            this.processButton.Size = new System.Drawing.Size(83, 35);
            this.processButton.TabIndex = 1;
            this.processButton.Text = "Process";
            this.processButton.UseVisualStyleBackColor = true;
            this.processButton.Click += new System.EventHandler(this.Process_Click);
            // 
            // outPathText
            // 
            this.outPathText.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsBindingSource, "OutPath", true));
            this.outPathText.Location = new System.Drawing.Point(6, 23);
            this.outPathText.Name = "outPathText";
            this.outPathText.Size = new System.Drawing.Size(338, 20);
            this.outPathText.TabIndex = 2;
            // 
            // settingsBindingSource
            // 
            this.settingsBindingSource.DataSource = typeof(System.Configuration.ApplicationSettingsBase);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Path";
            // 
            // selectoutfolderButton
            // 
            this.selectoutfolderButton.Location = new System.Drawing.Point(350, 21);
            this.selectoutfolderButton.Name = "selectoutfolderButton";
            this.selectoutfolderButton.Size = new System.Drawing.Size(29, 23);
            this.selectoutfolderButton.TabIndex = 4;
            this.selectoutfolderButton.Text = "...";
            this.selectoutfolderButton.UseVisualStyleBackColor = true;
            this.selectoutfolderButton.Click += new System.EventHandler(this.SelectoutfolderButtonClick);
            // 
            // outTab
            // 
            this.outTab.AutoSize = true;
            this.outTab.Checked = true;
            this.outTab.CheckState = System.Windows.Forms.CheckState.Checked;
            this.outTab.Location = new System.Drawing.Point(11, 42);
            this.outTab.Name = "outTab";
            this.outTab.Size = new System.Drawing.Size(182, 17);
            this.outTab.TabIndex = 10;
            this.outTab.Text = "Tab file - needs MapInfo installed";
            this.outTab.UseVisualStyleBackColor = true;
            // 
            // outMIF
            // 
            this.outMIF.AutoSize = true;
            this.outMIF.Checked = true;
            this.outMIF.CheckState = System.Windows.Forms.CheckState.Checked;
            this.outMIF.Location = new System.Drawing.Point(11, 19);
            this.outMIF.Name = "outMIF";
            this.outMIF.Size = new System.Drawing.Size(60, 17);
            this.outMIF.TabIndex = 9;
            this.outMIF.Text = "MIF file";
            this.outMIF.UseVisualStyleBackColor = true;
            // 
            // outFileName
            // 
            this.outFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.settingsBindingSource, "OutName", true));
            this.outFileName.Location = new System.Drawing.Point(6, 62);
            this.outFileName.Name = "outFileName";
            this.outFileName.Size = new System.Drawing.Size(127, 20);
            this.outFileName.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 46);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "File Name";
            // 
            // filePicker
            // 
            this.filePicker.FileName = "openFileDialog1";
            this.filePicker.Multiselect = true;
            this.filePicker.SupportMultiDottedExtensions = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(3, 153);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(376, 171);
            this.richTextBox1.TabIndex = 7;
            this.richTextBox1.Text = "";
            // 
            // fileListBox
            // 
            this.fileListBox.FormattingEnabled = true;
            this.fileListBox.HorizontalScrollbar = true;
            this.fileListBox.Location = new System.Drawing.Point(12, 49);
            this.fileListBox.Name = "fileListBox";
            this.fileListBox.Size = new System.Drawing.Size(341, 319);
            this.fileListBox.TabIndex = 9;
            // 
            // ucVEarth
            // 
            this.ucVEarth.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ucVEarth.AutoSize = true;
            this.ucVEarth.DashboardStyle = VEarth.ucVEarth.DashboardStyleEnum.Tiny;
            this.ucVEarth.DisambiguationMode = VEarth.ucVEarth.DisambiguationEnum.Ignore;
            this.ucVEarth.Location = new System.Drawing.Point(12, 374);
            this.ucVEarth.MapLocation = "The Netherlands";
            this.ucVEarth.MapStyle = VEarth.ucVEarth.MapStyleEnum.Aerial;
            this.ucVEarth.Name = "ucVEarth";
            this.ucVEarth.Size = new System.Drawing.Size(737, 421);
            this.ucVEarth.TabIndex = 0;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // fileRadioButton
            // 
            this.fileRadioButton.AutoSize = true;
            this.fileRadioButton.Location = new System.Drawing.Point(119, 3);
            this.fileRadioButton.Name = "fileRadioButton";
            this.fileRadioButton.Size = new System.Drawing.Size(46, 17);
            this.fileRadioButton.TabIndex = 10;
            this.fileRadioButton.Text = "Files";
            this.fileRadioButton.UseVisualStyleBackColor = true;
            this.fileRadioButton.CheckedChanged += new System.EventHandler(this.FolderRadioButtonCheckedChanged);
            // 
            // folderRadioButton
            // 
            this.folderRadioButton.AutoSize = true;
            this.folderRadioButton.Checked = true;
            this.folderRadioButton.Location = new System.Drawing.Point(119, 25);
            this.folderRadioButton.Name = "folderRadioButton";
            this.folderRadioButton.Size = new System.Drawing.Size(85, 17);
            this.folderRadioButton.TabIndex = 10;
            this.folderRadioButton.TabStop = true;
            this.folderRadioButton.Text = "Whole folder";
            this.folderRadioButton.UseVisualStyleBackColor = true;
            this.folderRadioButton.CheckedChanged += new System.EventHandler(this.FolderRadioButtonCheckedChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.outputTab);
            this.tabControl1.Controls.Add(this.settingsTab);
            this.tabControl1.Controls.Add(this.aboutTabPage);
            this.tabControl1.Location = new System.Drawing.Point(358, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(396, 356);
            this.tabControl1.TabIndex = 11;
            // 
            // outputTab
            // 
            this.outputTab.AutoScroll = true;
            this.outputTab.Controls.Add(this.groupBox1);
            this.outputTab.Controls.Add(this.sameAsInputCheck);
            this.outputTab.Controls.Add(this.outPathText);
            this.outputTab.Controls.Add(this.richTextBox1);
            this.outputTab.Controls.Add(this.label1);
            this.outputTab.Controls.Add(this.outFileName);
            this.outputTab.Controls.Add(this.selectoutfolderButton);
            this.outputTab.Controls.Add(this.label3);
            this.outputTab.Controls.Add(this.processButton);
            this.outputTab.Location = new System.Drawing.Point(4, 22);
            this.outputTab.Name = "outputTab";
            this.outputTab.Padding = new System.Windows.Forms.Padding(3);
            this.outputTab.Size = new System.Drawing.Size(388, 330);
            this.outputTab.TabIndex = 0;
            this.outputTab.Text = "Output";
            this.outputTab.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.outMIF);
            this.groupBox1.Controls.Add(this.outTab);
            this.groupBox1.Location = new System.Drawing.Point(5, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 61);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Format";
            // 
            // sameAsInputCheck
            // 
            this.sameAsInputCheck.AutoSize = true;
            this.sameAsInputCheck.Location = new System.Drawing.Point(271, 3);
            this.sameAsInputCheck.Name = "sameAsInputCheck";
            this.sameAsInputCheck.Size = new System.Drawing.Size(108, 17);
            this.sameAsInputCheck.TabIndex = 11;
            this.sameAsInputCheck.Text = "Sync to Inport Dir";
            this.sameAsInputCheck.UseVisualStyleBackColor = true;
            this.sameAsInputCheck.CheckedChanged += new System.EventHandler(this.SameAsInputCheckCheckedChanged);
            // 
            // settingsTab
            // 
            this.settingsTab.Controls.Add(this.noListBoxCheckBox);
            this.settingsTab.Controls.Add(this.loadMapCheckBox);
            this.settingsTab.Location = new System.Drawing.Point(4, 22);
            this.settingsTab.Name = "settingsTab";
            this.settingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingsTab.Size = new System.Drawing.Size(388, 330);
            this.settingsTab.TabIndex = 1;
            this.settingsTab.Text = "Settings";
            this.settingsTab.UseVisualStyleBackColor = true;
            // 
            // noListBoxCheckBox
            // 
            this.noListBoxCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.settingsBindingSource, "NoListBox", true));
            this.noListBoxCheckBox.Location = new System.Drawing.Point(16, 15);
            this.noListBoxCheckBox.Name = "noListBoxCheckBox";
            this.noListBoxCheckBox.Size = new System.Drawing.Size(340, 24);
            this.noListBoxCheckBox.TabIndex = 4;
            this.noListBoxCheckBox.Text = "Don\'t load list box (improves performance - better for large folders)";
            this.noListBoxCheckBox.UseVisualStyleBackColor = true;
            // 
            // loadMapCheckBox
            // 
            this.loadMapCheckBox.DataBindings.Add(new System.Windows.Forms.Binding("CheckState", this.settingsBindingSource, "LoadMap", true));
            this.loadMapCheckBox.Location = new System.Drawing.Point(16, 38);
            this.loadMapCheckBox.Name = "loadMapCheckBox";
            this.loadMapCheckBox.Size = new System.Drawing.Size(169, 24);
            this.loadMapCheckBox.TabIndex = 3;
            this.loadMapCheckBox.Text = "Show map (needs app restart)";
            this.loadMapCheckBox.UseVisualStyleBackColor = true;
            // 
            // aboutTabPage
            // 
            this.aboutTabPage.Controls.Add(this.aboutBox);
            this.aboutTabPage.Location = new System.Drawing.Point(4, 22);
            this.aboutTabPage.Name = "aboutTabPage";
            this.aboutTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.aboutTabPage.Size = new System.Drawing.Size(388, 330);
            this.aboutTabPage.TabIndex = 2;
            this.aboutTabPage.Text = "About";
            this.aboutTabPage.UseVisualStyleBackColor = true;
            // 
            // aboutBox
            // 
            this.aboutBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.aboutBox.Location = new System.Drawing.Point(3, 3);
            this.aboutBox.Name = "aboutBox";
            this.aboutBox.ReadOnly = true;
            this.aboutBox.Size = new System.Drawing.Size(382, 324);
            this.aboutBox.TabIndex = 8;
            this.aboutBox.Text = "";
            // 
            // PhotoMapperUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(766, 800);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.folderRadioButton);
            this.Controls.Add(this.fileRadioButton);
            this.Controls.Add(this.fileListBox);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.ucVEarth);
            this.HelpButton = true;
            this.KeyPreview = true;
            this.MaximumSize = new System.Drawing.Size(774, 2000);
            this.MinimumSize = new System.Drawing.Size(774, 413);
            this.Name = "PhotoMapperUI";
            this.Text = "Form1";
            this.HelpRequested += new System.Windows.Forms.HelpEventHandler(this.PhotoMapper_HelpRequested);
            ((System.ComponentModel.ISupportInitialize)(this.settingsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.outputTab.ResumeLayout(false);
            this.outputTab.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.settingsTab.ResumeLayout(false);
            this.aboutTabPage.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private VEarth.ucVEarth ucVEarth;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button processButton;
        private System.Windows.Forms.TextBox outPathText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button selectoutfolderButton;
        private System.Windows.Forms.OpenFileDialog filePicker;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TextBox outFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox outTab;
        private System.Windows.Forms.CheckBox outMIF;
        private System.Windows.Forms.CheckedListBox fileListBox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.RadioButton folderRadioButton;
        private System.Windows.Forms.RadioButton fileRadioButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage outputTab;
        private System.Windows.Forms.TabPage settingsTab;
        private System.Windows.Forms.CheckBox loadMapCheckBox;
        private System.Windows.Forms.CheckBox noListBoxCheckBox;
        private System.Windows.Forms.BindingSource settingsBindingSource;
        private System.Windows.Forms.CheckBox sameAsInputCheck;
        private System.Windows.Forms.TabPage aboutTabPage;
        private System.Windows.Forms.RichTextBox aboutBox;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

