﻿namespace WinFormsParser
{
    partial class Show_Button
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Show_Button));
            this.GB_Fields = new System.Windows.Forms.GroupBox();
            this.LB_FieldsList = new System.Windows.Forms.ListBox();
            this.GB_Table = new System.Windows.Forms.GroupBox();
            this.DataTable = new Parser.MyDataGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.GB_Fields.SuspendLayout();
            this.GB_Table.SuspendLayout();
            this.SuspendLayout();
            // 
            // GB_Fields
            // 
            this.GB_Fields.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.GB_Fields.Controls.Add(this.LB_FieldsList);
            this.GB_Fields.Location = new System.Drawing.Point(12, 12);
            this.GB_Fields.Name = "GB_Fields";
            this.GB_Fields.Size = new System.Drawing.Size(192, 379);
            this.GB_Fields.TabIndex = 0;
            this.GB_Fields.TabStop = false;
            this.GB_Fields.Text = "Список полей";
            // 
            // LB_FieldsList
            // 
            this.LB_FieldsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.LB_FieldsList.FormattingEnabled = true;
            this.LB_FieldsList.Location = new System.Drawing.Point(7, 20);
            this.LB_FieldsList.Name = "LB_FieldsList";
            this.LB_FieldsList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.LB_FieldsList.Size = new System.Drawing.Size(179, 342);
            this.LB_FieldsList.TabIndex = 0;
          
            this.LB_FieldsList.SelectedValueChanged += new System.EventHandler(this.LB_FieldsList_SelectedValueChanged);
            // 
            // GB_Table
            // 
            this.GB_Table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GB_Table.Controls.Add(this.DataTable);
            this.GB_Table.Location = new System.Drawing.Point(210, 12);
            this.GB_Table.Name = "GB_Table";
            this.GB_Table.Size = new System.Drawing.Size(559, 433);
            this.GB_Table.TabIndex = 2;
            this.GB_Table.TabStop = false;
            this.GB_Table.Text = "Таблица полей";
            // 
            // DataTable
            // 
            this.DataTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
        
            this.DataTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.DataTable.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DataTable.LineColor = System.Drawing.Color.Black;
            this.DataTable.Location = new System.Drawing.Point(13, 20);
            this.DataTable.Margin = new System.Windows.Forms.Padding(0);
            this.DataTable.Name = "DataTable";
            this.DataTable.RowHeight = 18;
            this.DataTable.Size = new System.Drawing.Size(532, 399);
            this.DataTable.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 407);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(104, 407);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // Show_Button
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 457);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.GB_Table);
            this.Controls.Add(this.GB_Fields);
            this.Name = "Show_Button";
            this.Text = "JSONParser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.GB_Fields.ResumeLayout(false);
            this.GB_Table.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox GB_Fields;
        private System.Windows.Forms.ListBox LB_FieldsList;
        private System.Windows.Forms.GroupBox GB_Table;
        private Parser.MyDataGrid DataTable;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}

