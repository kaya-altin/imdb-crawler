using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImdbCrawler.Core
{
    public class Casts
    {
        #region Properties

        private long m_ID;
        private string m_Name;
        private string m_Character;
        private string m_Url;
        private string m_ImageUrl;
        private string m_CastID;
        private long m_ObjectID;
        private int m_CollectionType = 1;


        public long ID
        {
            get { return m_ID; }
            set { m_ID = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public string Character
        {
            get { return m_Character; }
            set { m_Character = value; }
        }

        public string Url
        {
            get { return m_Url; }
            set { m_Url = value; }
        }

        public string ImageUrl
        {
            get { return m_ImageUrl; }
            set { m_ImageUrl = value; }
        }

        public string CastID
        {
            get { return m_CastID; }
            set { m_CastID = value; }
        }

        public long ObjectID
        {
            get { return m_ObjectID; }
            set { m_ObjectID = value; }
        }

        public int CollectionType
        {
            get { return m_CollectionType; }
            set { m_CollectionType = value; }
        }

        #endregion

        public Casts() { }

        public Casts(string Name, string Character, string Url, string ImageUrl, string CastID, long ObjectID, int CollectionType)
        {
            this.Name = Name;
            this.Character = Character;
            this.Url = Url;
            this.ImageUrl = ImageUrl;
            this.CastID = CastID;
            this.ObjectID = ObjectID;
            this.CollectionType = CollectionType;
        }
    }

}
