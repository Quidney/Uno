namespace Uno
{
    partial class AdminConsole
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
            this.pnlChatBox = new Uno.Classes.CustomTableLayoutPanel();
            this.lblTitle = new Uno.Classes.CustomLabel();
            this.btnMinimize = new Uno.Classes.CustomButton();
            this.btnMaximize = new Uno.Classes.CustomButton();
            this.btnClose = new Uno.Classes.CustomButton();
            this.txtCommandLog = new Uno.Classes.CustomRichTextBox();
            this.txtCommandInput = new Uno.Classes.CustomTextBox();
            this.btnExecuteCommand = new Uno.Classes.CustomButton();
            this.cmbPlayerSelection = new System.Windows.Forms.ComboBox();
            this.pnlChatBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlChatBox
            // 
            this.pnlChatBox.ColumnCount = 5;
            this.pnlChatBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 75F));
            this.pnlChatBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.pnlChatBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.pnlChatBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.pnlChatBox.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
            this.pnlChatBox.Controls.Add(this.lblTitle, 0, 0);
            this.pnlChatBox.Controls.Add(this.btnMinimize, 2, 0);
            this.pnlChatBox.Controls.Add(this.btnMaximize, 3, 0);
            this.pnlChatBox.Controls.Add(this.btnClose, 4, 0);
            this.pnlChatBox.Controls.Add(this.txtCommandLog, 0, 1);
            this.pnlChatBox.Controls.Add(this.txtCommandInput, 0, 2);
            this.pnlChatBox.Controls.Add(this.btnExecuteCommand, 2, 2);
            this.pnlChatBox.Controls.Add(this.cmbPlayerSelection, 1, 2);
            this.pnlChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChatBox.Location = new System.Drawing.Point(0, 0);
            this.pnlChatBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlChatBox.Name = "pnlChatBox";
            this.pnlChatBox.RowCount = 3;
            this.pnlChatBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.pnlChatBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlChatBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 43F));
            this.pnlChatBox.Size = new System.Drawing.Size(1059, 923);
            this.pnlChatBox.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AssignedCard = null;
            this.lblTitle.AutoSize = true;
            this.pnlChatBox.SetColumnSpan(this.lblTitle, 2);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(4, 0);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(891, 43);
            this.lblTitle.TabIndex = 22;
            this.lblTitle.Text = "Admin Console - ";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(903, 4);
            this.btnMinimize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(44, 35);
            this.btnMinimize.TabIndex = 21;
            this.btnMinimize.Text = "🗕";
            this.btnMinimize.UseVisualStyleBackColor = true;
            // 
            // btnMaximize
            // 
            this.btnMaximize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Location = new System.Drawing.Point(955, 4);
            this.btnMaximize.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(44, 35);
            this.btnMaximize.TabIndex = 20;
            this.btnMaximize.Text = "🗖";
            this.btnMaximize.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(1007, 4);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(48, 35);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "✖";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtCommandLog
            // 
            this.txtCommandLog.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtCommandLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.pnlChatBox.SetColumnSpan(this.txtCommandLog, 5);
            this.txtCommandLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommandLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommandLog.Location = new System.Drawing.Point(3, 45);
            this.txtCommandLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCommandLog.Name = "txtCommandLog";
            this.txtCommandLog.ReadOnly = true;
            this.txtCommandLog.Size = new System.Drawing.Size(1053, 833);
            this.txtCommandLog.TabIndex = 18;
            this.txtCommandLog.Text = "DO NOT EXECUTE ANY COMMANDS IF YOU DON\'T KNOW WHAT YOU\'RE DOING!!!!\nTHIS IS FOR D" +
    "EBUG PURPOSES ONLY!\n\nDEVELOPERS ARE NOT RESPONSIBLE FOR CRASHES OR LOST GAME DAT" +
    "A\nProceed with caution:\n";
            // 
            // txtCommandInput
            // 
            this.txtCommandInput.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtCommandInput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCommandInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCommandInput.Location = new System.Drawing.Point(3, 882);
            this.txtCommandInput.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCommandInput.Name = "txtCommandInput";
            this.txtCommandInput.Size = new System.Drawing.Size(788, 34);
            this.txtCommandInput.TabIndex = 17;
            // 
            // btnExecuteCommand
            // 
            this.pnlChatBox.SetColumnSpan(this.btnExecuteCommand, 3);
            this.btnExecuteCommand.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExecuteCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExecuteCommand.Location = new System.Drawing.Point(902, 882);
            this.btnExecuteCommand.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnExecuteCommand.Name = "btnExecuteCommand";
            this.btnExecuteCommand.Size = new System.Drawing.Size(154, 39);
            this.btnExecuteCommand.TabIndex = 15;
            this.btnExecuteCommand.Text = "Send Message";
            this.btnExecuteCommand.UseVisualStyleBackColor = true;
            this.btnExecuteCommand.Click += new System.EventHandler(this.btnSendDataToServer_Click);
            // 
            // cmbPlayerSelection
            // 
            this.cmbPlayerSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbPlayerSelection.FormattingEnabled = true;
            this.cmbPlayerSelection.Location = new System.Drawing.Point(797, 883);
            this.cmbPlayerSelection.Name = "cmbPlayerSelection";
            this.cmbPlayerSelection.Size = new System.Drawing.Size(99, 24);
            this.cmbPlayerSelection.TabIndex = 23;
            this.cmbPlayerSelection.DropDown += new System.EventHandler(this.cmbPlayerSelection_DropDown);
            // 
            // AdminConsole
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 923);
            this.Controls.Add(this.pnlChatBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AdminConsole";
            this.Text = "ChatBox";
            this.pnlChatBox.ResumeLayout(false);
            this.pnlChatBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Classes.CustomTableLayoutPanel pnlChatBox;
        private Classes.CustomTextBox txtCommandInput;
        private Classes.CustomButton btnExecuteCommand;
        private Classes.CustomRichTextBox txtCommandLog;
        private Classes.CustomButton btnClose;
        private Classes.CustomButton btnMinimize;
        private Classes.CustomButton btnMaximize;
        private Classes.CustomLabel lblTitle;
        private System.Windows.Forms.ComboBox cmbPlayerSelection;
    }
}