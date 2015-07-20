using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonceUtil
{
    public static class NonceUtil
    {
        private const string SALT_CHARS = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

        private static Random rand = new Random();

        public static string GenerateNonce()
        {
            throw new NotImplementedException();
        }

        public static bool CheckNonce()
        {
            throw new NotImplementedException();
        }

        internal static string GenerateSalt(int saltLength)
        {
            var salt = new StringBuilder();
            
            var chars = SALT_CHARS.ToCharArray();
		    var ll = chars.Length - 1;

            while (salt.Length < saltLength)
                salt.Append(chars[rand.Next(0, ll)]);

            return (salt.ToString());
        }
    }
}
