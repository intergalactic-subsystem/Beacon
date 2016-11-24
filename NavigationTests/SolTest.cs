using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Navigation.lib;
using Navigation.lib.Math;

namespace NavigationTests
{
    [TestClass]
    public class SolTest
    {
        [TestMethod]
        public void TestSol()
        {
            var sol = Star.Create("Sol", 1.989E+030, 695000000);
            var earth = Planet.Create("Earth", sol, 5.976E+024, 6378140, 150000000000, 480000);
            var moon = Moon.Create("Moon", earth, 7.34767309E+22, 1737000, 384400000);

            var excpectedL1 = new Vector3(149739990234.377, 0.0017333984375, 0.0017333984375);
            var excpectedL2 = new Vector3(149739990234.377, 0.0017333984375, 0.0017333984375);
            Assert.AreEqual(true, excpectedL1.Distance(sol.Position) - earth.L1.Distance(sol.Position) < 0.01);
            Assert.AreEqual(true, excpectedL2.Distance(sol.Position) - earth.L2.Distance(sol.Position) < 0.01);
        }
    }
}
