namespace PAT.GUI
{
    partial class VerifyAllClusters
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ImbalanceGridView = new System.Windows.Forms.DataGridView();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.resultVerifyGridView = new System.Windows.Forms.DataGridView();
            this.VerifyAll_Btn = new System.Windows.Forms.Button();
            this.Close_Btn = new System.Windows.Forms.Button();
            this.Verify_Btn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.OutputTxtBox = new System.Windows.Forms.RichTextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.verifyStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.numericTimeout = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.STOP_Btn = new System.Windows.Forms.Button();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImbalanceGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultVerifyGridView)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ImbalanceGridView);
            this.groupBox1.Controls.Add(this.resultVerifyGridView);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(779, 232);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Result";
            // 
            // ImbalanceGridView
            // 
            this.ImbalanceGridView.AllowUserToAddRows = false;
            this.ImbalanceGridView.AllowUserToDeleteRows = false;
            this.ImbalanceGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImbalanceGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column4,
            this.Column5,
            this.Column6});
            this.ImbalanceGridView.Location = new System.Drawing.Point(389, 21);
            this.ImbalanceGridView.MultiSelect = false;
            this.ImbalanceGridView.Name = "ImbalanceGridView";
            this.ImbalanceGridView.ReadOnly = true;
            this.ImbalanceGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ImbalanceGridView.Size = new System.Drawing.Size(379, 200);
            this.ImbalanceGridView.TabIndex = 8;
            this.ImbalanceGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ImbalanceGridView_CellClick);
            // 
            // Column4
            // 
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column4.DefaultCellStyle = dataGridViewCellStyle1;
            this.Column4.HeaderText = "Imbalance Clusters";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.Width = 120;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Congestion";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 90;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Verification State";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 120;
            // 
            // resultVerifyGridView
            // 
            this.resultVerifyGridView.AllowUserToAddRows = false;
            this.resultVerifyGridView.AllowUserToDeleteRows = false;
            this.resultVerifyGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultVerifyGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.resultVerifyGridView.Location = new System.Drawing.Point(9, 21);
            this.resultVerifyGridView.MultiSelect = false;
            this.resultVerifyGridView.Name = "resultVerifyGridView";
            this.resultVerifyGridView.ReadOnly = true;
            this.resultVerifyGridView.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultVerifyGridView.Size = new System.Drawing.Size(359, 200);
            this.resultVerifyGridView.TabIndex = 8;
            this.resultVerifyGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultVerifyGridView_CellClick);
            // 
            // VerifyAll_Btn
            // 
            this.VerifyAll_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VerifyAll_Btn.Location = new System.Drawing.Point(482, 244);
            this.VerifyAll_Btn.Name = "VerifyAll_Btn";
            this.VerifyAll_Btn.Size = new System.Drawing.Size(75, 23);
            this.VerifyAll_Btn.TabIndex = 3;
            this.VerifyAll_Btn.Text = "Verify all";
            this.VerifyAll_Btn.UseVisualStyleBackColor = true;
            this.VerifyAll_Btn.Click += new System.EventHandler(this.VerifyAll_Btn_Click);
            // 
            // Close_Btn
            // 
            this.Close_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Close_Btn.Location = new System.Drawing.Point(698, 244);
            this.Close_Btn.Name = "Close_Btn";
            this.Close_Btn.Size = new System.Drawing.Size(75, 23);
            this.Close_Btn.TabIndex = 4;
            this.Close_Btn.Text = "Close";
            this.Close_Btn.UseVisualStyleBackColor = true;
            this.Close_Btn.Click += new System.EventHandler(this.Close_Btn_Click);
            // 
            // Verify_Btn
            // 
            this.Verify_Btn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Verify_Btn.Location = new System.Drawing.Point(590, 244);
            this.Verify_Btn.Name = "Verify_Btn";
            this.Verify_Btn.Size = new System.Drawing.Size(75, 23);
            this.Verify_Btn.TabIndex = 2;
            this.Verify_Btn.Text = "Verify";
            this.Verify_Btn.UseVisualStyleBackColor = true;
            this.Verify_Btn.Click += new System.EventHandler(this.Verify_Btn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OutputTxtBox);
            this.groupBox2.Location = new System.Drawing.Point(4, 273);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(773, 373);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output";
            // 
            // OutputTxtBox
            // 
            this.OutputTxtBox.Enabled = false;
            this.OutputTxtBox.Location = new System.Drawing.Point(8, 19);
            this.OutputTxtBox.Name = "OutputTxtBox";
            this.OutputTxtBox.ReadOnly = true;
            this.OutputTxtBox.Size = new System.Drawing.Size(759, 348);
            this.OutputTxtBox.TabIndex = 0;
            this.OutputTxtBox.Text = "";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel,
            this.verifyStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 659);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(39, 17);
            this.statusLabel.Text = "Ready";
            // 
            // verifyStatus
            // 
            this.verifyStatus.Margin = new System.Windows.Forms.Padding(600, 3, 0, 2);
            this.verifyStatus.Name = "verifyStatus";
            this.verifyStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // numericTimeout
            // 
            this.numericTimeout.Location = new System.Drawing.Point(279, 247);
            this.numericTimeout.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericTimeout.Name = "numericTimeout";
            this.numericTimeout.Size = new System.Drawing.Size(45, 20);
            this.numericTimeout.TabIndex = 7;
            this.numericTimeout.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(182, 249);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Time out (minute):";
            // 
            // STOP_Btn
            // 
            this.STOP_Btn.Enabled = false;
            this.STOP_Btn.Location = new System.Drawing.Point(353, 244);
            this.STOP_Btn.Name = "STOP_Btn";
            this.STOP_Btn.Size = new System.Drawing.Size(99, 23);
            this.STOP_Btn.TabIndex = 9;
            this.STOP_Btn.Text = "Stop Verify all";
            this.STOP_Btn.UseVisualStyleBackColor = true;
            this.STOP_Btn.Click += new System.EventHandler(this.STOP_Btn_Click);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "Density Clusters";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column1.Width = 110;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Congestion";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 90;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Verification State";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 114;
            // 
            // VerifyAllClusters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(784, 681);
            this.Controls.Add(this.STOP_Btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericTimeout);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.VerifyAll_Btn);
            this.Controls.Add(this.Verify_Btn);
            this.Controls.Add(this.Close_Btn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximumSize = new System.Drawing.Size(800, 720);
            this.MinimumSize = new System.Drawing.Size(800, 720);
            this.Name = "VerifyAllClusters";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Result verify all clusters";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImbalanceGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.resultVerifyGridView)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button Verify_Btn;
        private System.Windows.Forms.Button VerifyAll_Btn;
        private System.Windows.Forms.Button Close_Btn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView resultVerifyGridView;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.RichTextBox OutputTxtBox;
        private System.Windows.Forms.ToolStripStatusLabel verifyStatus;
        private System.Windows.Forms.NumericUpDown numericTimeout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button STOP_Btn;
        private System.Windows.Forms.DataGridView ImbalanceGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
    }
}