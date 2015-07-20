using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Diagnostics;

namespace NonceUtil.Tests
{
    [TestClass]
    public class GenerateSaltTests
    {
        [TestMethod]
        public void GenerateSalt_Single()
        {
            // Setup
            var saltLength = 15;

            // Execution
            var salt = NonceUtil.GenerateSalt(saltLength);

            // Assertion
            Assert.IsFalse(String.IsNullOrEmpty(salt));
            Assert.AreEqual(saltLength, salt.Length);
        }

        [TestMethod]
        public void GenerateSalt_Multiple()
        {
            // Setup
            var saltLength = 15;
            var saltCount = 100;
            var saltList = new List<string>();

            // Execution
            for (int i = 0; i < saltCount; i++)
            {
                var salt = NonceUtil.GenerateSalt(saltLength);

                if (!saltList.Contains(salt))
                    saltList.Add(salt);
            }

            // Assertion
            Assert.AreEqual(saltCount, saltList.Count);
        }
    }
}
