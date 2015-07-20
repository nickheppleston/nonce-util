using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NonceUtil.Tests
{
    [TestClass]
    public class CheckNonceTests
    {
        [TestMethod]
        public void CheckNonce_ValidNonce()
        {
            // Setup
            var nonceSecret = "noncesecret";
            var nonce = NonceUtil.GenerateNonce(nonceSecret);

            // Execution
            var result = NonceUtil.CheckNonce(nonceSecret, nonce);

            // Assertion
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckNonce_InvalidNonce_ReturnsFalse()
        {
            // Setup
            var nonceSecret = "noncesecret";
            var invalidNonce = "LpGobA3qy85Wlw8,1437406095.94737,4sAX+QAjE1HAaLKkBW2bPjGqHhMCA6hedDxDm0bIs9E="; // Pass an invalid nonce - the supplied salt does not match the hashed salt.

            // Execution
            var result = NonceUtil.CheckNonce(nonceSecret, invalidNonce);

            // Assertion
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckNonce_NonceExceedsTimeout_ReturnsFalse()
        {
            // Setup
            var nonceSecret = "noncesecret";
            var nonce = NonceUtil.GenerateNonce(nonceSecret, 1);

            Thread.Sleep(2000); // Introduce an artifical delay to ensure we go over the nonce expiry date/time.

            // Execution
            var result = NonceUtil.CheckNonce(nonceSecret, nonce);

            // Assertion
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CheckNonce_InvalidNonceStructure_ReturnsFalse()
        {
            // Setup - None

            // Execution
            var result = NonceUtil.CheckNonce("secret", "invalid,nonce"); // Pass an invalid nonce with only two sections (separated by comma), instead of three.

            // Assertion
            Assert.IsFalse(result);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNonce_EmptySecret_ThrowsException()
        {
            NonceUtil.CheckNonce(String.Empty, "nonce");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNonce_EmptyNonce_ThrowsException()
        {
            NonceUtil.CheckNonce("secret", String.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CheckNonce_EmptySecretAndNonce_ThrowsException()
        {
            NonceUtil.CheckNonce(String.Empty, String.Empty);
        }
    }
}
