namespace Netfolder_Client
{
    partial class Form2
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtPW = new System.Windows.Forms.TextBox();
            this.btn_LOGIN = new System.Windows.Forms.Button();
            this.btn_JOIN = new System.Windows.Forms.Button();
            this.btn_SEARCHPW = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(258, 150);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(272, 25);
            this.txtID.TabIndex = 1;
            // 
            // txtPW
            // 
            this.txtPW.Location = new System.Drawing.Point(258, 196);
            this.txtPW.Name = "txtPW";
            this.txtPW.Size = new System.Drawing.Size(272, 25);
            this.txtPW.TabIndex = 1;
            // 
            // btn_LOGIN
            // 
            this.btn_LOGIN.Location = new System.Drawing.Point(235, 256);
            this.btn_LOGIN.Name = "btn_LOGIN";
            this.btn_LOGIN.Size = new System.Drawing.Size(75, 23);
            this.btn_LOGIN.TabIndex = 3;
            this.btn_LOGIN.Text = "LOGIN";
            this.btn_LOGIN.UseVisualStyleBackColor = true;
            this.btn_LOGIN.Click += new System.EventHandler(this.btn_LOGIN_Click);
            // 
            // btn_JOIN
            // 
            this.btn_JOIN.Location = new System.Drawing.Point(332, 256);
            this.btn_JOIN.Name = "btn_JOIN";
            this.btn_JOIN.Size = new System.Drawing.Size(75, 23);
            this.btn_JOIN.TabIndex = 3;
            this.btn_JOIN.Text = "JOIN";
            this.btn_JOIN.UseVisualStyleBackColor = true;
            this.btn_JOIN.Click += new System.EventHandler(this.btn_JOIN_Click);
            // 
            // btn_SEARCHPW
            // 
            this.btn_SEARCHPW.Location = new System.Drawing.Point(430, 256);
            this.btn_SEARCHPW.Name = "btn_SEARCHPW";
            this.btn_SEARCHPW.Size = new System.Drawing.Size(105, 23);
            this.btn_SEARCHPW.TabIndex = 3;
            this.btn_SEARCHPW.Text = "SEARCHPW";
            this.btn_SEARCHPW.UseVisualStyleBackColor = true;
            this.btn_SEARCHPW.Click += new System.EventHandler(this.btn_SEARCHPW_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(232, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 203);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "PW";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_SEARCHPW);
            this.Controls.Add(this.btn_JOIN);
            this.Controls.Add(this.btn_LOGIN);
            this.Controls.Add(this.txtPW);
            this.Controls.Add(this.txtID);
            this.Name = "Form2";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtPW;
        private System.Windows.Forms.Button btn_LOGIN;
        private System.Windows.Forms.Button btn_JOIN;
        private System.Windows.Forms.Button btn_SEARCHPW;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

