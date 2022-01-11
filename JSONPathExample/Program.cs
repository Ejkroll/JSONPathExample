using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace JSONPathExample
{
    class Program
    {
        static void Main(string[] args)
        {
            GetPropertyNames();

            Filter();

            MergingRecords();

            Serialization();

            Console.ReadKey();
        }

        static void GetPropertyNames()
        {
            var data = GetData("Employees");
            var root = JToken.Parse(data);
            var obj = (JObject)root.SelectToken("Employees[0]");
            foreach (var prop in obj.Properties())
                Console.WriteLine($"{prop.Name}");
        }
        static void Filter()
        {
            var data = GetData("Employees");
            var root = JToken.Parse(data);
            var tokens = root.SelectTokens("Employees[?(@.Age >= 30)]");
            foreach (var token in tokens)
                Console.WriteLine($"{token}");
        }
        static void MergingRecords()
        {
            var data = GetData("Employees");
            var root = JToken.Parse(data);
            foreach (var emergencyContact in root.SelectTokens($"EmergencyContacts[*]"))
            {
                var eid = emergencyContact.Value<int>("EmployeeId");
                var token = root.SelectToken($"$.Employees[?(@.Id == {eid})]");
                    token["EmergencyContact"] = emergencyContact;

                Console.WriteLine($"{token}");
            }
        }
        static void Serialization()
        {
            var x = new
            {
                Id = 5,
                First = "Josh",
                Last = "Parker",
                Age = 45
            };

            Console.WriteLine($"{JToken.FromObject(x)}");
        }


        static string GetData(string fileName)
        {
            return File.ReadAllText($".//Examples//{fileName}.json");
        }
    }
}
