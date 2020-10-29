using BOLayer;
using DataLayer;
using System;
using ViewLayer;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Code Used to write 3 admins to the file
            /*
            Admin admin1 = new Admin {Username = "admin1", Pin = "12345" };
            Admin admin2 = new Admin { Username = "admin2", Pin = "12345" };
            Admin admin3 = new Admin { Username = "admin3", Pin = "12345" };
            Data obj = new Data();
            obj.SaveToFile(admin1);
            obj.SaveToFile(admin2);
            obj.SaveToFile(admin3);
            */

            View view = new View();
            view.loginScreen();
        }
    }
}
