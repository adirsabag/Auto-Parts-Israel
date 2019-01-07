using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BCrypt.Net;

namespace HW2.Models
{
    public class Hash
    {
        // encrypt password
        public static string PassHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }

        // decrypt password
        public static bool PassVerify(string password, string hash) //'password' is the original password, 'hash' is the enqrypted one
        {
            return BCrypt.Net.BCrypt.CheckPassword(password, hash);
        }
    }
}