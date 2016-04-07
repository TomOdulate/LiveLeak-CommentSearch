using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveLeakCommentSearch.Properties;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

#pragma warning disable 1998
namespace LiveLeakCommentSearch
{
    public enum SearchType { BadWord, UserName, SingleComment, Unfiltered };
    public class LiveLeakComment
    {
        public String Comment { get; set; }
        public String Username { get; set; }
        public bool HasErrors { get; set; }
        public long CommentId { get; set; }
        public string TimeEstimate { get; set; }
        public int BadWordMatchCount;
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(CommentId);
            sb.Append(",");
            sb.Append(BadWordMatchCount);
            sb.Append(",");
            sb.Append(Username);
            sb.Append(",");
            sb.Append(Comment.Replace('\n', ' '));
            sb.AppendLine();
            return sb.ToString();
        }
    }
    class LiveLeakCommentAnalyser
    {
        public List<string> BadWordList { get; set; }
        public LiveLeakCommentAnalyser()
        {
            BadWordList = LoadWordList();
        }
        public static List<string> LoadWordList()
        {
            var words = new List<string>();
            var wordsFilePath = Application.StartupPath + Resources.BadWordFile;
            words.AddRange(File.ReadLines(wordsFilePath));
            words.RemoveAll(word => word.StartsWith("##"));
            return words;
        }
        public int ParseComment(LiveLeakComment comment)
        {
            if (BadWordList.Count == 0)
                throw new NoNullAllowedException("Bad word list is empty");

            comment.BadWordMatchCount = 0;
            foreach (var regex in BadWordList.Select(strRegex => new Regex(strRegex, RegexOptions.IgnoreCase)))
                comment.BadWordMatchCount += regex.Matches(comment.Comment).Count;

            return comment.BadWordMatchCount;
        }
        public bool CheckMatch(SearchType type, LiveLeakComment comment, IEnumerable<string> usernameList, bool useWordList)
        {
            bool bFoundSomething;
            switch (type)
            {
                case SearchType.UserName:
                    bFoundSomething = CompareUserName(usernameList, comment);
                    break;
                case SearchType.SingleComment:
                    bFoundSomething = comment.Comment != string.Empty;
                    break;
                default:
                    if (useWordList)
                        bFoundSomething = CompareMessage(comment);
                    else
                        bFoundSomething = comment.Comment != string.Empty;
                    break;
            }

            return bFoundSomething;
        }
        private static bool CompareUserName(IEnumerable<string> usernameList, LiveLeakComment comment)
        {
            return usernameList.Any(targetUsername => String.Compare(
                comment.Username.ToLower(), targetUsername.ToLower(),
                StringComparison.Ordinal) == 0);
        }
        private bool CompareMessage(LiveLeakComment comment)
        {
            return ParseComment(comment) > 0;
        }
    }
    class CommentManager
    {
        public bool BStopRunning = false;
        public bool BForwards { get; set; }
        private readonly string _commentUrl = Resources.CommentUrl;
        internal enum CurrentStatus { InitialisingForwards, InitialisingBackwards, GettingComments, Parsing }
        
        public CommentManager(bool forwards)
        {
            BForwards = forwards;
        }
        public async Task<string> GetCommentPage(long commentId)
        {
            using (var client = new WebClient())
            {
                var uri = new UriBuilder(_commentUrl + commentId);
                return await client.DownloadStringTaskAsync(uri.Uri);
            }
        }
        public async Task<LiveLeakComment> ParseCommentFromPage(string htmlPage, long commentId)
        {
            var commentObj = new LiveLeakComment();
            commentObj.CommentId = commentId;
            var html = new HtmlDocument();
            html.LoadHtml(htmlPage);
            var root = html.DocumentNode;

            try
            {    
                // Is there any comment text?
                if (root.Descendants().Any(n => n.GetAttributeValue("id", "").Equals("comment_text_" + commentId)))
                {
                    // Parse the comment text
                    commentObj.Comment = root.Descendants()
                        .First(n => n.GetAttributeValue("id", "").Equals("comment_text_" + commentId))
                        .InnerText.Trim();
                    commentObj.Comment = SanitizeString(commentObj.Comment);

                    // parse the username
                    var strUsername = root.Descendants()
                        .First(n => n.GetAttributeValue("class", "").Equals("user_popup_menu_link"))
                        .InnerText.Trim();
                    
                    var a = strUsername.Split('(');                    
                    commentObj.Username = SanitizeString(a[0]).Trim();

                    // parse the time of comment
                    string[] dd = { "Posted ", " By" };
                    try
                    {
                        commentObj.TimeEstimate = root.Descendants()
                            .First(n => n.GetAttributeValue("class", "").Equals("comment_right"))
                            .ChildNodes[3].InnerText
                            .Split(dd, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    catch (Exception)
                    {
                        commentObj.TimeEstimate = "Unknown";                        
                    }
                }
                else
                {
                    // no comment on this page!
                    commentObj.HasErrors = true;
                }
            }
            catch (Exception)
            {
                commentObj.Username = "UNKNOWN";
                commentObj.HasErrors = true;
                throw;
            }
            return commentObj;
        }
        public bool IsComment(string page)
        {
            return page.Contains("comment_text_");
        }
        public void CreateGridRow(ref DataGridView targetGrid, LiveLeakComment comment)
        {
            targetGrid.Rows.Add(comment.CommentId,
                comment.Username,
                comment.BadWordMatchCount,
                comment.Comment,
                _commentUrl + comment.CommentId);
        }
        protected string SanitizeString(string dirtyString)
        {
            dirtyString = dirtyString.Replace("&amp;", "&");
            dirtyString = dirtyString.Replace("&quot;", "\"");
            var cleanString = Regex.Replace(dirtyString, "[^ -~]", " ");
            return cleanString;
        }
    }
}
