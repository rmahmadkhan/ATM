using BOLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace DataLayer
{
    public class Data
    {
        //Save an Admin object to File in Json format
        public void AddToFile(Admin admin)
        {
            string jsonOutput = JsonSerializer.Serialize(admin);
            File.AppendAllText("admins.txt", jsonOutput + Environment.NewLine);
        }

        // Save a Customer object to file in Json format
        public void AddToFile(Customer customer)
        {
            string jsonOutput = JsonSerializer.Serialize(customer);
            File.AppendAllText("customers.txt", jsonOutput + Environment.NewLine);
        }

        // Returns a list of objects from file
        public List<T> ReadFile<T>(string FileName)
        {
            List<T> list = new List<T>();
            string FilePath = Path.Combine(Environment.CurrentDirectory, FileName);
            StreamReader sr = new StreamReader(FilePath);

            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                list.Add(JsonSerializer.Deserialize<T>(line));
            }
            sr.Close();

            return list;
        }

        // Checks if an Admin object in File
        public bool isInFile(Admin user)
        {
            List<Admin> list = ReadFile<Admin>("admins.txt");
            foreach (Admin admin in list)
            {
                if (admin.Username == user.Username && admin.Pin == user.Pin)
                {
                    return true;
                }
            }
            return false;
        }

        // Checks if an Customer object in File
        public bool isInFile(Customer user)
        {
            List<Customer> list = ReadFile<Customer>("customers.txt");
            foreach (Customer customer in list)
            {
                if (customer.Username == user.Username && customer.Pin == user.Pin)
                {
                    return true;
                }
            }
            return false;
        }

        // Method to get the last account number
        public int getLastAccountNumber()
        {
            List<Customer> list = ReadFile<Customer>("customers.txt");
            if(list.Count > 0)
            {
                Customer customer = list[list.Count - 1];
                return customer.AccountNo;
            }
            return 0;
        }
    }
}
