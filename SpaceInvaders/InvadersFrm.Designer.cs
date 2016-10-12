namespace SpaceInvaders
{
    partial class InvadersFrm
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
            this.animationTimer = new System.Windows.Forms.Timer(this.components);
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.NameInput = new System.Windows.Forms.TextBox();
            this.Name2Input = new System.Windows.Forms.TextBox();
            this.SaveScoreButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // animationTimer
            // 
            this.animationTimer.Enabled = true;
            this.animationTimer.Interval = 30;
            this.animationTimer.Tick += new System.EventHandler(this.animationTimer_Tick);
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 30;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // NameInput
            // 
            this.NameInput.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.NameInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.NameInput.Enabled = false;
            this.NameInput.Location = new System.Drawing.Point(273, 365);
            this.NameInput.MaxLength = 20;
            this.NameInput.Name = "NameInput";
            this.NameInput.Size = new System.Drawing.Size(225, 20);
            this.NameInput.TabIndex = 1;
            this.NameInput.Visible = false;
            // 
            // Name2Input
            // 
            this.Name2Input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Name2Input.Enabled = false;
            this.Name2Input.Location = new System.Drawing.Point(273, 394);
            this.Name2Input.MaxLength = 20;
            this.Name2Input.Name = "Name2Input";
            this.Name2Input.Size = new System.Drawing.Size(225, 20);
            this.Name2Input.TabIndex = 2;
            this.Name2Input.Visible = false;
            // 
            // SaveScoreButton
            // 
            this.SaveScoreButton.Enabled = false;
            this.SaveScoreButton.Location = new System.Drawing.Point(504, 365);
            this.SaveScoreButton.Name = "SaveScoreButton";
            this.SaveScoreButton.Size = new System.Drawing.Size(86, 49);
            this.SaveScoreButton.TabIndex = 3;
            this.SaveScoreButton.Text = "Record Score";
            this.SaveScoreButton.UseVisualStyleBackColor = true;
            this.SaveScoreButton.Visible = false;
            this.SaveScoreButton.Click += new System.EventHandler(this.SaveScoreButton_Click);
            // 
            // InvadersFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 602);
            this.Controls.Add(this.SaveScoreButton);
            this.Controls.Add(this.Name2Input);
            this.Controls.Add(this.NameInput);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(800, 640);
            this.Name = "InvadersFrm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Space Invaders";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.InvadersFrm_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InvadersFrm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.InvadersFrm_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer animationTimer;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.TextBox NameInput;
        private System.Windows.Forms.TextBox Name2Input;
        private System.Windows.Forms.Button SaveScoreButton;
    }
}

