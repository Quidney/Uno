namespace Uno
{
    partial class ChatBox
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
            this.txtChatBox = new Uno.Classes.CustomRichTextBox();
            this.txtMessageBox = new Uno.Classes.CustomTextBox();
            this.btnSendMessage = new Uno.Classes.CustomButton();
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
            this.pnlChatBox.Controls.Add(this.txtChatBox, 0, 1);
            this.pnlChatBox.Controls.Add(this.txtMessageBox, 0, 2);
            this.pnlChatBox.Controls.Add(this.btnSendMessage, 2, 2);
            this.pnlChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChatBox.Location = new System.Drawing.Point(0, 0);
            this.pnlChatBox.Name = "pnlChatBox";
            this.pnlChatBox.RowCount = 3;
            this.pnlChatBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlChatBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.pnlChatBox.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.pnlChatBox.Size = new System.Drawing.Size(794, 750);
            this.pnlChatBox.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AssignedCard = null;
            this.lblTitle.AutoSize = true;
            this.pnlChatBox.SetColumnSpan(this.lblTitle, 2);
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(3, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(668, 35);
            this.lblTitle.TabIndex = 22;
            this.lblTitle.Text = "ChatBox";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // btnMinimize
            // 
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.Location = new System.Drawing.Point(677, 3);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(33, 29);
            this.btnMinimize.TabIndex = 21;
            this.btnMinimize.Text = "🗕";
            this.btnMinimize.UseVisualStyleBackColor = true;
            // 
            // btnMaximize
            // 
            this.btnMaximize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.Location = new System.Drawing.Point(716, 3);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(33, 29);
            this.btnMaximize.TabIndex = 20;
            this.btnMaximize.Text = "🗖";
            this.btnMaximize.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(755, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(36, 29);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "✖";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtChatBox
            // 
            this.txtChatBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.txtChatBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pnlChatBox.SetColumnSpan(this.txtChatBox, 5);
            this.txtChatBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtChatBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChatBox.Location = new System.Drawing.Point(2, 37);
            this.txtChatBox.Margin = new System.Windows.Forms.Padding(2);
            this.txtChatBox.Name = "txtChatBox";
            this.txtChatBox.ReadOnly = true;
            this.txtChatBox.Size = new System.Drawing.Size(790, 676);
            this.txtChatBox.TabIndex = 18;
            this.txtChatBox.Text = "";
            // 
            // txtMessageBox
            // 
            this.txtMessageBox.AccessibleRole = System.Windows.Forms.AccessibleRole.Text;
            this.pnlChatBox.SetColumnSpan(this.txtMessageBox, 2);
            this.txtMessageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessageBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageBox.Location = new System.Drawing.Point(2, 717);
            this.txtMessageBox.Margin = new System.Windows.Forms.Padding(2);
            this.txtMessageBox.Name = "txtMessageBox";
            this.txtMessageBox.Size = new System.Drawing.Size(670, 29);
            this.txtMessageBox.TabIndex = 17;
            // 
            // btnSendMessage
            // 
            this.pnlChatBox.SetColumnSpan(this.btnSendMessage, 3);
            this.btnSendMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSendMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSendMessage.Location = new System.Drawing.Point(676, 717);
            this.btnSendMessage.Margin = new System.Windows.Forms.Padding(2);
            this.btnSendMessage.Name = "btnSendMessage";
            this.btnSendMessage.Size = new System.Drawing.Size(116, 31);
            this.btnSendMessage.TabIndex = 15;
            this.btnSendMessage.Text = "Send Message";
            this.btnSendMessage.UseVisualStyleBackColor = true;
            this.btnSendMessage.Click += new System.EventHandler(this.btnSendDataToServer_Click);
            // 
            // ChatBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 750);
            this.Controls.Add(this.pnlChatBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "ChatBox";
            this.Text = "ChatBox";
            this.pnlChatBox.ResumeLayout(false);
            this.pnlChatBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Classes.CustomTableLayoutPanel pnlChatBox;
        private Classes.CustomTextBox txtMessageBox;
        private Classes.CustomButton btnSendMessage;
        private Classes.CustomRichTextBox txtChatBox;
        private Classes.CustomButton btnClose;
        private Classes.CustomButton btnMinimize;
        private Classes.CustomButton btnMaximize;
        private Classes.CustomLabel lblTitle;
    }
}