namespace FaceRecLibrary.Types
{
    public class IdentityInfo
    {
        public IdentityInfo(string name)
        {
            Name = name;
        }

        public IdentityInfo()
        {

        }

        public string Name { get; set; }

        public string _ID { get; set; }

        public int ?Label { get; set; }

        public bool Equals(IdentityInfo identity)
        {
            if (this._ID != null && this._ID == identity._ID)
                return true;
            if (identity.Label == null && this.Label == null)
                if (identity.Name?.Equals(this.Name) != null)
                    return true;
                else
                    return false;
            if (identity.Label == this.Label) {
                if (this.Name == null)
                    this.Name = identity.Name;
                if (this._ID == null)
                    this._ID = identity._ID;
                return true;
            }
            return false;
        }
    }
}