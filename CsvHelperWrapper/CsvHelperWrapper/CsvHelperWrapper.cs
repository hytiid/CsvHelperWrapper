using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace CsvHelper
{
    public static class CsvHelperWrapper
    {
        public static CsvReader GenerateCsvReaderFromCsvContent(string zipPath, string csvPath, Encoding encoding)
        {
            using (ZipFile zip = new ZipFile(zipPath))
                return GenerateCsvReaderFromCsvContent(zip, csvPath, encoding);
        }
        public static CsvReader GenerateCsvReaderFromCsvContent(ZipFile zip, string csvPath, Encoding encoding)
        {
            string content = ZipReader.ReadContent(zip, csvPath);
            return GenerateCsvReaderFromCsvContent(content, encoding);
        }
        public static CsvReader GenerateCsvReaderFromCsvContent(string csvContent, Encoding encoding)
        {
            var ms = new MemoryStream(encoding.GetBytes(csvContent));
            var sr = new StreamReader(ms);
            return GenerateCsvReaderFromStream(sr);
        }
        public static CsvReader GenerateCsvReaderFromStream(StreamReader sr)
        {
            return new CsvReader(sr, System.Globalization.CultureInfo.InvariantCulture);
        }

        public static TData ReadContent<TData>(CsvReader csvReader, Func<CsvReader, TData> getRecord)
        {
            return ReadContents(csvReader, getRecord).First();
        }
        public static IEnumerable<TData> ReadContents<TData>(CsvReader csvReader, Func<CsvReader, TData> getRecord)
        {
            //read header
            csvReader.Read();
            csvReader.ReadHeader();
            //read contents
            var ret = new List<TData>();
            while (csvReader.Read())
                ret.Add(getRecord(csvReader));

            return ret;
        }
    }
}
