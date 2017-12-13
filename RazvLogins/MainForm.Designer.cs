namespace RazvLogins
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
            this.BTest = new System.Windows.Forms.Button();
            this.BEnd = new System.Windows.Forms.Button();
            this.TTestik = new System.Windows.Forms.TextBox();
            this.SmartMultiPage = new System.Windows.Forms.TabControl();
            this.CheckLoad = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // BTest
            // 
            this.BTest.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BTest.Location = new System.Drawing.Point(616, 517);
            this.BTest.Name = "BTest";
            this.BTest.Size = new System.Drawing.Size(210, 50);
            this.BTest.TabIndex = 0;
            this.BTest.Text = "Логин Пробник";
            this.BTest.UseVisualStyleBackColor = true;
            this.BTest.Visible = false;
            this.BTest.Click += new System.EventHandler(this.BTest_Click);
            // 
            // BEnd
            // 
            this.BEnd.BackColor = System.Drawing.Color.OrangeRed;
            this.BEnd.Font = new System.Drawing.Font("Comic Sans MS", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BEnd.Location = new System.Drawing.Point(386, 517);
            this.BEnd.Name = "BEnd";
            this.BEnd.Size = new System.Drawing.Size(210, 50);
            this.BEnd.TabIndex = 1;
            this.BEnd.Text = "ЗАКРЫТЬ";
            this.BEnd.UseVisualStyleBackColor = false;
            // 
            // TTestik
            // 
            this.TTestik.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.TTestik.Location = new System.Drawing.Point(21, 504);
            this.TTestik.Name = "TTestik";
            this.TTestik.Size = new System.Drawing.Size(332, 22);
            this.TTestik.TabIndex = 3;
            this.TTestik.Visible = false;
            // 
            // SmartMultiPage
            // 
            this.SmartMultiPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SmartMultiPage.Location = new System.Drawing.Point(21, 25);
            this.SmartMultiPage.Name = "SmartMultiPage";
            this.SmartMultiPage.SelectedIndex = 0;
            this.SmartMultiPage.Size = new System.Drawing.Size(1267, 471);
            this.SmartMultiPage.TabIndex = 4;
            // 
            // CheckLoad
            // 
            this.CheckLoad.AutoSize = true;
            this.CheckLoad.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CheckLoad.Location = new System.Drawing.Point(616, -1);
            this.CheckLoad.Name = "CheckLoad";
            this.CheckLoad.Size = new System.Drawing.Size(162, 22);
            this.CheckLoad.TabIndex = 0;
            this.CheckLoad.Text = "Сразу начать загрузку";
            this.CheckLoad.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1308, 588);
            this.Controls.Add(this.CheckLoad);
            this.Controls.Add(this.SmartMultiPage);
            this.Controls.Add(this.TTestik);
            this.Controls.Add(this.BEnd);
            this.Controls.Add(this.BTest);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Окошечко";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BTest;
        private System.Windows.Forms.Button BEnd;
        private System.Windows.Forms.TextBox TTestik;
        private System.Windows.Forms.TabControl SmartMultiPage;
        private System.Windows.Forms.CheckBox CheckLoad;
    }
}

