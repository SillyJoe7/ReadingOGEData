using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReadingOGEData
{
    class Program
    {
        static void Main(string[] args)
        {
            List<OGERecord> data = Read();
            Console.WriteLine("Total records: " + data.Count);

            List<OGERecord> inactiveRecords = data.Where(r => r.CloudLifecycleState == "inactive").ToList();
            Console.WriteLine("Number of inactive records:" + inactiveRecords.Count);

            List<string> inactiveNames = inactiveRecords.Select(r => r.DisplayName).Distinct().OrderBy(name => name).ToList();
            Console.WriteLine("Inactive Users:");
            foreach (var name in inactiveNames)
            {
                Console.WriteLine(name);
            }
        }

        public static List<OGERecord> Read()
        {
            string path = "ogedata.csv";
            List<OGERecord> records = new List<OGERecord>();

            using StreamReader s = new StreamReader(path);

            //This skips the header cause im #awesome
            s.ReadLine();
            

            string line;

            while ((line = s.ReadLine()) != null)
            {
                string[] parts = line.Split(',');

                OGERecord record = new OGERecord
                {
                    DisplayName = parts[0],
                    FirstName = parts[1],
                    LastName = parts[2],
                    WorkEmail = parts[3],
                    CloudLifecycleState = parts[4],
                    IdentityID = parts[5],
                    IsManager = parts[6],
                    Department = parts[7],
                    JobTitle = parts[8],
                    Uid = parts[9],
                    AccessType = parts[10],
                    AccessSourceName = parts[11],
                    AccessDisplayName = parts[12],
                    AccessDescription = parts[13]
                };

                records.Add(record);
            }

            return records;
        }
    }

    public struct OGERecord
    {
        public string DisplayName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string WorkEmail { get; set; }
        public string CloudLifecycleState { get; set; }
        public string IdentityID { get; set; }
        public string IsManager { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string Uid { get; set; }
        public string AccessType { get; set; }
        public string AccessSourceName { get; set; }
        public string AccessDisplayName { get; set; }
        public string AccessDescription { get; set; }
    }
}
