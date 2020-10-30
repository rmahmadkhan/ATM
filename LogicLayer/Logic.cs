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

                // Updating Username
                string un = getUsername();
                if (!string.IsNullOrEmpty(un))
                {
                    account.Username = un;
                }
                // Updates Pin
                string pin = getPin();
                if (string.IsNullOrEmpty(pin))
                {
                    account.Pin = pin;
                }
                // Updating the Holder's Name
                string name = getName();
                if (name != null)
                {
                    account.Name = name;
                }
                // Gets Status of Account (Active/Disabled)
                string status = getStatus();
                if (status != null)
                {
                    account.Status = status;
                }

                //Updates data in file
                data.UpdateInFile(account);
                Console.WriteLine($"Account # {account.AccountNo} has been successfully been updated.");
            }
            else
            {
                Console.WriteLine($"Account number {accNo} does not exist!");
                return;
            }
        }

        // Method to search account information
        public void SearchAccount()
        {
            // Creating a customer object to store values
            Customer customer = new Customer();

            Console.WriteLine("---SEARCH MENU---\n");
            Console.WriteLine("Please enter in the fields you wish to include in search (leave blank otherwise):\n");

            // Getting Account Number and applying checks
            string accNum = string.Empty;
        getAccountNumber:
            {
                Console.Write("Account Number: ");
                accNum = Console.ReadLine();
                if (!string.IsNullOrEmpty(accNum))
                {
                    try
                    {
                        customer.AccountNo = Convert.ToInt32(accNum);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid Input! Enter a number.");
                        goto getAccountNumber;
                    }

                }
            }

            // Has valid or empty value
            string uname = getUsername();
            customer.Username = uname;

            // Has valid or empty value
            string name = getName();
            customer.Name = name;

            // Has valid or empty value
            string type = getAccountType();
            customer.AccountType = type;

            // Getting Balance and applying checks
            // Has valid or empty value
            string balance = string.Empty;
        getBalance:
            {
                Console.Write("Balance: ");
                balance = Console.ReadLine();
                if (!string.IsNullOrEmpty(balance))
                {
                    try
                    {
                        customer.Balance = Convert.ToInt32(balance);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Invalid Input! Enter a number.");
                        goto getBalance;
                    }

                }
            }

            // Has valid or empty value
            string status = getStatus();
            customer.Status = status;

            // Getting account list from file
            Data data = new Data();
            List<Customer> list = data.ReadFile<Customer>("customers.txt");

            List<Customer> outList = new List<Customer>();
            if (list.Count > 0)
            {
                foreach (Customer c in list)
                {
                    // Changing values if any is empty
                    if (string.IsNullOrEmpty(accNum))
                    {
                        customer.AccountNo = c.AccountNo;
                    }
                    if (string.IsNullOrEmpty(uname))
                    {
                        customer.Username = c.Username;
                    }
                    if (string.IsNullOrEmpty(name))
                    {
                        customer.Name = c.Name;
                    }
                    if (string.IsNullOrEmpty(type))
                    {
                        customer.AccountType = c.AccountType;
                    }
                    if (string.IsNullOrEmpty(balance))
                    {
                        customer.Balance = c.Balance;
                    }
                    if (string.IsNullOrEmpty(status))
                    {
                        customer.Status = c.Status;
                    }
                    
                    // if given object matches the object in the list, adding it to the list
                    if (customer.AccountNo == c.AccountNo &&
                        customer.Username == c.Username &&
                        customer.Name == c.Name &&
                        customer.AccountType == c.AccountType &&
                        customer.Balance == c.Balance &&
                        customer.Status == c.Status)
                    {
                        outList.Add(c);
                    }
                }

                // Printing the result
                Console.WriteLine("\n==== SEARCH RESULTS ====\n");
                if (outList.Count > 0)
                {
                    Console.WriteLine("Account No".PadRight(12)
                                + "Username".PadRight(10)
                                + "Holder's Name".PadRight(15)
                                + "Type".PadRight(9)
                                + "Balance".PadRight(10)
                                + "Status\n");
                    foreach (Customer c1 in outList)
                    {
                        Console.WriteLine(Convert.ToString(c1.AccountNo).PadRight(12)
                            + c1.Username.PadRight(10)
                            + c1.Name.PadRight(15)
                            + c1.AccountType.PadRight(9)
                            + Convert.ToString(c1.Balance).PadRight(10)
                            + c1.Status);
                    }
                }
                else
                {
                    Console.WriteLine("***NO DATA FOUND MATCHING WITH GIVEN DETAILS***");
                }
            }
            else
            {
                Console.WriteLine("No account in the file.");
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

        // Method to get account number from user through console
        // Used in DeleteAccount() & UpdateAccount()
        public int getAccNum()
        {
            int accNo = 0;
        getAccNo:
            {
                Console.Write("Account number: ");
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


        // Returns valid Username or null
        // Method to be used in UpdateAccount() & SearchAccount()
        public string getUsername()
        {
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
                // if username is valid or empty
                else
                {
                    return un;
                }
            }
        }

        // Returns valid Username or null
        // Method to be used in UpdateAccount()
        public string getPin()
        {
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
                    return pin;
                }
                return null;
            }
        }

        // Returns valid Holder's Name or null
        // Method to be used in UpdateAccount() & SearchAccount()
        public string getName()
        {
            Console.Write("Holder's Name: ");
            string name = Console.ReadLine();
            if (!string.IsNullOrEmpty(name) || !string.IsNullOrWhiteSpace(name))
            {
                return name;
            }
            return null;
        }

        // Returns valid Status or null
        // Method to be used in UpdateAccount() & SearchAccount()
        public string getStatus()
        {
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
                    return status;
                }
                return null;
            }
        }

        // Returns valid Status or null
        // Method to be used in SearchAccount()
        public string getAccountType()
        {
        getAccountType:
            {
                Console.Write("Account Type (Savings/Current): ");
                string type = Console.ReadLine();

                // Checks if Account type is valid
                if (!string.IsNullOrEmpty(type) && !(type == "Savings" || type == "Current"))
                {
                    Console.WriteLine("Wrong Input. Enter \"Savings\" & \"Current\"");
                    goto getAccountType;
                }
                return type;
            }
        }



    }
}
