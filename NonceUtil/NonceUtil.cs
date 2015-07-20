﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace NonceUtil
{
    public static class NonceUtil
    {
        private const string SALT_CHARS = "1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM";

        private static Random rand = new Random();

        /// <summary>
        /// Generates a Nonce - The generated string contains three parts, seperated by a comma.
        /// The first part is the individual salt. The seconds part is the time until the nonce 
        /// is valid. The third part is a hash of the salt, the time, and a secret value.
        /// </summary>
        /// <param name="secret">The secret containing at least 10 characters. The same value must be passed to CheckNonce()</param>
        /// <param name="nonceTimeoutSeconds">The Nonce timeout in seconds (Optional - defaults to 180 seconds)</param>
        /// <returns>The generated Nonce</returns>
        public static string GenerateNonce(string secret, int nonceTimeoutSeconds = 180)
        {
            if ((String.IsNullOrEmpty(secret)) || (secret.Length < 10))
                throw new ArgumentException("The Secret parameter is missing -or- less than 10 characters");
            if (nonceTimeoutSeconds <= 0)
                throw new ArgumentException("The Nonce Timeout Seconds parameter must be greater than zero");

            var salt = GenerateSalt(15);
            var timeoutSeconds = GetSecondsSinceEpoch(nonceTimeoutSeconds);
            var hash = ComputeHash(Encoding.UTF8.GetBytes(salt), Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(timeoutSeconds.ToString()));

            return (String.Format("{0},{1},{2}", salt, timeoutSeconds, Convert.ToBase64String(hash)));
        }

        /// <summary>
        /// Check a previously generated Nonce.
        /// </summary>
        /// <param name="secret">The secret passed to GenerateNonce()</param>
        /// <param name="nonce">The Nonce generated by a call to GenerateNonce()</param>
        /// <returns>bool indicating whether the nonce is valid (true) or invalid (false)</returns>
        public static bool CheckNonce(string secret, string nonce)
        {
            if (String.IsNullOrEmpty(secret))
                throw new ArgumentException("The Secret parameter is missing");
            if (String.IsNullOrEmpty(nonce))
                throw new ArgumentException("The Nonce parameter is missing");

            var nonceArray = nonce.Split(',');

            if (!ValidateNonceArray(nonceArray))
                return (false);

            var salt = nonceArray[0];
            var nonceTimeoutSeconds = Double.Parse(nonceArray[1]);
            var hash = nonceArray[2];

            if (hash != Convert.ToBase64String((ComputeHash(Encoding.UTF8.GetBytes(salt), Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(nonceTimeoutSeconds.ToString())))))
                return (false);

            if (GetSecondsSinceEpoch() > nonceTimeoutSeconds)
                return (false);

            return (true);
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

        private static byte[] ComputeHash(byte[] plainText, byte[] salt, byte[] timeout)
        {
            var sha256 = new SHA256Managed();

            byte[] plainTextWithSaltBytes = new byte[plainText.Length + salt.Length + timeout.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }

            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            for (int i = 0; i < timeout.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = timeout[i];
            }

            return (sha256.ComputeHash(plainTextWithSaltBytes));
        }

        private static bool ValidateNonceArray(string[] nonceArray)
        {
            if (nonceArray.Length != 3)
                return (false);

            for (int i = 0; i < nonceArray.Length; i++)
            {
                if (String.IsNullOrEmpty(nonceArray[i]))
                    return (false);
            }

            double doubleValue;
            if (!Double.TryParse(nonceArray[1], out doubleValue))
                return (false);

            return (true);
        }

        private static double GetSecondsSinceEpoch(int secondsToAdd = 0)
        {
            return ((DateTime.UtcNow.AddSeconds(secondsToAdd) - new DateTime(1970, 1, 1)).TotalSeconds);
        }
    }
}
