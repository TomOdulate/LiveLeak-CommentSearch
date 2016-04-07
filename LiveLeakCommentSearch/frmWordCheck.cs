using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveLeakCommentSearch;
using System.IO;
using LiveLeakCommentSearch.Properties;

namespace LiveLeakCommentSearch
{
    public partial class frmWordCheck : Form
    {
        private LiveLeakCommentAnalyser lca;
        
        public frmWordCheck()
        {
            InitializeComponent();
            lca = new LiveLeakCommentAnalyser();

            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            imgTick.Image = new Bitmap(myAssembly.GetManifestResourceStream("LiveLeakCommentSearch.Tick.jpg"));
            imgCross.Image = new Bitmap(myAssembly.GetManifestResourceStream("LiveLeakCommentSearch.Cross.jpg"));
            imgCross.Visible = imgTick.Visible = label1.Visible = false;        
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (txtTargetWord.Text.Length == 0) return;

            var c = new LiveLeakComment();
            c.Comment = txtTargetWord.Text.Trim();
            var result = lca.ParseComment(c);

            if (result != 0)
                Match();
            else
                NoMatch(c.Comment);

            txtTargetWord.Focus();
            txtTargetWord.SelectAll();
        }

        private void Match()
        {
            imgTick.Visible = label1.Visible = true;            
            ShowResult(true);
        }

        private void NoMatch(string word)
        {
            imgCross.Visible = label1.Visible = true;
            ShowResult(false);
            DialogResult result = MessageBox.Show( "'" + word 
                + "' was not matched.  Would you like to add it to the word list?"
                ,"Add to wordlist?"
                ,MessageBoxButtons.YesNo);

            if (result != DialogResult.Yes) return;

            File.AppendAllText(Application.StartupPath + Resources.BadWordFile
                , string.Format("\r\n\\b{0}\\b", word));

            label1.Text = string.Format("Added '{0}'", word.TrimStart());

            lca = new LiveLeakCommentAnalyser();    // Force a reload of the wordlist            
        }

        private void txtTargetWord_TextChanged(object sender, EventArgs e)
        {
            label1.Visible = false;
            imgTick.Visible = imgCross.Visible = false;            
        }

        private void ShowResult(bool found)
        {            
            label1.Text = found ? "Match found":"No Match found!";
            if (found)
            {
                imgTick.Visible = true;
                imgCross.Visible = false;
            }
            else
            {
                imgTick.Visible = false;
                imgCross.Visible = true;
            }
        }

        private void txtTargetWord_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                btnCheck_Click(this, null);
            }
        }
    }
}
