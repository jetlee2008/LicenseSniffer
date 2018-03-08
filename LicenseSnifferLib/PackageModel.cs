using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseSnifferLib
{
    public class PackageModel
    {
        public string PackageId { get; set; }

        public string PackageVersion { get; set; }

        public string LicenseInfo { get; set; }

        public string LicenseInfoLink { get; set; }

        public string Result { get; set; }

        public PackageModel(string packageInfo)
        {
            var infos = packageInfo.Split(':');
            PackageId = infos[0];
            PackageVersion = infos[1];
        }

        public override bool Equals(object obj)
        {
            var input = (PackageModel)obj;
            if (input.PackageId == this.PackageId && input.PackageVersion == this.PackageVersion)
            {
                return true;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return PackageId.GetHashCode() ^ PackageVersion.GetHashCode();
        }

        public override string ToString()
        {
            return $"{PackageId}:{PackageVersion}";
        }

    }
}
