
namespace NhatKySanXuat
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.checkBoxshowpass = new System.Windows.Forms.CheckBox();
            this.tbuser = new System.Windows.Forms.TextBox();
            this.tbpassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnchangepassword = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.bttthoat = new System.Windows.Forms.Button();
            this.bttdangnhap = new System.Windows.Forms.Button();
            this.checkBox_save_tk = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxshowpass
            // 
            this.checkBoxshowpass.AutoSize = true;
            this.checkBoxshowpass.Location = new System.Drawing.Point(169, 106);
            this.checkBoxshowpass.Margin = new System.Windows.Forms.Padding(4);
            this.checkBoxshowpass.Name = "checkBoxshowpass";
            this.checkBoxshowpass.Size = new System.Drawing.Size(118, 21);
            this.checkBoxshowpass.TabIndex = 5;
            this.checkBoxshowpass.Text = "Hiện mật khẩu";
            this.checkBoxshowpass.UseVisualStyleBackColor = true;
            this.checkBoxshowpass.CheckedChanged += new System.EventHandler(this.checkBoxshowpass_CheckedChanged);
            // 
            // tbuser
            // 
            this.tbuser.Location = new System.Drawing.Point(252, 28);
            this.tbuser.Margin = new System.Windows.Forms.Padding(4);
            this.tbuser.MaxLength = 10;
            this.tbuser.Name = "tbuser";
            this.tbuser.Size = new System.Drawing.Size(132, 23);
            this.tbuser.TabIndex = 1;
            this.tbuser.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbpassword_KeyDown);
            // 
            // tbpassword
            // 
            this.tbpassword.Location = new System.Drawing.Point(252, 67);
            this.tbpassword.Margin = new System.Windows.Forms.Padding(4);
            this.tbpassword.MaxLength = 10;
            this.tbpassword.Multiline = true;
            this.tbpassword.Name = "tbpassword";
            this.tbpassword.PasswordChar = '*';
            this.tbpassword.Size = new System.Drawing.Size(132, 23);
            this.tbpassword.TabIndex = 2;
            this.tbpassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbpassword_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(166, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Tài khoản";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(166, 73);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mật khẩu";
            // 
            // btnchangepassword
            // 
            this.btnchangepassword.BackColor = System.Drawing.Color.Lavender;
            this.btnchangepassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnchangepassword.Image = global::NhatKySanXuat.Properties.Resources.Security_Password_2_icon;
            this.btnchangepassword.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnchangepassword.Location = new System.Drawing.Point(289, 146);
            this.btnchangepassword.Margin = new System.Windows.Forms.Padding(4);
            this.btnchangepassword.Name = "btnchangepassword";
            this.btnchangepassword.Size = new System.Drawing.Size(132, 36);
            this.btnchangepassword.TabIndex = 7;
            this.btnchangepassword.Text = "Đổi mật khẩu";
            this.btnchangepassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnchangepassword.UseVisualStyleBackColor = false;
            this.btnchangepassword.Click += new System.EventHandler(this.btnchangepassword_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NhatKySanXuat.Properties.Resources.anhlogin;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(147, 115);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // bttthoat
            // 
            this.bttthoat.BackColor = System.Drawing.Color.Lavender;
            this.bttthoat.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttthoat.Image = global::NhatKySanXuat.Properties.Resources.Button_Close_icon;
            this.bttthoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bttthoat.Location = new System.Drawing.Point(159, 146);
            this.bttthoat.Margin = new System.Windows.Forms.Padding(4);
            this.bttthoat.Name = "bttthoat";
            this.bttthoat.Size = new System.Drawing.Size(122, 36);
            this.bttthoat.TabIndex = 4;
            this.bttthoat.Text = "Thoát";
            this.bttthoat.UseVisualStyleBackColor = false;
            this.bttthoat.Click += new System.EventHandler(this.button2_Click);
            // 
            // bttdangnhap
            // 
            this.bttdangnhap.BackColor = System.Drawing.Color.Lavender;
            this.bttdangnhap.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bttdangnhap.Image = global::NhatKySanXuat.Properties.Resources.Button_Next_icon;
            this.bttdangnhap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.bttdangnhap.Location = new System.Drawing.Point(29, 146);
            this.bttdangnhap.Margin = new System.Windows.Forms.Padding(4);
            this.bttdangnhap.Name = "bttdangnhap";
            this.bttdangnhap.Size = new System.Drawing.Size(122, 36);
            this.bttdangnhap.TabIndex = 3;
            this.bttdangnhap.Text = "Đăng Nhập";
            this.bttdangnhap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.bttdangnhap.UseVisualStyleBackColor = false;
            this.bttdangnhap.Click += new System.EventHandler(this.bttdangnhap_Click);
            // 
            // checkBox_save_tk
            // 
            this.checkBox_save_tk.AutoSize = true;
            this.checkBox_save_tk.Location = new System.Drawing.Point(304, 106);
            this.checkBox_save_tk.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_save_tk.Name = "checkBox_save_tk";
            this.checkBox_save_tk.Size = new System.Drawing.Size(115, 21);
            this.checkBox_save_tk.TabIndex = 6;
            this.checkBox_save_tk.Text = "Nhớ tài khoản";
            this.checkBox_save_tk.UseVisualStyleBackColor = true;
            this.checkBox_save_tk.CheckedChanged += new System.EventHandler(this.checkBox_save_tk_CheckedChanged);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(432, 195);
            this.Controls.Add(this.checkBox_save_tk);
            this.Controls.Add(this.btnchangepassword);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbpassword);
            this.Controls.Add(this.tbuser);
            this.Controls.Add(this.checkBoxshowpass);
            this.Controls.Add(this.bttthoat);
            this.Controls.Add(this.bttdangnhap);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(448, 234);
            this.MinimumSize = new System.Drawing.Size(448, 234);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Đăng nhập";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bttdangnhap;
        private System.Windows.Forms.Button bttthoat;
        private System.Windows.Forms.CheckBox checkBoxshowpass;
        private System.Windows.Forms.TextBox tbuser;
        private System.Windows.Forms.TextBox tbpassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnchangepassword;
        private System.Windows.Forms.CheckBox checkBox_save_tk;
    }
}