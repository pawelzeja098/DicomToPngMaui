//using Javax.Security.Auth.Login;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DicomToPngMaui.Data
{
    public class User
    {
        private string storedlogin = "admin";
        private string storedpassword = "admin";
        
        public bool VerifyLogin(string enteredLogin, string enteredPassword)
        {
            return (enteredLogin == storedlogin && enteredPassword == storedpassword);
        }

        
    }
}
