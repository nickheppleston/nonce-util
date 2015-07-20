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
            // Setup - None

            // Execution
            var salt = NonceUtil.GenerateSalt();

            // Assertion
            Assert.IsFalse(String.IsNullOrEmpty(salt));
            Assert.AreEqual(15, salt.Length); // Salt length is defined as a const = 15
        }

        [TestMethod]
        public void GenerateSalt_Multiple()
        {
            // Setup
            var saltCount = 100;
            var saltList = new List<string>();

            // Execution
            for (int i = 0; i < saltCount; i++)
            {
                var salt = NonceUtil.GenerateSalt();

                if (!saltList.Contains(salt))
                    saltList.Add(salt);
            }

            // Assertion
            Assert.AreEqual(saltCount, saltList.Count);
        }
    }
}
