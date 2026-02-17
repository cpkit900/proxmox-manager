namespace ProxmoxManager.Forms;

partial class AddConnectionForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.TextBox txtUrl;
    private System.Windows.Forms.TextBox txtUsername;
    private System.Windows.Forms.TextBox txtPassword;
    private System.Windows.Forms.TextBox txtRealm;
    private System.Windows.Forms.Button btnSave;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.txtUrl = new System.Windows.Forms.TextBox();
        this.txtUsername = new System.Windows.Forms.TextBox();
        this.txtPassword = new System.Windows.Forms.TextBox();
        this.txtRealm = new System.Windows.Forms.TextBox();
        this.btnSave = new System.Windows.Forms.Button();
        this.lblStatus = new System.Windows.Forms.Label();
        this.label1 = new System.Windows.Forms.Label();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.SuspendLayout();
        // 
        // txtUrl
        // 
        this.txtUrl.Location = new System.Drawing.Point(100, 20);
        this.txtUrl.Name = "txtUrl";
        this.txtUrl.Size = new System.Drawing.Size(250, 23);
        this.txtUrl.TabIndex = 0;
        // 
        // txtUsername
        // 
        this.txtUsername.Location = new System.Drawing.Point(100, 50);
        this.txtUsername.Name = "txtUsername";
        this.txtUsername.Size = new System.Drawing.Size(250, 23);
        this.txtUsername.TabIndex = 1;
        // 
        // txtPassword
        // 
        this.txtPassword.Location = new System.Drawing.Point(100, 80);
        this.txtPassword.Name = "txtPassword";
        this.txtPassword.PasswordChar = '*';
        this.txtPassword.Size = new System.Drawing.Size(250, 23);
        this.txtPassword.TabIndex = 2;
        // 
        // txtRealm
        // 
        this.txtRealm.Location = new System.Drawing.Point(100, 110);
        this.txtRealm.Name = "txtRealm";
        this.txtRealm.Size = new System.Drawing.Size(250, 23);
        this.txtRealm.TabIndex = 3;
        // 
        // btnSave
        // 
        this.btnSave.Location = new System.Drawing.Point(100, 150);
        this.btnSave.Name = "btnSave";
        this.btnSave.Size = new System.Drawing.Size(75, 23);
        this.btnSave.TabIndex = 4;
        this.btnSave.Text = "Save";
        this.btnSave.UseVisualStyleBackColor = true;
        // 
        // lblStatus
        // 
        this.lblStatus.AutoSize = true;
        this.lblStatus.Location = new System.Drawing.Point(100, 180);
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(39, 15);
        this.lblStatus.TabIndex = 5;
        this.lblStatus.Text = "";
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(20, 23);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(31, 15);
        this.label1.Text = "URL:";
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(20, 53);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(63, 15);
        this.label2.Text = "Username:";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(20, 83);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(60, 15);
        this.label3.Text = "Password:";
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(20, 113);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(42, 15);
        this.label4.Text = "Realm:";
        // 
        // AddConnectionForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 220);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.lblStatus);
        this.Controls.Add(this.btnSave);
        this.Controls.Add(this.txtRealm);
        this.Controls.Add(this.txtPassword);
        this.Controls.Add(this.txtUsername);
        this.Controls.Add(this.txtUrl);
        this.Name = "AddConnectionForm";
        this.Text = "Add Proxmox Connection";
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
