namespace ProxmoxManager;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private System.Windows.Forms.DataGridView dgvConnections;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnSync;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel lblStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private System.Windows.Forms.TreeView tvDetails;

    private void InitializeComponent()
    {
        this.dgvConnections = new System.Windows.Forms.DataGridView();
        this.btnAdd = new System.Windows.Forms.Button();
        this.btnSync = new System.Windows.Forms.Button();
        this.statusStrip1 = new System.Windows.Forms.StatusStrip();
        this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
        this.tvDetails = new System.Windows.Forms.TreeView();
        
        ((System.ComponentModel.ISupportInitialize)(this.dgvConnections)).BeginInit();
        this.statusStrip1.SuspendLayout();
        this.SuspendLayout();
        
        // 
        // dgvConnections
        // 
        this.dgvConnections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
        this.dgvConnections.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.dgvConnections.Location = new System.Drawing.Point(12, 50);
        this.dgvConnections.Name = "dgvConnections";
        this.dgvConnections.Size = new System.Drawing.Size(776, 200);
        this.dgvConnections.TabIndex = 0;
        this.dgvConnections.AutoGenerateColumns = false;
        
        var colUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colUrl.DataPropertyName = "Url";
        colUrl.HeaderText = "URL";
        colUrl.Width = 200;
        
        var colUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colUser.DataPropertyName = "Username";
        colUser.HeaderText = "Username";
        
        var colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colStatus.DataPropertyName = "Status";
        colStatus.HeaderText = "Status";
        
        var colNodes = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colNodes.DataPropertyName = "NodesList";
        colNodes.HeaderText = "Nodes";
        colNodes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;

        var colVms = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colVms.DataPropertyName = "VmCount";
        colVms.HeaderText = "VMs";
        colVms.Width = 60;

        var colCts = new System.Windows.Forms.DataGridViewTextBoxColumn();
        colCts.DataPropertyName = "CtCount";
        colCts.HeaderText = "CTs";
        colCts.Width = 60;

        this.dgvConnections.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            colUrl,
            colUser,
            colStatus,
            colNodes,
            colVms,
            colCts
        });
        // 
        // btnAdd
        // 
        this.btnAdd.Location = new System.Drawing.Point(12, 12);
        this.btnAdd.Name = "btnAdd";
        this.btnAdd.Size = new System.Drawing.Size(120, 30);
        this.btnAdd.TabIndex = 1;
        this.btnAdd.Text = "Add Connection";
        this.btnAdd.UseVisualStyleBackColor = true;
        // 
        // btnSync
        // 
        this.btnSync.Location = new System.Drawing.Point(138, 12);
        this.btnSync.Name = "btnSync";
        this.btnSync.Size = new System.Drawing.Size(120, 30);
        this.btnSync.TabIndex = 2;
        this.btnSync.Text = "Sync All";
        this.btnSync.UseVisualStyleBackColor = true;
        // 
        // statusStrip1
        // 
        this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
        this.statusStrip1.Location = new System.Drawing.Point(0, 428);
        this.statusStrip1.Name = "statusStrip1";
        this.statusStrip1.Size = new System.Drawing.Size(800, 22);
        this.statusStrip1.TabIndex = 3;
        this.statusStrip1.Text = "statusStrip1";
        // 
        // lblStatus
        // 
        this.lblStatus.Name = "lblStatus";
        this.lblStatus.Size = new System.Drawing.Size(39, 17);
        this.lblStatus.Text = "Ready";
        //
        // tvDetails
        //
        this.tvDetails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
        this.tvDetails.Location = new System.Drawing.Point(12, 260);
        this.tvDetails.Name = "tvDetails";
        this.tvDetails.Size = new System.Drawing.Size(776, 160);
        this.tvDetails.TabIndex = 4;
        // 
        // MainForm
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.tvDetails);
        this.Controls.Add(this.statusStrip1);
        this.Controls.Add(this.btnSync);
        this.Controls.Add(this.btnAdd);
        this.Controls.Add(this.dgvConnections);
        this.Name = "MainForm";
        this.Text = "Proxmox Manager";
        ((System.ComponentModel.ISupportInitialize)(this.dgvConnections)).EndInit();
        this.statusStrip1.ResumeLayout(false);
        this.statusStrip1.PerformLayout();
        this.ResumeLayout(false);
        this.PerformLayout();
    }
}
