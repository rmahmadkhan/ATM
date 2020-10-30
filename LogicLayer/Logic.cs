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
                string un = Console.ReadLine();

                // Checks if Username is valid or not (Username can only contain A-Z, a-z & 0-9)
                if (un == "" || !isValidUsername(un))
                {
                    Console.WriteLine("Enter valid Username (Username can only contain A-Z, a-z & 0-9)");
                    goto getUsername;
                }
                customer.Username = un;
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
            string name = Console.ReadLine();
            if (name != "" || name != " ")
            {
                customer.Name = name;
            }

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

        // Method to get account number from user through console
        public int getAccNum()
        {
            int accNo = 0;
        getAccNo:
            {
                Console.Write("Enter the account number: ");
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
            return accNo;
        }

        // Deletes an account from file
        public void DeleteAccount()
        {
            // Get the account number from user through console
            int accNo = getAccNum();

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
                Console.WriteLine($"Account number {accNo} does not exist!");
            }
        }

        // prints data of an account
        public void PrintAccontDetails(Customer account)
        {
            Console.WriteLine($"Account # {account.AccountNo}\n" +
                $"Type: {account.AccountType}\n" +
                $"Holder: {account.Name}\n" + "" +
                $"Balance: {account.Balance}\n" +
                $"Status: {account.Status}");
        }

        // Update any account details
        public void UpdateAccount()
        {
            // Gets the account number from user through console
            int accNo = getAccNum();

            Data data = new Data();
            Customer account = new Customer();
            if (data.isInFile(accNo, out account))
            {
                // printing account details 
                PrintAccontDetails(account);
                Console.WriteLine("\nPlease enter in the fields you wish to update (leave blank otherwise):\n");

            getUsername:
                {
                    Console.Write("Username: ");
                    string un = Console.ReadLine();

                    // Checks if Username is valid or not (Username can only contain A-Z, a-z & 0-9)
                    if (!isValidUsername(un))
                    {
                        Console.WriteLine("Enter valid Username (Username can only contain A-Z, a-z & 0-9)");
                        goto getUsername;
                    }
                    // if username is valid, changes its value
                    else if (!string.IsNullOrEmpty(un) && isValidUsername(un))
                    {
                        account.Username = un;
                    }
                }

            getPin:
                {
                    Console.Write("5-digit Pin: ");
                    string pin = Console.ReadLine();

                    // Checks if Pin is valid or not (Pin is 5-digit & can only contain 0-9)
                    if (!string.IsNullOrEmpty(pin) && !isValidPin(pin))
                    {
                        Console.WriteLine("Enter valid Pin (Pin is 5-digit & can only contain 0-9)");
                        goto getPin;
                    }
                    // if pin is valid, changes its value else do nothing
                    else if (isValidPin(pin))
                    {
                        account.Pin = pin;
                    }
                }
            // Updating the Holder's Name
                Console.Write("Holder's Name: ");
                string name = Console.ReadLine();
                if(!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(name))
                {
                    account.Name = name;
                }
            // Gets Status of Account (Active/Disabled)
            getStatus:
                {
                    Console.Write("Status (Active/Disabled): ");
                    string status = Console.ReadLine();

                    // Checks if Status is valid
                    if (status != "" && !(status == "Active" || status == "Disabled"))
                    {
                        Console.WriteLine("Wrong Input. Enter \"Active\" & \"Disabled\"");
                        goto getStatus;
                    }
                    // changing status if entered valid
                    else if (status == "Active" || status == "Disabled")
                    {
                        account.Status = status;
                    }
                }

                data.UpdateInFile(account);
                Console.WriteLine($"Account # {account.AccountNo} has been successfully been updated.");
            }
            else
            {
                Console.WriteLine($"Account number {accNo} does not exist!");
                return;
            }
        }
    }
}
