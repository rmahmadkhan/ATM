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
        public Boolean verifyLogin(Admin admin)
        {
            Data adminData = new Data();
            return adminData.isInFile(admin);
        }

        // Method to verify login of customer
        public Boolean verifyLogin(Customer customer)
        {
            Data customerData = new Data();
            return customerData.isInFile(customer);
        }
    }
}
