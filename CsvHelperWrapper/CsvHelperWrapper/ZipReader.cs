using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace CsvHelper
{
    public static class ZipReader
    {
        public static string ReadContent(string zipPath, string regex)
        {
            return ReadContent(ReadContents(zipPath, regex), regex);
        }
        public static string ReadContent(ZipFile zip, string regex)
        {
            return ReadContent(ReadContents(zip, regex), regex);
        }

        public static IEnumerable<string> ReadContents(string zipPath, string regex)
        {
            using (ZipFile zip = new ZipFile(zipPath))
                return ReadContents(zip, regex);
        }
        public static IEnumerable<string> ReadContents(ZipFile zip, string regex)
        {
            Regex rgx = new Regex(regex);
            IEnumerable<ZipEntry> entries = zip.Entries.Where(entry => rgx.IsMatch(entry.FileName));
            return entries.Select(entry => ReadZipEntry(entry));
        }

        private static string ReadContent(IEnumerable<string> contents, string regex)
        {
            Require(contents != null,
                string.Format("{0} is not included.", regex));
            Require(contents.Count() == 1,
                string.Format("found {0} file(s) for regex={1}. This method expect to find one file.", contents.Count(), regex));
            return contents.First();
        }
        private static string ReadZipEntry(ZipEntry entry)
        {
            using (var sr = new StreamReader(entry.OpenReader()))
                return sr.ReadToEnd();
        }

        private static void Require(bool expected, string errorMessage)
        {
            if (!expected)
                throw new Exception(errorMessage);
        }
    }
}
