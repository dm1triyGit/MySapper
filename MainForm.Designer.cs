namespace Sapper
{
    partial class MainForm
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.PlMineField = new System.Windows.Forms.Panel();
            this.BtnCreateField = new System.Windows.Forms.Button();
            this.LblHeaderX = new System.Windows.Forms.Label();
            this.PBFlag = new System.Windows.Forms.PictureBox();
            this.LblNumFlags = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.PBFlag)).BeginInit();
            this.SuspendLayout();
            // 
            // PlMineField
            // 
            this.PlMineField.Location = new System.Drawing.Point(107, 43);
            this.PlMineField.Name = "PlMineField";
            this.PlMineField.Size = new System.Drawing.Size(401, 401);
            this.PlMineField.TabIndex = 0;
            this.PlMineField.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PlMineField_MouseClick);
            // 
            // BtnCreateField
            // 
            this.BtnCreateField.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BtnCreateField.Location = new System.Drawing.Point(280, 8);
            this.BtnCreateField.Name = "BtnCreateField";
            this.BtnCreateField.Size = new System.Drawing.Size(95, 29);
            this.BtnCreateField.TabIndex = 1;
            this.BtnCreateField.Text = "Создать";
            this.BtnCreateField.UseVisualStyleBackColor = true;
            this.BtnCreateField.Click += new System.EventHandler(this.BtnCreateField_Click);
            // 
            // LblHeaderX
            // 
            this.LblHeaderX.AutoSize = true;
            this.LblHeaderX.Font = new System.Drawing.Font("Arial Narrow", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblHeaderX.Location = new System.Drawing.Point(568, 57);
            this.LblHeaderX.Name = "LblHeaderX";
            this.LblHeaderX.Size = new System.Drawing.Size(29, 31);
            this.LblHeaderX.TabIndex = 3;
            this.LblHeaderX.Text = "X";
            this.LblHeaderX.Visible = false;
            // 
            // PBFlag
            // 
            this.PBFlag.Image = global::Sapper.Properties.Resources.Flag2;
            this.PBFlag.Location = new System.Drawing.Point(514, 43);
            this.PBFlag.Name = "PBFlag";
            this.PBFlag.Size = new System.Drawing.Size(59, 50);
            this.PBFlag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PBFlag.TabIndex = 2;
            this.PBFlag.TabStop = false;
            this.PBFlag.Visible = false;
            // 
            // LblNumFlags
            // 
            this.LblNumFlags.AutoSize = true;
            this.LblNumFlags.Font = new System.Drawing.Font("Arial Narrow", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LblNumFlags.Location = new System.Drawing.Point(590, 48);
            this.LblNumFlags.Name = "LblNumFlags";
            this.LblNumFlags.Size = new System.Drawing.Size(39, 43);
            this.LblNumFlags.TabIndex = 3;
            this.LblNumFlags.Text = "X";
            this.LblNumFlags.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(647, 470);
            this.Controls.Add(this.LblNumFlags);
            this.Controls.Add(this.LblHeaderX);
            this.Controls.Add(this.PBFlag);
            this.Controls.Add(this.BtnCreateField);
            this.Controls.Add(this.PlMineField);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sapper";
            ((System.ComponentModel.ISupportInitialize)(this.PBFlag)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel PlMineField;
        private System.Windows.Forms.Button BtnCreateField;
        private System.Windows.Forms.PictureBox PBFlag;
        private System.Windows.Forms.Label LblHeaderX;
        private System.Windows.Forms.Label LblNumFlags;
    }
}

