using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Deployment.Application;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atenta.ELL;
using LiveLeakCommentSearch.Properties;

namespace LiveLeakCommentSearch
{
    public partial class FrmCommentMonitor : Form
    {
        private CommentManager _cm;
        private LiveLeakCommentAnalyser _ca;
        private readonly Ell _log;                                                    // Handles writing to Windows event log
        private State _state;                                                         // Used to hold / save current state of search
        private bool _userStopped;                                                    // Flag used to indicate search should stop
        private readonly int _scanLimit = Settings.Default.ScanLimit;                 // The number of times to try looking ahead for next comment.
        private readonly int _scanBackwardsBias = Settings.Default.ScanBackwardsBias; // Bias added to Scanlimit when looking backwards for next comment.
        private readonly int _scanForwardRate = Settings.Default.ScanForwardRate;     // How many comments to jump forwards when looking for start point.
        private readonly int _scanBackwardRate = Settings.Default.ScanBackwardsRate;  // How many comments to jump backwards when looking for start point.
        private readonly int _waitTime = Settings.Default.WaitTime;                   // How long to wait for a comment to appear 
        private readonly int _waitMax = Settings.Default.WaitMax;                     // How many times to wait.
        private NotifyIcon _notifyIcon1;
        
        public FrmCommentMonitor()
        {
            InitializeComponent();
            numCommentId.Value = Settings.Default.StartingId;
            _log = new Ell(Resources.TrayIcon_Text_LCS, "LiveLeakCommentSearch");
            CheckVersionAndEventLog();
            CreateToolTipsAndIcons();
            InitializeStateObject();
        }
        private async void BeginSearch(SearchType type)
        {
            _userStopped = false;
            var nameList = new List<string>(txtUserName.Text.Split(',').Select(s=>s.Trim()));
            nameList.RemoveAll(strNull => strNull.Trim().Length == 0);

            var startId = (type == SearchType.SingleComment)
                ? long.Parse(numCommentId.Value.ToString(CultureInfo.InvariantCulture))
                : Settings.Default.StartingId;

            // Check if can reload previous state?
            if (_state.Changed & type != SearchType.SingleComment)
            {
                var str = new StringBuilder();
                str.AppendLine(Resources.BeginSearch_resume_last_search);
                str.AppendLine();
                
                switch (_state.Type)
                {
                    case SearchType.BadWord:
                        str.AppendFormat("Type : {0}\r\n", "Bad word search");
                        break;
                    case SearchType.UserName:
                        str.AppendFormat("Type : {0}\r\n", "User");
                        break;
                    case SearchType.Unfiltered:
                        str.AppendFormat("Type : {0}\r\n", "All comments");
                        break;
                }

                if (_state.Type == SearchType.UserName)
                {
                    str.Append("Names: ");
                    foreach (var name in _state.Names)
                        str.AppendFormat("{0} ", name);
                    str.AppendLine();
                }

                var ts = DateTime.Now.Subtract(new DateTime(_state.TimeStamp));
                str.AppendFormat("Direction: {0}\r\n", (_state.Forwards) ? "Forwards" : "Backwards");
                str.AppendFormat("Comment: {0}\r\n", _state.LastId);
                str.AppendFormat("Age: {0:N} {1}", ts.TotalHours < 1 ? ts.TotalMinutes : ts.TotalHours, ts.TotalHours < 1 ? "Minutes":"Hours");
                
                var dialogResult = MessageBox.Show(str.ToString()
                    , Resources.Resume_previous_search_Caption
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dialogResult == DialogResult.Yes)
                {
                    type = _state.Type;
                    nameList = _state.Names;
                    startId = _state.LastId;
                    RestoreInterfaceFromState();
                }
                else
                {
                    _state = new State();
                    _state.StateChanged += (o,e) => btnExport.Enabled = _state.Changed;
                }
            }

            try
            {
                await SearchTaskAsync(type, (radioButton1.Checked), nameList, startId);
            }
            catch (Exception ex)
            {
                _log.LogEvent(ex.Message + ".\n" + ex.Source + ex.StackTrace
                    , EventLogEntryType.Error);
                _cm.BStopRunning = true;
            }
            finally
            {
                EnableDisableControls(true);
                if (type != SearchType.SingleComment && !_userStopped)
                {
                    var sb = new StringBuilder();
                    sb.Append(Resources.FrmCommentMonitor_Search_Stopped);
                    sb.AppendFormat("{0} until {1}", 
                        (radioButton1.Checked) ? "Forward" : "Backwards",
                        lblCurrentCommentCount.Text);
                    _log.LogEvent(sb.ToString(), EventLogEntryType.Warning);
                    
                    if (chkNotifications.Checked)
                    {
                        _notifyIcon1.ShowBalloonTip(5, "LCS", sb.ToString(),
                            ToolTipIcon.Warning);
                    }
                }
            }
        }
        private async Task<long> FindStartingPointTaskAsync(long startingId = 56300428L)
        {
            ShowHidePopup(true, Resources.ShowHidePopup_Please_wait);
            var count = await FindLatestCommentId(startingId);
            ShowHidePopup(false, Resources.ShowHidePopup_Please_wait);
            Debug.Print("*** Saving starting point as {0}", count);
            Settings.Default.StartingId = count;
            Settings.Default.Save();
            return count;
        }
        private async Task<LiveLeakComment> GetCommentTaskAsync(long commentId)
        {
            var page = await _cm.GetCommentPage(commentId);
            return _cm.ParseCommentFromPage(page, commentId).Result;
        }
        // ReSharper disable once FunctionComplexityOverflow
        private async Task SearchTaskAsync(SearchType type, bool forwards, List<string> usernameList = null, long cId = 0L)
        {
            var commentId = cId;
            var processedComments = 0L;
            var waitCount = 0;
            _cm = new CommentManager(forwards);
            _ca = new LiveLeakCommentAnalyser();
            EnableDisableControls(false);

            try
            {
                // If this is a new run (ignoring single comment requests)
                // Find the latest comment Id as a starting point, saving 
                // it for next time.
                if (type != SearchType.SingleComment && !_state.Changed)
                    commentId = await FindStartingPointTaskAsync(commentId);

                // Is this a new or continuation?
                if (_state.Changed && type != SearchType.SingleComment)
                {
                    processedComments = _state.Processed;
                    waitCount = 0;
                }
                else
                    _state.StartId = commentId;

                while (!_cm.BStopRunning)
                {
                    var comment = await GetCommentTaskAsync(commentId);

                    UpdateDisplay(commentId, processedComments,
                        new KeyValuePair<string, Color>("Downloading", Color.GreenYellow), 
                        comment.TimeEstimate);

                    if (!comment.HasErrors)
                    {
                        // Reset any previous wait count
                        waitCount = 0;

                        if (_ca.CheckMatch(type, comment, usernameList, chkUseWordlist.Checked))
                            _cm.CreateGridRow(ref grdOutput, comment);

                        // Forwards or backwards?
                        if (forwards)
                            commentId++;
                        else
                            commentId--;

                        // Save state / progress
                        if (type != SearchType.SingleComment)
                            _state.Update(commentId, type, usernameList, forwards,comment.TimeEstimate);

                        // Only wanted a single comment? stop the loop
                        if (type == SearchType.SingleComment) _cm.BStopRunning = true;

                        ++processedComments;
                    }
                    else
                    {
                        if (type != SearchType.SingleComment)
                        {
                            // Test for end and quit if reached.
                            var end = await CheckForEnd(commentId, processedComments, forwards);
                            if (end != -1)
                            {
                                Debug.Print("*** Skipping {0} to {1}.", commentId, end);
                                commentId = end;
                            }
                            else // Reached end of comments...
                            {
                                // If moving forwards in time...
                                if (forwards)
                                {
                                    // ... more comments may appear. So wait WaitCountMax times.
                                    waitCount++;
                                    if (waitCount <= _waitMax)
                                    {
                                        Debug.Print("*** Waiting for comment {0} {1} time.",
                                            commentId, waitCount);
                                        UpdateDisplay(commentId, processedComments,
                                            new KeyValuePair<string, 
                                            Color>(string.Format("Waiting ({0})", waitCount),Color.PaleVioletRed));

                                        await Task.Delay(_waitTime*waitCount);
                                    }
                                    else
                                    {
                                        Debug.Print("*** Finished waiting {0:D}s for {1}. Stopping search.",
                                            (waitCount*(_waitMax/1000)), commentId);
                                        UpdateDisplay(commentId, processedComments,
                                            new KeyValuePair<string, Color>("Stopped", Color.Transparent));
                                        _cm.BStopRunning = true;
                                    }
                                }
                                else  // Moving backwards
                                {
                                    Debug.Print("*** No more comments found {0}. Stopping.", commentId);
                                    _cm.BStopRunning = true;
                                }
                            }
                        }
                        else
                        {
                            _cm.BStopRunning = true;
                        }
                    }
                    if (commentId <= 0 && !forwards) _cm.BStopRunning = true;
                }
            }
            catch (WebException ex)
            {
                _log.LogEvent(ex.Message + ".\n" + ex.Source + ex.StackTrace, EventLogEntryType.Error);
                MessageBox.Show(Resources.Unable_to_connect, Resources.Web_Error, MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
            finally
            {
                EnableDisableControls(true);
                UpdateDisplay(commentId, processedComments, 
                    new KeyValuePair<string, Color>("Stopped", Color.Transparent));
            }
        }
        private async Task<long> FindLatestCommentId(long commentCount)
        {
            var bFinished = false;
            while (!_cm.BStopRunning && !bFinished)
            {
                Debug.Print("*** Searching {1} comment {0}.", commentCount, "forwards");
                UpdateDisplay(commentCount, 0, new KeyValuePair<string, Color>("Locating start",Color.DarkSeaGreen));
                var commentContent = await _cm.GetCommentPage(commentCount);
                if (_cm.IsComment(commentContent))
                {
                    commentCount += _scanForwardRate;
                }
                else
                {
                    // Have we hit the end of comments?
                    var skippedComment = await CheckForEnd(commentCount, 0, true);
                    if (skippedComment == -1)
                    {
                        Debug.Print("*** End reached {0}, scanning backwards", commentCount);
                        var bFinishedBackwards = false;
                        
                        // Now scan backwards to until we get a comment 
                        while (!_cm.BStopRunning && !bFinishedBackwards)
                        {
                            commentCount -= _scanBackwardRate;
                            commentContent = await _cm.GetCommentPage(commentCount);
                            if (_cm.IsComment(commentContent))
                            {
                                bFinishedBackwards = true;
                            }
                            Debug.Print("*** Searching {1} comment {0}.", commentCount, "backwards");
                            UpdateDisplay(commentCount, 0, new KeyValuePair<string, Color>("Narrowing", Color.LimeGreen));
                        }
                        bFinished = true;
                    }
                    else
                    {
                        commentCount = skippedComment;    
                    }
                    
                }
            }
            return commentCount;
        }
        /// <summary>
        /// Sometimes there may be a gap in the comment Id's, so need to 'scan' forwards (or backwards) 
        /// to test whether we've really hit the last comment Id, or just a gap in the Id's
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="processed"></param>
        /// <param name="forwards"></param>
        /// <returns></returns>
        private async Task<long> CheckForEnd(long commentId, long processed, bool forwards)
        {
            var retVal = -1L;
            var limit = (forwards) ? _scanLimit : _scanLimit + _scanBackwardsBias;

            for (var i = 1; i <= limit; i++)
            {
                Debug.Print("*** Comment {0} is blank. Scanning for {1}",
                    commentId, (forwards) ? commentId + i : commentId - i);
                UpdateDisplay(commentId, processed, 
                    new KeyValuePair<string, Color>(string.Format("Scanning ({0})", i), Color.DarkOrange));

                if (forwards)
                    commentId++;
                else
                {
                    commentId--;
                    await Task.Delay(250);
                }

                var comment = await _cm.GetCommentPage(commentId);
                if (!_cm.IsComment(comment)) continue;
                retVal = commentId;
                break;
            }    
            return retVal;
        }
        
        #region Initialisation
        private void InitializeStateObject()
        {
            _state = new State();
            _state.StateChanged += (o, e) => btnExport.Enabled = _state.Changed;
            if (Settings.Default.State == null) return;
            _state.Deserialise(Settings.Default.State);
            RestoreInterfaceFromState();
        }
        private void CreateToolTipsAndIcons()
        {
            // Create ToolTips
            var toolTip = new ToolTip { AutoPopDelay = 10000, InitialDelay = 100, ReshowDelay = 500, ShowAlways = true };
            toolTip.SetToolTip(txtUserName, Resources.Find_User_ToolTip);
            toolTip.SetToolTip(btnFindUser, Resources.Find_User_ToolTip);

            // Create the NotifyIcon.
            Icon = Resources.IconMain;
            _notifyIcon1 = new NotifyIcon(components)
            {
                Icon = Resources.IconMain,
                Text = Resources.TrayIcon_Text_LCS,
                Visible = true
            };
            _notifyIcon1.MouseClick += _notifyIcon1_MouseClick;
            _notifyIcon1.BalloonTipClicked += _notifyIcon1_BalloonTipClicked;
        }
        private void CheckVersionAndEventLog()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return;

            toolStripStatusLabelVersion.Text = String.Format("ver {0}"
                , ApplicationDeployment.CurrentDeployment.CurrentVersion);

            // Do we need to create an Event Log source?
            if (ApplicationDeployment.CurrentDeployment.IsFirstRun)
                Process.Start(Application.StartupPath + "\\EllConsole.exe", "-sLCS -nLiveLeakCommentSearch -c -v -q");
        }
        #endregion
        
        #region Event Handlers
        private void btnStart_Click(object sender, EventArgs e)
        {
            BeginSearch(chkUseWordlist.Checked ? SearchType.BadWord : SearchType.Unfiltered);
        }
        private void btnFindUser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUserName.Text.Trim()))
            {
                BeginSearch(SearchType.UserName);
                return;
            }

            MessageBox.Show(Resources.You_must_enter_a_username);
        }
        private void btnFindComment_Click(object sender, EventArgs e)
        {
            BeginSearch(SearchType.SingleComment);
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_cm != null) _cm.BStopRunning = true;
            _userStopped = true;
            EnableDisableControls(true);
            Settings.Default.State = _state.Serialise();
            Settings.Default.Save();
        }
        private void btnEditWordList_Click(object sender, EventArgs e)
        {
            Process.Start(Application.StartupPath + Resources.BadWordFile);
        }
        private void bntClear_Click(object sender, EventArgs e)
        {
            grdOutput.Rows.Clear();
            txtUserName.Clear();
            lblProcessedComentsCount.Text = "";
            lblCurrentCommentCount.Text = "";
        }
        private void grdOutput_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (!(senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn) || e.RowIndex < 0) return;

            // Ensure a valid URL before spawning browser process.
            var s = grdOutput[4, e.RowIndex].Value.ToString();
            Uri uriResult;
            if (Uri.TryCreate(s, UriKind.Absolute, out uriResult) && uriResult.Scheme == Uri.UriSchemeHttp)
                Process.Start(s);
        }
        private void grdOutput_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // Auto open?
            if (chkAutoOpen.Checked)
            {
                grdOutput_CellContentClick(grdOutput, new DataGridViewCellEventArgs(5, e.RowIndex));
            }

            // Notification required?
            if (chkNotifications.Checked)
            {
                var sb = new StringBuilder();
                sb.Append(grdOutput[1, grdOutput.Rows.GetLastRow(DataGridViewElementStates.None)].Value as string);
                sb.AppendLine(" Said:");
                sb.AppendLine(grdOutput[3, grdOutput.Rows.GetLastRow(DataGridViewElementStates.None)].Value as string);
                _notifyIcon1.ShowBalloonTip(5, "LCS found a Match", sb.ToString(), ToolTipIcon.Info);
            }

            if (grdOutput.RowCount > 0)
                btnExport.Enabled = true;
        }
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            var obj = sender as TextBox;
            if (obj == null) return;
            btnFindUser.Enabled = (!string.IsNullOrWhiteSpace(obj.Text));
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_cm != null) _cm.BStopRunning = true;
            
            if (_state.Changed)
            {
                Settings.Default.State = _state.Serialise();
                Settings.Default.Save();
            }
        }
        void _notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            Activate();
            ShowInTaskbar = true;
        }
        void _notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                WindowState = FormWindowState.Normal;

            Activate();
            ShowInTaskbar = true;
        }
        private void FrmCommentMonitor_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
                ShowInTaskbar = false;
        }
        private void chkUseWordlist_CheckedChanged(object sender, EventArgs e)
        {
            var a = sender as CheckBox;
            if (a != null) btnEditWordList.Enabled = a.Checked;
        }
        private void btnCheckWord_Click(object sender, EventArgs e)
        {
            var frm = new frmWordCheck();
            frm.ShowDialog();
        }
        private void grdOutput_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            if (grdOutput.RowCount == 0)
                btnExport.Enabled = false;
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (grdOutput.Rows.Count > 0)
            {
                var result = MessageBox.Show(Resources.FrmCommentMonitor_Importing_will_erase_the_results
                    , Resources.FrmCommentMonitor_Import_over_existing_results
                    , MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                if (result != DialogResult.OK) return;
            }

            var statusOnly = MessageBox.Show(Resources.include_all_results
                    , Resources.Include_all_results_or_just_state
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            Import(statusOnly == DialogResult.Yes);
        }
        private void grdOutput_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int currentMouseOverRow = grdOutput.HitTest(e.X, e.Y).RowIndex;
                if (currentMouseOverRow >= 0)
                {
                    grdOutput.Rows[currentMouseOverRow].Selected = true;
                    var mi = new MenuItem("Copy URL to clipboard");
                    mi.Click += (s, ev) =>
                    {
                        var val = grdOutput.Rows[currentMouseOverRow].Cells[0].Value;
                        Clipboard.SetData(DataFormats.Text
                            , string.Format("http://www.liveleak.com/comment?a=view_comment&comment_id={0}"
                            , val));
                    };

                    ContextMenu m = new ContextMenu();
                    m.MenuItems.Add(mi);
                    m.Show(grdOutput, new Point(e.X, e.Y));
                }
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(Resources.export_current_state_only
                , Resources.Export_State_only
                , MessageBoxButtons.YesNo, MessageBoxIcon.Question
                , MessageBoxDefaultButton.Button2);
            Export(result == DialogResult.Yes);
        }
        private void grdOutput_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            ShowHidePopup(grdOutput.SelectedRows.Count > 25
                , "Deleting large amount of data, please wait."
                , false);
        }
        private void lblCurrentCommentCount_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, lblCurrentCommentCount.Text);
        }
        private void tmrLabelFlash_Tick(object sender, EventArgs e)
        {
            label1.Visible = !label1.Visible;
            Application.DoEvents();
        }
        #endregion
        
        #region Interface Related Helpers
        private void UpdateDisplay(long commentId, long processedComments, KeyValuePair<string, Color> currentStatus, string time = "")
        {
            lblCurrentCommentCount.Text = commentId.ToString(CultureInfo.InvariantCulture);
            lblProcessedComentsCount.Text = processedComments.ToString(CultureInfo.InvariantCulture);
            lblStatus.Text = currentStatus.Key;
            lblStatus.BackColor = currentStatus.Value;
            tslblTime.Text = time;
            Application.DoEvents();
        }
        private void EnableDisableControls(bool enable)
        {
            groupBox1.Enabled = enable;
            //btnFindComment.Enabled = enable;  // Now using the chkbox to enable / disable this button.
            btnStart.Enabled = enable;
            txtUserName.Enabled = enable;
            numCommentId.Enabled = enable;
            bntClear.Enabled = enable;
            btnStop.Enabled = !btnStart.Enabled;
            if (txtUserName.Text.Length > 0)
                btnFindUser.Enabled = enable;

        }
        private void ShowHidePopup(bool show, string msg, bool flash = true)
        {
            var pnl = panel2;
            label1.Text = msg;
            pnl.Dock = DockStyle.Fill;
            label1.Visible = true;

            if (show)
            {
                if (flash) tmrLabelFlash.Start();
                pnl.Show();
            }
            else
            {
                tmrLabelFlash.Stop();
                pnl.Hide();
            }
            Application.DoEvents();
        }
        private void RestoreInterfaceFromState()
        {
            bntClear.Enabled = true;
            chkUseWordlist.Checked = _state.Type == SearchType.BadWord;
            if (_state.Forwards)
                radioButton1.Checked = true;
            else
                radioButton2.Checked = true;

            txtUserName.Clear();
            foreach (var name in _state.Names)
                txtUserName.AppendText(name + ",");
            txtUserName.Text = txtUserName.Text.Trim(',');
            btnExport.Enabled = false;
        }
        #endregion

        #region Import Export Related Methods
        private void Export(bool statusOnly = false)
        {
            // Choose an output directory
            var fdDlg = new FolderBrowserDialog
            {
                Description = Resources.FrmCommentMonitor_select_an_output_directory,
                ShowNewFolderButton = true
            };
            var dlgResult = fdDlg.ShowDialog();

            if (dlgResult != DialogResult.OK) return;

            var df = new DirectoryInfo(fdDlg.SelectedPath);
            if (df.GetFileSystemInfos().Length > 0)
            {
                var confirmation = MessageBox.Show(Resources.FrmCommentMonitor_location_already_contains_data
                    , Resources.FrmCommentMonitor_Target_not_Empty, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                if (confirmation == DialogResult.Cancel) return;
            }

            if (!statusOnly)
            {
                if (dlgResult == DialogResult.OK)
                {
                    panel1.Enabled = false;  
                    // Build a list of usernames
                    var allNames = new List<string>();
                    const int uNameIndex = 1;
                    const int idIndex = 0;
                    const int commentIndex = 3;
                    foreach (var row in grdOutput.Rows.Cast<DataGridViewRow>()
                        .Where(row => !allNames.Contains(row.Cells[uNameIndex].Value)))
                        allNames.Add(row.Cells[uNameIndex].Value.ToString());

                    // Build a list of users comments by ID.
                    var mydict = new Dictionary<string, string>[allNames.Count];
                    for (var i = 0; i < allNames.Count; i++)
                    {
                        mydict[i] = new Dictionary<string, string>();
                        foreach (DataGridViewRow row in grdOutput.Rows)
                        {
                            if (String.CompareOrdinal(row.Cells[uNameIndex].Value.ToString(), allNames[i]) == 0)
                            {
                                try
                                {
                                    mydict[i].Add(row.Cells[idIndex].Value.ToString(),
                                        row.Cells[commentIndex].Value.ToString());
                                }
                                catch (ArgumentException x)
                                {
                                    _log.LogEvent(x.Message + " - "
                                                  + allNames[i]
                                                  + " - " + row.Cells[idIndex].Value
                                                  + " : " + row.Cells[commentIndex].Value
                                                  , EventLogEntryType.Error);
                                }
                            }
                        }
                        Application.DoEvents();
                    }

                    // Now we have a target path the usernames, comments + Id's we're ready for export.            
                    for (var i = 0; i < allNames.Count; i++)
                    {
                        foreach (var kvp in mydict[i])
                        {
                            var sb = new StringBuilder();
                            sb.AppendLine(kvp.Key + "--" + kvp.Value);
                            File.AppendAllText(
                                fdDlg.SelectedPath + string.Format("\\{0}.txt", SanitizeFileName(allNames[i])),
                                sb.ToString());
                            Application.DoEvents();
                        }
                    }
                }
            }

            // Export is done, now lets save the state, for reloading later.
            if (File.Exists(fdDlg.SelectedPath + @"\_StateFile_.dat"))
                File.Delete(fdDlg.SelectedPath + @"\_StateFile_.dat");

            var stateFile = File.CreateText(fdDlg.SelectedPath + @"\_StateFile_.dat");
            stateFile.Write(_state.Serialise());
            stateFile.Close();

            MessageBox.Show((statusOnly) ? Resources.Status_Exported 
                : grdOutput.Rows.Count + Resources.rows_Exported
                    , Resources.FrmCommentMonitor_Export_complete
                    , MessageBoxButtons.OK, MessageBoxIcon.Information);

            grdOutput.Rows.Clear();
            panel1.Enabled = true;
        }
        private void Import(bool statusOnly = false)
        {
            // Choose an input directory
            var fdDlg = new FolderBrowserDialog
            {
                Description = Resources.FrmCommentMonitor_select_input_directory,
                ShowNewFolderButton = true
            };
            var dlgResult = fdDlg.ShowDialog();

            if (dlgResult != DialogResult.OK) return;
            // Check for state file, quit if not found.
            if (!File.Exists(fdDlg.SelectedPath + @"\_StateFile_.dat"))
            {
                MessageBox.Show(Resources.FrmCommentMonitort_Cannot_import
                    , Resources.FrmCommentMonitor_Import_Data_file_missing, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (statusOnly)
            {
                var df = new DirectoryInfo(fdDlg.SelectedPath);
                grdOutput.Rows.Clear();
                panel1.Enabled = false;
                grdOutput.RowsAdded -= grdOutput_RowsAdded;
                var fileNames = df.EnumerateFiles();
                foreach (var fName in fileNames)
                {
                    if (String.Compare(fName.Name.Split('.')[0], "_StateFile_", StringComparison.Ordinal) != 0)
                    {
                        // Build a comment object & load data from file
                        var newComment = new LiveLeakComment();
                        var allLines = File.ReadAllLines(fName.FullName);
                        string[] splitChars = { "--" };
                        foreach (var line in allLines)
                        {
                            try
                            {
                                newComment.Username = fName.Name.Split('.')[0];
                                newComment.CommentId = long.Parse(line.Split(splitChars, StringSplitOptions.None)[0]);
                                newComment.Comment = line.Split(splitChars, StringSplitOptions.None)[1];
                                newComment.HasErrors = false;
                                _ca = new LiveLeakCommentAnalyser();
                                newComment.BadWordMatchCount = _ca.ParseComment(newComment);
                                _cm = new CommentManager(true);
                                _cm.CreateGridRow(ref grdOutput, newComment);
                                Application.DoEvents();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(string.Format("Unable to import from this file {0}. {1}", fName.FullName, ex.Message));
                                _log.LogEvent(string.Format("Unable to inport from this file {0}. {1}", fName.FullName, ex.Message)
                                    , EventLogEntryType.Error);
                                break;
                            }
                        }
                    }
                }
                // Re-attach the event handler & sort the dataGrid.
                grdOutput.RowsAdded += grdOutput_RowsAdded;
                grdOutput.Sort(grdOutput.Columns[0], ListSortDirection.Ascending);
                panel1.Enabled = true;
            }

            _state.RestoreStateFromFile(fdDlg.SelectedPath + @"\_StateFile_.dat");
            _state.StateChanged += (o, e) => btnExport.Enabled = _state.Changed;
            RestoreInterfaceFromState();
            
            MessageBox.Show((statusOnly) ? grdOutput.Rows.Count + Resources.Data_rows_imported 
                : Resources.Status_imported
                , Resources.Import_complete
                , MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private static string SanitizeFileName(string name)
        {
            var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
            var invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);
            return Regex.Replace(name, invalidRegStr, "_");
        }
        #endregion
    }
}
