using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ImdbCrawler.Core
{
    public class Movies
    {
        #region Properties

        public string URL { get; set; }



        private long m_MovieID;
        private string m_OrginalName;
        private string m_OtherName;
        private string m_Director;
        private string m_Writer;
        private string m_Genre;
        private string m_Year;
        private string m_UserRating;
        private string m_Votes;
        private string m_ImdbUserRating;
        private string m_ImdbVotes;
        private string m_TmdbUserRating;
        private string m_TmdbVotes;
        private string m_RunningTime;
        private string m_Country;
        private string m_Language;
        private string m_EnglishPlot;
        private string m_OtherPlot;
        private string m_Budget;
        private string m_ProductionCompany;
        private string m_ImdbNumber;
        private string m_TmdbNumber;
        private string m_ArchivesNumber;
        private string m_PersonalRating;
        private string m_Subtitle;
        private string m_Dubbing;
        private string m_UserColumn1;
        private string m_UserColumn2;
        private string m_UserColumn3;
        private string m_UserColumn4;
        private string m_UserColumn5;
        private string m_UserColumn6;
        private string m_RlsType;
        private string m_RlsGroup;
        private string m_Poster;
        private string m_Note;
        private bool m_Seen = false;
        private bool m_IsSyncWaiting = false;
        private int? m_ContentProvider;
        private Nullable<DateTime> m_InsertDate;
        private Nullable<DateTime> m_UpdateDate;

        public List<Casts> Casts { get; set; }

        public long MovieID
        {
            get { return m_MovieID; }
            set { m_MovieID = value; }
        }

        public string OrginalName
        {
            get { return m_OrginalName; }
            set { m_OrginalName = HttpUtility.HtmlDecode(value); }
        }

        public string OtherName
        {
            get { return m_OtherName; }
            set { m_OtherName = HttpUtility.HtmlDecode(value); }
        }

        public string Director
        {
            get { return m_Director; }
            set { m_Director = HttpUtility.HtmlDecode(value); }
        }

        public string Writer
        {
            get { return m_Writer; }
            set { m_Writer = HttpUtility.HtmlDecode(value); }
        }

        public string Genre
        {
            get { return m_Genre; }
            set { m_Genre = HttpUtility.HtmlDecode(value); }
        }

        public string Year
        {
            get { return m_Year; }
            set { m_Year = HttpUtility.HtmlDecode(value); }
        }

        public string UserRating
        {
            get { return m_UserRating; }
            set { m_UserRating = HttpUtility.HtmlDecode(value); }
        }

        public string Votes
        {
            get { return m_Votes; }
            set { m_Votes = value; }
        }

        public string ImdbUserRating
        {
            get { return m_ImdbUserRating; }
            set { m_ImdbUserRating = HttpUtility.HtmlDecode(value); }
        }

        public string ImdbVotes
        {
            get { return m_ImdbVotes; }
            set { m_ImdbVotes = value; }
        }

        public string TmdbUserRating
        {
            get { return m_TmdbUserRating; }
            set { m_TmdbUserRating = HttpUtility.HtmlDecode(value); }
        }

        public string TmdbVotes
        {
            get { return m_TmdbVotes; }
            set { m_TmdbVotes = value; }
        }

        public string RunningTime
        {
            get { return m_RunningTime; }
            set { m_RunningTime = HttpUtility.HtmlDecode(value); }
        }

        public string Country
        {
            get { return m_Country; }
            set { m_Country = HttpUtility.HtmlDecode(value); }
        }

        public string Language
        {
            get { return m_Language; }
            set { m_Language = HttpUtility.HtmlDecode(value); }
        }

        public string EnglishPlot
        {
            get { return m_EnglishPlot; }
            set { m_EnglishPlot = HttpUtility.HtmlDecode(value); }
        }

        public string OtherPlot
        {
            get { return m_OtherPlot; }
            set { m_OtherPlot = HttpUtility.HtmlDecode(value); }
        }

        public string Budget
        {
            get { return m_Budget; }
            set { m_Budget = HttpUtility.HtmlDecode(value); }
        }


        public string ProductionCompany
        {
            get { return m_ProductionCompany; }
            set { m_ProductionCompany = HttpUtility.HtmlDecode(value); }
        }

        public string ImdbNumber
        {
            get { return m_ImdbNumber; }
            set { m_ImdbNumber = value; }
        }

        public string TmdbNumber
        {
            get { return m_TmdbNumber; }
            set { m_TmdbNumber = value; }
        }

        public string ArchivesNumber
        {
            get { return m_ArchivesNumber; }
            set { m_ArchivesNumber = value; }
        }

        public string PersonalRating
        {
            get { return m_PersonalRating; }
            set { m_PersonalRating = value; }
        }

        public string Subtitle
        {
            get { return m_Subtitle; }
            set { m_Subtitle = value; }
        }

        public string Dubbing
        {
            get { return m_Dubbing; }
            set { m_Dubbing = value; }
        }

        public string UserColumn1
        {
            get { return m_UserColumn1; }
            set { m_UserColumn1 = value; }
        }

        public string UserColumn2
        {
            get { return m_UserColumn2; }
            set { m_UserColumn2 = value; }
        }

        public string UserColumn3
        {
            get { return m_UserColumn3; }
            set { m_UserColumn3 = value; }
        }

        public string UserColumn4
        {
            get { return m_UserColumn4; }
            set { m_UserColumn4 = value; }
        }

        public string UserColumn5
        {
            get { return m_UserColumn5; }
            set { m_UserColumn5 = value; }
        }

        public string UserColumn6
        {
            get { return m_UserColumn6; }
            set { m_UserColumn6 = value; }
        }

        public string RlsType
        {
            get { return m_RlsType; }
            set { m_RlsType = value; }
        }

        public string RlsGroup
        {
            get { return m_RlsGroup; }
            set { m_RlsGroup = value; }
        }

        public string Poster
        {
            get { return m_Poster; }
            set { m_Poster = value; }
        }

        public string Note
        {
            get { return m_Note; }
            set { m_Note = value; }
        }

        public bool Seen
        {
            get { return m_Seen; }
            set { m_Seen = value; }
        }


        public bool IsSyncWaiting
        {
            get { return m_IsSyncWaiting; }
            set { m_IsSyncWaiting = value; }
        }
        public int? ContentProvider
        {
            get { return m_ContentProvider; }
            set { m_ContentProvider = value; }
        }


        public Nullable<DateTime> InsertDate
        {
            get { return m_InsertDate; }
            set { m_InsertDate = value; }
        }

        public Nullable<DateTime> UpdateDate
        {
            get { return m_UpdateDate; }
            set { m_UpdateDate = value; }
        }


        #endregion
    }
}
