using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Challenge1
{
    [TestFixture]
    class UnreadableTest
    {

        [Test]
        public void RemoveItem_WhenArrayContainsString()
        {
            var array = new string[] { "a", "b", "c" };

            new Unreadable().RemoveItem("a", ref array);

            Assert.That(array, Has.Exactly(0).EqualTo("a"));

        }

        [Test]
        public void RemoveItem_WhenArrayDoeNotContainsString()
        {
            var array = new string[] { "a", "b", "c" };

            new Unreadable().RemoveItem("d", ref array);

            Assert.That(array, Has.Exactly(3).Length);

        }

        [Test]
        public void RemoveItem_WhenArrayIsEmpty()
        {
            var array = new string[] { };

            new Unreadable().RemoveItem("", ref array);

            Assert.That(array, Has.Exactly(0).Length);

        }
    }
}
