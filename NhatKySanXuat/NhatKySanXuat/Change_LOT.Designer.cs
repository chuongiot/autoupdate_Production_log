
namespace NhatKySanXuat
{
    partial class Change_LOT
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonsave = new System.Windows.Forms.Button();
            this.tb_lot_old = new System.Windows.Forms.TextBox();
            this.tb_dotsx_old = new System.Windows.Forms.TextBox();
            this.tb_so_me_old = new System.Windows.Forms.TextBox();
            this.tb_thiet_bi_old = new System.Windows.Forms.TextBox();
            this.tb_lot_new = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "LOT hiện tại";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Thiết bị";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Số mẻ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Đợt sx";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "LOT mới";
            // 
            // buttonsave
            // 
            this.buttonsave.BackColor = System.Drawing.Color.SkyBlue;
            this.buttonsave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonsave.Image = global::NhatKySanXuat.Properties.Resources.Button_Refresh_icon;
            this.buttonsave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonsave.Location = new System.Drawing.Point(15, 121);
            this.buttonsave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.buttonsave.Name = "buttonsave";
            this.buttonsave.Size = new System.Drawing.Size(91, 31);
            this.buttonsave.TabIndex = 111;
            this.buttonsave.Text = "Cập nhật";
            this.buttonsave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonsave.UseVisualStyleBackColor = false;
            this.buttonsave.Click += new System.EventHandler(this.buttonsave_Click);
            // 
            // tb_lot_old
            // 
            this.tb_lot_old.Location = new System.Drawing.Point(89, 2);
            this.tb_lot_old.Name = "tb_lot_old";
            this.tb_lot_old.Size = new System.Drawing.Size(143, 20);
            this.tb_lot_old.TabIndex = 112;
            // 
            // tb_dotsx_old
            // 
            this.tb_dotsx_old.Location = new System.Drawing.Point(89, 24);
            this.tb_dotsx_old.Name = "tb_dotsx_old";
            this.tb_dotsx_old.Size = new System.Drawing.Size(143, 20);
            this.tb_dotsx_old.TabIndex = 113;
            // 
            // tb_so_me_old
            // 
            this.tb_so_me_old.Location = new System.Drawing.Point(89, 48);
            this.tb_so_me_old.Name = "tb_so_me_old";
            this.tb_so_me_old.Size = new System.Drawing.Size(143, 20);
            this.tb_so_me_old.TabIndex = 114;
            // 
            // tb_thiet_bi_old
            // 
            this.tb_thiet_bi_old.Location = new System.Drawing.Point(89, 70);
            this.tb_thiet_bi_old.Name = "tb_thiet_bi_old";
            this.tb_thiet_bi_old.Size = new System.Drawing.Size(143, 20);
            this.tb_thiet_bi_old.TabIndex = 115;
            // 
            // tb_lot_new
            // 
            this.tb_lot_new.Location = new System.Drawing.Point(89, 92);
            this.tb_lot_new.Name = "tb_lot_new";
            this.tb_lot_new.Size = new System.Drawing.Size(143, 20);
            this.tb_lot_new.TabIndex = 116;
            // 
            // Change_LOT
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(244, 163);
            this.Controls.Add(this.tb_lot_new);
            this.Controls.Add(this.tb_thiet_bi_old);
            this.Controls.Add(this.tb_so_me_old);
            this.Controls.Add(this.tb_dotsx_old);
            this.Controls.Add(this.tb_lot_old);
            this.Controls.Add(this.buttonsave);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Change_LOT";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đổi LOT";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonsave;
        private System.Windows.Forms.TextBox tb_lot_old;
        private System.Windows.Forms.TextBox tb_dotsx_old;
        private System.Windows.Forms.TextBox tb_so_me_old;
        private System.Windows.Forms.TextBox tb_thiet_bi_old;
        private System.Windows.Forms.TextBox tb_lot_new;
    }
}