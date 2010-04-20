using System;
using OpenMetaverse;

namespace OxViewer.LibOMV
{
    [Serializable]
    public class AssetBase
    {
        private byte[] m_data;
        private AssetMetadata m_metadata;

        public AssetBase()
        {
            m_metadata = new AssetMetadata();
        }

        public AssetBase(UUID assetID, string name)
        {
            m_metadata = new AssetMetadata();
            m_metadata.FullID = assetID;
            m_metadata.Name = name;
        }

        public virtual byte[] Data
        {
            get { return m_data; }
            set { m_data = value; }
        }

        public UUID FullID
        {
            get { return m_metadata.FullID; }
            set { m_metadata.FullID = value; }
        }

        public string ID
        {
            get { return m_metadata.ID; }
            set { m_metadata.ID = value; }
        }

        public string Name
        {
            get { return m_metadata.Name; }
            set { m_metadata.Name = value; }
        }

        public string Description
        {
            get { return m_metadata.Description; }
            set { m_metadata.Description = value; }
        }

        public sbyte Type
        {
            get { return m_metadata.Type; }
            set { m_metadata.Type = value; }
        }

        public bool Local
        {
            get { return m_metadata.Local; }
            set { m_metadata.Local = value; }
        }

        public bool Temporary
        {
            get { return m_metadata.Temporary; }
            set { m_metadata.Temporary = value; }
        }

        // We have methods here because properties are serialized, and we don't
        // want that.
        public virtual AssetMetadata getMetadata()
        {
            return m_metadata;
        }

        public virtual void setMetadata(AssetMetadata metadata)
        {
            m_metadata = metadata;
        }
    }

    public class AssetMetadata
    {
        private UUID m_fullid;
        private string m_name = String.Empty;
        private string m_description = String.Empty;
        private DateTime m_creation_date;
        private sbyte m_type;
        private string m_content_type;
        private byte[] m_sha1;
        private bool m_local = false;
        private bool m_temporary = false;
        //private Dictionary<string, Uri> m_methods = new Dictionary<string, Uri>();
        //private OSDMap m_extra_data;

        public UUID FullID
        {
            get { return m_fullid; }
            set { m_fullid = value; }
        }

        public string ID
        {
            get { return m_fullid.ToString(); }
            set { m_fullid = new UUID(value); }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Description
        {
            get { return m_description; }
            set { m_description = value; }
        }

        public DateTime CreationDate
        {
            get { return m_creation_date; }
            set { m_creation_date = value; }
        }

        public sbyte Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        public string ContentType
        {
            get { return m_content_type; }
            set { m_content_type = value; }
        }

        public byte[] SHA1
        {
            get { return m_sha1; }
            set { m_sha1 = value; }
        }

        public bool Local
        {
            get { return m_local; }
            set { m_local = value; }
        }

        public bool Temporary
        {
            get { return m_temporary; }
            set { m_temporary = value; }
        }

        //public Dictionary<string, Uri> Methods
        //{
        //    get { return m_methods; }
        //    set { m_methods = value; }
        //}

        //public OSDMap ExtraData
        //{
        //    get { return m_extra_data; }
        //    set { m_extra_data = value; }
        //}
    }
}
