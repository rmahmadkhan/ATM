using BOLayer;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
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

        // Method to verify, is username in the file
        public int isUserActive(string user)
        {
            Data data = new Data();
            return data.isUserActive(user);
        }

        // Method to verify login of customer
        public bool VerifyLogin(Customer customer)
        {
            Data customerData = new Data();
            return customerData.canLogin(customer);
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

        // Encryption Method
        // For alphabets we swap A with Z, B with Y and so on.
        // A B C D E F G H I J K L M N O P Q R S T U V W X Y Z
        // Z Y X W V U T S R Q P O N M L K J I H G F E D C B A
        // For Number we have
        // 0123456789
        // 9876543210
        public string EncryptionDecryption(string username)
        {
            string output = "";
            foreach (char c in username)
            {
                if (c >= 'A' && c <= 'Z')
                {
                    output += Convert.ToChar(('Z' - (c - 'A')));
                }
                else if (c >= 'a' && c <= 'z')
                {
                    output += Convert.ToChar(('z' - (c - 'a')));
                }
                else if (c >= '0' && c <= '9')
                {
                    output += (9 - char.GetNumericValue(c));
                }
            }
            return output;
        }

        // Disables an account
        public void DisableAccount(string username)
        {
            Data data = new Data();
            Customer customer = data.GetCustomer(username);
            // updating the status
            customer.Status = "Disabled";
            // saving back to file
            data.UpdateInFile(customer);
        }

        // Method to Create Account of a Customer
        public void CreateAccount()
        {
            Data data = new Data();
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

                // Doing encryption
                customer.Username = EncryptionDecryption(un);
                // If username is already assigned to someone
                if (data.isInFile(customer.Username))
                {
                    Console.WriteLine("Username already exists!! Enter again.");
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
                // Doing Encryption
                customer.Pin = EncryptionDecryption(customer.Pin);
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

            getUsername:
                {
                    // Updating Username
                    string un = getUsername();
                    if (!string.IsNullOrEmpty(un))
                    {
                        account.Username = EncryptionDecryption(un);
                        if (data.isInFile(account.Username))
                        {
                            Console.WriteLine("Username already exists!! Enter again.");
                            goto getUsername;
                        }
                    }
                }
                // Updates Pin
                string pin = getPin();
                if (!string.IsNullOrEmpty(pin))
                {
                    // doing encryption
                    account.Pin = EncryptionDecryption(pin);
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
            customer.Username = EncryptionDecryption(uname);

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
                            + EncryptionDecryption(c1.Username).PadRight(10)
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

        // Method to print reports
        public void ViewReports()
        {
        getOption:
            {
                Console.WriteLine("----- VIEW REPORTS -----\n");
                Console.WriteLine("1---Accounts By Amount\n" +
                    "2---Accounts By Date");
                // Reading option
                string option = Console.ReadLine();
                Data data = new Data();
                if (option == "1")
                {
                    Console.WriteLine("Please provide two amounts to get data in between them");
                    int min, max;
                getMin:
                    {
                        Console.Write("Enter first amount: ");
                        try
                        {
                            min = Convert.ToInt32(Console.ReadLine());
                        }
                        // if user enters other than number
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid Input. Try again.");
                            goto getMin;
                        }
                    }
                getMax:
                    {
                        Console.Write("Enter second amount: ");
                        try
                        {
                            max = Convert.ToInt32(Console.ReadLine());
                        }
                        // if user enters other than number
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid Input. Try again.");
                            goto getMax;
                        }
                    }
                    // Checking if user entered min value at max then interchanging them
                    if (min > max)
                    {
                        int t = max;
                        max = min;
                        min = t;
                    }
                    // Getting the list of customer objects from file
                    List<Customer> list = data.ReadFile<Customer>("customers.txt");
                    // Printing the result
                    Console.WriteLine("\n==== SEARCH RESULTS ====\n");
                    if (list.Count > 0)
                    {
                        Console.WriteLine("Account No".PadRight(12)
                                    + "Username".PadRight(10)
                                    + "Holder's Name".PadRight(15)
                                    + "Type".PadRight(9)
                                    + "Balance".PadRight(10)
                                    + "Status\n");
                        foreach (Customer c in list)
                        {
                            if (c.Balance >= min && c.Balance <= max)
                            {
                                Console.WriteLine(Convert.ToString(c.AccountNo).PadRight(12)
                                    + EncryptionDecryption(c.Username).PadRight(10)
                                    + c.Name.PadRight(15)
                                    + c.AccountType.PadRight(9)
                                    + Convert.ToString(c.Balance).PadRight(10)
                                    + c.Status);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("***NO DATA FOUND MATCHING WITH GIVEN DETAILS***");
                    }
                }
                else if (option == "2")
                {
                    Console.WriteLine("Please provide two dates in given format to get data in between them");
                    string startDate, endDate;
                    string formatString = "dd/MM/yyyy";
                    DateTime d1, d2;
                getStartDate:
                    {
                        Console.Write("Enter first date (dd/MM/yyyy): ");
                        startDate = Console.ReadLine();
                        try
                        {
                            d1 = DateTime.ParseExact(startDate, formatString, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid input. Try again.");
                            goto getStartDate;
                        }
                    }
                getEndDate:
                    {
                        Console.Write("Enter second date (dd/MM/yyyy): ");
                        endDate = Console.ReadLine();
                        try
                        {
                            d2 = DateTime.ParseExact(endDate, formatString, System.Globalization.CultureInfo.InvariantCulture);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Invalid input. Try again.");
                            goto getEndDate;
                        }
                    }
                    // Checks if user entered previous date at ending date and then interchaning them
                    if (d1 > d2)
                    {
                        DateTime dtemp = d2;
                        d2 = d1;
                        d1 = dtemp;
                    }
                    // Getting the list of transaction objects from file
                    List<Transaction> transactions = data.ReadFile<Transaction>("transactions.txt");
                    // Printing the result
                    Console.WriteLine("\n==== SEARCH RESULTS ====\n");
                    if (transactions.Count > 0)
                    {
                        Console.WriteLine("Transaction Type".PadRight(18)
                                    + "Username".PadRight(10)
                                    + "Holder's Name".PadRight(15)
                                    + "Amount".PadRight(10)
                                    + "Date\n");
                        foreach (Transaction t in transactions)
                        {
                            DateTime d = DateTime.ParseExact(t.Date, formatString, System.Globalization.CultureInfo.InvariantCulture);
                            if (d >= d1 && d <= d2)
                            {
                                Console.WriteLine(Convert.ToString(t.TransactionType).PadRight(18)
                                    + EncryptionDecryption(t.Username).PadRight(10)
                                    + t.HoldersName.PadRight(15)
                                    + Convert.ToString(t.TransactionAmount).PadRight(10)
                                    + t.Date);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("***NO DATA FOUND MATCHING WITH GIVEN DETAILS***");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Input. Try again.");
                    goto getOption;
                }
            }
        }

        // ******** CUSTOMER LOGIC ********

        // Method to withdraw cash
        public void CashWithdraw(string username)
        {
            Console.WriteLine("----- Withdraw Cash -----\n");
        getOption:
            {
                Console.WriteLine("1---Fast Cash\n" +
                    "2---Normal Cash");
                Console.Write("\nPlease select a mode of withdrawl (1/2): ");
                try
                {
                    // Getting input if user wants Fast Cash or Normal Cash
                    string option = Console.ReadLine();
                    if (option == "1" || option == "2")
                    {
                        switch (option)
                        {
                            // In case of Fast Cash
                            case "1":
                                Console.Clear();
                                Console.WriteLine("==== FAST CASH ====\n");
                                // Putting all possible choices of Fast Cash in a list
                                List<int> FastCashOptions = new List<int>(new int[] { 500, 1000, 2000, 5000, 10000, 15000, 20000 });
                                // Printing all possible choices
                                Console.WriteLine($"1---{FastCashOptions[0]}\n" +
                                    $"2---{FastCashOptions[1]}\n" +
                                    $"3---{FastCashOptions[2]}\n" +
                                    $"4---{FastCashOptions[3]}\n" +
                                    $"5---{FastCashOptions[4]}\n" +
                                    $"6---{FastCashOptions[5]}\n" +
                                    $"7---{FastCashOptions[6]}\n");
                            getDomination:
                                {
                                    // Getting input that which denomination the user want
                                    Console.Write("Select one of the denominations of money: ");
                                    string op = Console.ReadLine();
                                    if (op == "1" || op == "2" || op == "3" || op == "4" || op == "5" || op == "6" || op == "7")
                                    {
                                        Data data = new Data();
                                        int opt = Convert.ToInt32(op);
                                        Console.Write($"Are you sure you want to withdraw Rs.{FastCashOptions[opt - 1]} (Y/N)? ");
                                        if (Console.ReadLine() == "Y")
                                        {
                                            Customer customer = data.GetCustomer(username);
                                            int totalAmount = data.TodaysTransactionsAmount(customer.AccountNo);
                                            // Checking if the withdrawl amount exceeds 20k
                                            if ((totalAmount + FastCashOptions[opt - 1]) <= 20000)
                                            {
                                                // Checking if user has sufficent balance
                                                if (customer != null && customer.Balance > FastCashOptions[opt - 1])
                                                {
                                                    // Doing the transaction
                                                    data.DeductBalance(customer, FastCashOptions[opt - 1]);
                                                    Console.WriteLine("\nCash Successfully Withdrawn!");

                                                    // Making and recording transaction to file
                                                    Transaction transaction = MakeTransaction(customer, FastCashOptions[opt - 1], "Cash Withdrawl");

                                                    // Asking if user wants a receipt
                                                    Console.Write("Do you wish to print a receipt(Y/N)? ");
                                                    if (Console.ReadLine() == "Y")
                                                    {
                                                        // printing receipt
                                                        PrintReceipt(transaction, "Withdrawn");
                                                    }
                                                }
                                                else
                                                {
                                                    Console.WriteLine("Insufficent Balance. Transaction failed!");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine($"You have already withdrawn Rs.{totalAmount} today.\n" +
                                                    $"Cannot withdrawn more than Rs. 20,000 on same day.");
                                            }
                                        }
                                        // In case user did not confirm for transaction
                                        else
                                        {
                                            Console.WriteLine("Transaction was not confirmed. Transaction Failed!");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid Input. Please try again.");
                                        goto getDomination;
                                    }
                                }
                                break;
                            // In case of Normal Cash
                            case "2":
                                Console.Clear();
                                Console.WriteLine("==== NORMAL CASH ====\n");
                            getAmount:
                                {
                                    Console.Write("Enter the withdrawal amount: ");
                                    Data data = new Data();
                                    try
                                    {
                                        int amount = Convert.ToInt32(Console.ReadLine());
                                        // Asking for Confirmation
                                        Console.Write($"Are you sure you want to withdraw Rs.{amount} (Y/N)? ");
                                        if (Console.ReadLine() == "Y")
                                        {
                                            Customer customer = data.GetCustomer(username);
                                            // Checking if user has sufficent balance
                                            if (customer != null && customer.Balance > amount)
                                            {
                                                // Doing the transaction
                                                data.DeductBalance(customer, amount);
                                                Console.WriteLine("\nCash Successfully Withdrawn!");

                                                // Making and recording transaction to file
                                                Transaction transaction = MakeTransaction(customer, amount, "Cash Withdrawl");

                                                // Asking if user wants a receipt
                                                Console.Write("Do you wish to print a receipt(Y/N)? ");
                                                string y = Console.ReadLine();
                                                if (y == "Y" || y == "y")
                                                {
                                                    // printing receipt
                                                    PrintReceipt(transaction, "Withdrawn");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Insufficent Balance. Transaction failed!");
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Invalid Input. Try again.");
                                            goto case "2";
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        Console.WriteLine("Invalid input. Pleas try again!");
                                        goto getAmount;
                                    }
                                }
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input. Please try again");
                        goto getOption;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input!!!");
                    goto getOption;
                }
            }
        }

        // Method to Cash Transfer
        public void CashTransfer(string username)
        {
            Data data = new Data();
            // Gets Customer object against given username
            Customer sender = new Customer();
            sender = data.GetCustomer(username);
        getAmount:
            {
                Console.WriteLine("----- Transfer Cash -----\n");
                Console.Write("Enter amount in multiples of 500: ");
                try
                {
                    // Reads amount in input
                    int amount = Convert.ToInt32(Console.ReadLine());
                    if (amount % 500 == 0)
                    {
                        if (amount <= sender.Balance)
                        {
                        getAccNo:
                            {
                                Console.Write("Enter the account number to which you want to transfer: ");
                                try
                                {
                                    int accNo = Convert.ToInt32(Console.ReadLine());
                                    Customer reciever = new Customer();
                                    if (data.isInFile(accNo, out reciever))
                                    {
                                        Console.Write($"You wish to deposit Rs. {amount} in account held by Mr. {reciever.Name}.\n" +
                                            "If this information is correct please re-enter the account number: ");
                                        try
                                        {
                                            int accNo2 = Convert.ToInt32(Console.ReadLine());
                                            // checking if user entered the same account number both times
                                            if (accNo == accNo2)
                                            {
                                                // Deduct amount from Sender's account
                                                data.DeductBalance(sender, amount);

                                                // Add amount to receiver's account
                                                data.AddAmount(reciever, amount);

                                                Console.WriteLine("Transaction confirmed.");

                                                // Making and recording transaction to file for Sender
                                                Transaction transaction = MakeTransaction(sender, amount, "Cash Transfer");

                                                // Making and recording transaction to file for Receiver
                                                Transaction transaction1 = MakeTransaction(reciever, amount, "Cash Transfer");

                                                // Asking if user wants a receipt
                                                Console.Write("Do you wish to print a receipt(Y/N)? ");
                                                string y = Console.ReadLine();
                                                if (y == "Y" || y == "y")
                                                {
                                                    // printing receipt
                                                    PrintReceipt(transaction, "Amount Transfered");
                                                }
                                            }
                                            else
                                            {
                                                Console.WriteLine("Did not enter same account number. Trasaction Failed!");
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("Did not enter same account number. Trasaction Failed!");
                                        }

                                    }
                                    else
                                    {
                                        Console.WriteLine("Given Account does not exist!!");
                                    }
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Invalid Input. Try again!");
                                    goto getAccNo;
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Insufficent Balance!!!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input. Try again!");
                        goto getAmount;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input. Try again!");
                    goto getAmount;
                }
            }
        }

        // Method to do a Deposit
        public void CashDeposit(string username)
        {
            Data data = new Data();
            // Gets Customer object against given username
            Customer customer = new Customer();
            customer = data.GetCustomer(username);
        getAmount:
            {
                Console.WriteLine("----- Deposit Cash -----\n");
                Console.Write("Enter amount to deposit: ");
                try
                {
                    int amount = Convert.ToInt32(Console.ReadLine());
                    // Add amount to the account
                    data.AddAmount(customer, amount);
                    Console.WriteLine("Cash Deposited Successfully.");

                    // Making and recording transaction to file for Sender
                    Transaction transaction = MakeTransaction(customer, amount, "Cash Deposit");

                    // Asking if user wants a receipt
                    Console.Write("Do you wish to print a receipt(Y/N)? ");
                    string y = Console.ReadLine();
                    if (y == "Y" || y == "y")
                    {
                        // printing receipt
                        PrintReceipt(transaction, "Amount Deposited");
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid Input. Try again!");
                    goto getAmount;
                }
            }
        }

        // Method to make a transaction and record in the file
        // To be used in CashWithdraw() & CashTransfer() & CashDeposit()
        public Transaction MakeTransaction(Customer c, int amount, string type)
        {
            // Adding data to Transaction variable for Sender
            Transaction transaction = new Transaction();
            transaction.AccountNo = c.AccountNo;
            transaction.Username = c.Username;
            transaction.HoldersName = c.Name;
            transaction.TransactionType = type;
            transaction.TransactionAmount = amount;
            DateTime date = DateTime.Now;
            transaction.Date = date.ToString("dd/MM/yyyy");
            transaction.Balance = c.Balance;
            // Appending transaction in transactions.txt
            Data data = new Data();
            data.AddToFile<Transaction>(transaction);
            return transaction;
        }

        // Method to display balance
        public void DisplayBalance(string username)
        {
            Data data = new Data();
            Customer customer = new Customer();
            customer = data.GetCustomer(username);
            Console.WriteLine($"Account # {customer.AccountNo}");
            // Getting today's date and putting it in a string
            DateTime date = DateTime.Now;
            string d = date.ToString("dd/MM/yyyy");
            Console.WriteLine($"Date: {d}\n");
            Console.WriteLine($"Balance: {customer.Balance}");
        }

        // Method to Print Receipt
        // To be used in WithdrawCash() & CashTransfer() & DepositCash()
        public void PrintReceipt(Transaction transaction, string t)
        {
            Console.WriteLine($"\nAccount # {transaction.AccountNo}");
            Console.WriteLine($"Date: {transaction.Date}\n");

            Console.WriteLine($"{t}: {transaction.TransactionAmount}");
            Console.WriteLine($"Balance: {transaction.Balance}");
        }
    }
}
