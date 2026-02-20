using System;
using System.Collections.Generic;
using System.IO;

namespace ReadingOGEData
{
    class Program
    {
        static void Main(string[] args)
        {
            List<OGERecord> data = Read();
            Console.WriteLine("Total records: " + data.Count);
        }

        public static List<OGERecord> Read()
        {
            string filePath = "ogedata.csv";
            List<OGERecord> records = new List<OGERecord>();

            using StreamReader reader = new StreamReader(filePath);

            reader.ReadLine(); // Skip header row

            string line;

            while ((line = reader.ReadLine()) != null)
            {
                string[] words = line.Split(',');

                // ⚠️ Adjust number of columns if needed
                OGERecord record = new OGERecord(
                    words[0],
                    words[1],
                    words[2],
                    words[3],
                    words[4]
                );

                records.Add(record);
            }

            return records;
        }
    }

    public struct OGERecord
    {
        public OGERecord(string col1, string col2, string col3, string col4, string col5)
        {
            Column1 = col1;
            Column2 = col2;
            Column3 = col3;
            Column4 = col4;
            Column5 = col5;
        }

        public string Column1 { get; }
        public string Column2 { get; }
        public string Column3 { get; }
        public string Column4 { get; }
        public string Column5 { get; }
    }
}