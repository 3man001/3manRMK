using LibraryDotNetFramework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Drawing;

namespace LibraryDotNetFramework.Tests
{
    [TestClass()]
    public class CheckStringTests
    {
        [TestMethod()]
        public void TaxpayerIdentificationNumberTest()
        {
            bool actual;
            Dictionary<string, bool> testINN = new Dictionary<string, bool>();
            testINN.Add("", false);
            testINN.Add("qwahfynfjndy", false);
            testINN.Add("qwahfynfjn", false);
            testINN.Add("000000000000", true);
            testINN.Add("00000000000", false);
            testINN.Add("0000000000", true);
            testINN.Add("000000000", false);
            testINN.Add("526317984689", true);
            testINN.Add("526317984686", false);
            testINN.Add("526317984669", false);
            testINN.Add("3664069397", true);
            testINN.Add("3664069395", false);
            foreach (string element in testINN.Keys)
            {
                actual = CheckString.TaxpayerIdentificationNumber(element);
                Assert.AreEqual(testINN[element], actual, "Строка на входе = \"" + element + "\"");
            }
        }

        [TestMethod()]
        public void GetColorAfterCheckINNTest()
        {
            Color actual;
            Dictionary<string, Color> testINN = new Dictionary<string, Color>();
            testINN.Add("", Color.Snow);
            testINN.Add("526317984689", Color.LightGreen);
            testINN.Add("526317984686", Color.LightCoral);
            foreach (string element in testINN.Keys)
            {
                actual = CheckString.GetColorAfterCheckINN(element);
                Assert.AreEqual(testINN[element], actual, "Строка на входе = \"" + element + "\"");
            }
        }
    }
}