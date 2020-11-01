using BOLayer;
using DataLayer;
using LogicLayer;
using System;
using ViewLayer;

namespace ATM
{
    class Program
    {
        static void Main(string[] args)
        {
            // Code Used to write an admin to the file
            /*
            Logic logic = new Logic();
            Admin admin = new Admin {Username = logic.EncryptionDecryption("admin"), Pin = logic.EncryptionDecryption("12345") };
            Data obj = new Data();
            obj.AddToFile(admin);
            */

            View view = new View();
            view.loginScreen();
        }
    }
}
