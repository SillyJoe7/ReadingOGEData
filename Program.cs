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

            List<OGERecord> inactiveRecords = data.Where(r => !r.CloudLifecycleState).ToList();

            List<string> inactiveNames = inactiveRecords.Select(r => r.DisplayName).Distinct().OrderBy(name => name).ToList();
            Console.WriteLine("Inactive Users:");
            foreach (var name in inactiveNames)
            {
                Console.WriteLine(name);
            }

            var groupedInactive = inactiveRecords.GroupBy(r => r.DisplayName);
            Console.WriteLine("Inactive Users and Their Access:");
            foreach (var group in groupedInactive)
            {
                Console.WriteLine($"User: {group.Key}");
                var accesses = group.Where(r => !string.IsNullOrEmpty(r.AccessType));
                foreach (var record in accesses)
                {
                    Console.Write($"      Source: {record.AccessSourceName}");
                    Console.WriteLine($"      Access: {record.AccessDisplayName}");
                }
            }

            var departments = data.Select(r => r.Department).Where(d => !string.IsNullOrEmpty(d)).Distinct().OrderBy(d => d).ToList();
            Console.WriteLine("Departments:");
            foreach (var dept in departments)
            {
                Console.WriteLine(dept);
            }

            var amtPerDept = data.Where(r => !string.IsNullOrEmpty(r.Department)).GroupBy(r => r.Department).Select(g => new { Department = g.Key, DeptCount = g.Select(r => r.DisplayName).Distinct().Count() }).OrderBy(d => d.Department);

            Console.WriteLine("Department Employee Counts:");
            foreach (var dept in amtPerDept)
            {
                Console.WriteLine($"{dept.Department} - {dept.DeptCount} employees");
            }

            var inactiveWithAccessPerDept = data.Where(r => !string.IsNullOrEmpty(r.Department)).GroupBy(r => r.Department).Select(g => new {Department = g.Key, InactiveWithAccessCount = g.Where(r => !r.CloudLifecycleState && !string.IsNullOrEmpty(r.AccessType)).GroupBy(r => r.DisplayName).Select(x => x.Key).Distinct().Count()}).OrderBy(x => x.Department);

            Console.WriteLine("Inactive Employees With Access Per Department:");
            foreach (var dept in inactiveWithAccessPerDept)
            {
                Console.WriteLine($"{dept.Department} - {dept.InactiveWithAccessCount} inactive employees with access");
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
                bool isManager;
                Boolean.TryParse(parts[6], out isManager);

                bool isActive = parts[4].Trim().ToLower() == "active";
                OGERecord record = new OGERecord
                {
                    DisplayName = parts[0],
                    FirstName = parts[1],
                    LastName = parts[2],
                    WorkEmail = parts[3],
                    CloudLifecycleState = isActive,
                    IdentityID = parts[5],
                    IsManager = isManager,
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
        public bool CloudLifecycleState { get; set; }
        public string IdentityID { get; set; }
        public bool IsManager { get; set; }
        public string Department { get; set; }
        public string JobTitle { get; set; }
        public string Uid { get; set; }
        public string AccessType { get; set; }
        public string AccessSourceName { get; set; }
        public string AccessDisplayName { get; set; }
        public string AccessDescription { get; set; }
    }
}
