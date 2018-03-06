using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseSnifferLib
{
    public class PackageModelEqualityComparer : IEqualityComparer<PackageModel>
    {
        public bool Equals(PackageModel x, PackageModel y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(PackageModel obj)
        {
            return obj.GetHashCode();
        }
    }
}
