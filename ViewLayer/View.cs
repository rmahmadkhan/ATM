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
            Console.WriteLine("-----Welcome to ATM-----\n" +
                            "Login as:\n" +
                            "1----Administrator\n" +
                            "2----Customer\n" +
                            "Enter 1 or 2\n");
            bool gotUser = false;
            while (!gotUser)
            {
                try
                {
                    int user = Convert.ToInt32(Console.ReadLine());
                    // Checking if input is correct
                    if (user == 1 || user == 2)
                    {
                        gotUser = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input. Please try again");
                        continue;
                    }

                    switch (user)
                    {
                        // Case 1 for Administrator Login
                        case 1:
                            Console.WriteLine("-----Administrator Login-----\n" +
                                "Please Enter your username & 5-digit Pin");
                            //Decalring an Admin object
                            Admin admin = new Admin();
                            bool isSignedin = false;
                            Logic logic = new Logic();
                            while (!isSignedin)
                            {
                                // Reading and storing username
                                Console.WriteLine("Username:");
                                admin.Username = Console.ReadLine();

                                // Reading and storing Pin
                                Console.WriteLine("5-digit Pin:");
                                admin.Pin = Console.ReadLine();
                                
                                // Verifying login details                                
                                if (logic.verifyLogin(admin))
                                {
                                    Console.WriteLine("---Loggedin as Administrator");
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
                        case 2:
                            Console.WriteLine("-----Customer Login-----\n" +
                                "Please Enter your username & 5-digit Pin");
                            //Decalring an Customer object
                            Customer customer = new Customer();
                            bool isSignedin2 = false;
                            Logic logic2 = new Logic();
                            while (!isSignedin2)
                            {
                                // Reading and storing username
                                Console.WriteLine("Username:");
                                customer.Username = Console.ReadLine();

                                // Reading and storing Pin
                                Console.WriteLine("5-digit Pin:");
                                customer.Pin = Console.ReadLine();

                                // Verifying login details                                
                                if (logic2.verifyLogin(customer))
                                {
                                    Console.WriteLine("---Loggedin as Customer");
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
                catch (Exception e)
                {
                    Console.WriteLine("Wrong Input. Please try again");
                }
            }
        }

        // Admin Screen and option
        public void AdminScreen()
        {
            Console.WriteLine("1----Create New Account\n" +
                "2----Delete Existing Account\n" +
                "3----Update Account Information\n" +
                "4----Search for Account\n" +
                "5----View Reports\n" +
                "6----Exit");
            
            bool gotOption = false;
            while(!gotOption)
            {
                try
                {
                    int option = Convert.ToInt32(Console.ReadLine());
                    // Checking if input is correct
                    if (option == 1 || option == 2 || option == 3 || option == 4 || option == 5 || option == 6)
                    {
                        gotOption = true;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Input. Please try again");
                        continue;
                    }
                    switch (option)
                    {

                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine("Wrong Input. Please try again");
                }
            }
        }
    }
}
