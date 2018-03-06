using LicenseSnifferLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PackagesCollector
{
    class Program
    {
        static void Main(string[] args)
        {
            string repositoryFile = args[0];
            if (!File.Exists(repositoryFile))
            {
                return;
            }
            var basePath = new DirectoryInfo(repositoryFile).Parent.Parent;

            HashSet<PackageModel> set = new HashSet<PackageModel>();

            var repositories = LoadXmlNodeList(repositoryFile, "repository");
            BuildHashSet(set, repositories, basePath);
            OutputPackageList(set, args[1]);
        }

        static void BuildHashSet(HashSet<PackageModel> set, XmlNodeList repositories, DirectoryInfo basePath)
        {
            foreach (XmlNode item in repositories)
            {
                var signleNugetFilePath = item.Attributes["path"].Value.Replace("..", basePath.FullName);
                if (!File.Exists(signleNugetFilePath))
                {
                    continue;
                }

                var packages = LoadXmlNodeList(signleNugetFilePath, "package");
                foreach (XmlNode package in packages)
                {
                    set.Add(new PackageModel
                    {
                        PackageId = package.Attributes["id"].Value,
                        PackageVersion = package.Attributes["version"].Value
                    });
                }
            }
        }

        static XmlNodeList LoadXmlNodeList(string xmlFile, string tagName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFile);

            return doc.GetElementsByTagName(tagName);
        }

        static void OutputPackageList(HashSet<PackageModel> set, string path)
        {
            StreamWriter sw = new StreamWriter(path, false);
            foreach (PackageModel m in set)
            {
                sw.WriteLine(m.ToString());
            }
            sw.Flush();
            sw.Close();
        }
    }
}
