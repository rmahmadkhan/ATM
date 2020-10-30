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
        // Appends an object to File in Json format
        public void AddToFile<T>(T user)
        {
            string jsonOutput = JsonSerializer.Serialize(user);
            if (user is Admin)
            {
                File.AppendAllText("admins.txt", jsonOutput + Environment.NewLine);
            }
            else if(user is Customer)
            {
                File.AppendAllText("customers.txt", jsonOutput + Environment.NewLine);
            }
        }

        // Clears Last data & Save new List to file in Json format
        public void SaveToFile<T>(List<T> list)
        {
            // Overwrite the file with first object in the list
            string jsonOutput = JsonSerializer.Serialize(list[0]);
            if (list[0] is Admin)
            {
                File.WriteAllText("admins.txt", jsonOutput + Environment.NewLine);
            }
            else if(list[0] is Customer)
            {
                File.WriteAllText("customers.txt", jsonOutput + Environment.NewLine);
            }

            // Appends the other objects of list to the file
            for(int i=1;i<list.Count;i++)
            {
                AddToFile(list[i]);
            }
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

        // Deletes a customer object from file
        public void DeleteFromFile(Customer customer)
        {
            List<Customer> list = ReadFile<Customer>("customers.txt");
            // Checking and remove the required object from the list
            foreach(Customer item in list)
            {
                if(item.AccountNo == customer.AccountNo)
                {
                    list.Remove(item);
                    break;
                }
            }
            // Overwriting the list to file
            SaveToFile<Customer>(list);
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

        // Checks if a Customer object in File
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

        // Checks if an account number is in File
        public bool isInFile(int accNo, out Customer outCustomer)
        {
            List<Customer> list = ReadFile<Customer>("customers.txt");
            foreach (Customer customer in list)
            {
                if (customer.AccountNo == accNo)
                {
                    outCustomer = customer;
                    return true;
                }
            }
            outCustomer = null;
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
