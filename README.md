ProgressDialog
==============

C# Multi-threaded background worker with dialog to show progress

Usage
=====
```c#
ProgressDialog pDialog = new ProgressDialog();
pDialog.Title = "Progress Title";
pDialog.DoWork += delegate(object dialog, DoWorkEventArgs dwe)
{
  for (int i = 0; i < (int)dwe.Argument; i++)
  {
    pDialog.ReportProgress(i);
    Thread.Sleep(100);
  }
};
pDialog.ProgressChanged += delegate(object dialog, ProgressChangedEventArgs pce) {
    pDialog.Message = pce.ProgressPercentage + "/100";
    pDialog.Progress = pce.ProgressPercentage % 100;
};
pDialog.Completed += delegate(object dialog, RunWorkerCompletedEventArgs e) {
    MessageBox.Show("Completed");
};
pDialog.Run(100);
```
