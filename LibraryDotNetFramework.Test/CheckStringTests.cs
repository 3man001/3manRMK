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
        public void GetColorAfterCheckStringTest()
        {
            Assert.AreEqual(Color.Snow, CheckString.GetColorAfterCheckString("", false));
            Assert.AreEqual(Color.LightGreen, CheckString.GetColorAfterCheckString("1qe1", true));
            Assert.AreEqual(Color.LightCoral, CheckString.GetColorAfterCheckString("1qe1", false));
        }

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
        public void PhoneTest()
        {
            bool actual;
            Dictionary<string, bool> testPhone = new Dictionary<string, bool>();
            testPhone.Add("", false);
            testPhone.Add("qwahfynfjndy", false);
            testPhone.Add("3664069395", false);
            testPhone.Add("+7(909) 176-92-36", true);
            testPhone.Add("+7(909) 176-92-3 ", false);
            testPhone.Add("+7(909) 176-92-  ", false);
            foreach (string element in testPhone.Keys)
            {
                actual = CheckString.Phone(element);
                Assert.AreEqual(testPhone[element], actual, "Строка на входе = \"" + element + "\"");
            }
        }

        [TestMethod()]
        public void NumbersTest()
        {
            bool actual;
            Dictionary<string, bool> testNuber = new Dictionary<string, bool>();
            testNuber.Add("", false);
            testNuber.Add("qwahfynfjndy", false);
            testNuber.Add("3664069395", true);
            testNuber.Add("19234,65656", true);
            testNuber.Add("19234.65656", false);
            testNuber.Add(",", false);
            foreach (string element in testNuber.Keys)
            {
                actual = CheckString.Numbers(element);
                Assert.AreEqual(testNuber[element], actual, "Строка на входе = \"" + element + "\"");
            }
        }

        [TestMethod()]
        public void EmailTest()
        {
            bool actual;
            Dictionary<string, bool> testEmail = new Dictionary<string, bool>();
            testEmail.Add("", false);
            testEmail.Add("qwahfynfjndy", false);
            testEmail.Add("   @   ", false);
            testEmail.Add("qwefdfdfs@fdsfsdf.erer", true);
            testEmail.Add("ывфывфы@вывыю.выв", false);
            testEmail.Add("qew@qww", true);
            foreach (string element in testEmail.Keys)
            {
                actual = CheckString.Email(element);
                Assert.AreEqual(testEmail[element], actual, "Строка на входе = \"" + element + "\"");
            }
        }

        [TestMethod()]
        public void FullNameTest()
        {
            bool actual;
            Dictionary<string, bool> testFullName = new Dictionary<string, bool>();
            testFullName.Add("", false);
            testFullName.Add("qwefdfdfs@fdsfsdf.erer", false);
            testFullName.Add("ывфывфы@вывыю.выв", false);
            testFullName.Add("Иванов Иван Иванович", true);
            foreach (string element in testFullName.Keys)
            {
                actual = CheckString.FullName(element);
                Assert.AreEqual(testFullName[element], actual, "Строка на входе = \"" + element + "\"");
            }
        }

        [TestMethod()]
        public void BuyerTest()
        {
            bool actual;
            Dictionary<string, bool> testByer = new Dictionary<string, bool>();
            testByer.Add("", false);
            testByer.Add("qwefdfdfs@fdsfsdf.erer", false);
            testByer.Add("ывфывфы@вывыю.выв", false);
            testByer.Add("Иванов Иван Иванович 5465 232323", true);
            testByer.Add("ООО Ромашка", true);
            foreach (string element in testByer.Keys)
            {
                actual = CheckString.Buyer(element);
                Assert.AreEqual(testByer[element], actual, "Строка на входе = \"" + element + "\"");
            }
        }
    }
}