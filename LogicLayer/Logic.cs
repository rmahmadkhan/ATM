using BOLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Logic
    {
        // Method to verify login of Admin
        public bool VerifyLogin(Admin admin)
        {
            Data adminData = new Data();
            return adminData.isInFile(admin);
        }

        // Method to verify login of customer
        public bool VerifyLogin(Customer customer)
        {
            Data customerData = new Data();
            return customerData.isInFile(customer);
        }

        // Method to check if Username is valid or not (Username can only contain A-Z, a-z & 0-9)
        public bool isValidUsername(string s)
        {
            foreach (char c in s)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        // Method to check if Pin is valid or not (Pin is 5-digit & can only contain 0-9)
        public bool isValidPin(string s)
        {
            if (s.Length != 5)
            {
                return false;
            }
            foreach (char c in s)
            {
                if (c >= '0' && c <= '9')
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        // Method to Create Account of a Customer
        public void CreateAccount()
        {
            Customer customer = new Customer();
            Console.WriteLine("---Creating New Account---\n" +
                "Enter User Details");
        getUsername:
            {
                Console.Write("Username: ");
                customer.Username = Console.ReadLine();

                // Checks if Username is valid or not (Username can only contain A-Z, a-z & 0-9)
                if (!isValidUsername(customer.Username))
                {
                    Console.WriteLine("Enter valid Username (Username can only contain A-Z, a-z & 0-9)");
                    goto getUsername;
                }
            }

        getPin:
            {
                Console.Write("5-digit Pin: ");
                customer.Pin = Console.ReadLine();

                // Checks if Pin is valid or not (Pin is 5-digit & can only contain 0-9)
                if (!isValidPin(customer.Pin))
                {
                    Console.WriteLine("Enter valid Pin (Pin is 5-digit & can only contain 0-9)");
                    goto getPin;
                }
            }
            // Gets Holders Name
            Console.Write("Holder's Name: ");
            customer.Name = Console.ReadLine();

        getAccountType:
            {
                Console.Write("Account Type (Savings/Current): ");
                customer.AccountType = Console.ReadLine();

                // Checks if Account type is valid
                if (!(customer.AccountType == "Savings" || customer.AccountType == "Current"))
                {
                    Console.WriteLine("Wrong Input. Enter \"Savings\" & \"Current\"");
                    goto getAccountType;
                }
            }
        // Gets Starting Balance
        getBalance:
            {
                try
                {
                    Console.Write("Starting Balance: ");
                    customer.Balance = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Wrong Input. Enter numbers only.");
                    goto getBalance;
                }
            }
        // Gets Status of Account (Active/Disabled)
        getStatus:
            {
                Console.Write("Status (Active/Disabled): ");
                customer.Status = Console.ReadLine();

                // Checks if Status is valid
                if (!(customer.Status == "Active" || customer.Status == "Disabled"))
                {
                    Console.WriteLine("Wrong Input. Enter \"Active\" & \"Disabled\"");
                    goto getStatus;
                }
            }
            Data data = new Data();

            // Assiging Account Number
            customer.AccountNo = data.getLastAccountNumber() + 1;

            // Appending Customer to file
            data.AddToFile(customer);

            Console.WriteLine($"Account Successfully Created – the account number assigned is: {customer.AccountNo}");
        }

        // Deletes an account from file
        public void DeleteAccount()
        {
            // Get the account number from user through console
            int accNo = 0;
        getAccNo:
            {
                Console.Write("Enter the account number which you want to delete: ");
                try
                {
                    accNo = Convert.ToInt32(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input! Please try again.");
                    goto getAccNo;
                }
            }


            Data data = new Data();
            Customer customer = new Customer();

            if (data.isInFile(accNo, out customer)) //Checks if the account number is in file
            {
                Console.Write($"You wish to delete the account held by Mr {customer.Name}.\n" +
                    "If this information is correct please re-enter the account number: ");
                try
                {
                    int tempAccNo = Convert.ToInt32(Console.ReadLine());
                    // if user enters the same account number
                    if (tempAccNo == accNo)
                    {
                        data.DeleteFromFile(customer);
                        Console.WriteLine("Account Deleted Successfully");
                        return;
                    }
                    // if user enters different account number
                    else
                    {
                        Console.WriteLine("No Account was deleted!");
                        return;
                    }
                }
                // if user does not enter a number
                catch (Exception)
                {
                    Console.WriteLine("No Account was deleted!");
                }
            }
            else
            {
                Console.WriteLine($"Account number {accNo} not found!");
            }
        }
    }
}
