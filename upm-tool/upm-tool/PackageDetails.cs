using System;

namespace upmtool {

    public class PackageDetails  
    {
        public string Name;

        public override string ToString() { return Name; }

        public override bool Equals(Object other)
        {
            // same name is same package
            return other != null && other is PackageDetails
                ? (other as PackageDetails).Name.Equals(Name)
                : false;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

    }
}