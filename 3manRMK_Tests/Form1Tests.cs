using Microsoft.VisualStudio.TestTools.UnitTesting;
using _3manRMK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _3manRMK.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        Form1 F1 = new Form1();

        [TestMethod()]
        public void ToDecimalTest()
        {
            Assert.AreEqual(12.999998m, F1.ToDecimal("12,999998"));
        }

        [TestMethod()]
        public void CheckSimbolsTest()
        {
        //    Assert.AreEqual(false, F1.CheckSimbols("", ""), "Неизв.Тип Проверка путой строки");
        //    Assert.AreEqual(false, F1.CheckSimbols("12345qwerrt", ""), "Неизв.Тип Проверка строки");

        //    Assert.AreEqual(false, F1.CheckSimbols("", "Число"), "Число Проверка путой строки");
        //    Assert.AreEqual(true, F1.CheckSimbols("1234567890,57656576577", "Число"), "Число Проверка коректного числа");
        //    Assert.AreEqual(false, F1.CheckSimbols("12345,67890,57656576577", "Число"), "Число Проверка некорректного числа");

        //    Assert.AreEqual(false, F1.CheckSimbols("", "Email"), "Маил Проверка путой строки");
        //    Assert.AreEqual(true, F1.CheckSimbols("qwert@test.mail.deb", "Email"), "Маил Проверка корректной почты");
        //    Assert.AreEqual(false, F1.CheckSimbols("Русская@почта.ру", "Email"), "Маил Проверка некорректной почты");

          //  Assert.AreEqual(false, F1.CheckSimbols("", "Phone"), "Тел. Проверка путой строки");

         //   Assert.AreEqual(false, F1.CheckSimbols("", "ФИО"), "ФИО Проверка путой строки");

       //     Assert.AreEqual(false, F1.CheckSimbols("", "ИНН"), "ИНН Проверка путой строки");

         //   Assert.AreEqual(false, F1.CheckSimbols("", "Покупатель"), "Покупатель Проверка путой строки");
        }
    }
}