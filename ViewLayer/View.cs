using BOLayer;
using LogicLayer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewLayer
{
    public class View
    {
        public void loginScreen()
        {
            Console.WriteLine("-----Welcome to ATM-----\n\n" +
                            "Login as:\n" +
                            "1----Administrator\n" +
                            "2----Customer\n\n" +
                            "Enter 1 or 2\n");

            try
            {
            getUser:
                {
                    string user = Console.ReadLine();
                    // Checking if input is correct
                    if (user == "1" || user == "2")
                    {

                        switch (user)
                        {
                            // Case 1 for Administrator Login
                            case "1":
                                Console.WriteLine("-----Administrator Login-----\n" +
                                    "Please Enter your username & 5-digit Pin");
                                //Decalring an Admin object
                                Admin admin = new Admin();
                                bool isSignedin = false;
                                Logic logic = new Logic();
                                while (!isSignedin)
                                {
                                    // Reading and storing username
                                    Console.Write("Username: ");
                                    admin.Username = Console.ReadLine();

                                    // Reading and storing Pin
                                    Console.Write("5-digit Pin: ");
                                    admin.Pin = Console.ReadLine();

                                    // Verifying login details                                
                                    if (logic.VerifyLogin(admin))
                                    {
                                        Console.WriteLine("\n---Loggedin as Administrator---\n");
                                        isSignedin = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong Username/Pin. Try again.");
                                    }
                                }
                                // As successfully signedin, displaying admin screen
                                AdminScreen();
                                break;

                            // Case 2 for Customer Login
                            case "2":
                                Console.WriteLine("-----Customer Login-----\n" +
                                    "Please Enter your username & 5-digit Pin");
                                //Decalring an Customer object
                                Customer customer = new Customer();
                                bool isSignedin2 = false;
                                Logic logic2 = new Logic();
                                while (!isSignedin2)
                                {
                                    // Reading and storing username
                                    Console.Write("Username: ");
                                    customer.Username = Console.ReadLine();

                                    // Reading and storing Pin
                                    Console.Write("5-digit Pin: ");
                                    customer.Pin = Console.ReadLine();

                                    // Verifying login details                                
                                    if (logic2.VerifyLogin(customer))
                                    {
                                        Console.WriteLine("\n---Loggedin as Customer---\n");
                                        isSignedin = true;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Wrong Username/Pin. Try again.");
                                    }
                                }
                                // As successfully signedin, displaying admin screen
                                AdminScreen();
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input. Please try again");
                        goto getUser;
                    }
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Please try again");
            }
        }

        // Admin Screen and option
        public void AdminScreen()
        {
        adminScreen:
            {
                Console.Clear();
                Console.WriteLine("-----Admin Menu-----\n");
                Console.WriteLine("1----Create New Account\n" +
                    "2----Delete Existing Account\n" +
                    "3----Update Account Information\n" +
                    "4----Search for Account\n" +
                    "5----View Reports\n" +
                    "6----Exit");

                try
                {
                getAdminOption:
                    {
                        string option = Console.ReadLine();
                        // Checking if input is correct
                        if (option == "1" || option == "2" || option == "3" || option == "4" || option == "5" || option == "6")
                        {
                            Logic logic = new Logic();
                            switch (option)
                            {
                                // To create a new account
                                case "1":
                                    logic.CreateAccount();
                                    break;
                                case "2":
                                    logic.DeleteAccount();
                                    break;
                                //case 3:
                                //    logic.UpdateAccount();
                                //    break;
                                //    case 4:
                                //        logic.SearchAccount();
                                //        break;
                                //    case 5:
                                //        logic.ViewReports();
                                //        break;
                                // Exits the applicaion
                                case "6":
                                    System.Environment.Exit(0);
                                    break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Wrong Input. Please try again");
                            goto getAdminOption;
                        }
                    }
                    // Asks user if to continue or not
                    Console.Write("\nDo you wish to continue(y/n): ");
                    string wish = Console.ReadLine();
                    if (wish == "y" || wish == "Y")
                    {
                        goto adminScreen;
                    }
                    else
                    {
                        System.Environment.Exit(0);
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Please try again");
                }
            }
        }
    }
}
