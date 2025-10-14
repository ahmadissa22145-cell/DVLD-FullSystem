using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace DVLD_Buisness
{
    public class clsHasingData
    {
        public static string HashCompute(string inputData)
        {
            StringBuilder stringBuilder = new StringBuilder(inputData);

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(stringBuilder.ToString()));

                return BitConverter.ToString(hashBytes).Replace("-", string.Empty).ToLower();
            }
        }
    }
}
