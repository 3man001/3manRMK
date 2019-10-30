namespace _3manRMK_0
{
    partial class ErrorsForm
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
            this.LTextError = new System.Windows.Forms.Label();
            this.LSD = new System.Windows.Forms.Label();
            this.LMode = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.LAdvancedmode = new System.Windows.Forms.Label();
            this.LAllCode = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LTextError
            // 
            this.LTextError.AutoSize = true;
            this.LTextError.Location = new System.Drawing.Point(12, 9);
            this.LTextError.Name = "LTextError";
            this.LTextError.Size = new System.Drawing.Size(59, 13);
            this.LTextError.TabIndex = 0;
            this.LTextError.Text = "Результат";
            // 
            // LSD
            // 
            this.LSD.AutoSize = true;
            this.LSD.Location = new System.Drawing.Point(12, 111);
            this.LSD.Name = "LSD";
            this.LSD.Size = new System.Drawing.Size(28, 13);
            this.LSD.TabIndex = 1;
            this.LSD.Text = "LSD";
            // 
            // LMode
            // 
            this.LMode.AutoSize = true;
            this.LMode.Location = new System.Drawing.Point(12, 32);
            this.LMode.Name = "LMode";
            this.LMode.Size = new System.Drawing.Size(42, 13);
            this.LMode.TabIndex = 2;
            this.LMode.Text = "Режим";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(78, 51);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LAdvancedmode
            // 
            this.LAdvancedmode.AutoSize = true;
            this.LAdvancedmode.Location = new System.Drawing.Point(12, 57);
            this.LAdvancedmode.Name = "LAdvancedmode";
            this.LAdvancedmode.Size = new System.Drawing.Size(108, 13);
            this.LAdvancedmode.TabIndex = 4;
            this.LAdvancedmode.Text = "Расширеннй Режим";
            // 
            // LAllCode
            // 
            this.LAllCode.AutoSize = true;
            this.LAllCode.Location = new System.Drawing.Point(12, 83);
            this.LAllCode.Name = "LAllCode";
            this.LAllCode.Size = new System.Drawing.Size(78, 13);
            this.LAllCode.TabIndex = 5;
            this.LAllCode.Text = "Список Кодов";
            // 
            // ErrorsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(430, 261);
            this.Controls.Add(this.LAllCode);
            this.Controls.Add(this.LAdvancedmode);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LMode);
            this.Controls.Add(this.LSD);
            this.Controls.Add(this.LTextError);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorsForm";
            this.Text = "Ошибка :(";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LTextError;
        private System.Windows.Forms.Label LSD;
        private System.Windows.Forms.Label LMode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label LAdvancedmode;
        private System.Windows.Forms.Label LAllCode;
    }
}