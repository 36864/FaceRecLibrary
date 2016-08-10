using System;

namespace FaceRecLibrary.Utilities
{
    public class IdentityInfo
    {
        public string Name { get; set; }

        public int ?_ID { get; set; }

        public int Label { get; set; }

        public bool Equals(IdentityInfo identity)
        {
            if (identity.Label == -1 && this.Label == -1)
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