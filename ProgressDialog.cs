using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public class ProgressDialog
{
    private BackgroundWorker worker = new BackgroundWorker();

    public event CancelEventHandler Canceled;
    public event RunWorkerCompletedEventHandler Completed;
    public event ProgressChangedEventHandler ProgressChanged;
    public event DoWorkEventHandler DoWork;

    private loadingDialog dialog = new loadingDialog();

    public ProgressDialog()
    {
        worker = new BackgroundWorker();
        worker.ProgressChanged += Worker_ProgressChanged;
        worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        worker.DoWork += worker_DoWork;
        worker.WorkerSupportsCancellation = true;
        worker.WorkerReportsProgress = true;
        dialog.Canceled += dialog_Canceled;
    }

    void dialog_Canceled(object sender, CancelEventArgs e)
    {
        worker.CancelAsync();
        if (Canceled != null)
        {
            Canceled(this, e);
        }
    }

    void worker_DoWork(object sender, DoWorkEventArgs e)
    {
        if (DoWork != null)
        {
            DoWork(this, e);
            e.Cancel = Cancelled;
        }
        else
        {
            MessageBox.Show("No work to do!", "No Work", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void Run()
    {
        worker.RunWorkerAsync();
        dialog.ShowDialog();
    }

    public void Run(object argument)
    {
        worker.RunWorkerAsync(argument);
        dialog.ShowDialog();
    }

    private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
        if (Completed != null)
        {
            Completed(this, e);
        }
        dialog.Close();
    }

    private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
        if (ProgressChanged != null)
        {
            ProgressChanged(this, e);
        }
    }

    public void ReportProgress(int percentProgress)
    {
        worker.ReportProgress(percentProgress);
    }

    public string Title
    {
        get { return dialog.Text; }
        set { dialog.Text = value; }
    }

    public string Message
    {
        get { return dialog.message.Text; }
        set { dialog.message.Text = value; }
    }

    public int Progress
    {
        get { return dialog.progressBar.Value; }
        set { dialog.progressBar.Value = value; }
    }

    public bool Cancelled
    {
        get { return worker.CancellationPending; }
    }

    public ProgressBarStyle Style
    {
        get { return dialog.progressBar.Style; }
        set { dialog.progressBar.Style = value; }
    }

    private class loadingDialog : Form
    {
        public System.Windows.Forms.Label message;
        public System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button bCancel;
        public event CancelEventHandler Canceled;

        public loadingDialog()
        {
            this.message = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // message
            // 
            this.message.AutoSize = true;
            this.message.Location = new System.Drawing.Point(12, 9);
            this.message.Name = "message";
            this.message.Size = new System.Drawing.Size(54, 13);
            this.message.TabIndex = 0;
            this.message.Text = "Loading...";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 25);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(421, 23);
            this.progressBar.TabIndex = 1;
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.Location = new System.Drawing.Point(358, 54);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // loadingDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 89);
            this.ControlBox = false;
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "loadingDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Loading";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel?", "Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
            {
                bCancel.Enabled = false;
                this.Text = "Canceling...";
                if (Canceled != null)
                {
                    Canceled(this, new CancelEventArgs(true));
                }
            }
        }
    }
}
