using HtmlAgilityPack;
using LicenseSnifferLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LicenseQuerier
{
    class Program
    {
        static void Main(string[] args)
        {
            var packages = LoadPackageInfo(args[0]);
            foreach (PackageModel package in packages)
            {
                QueryLicenseInfo(package);
            }

            OutputResult(packages, args[1]);
        }

        private static void OutputResult(List<PackageModel> packages, string writePath)
        {
            StreamWriter sw = new StreamWriter(writePath);
            foreach (PackageModel package in packages)
            {
                sw.WriteLine($"{package.PackageId}\t{package.PackageVersion}\t{package.LicenseInfo}\t{package.LicenseInfoLink}\t{package.Result}");
            }
            sw.Flush();
            sw.Close();
        }

        private static List<PackageModel> LoadPackageInfo(string path)
        {
            StreamReader sr = new StreamReader(path);
            var package = new List<PackageModel>();
            string line = string.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                package.Add(new PackageModel(line));
            }
            return package;
        }

        private static void QueryLicenseInfo(PackageModel package)
        {
            try
            {
                string url = $"https://www.nuget.org/packages/{package.PackageId}/{package.PackageVersion}";
                WebClient client = new WebClient();
                var html = client.DownloadString(url);
                ParseHtmlRetrievingLicenseInfo(html, package);
            }
            catch (Exception e)
            {
                package.Result = e.Message;
            }
        }

        private static string ParseHtmlRetrievingLicenseInfo(string html, PackageModel package)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            HtmlNode licenseNode = null;
            var licenseNode2 = doc.DocumentNode.SelectNodes("//aside/ul/li")[2].ChildNodes[3];
            var licenseNode1 = doc.DocumentNode.SelectNodes("//aside/ul/li")[1].ChildNodes[3];
            if (licenseNode2.InnerText.Contains("License"))
            {
                licenseNode = licenseNode2;
            }
            if (licenseNode1.InnerText.Contains("License"))
            {
                licenseNode = licenseNode1;
            }

            package.LicenseInfo = licenseNode.InnerText.Replace("\r\n", "").Trim();
            package.LicenseInfoLink = licenseNode.Attributes["href"].Value;

            return null;
        }
    }
}
