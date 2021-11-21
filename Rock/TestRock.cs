using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Rock;

namespace Rock.Tests
{
    [TestClass()]
    class TestRock
    {
        [TestMethod()]
        public void testGenerateGamesRequest()
        {
            Rock rock = new Rock();
            rock.SetConfig(new ConfigFromStubb());
            string result = rock.generateGamesListRequest();
            Assert.AreEqual("Please select 0 - Rock Paper Scissors 1 - Star Wars", result);
        }
 
    }
}
