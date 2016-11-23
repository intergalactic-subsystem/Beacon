using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Navigation.lib.Math;

namespace NavigationTests
{
    [TestClass]
    public class GravitationSource
    {
        [TestMethod]
        public void TestGravityForce()
        {
            var earth = new Navigation.lib.GravitationSource(
                5.98E24,
                6.38E6,
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0)
            );

            var me = new Navigation.lib.GravitationSource(
                68,
                2,
                new Vector3(0, 6.38E6, 0),
                new Vector3(0, 0, 0)
            );

            Assert.IsTrue(Math.Abs(earth.ForceTo(me) - 666.0) < 1);
        }

        [TestMethod]
        public void TestAccelerationTo()
        {
            var earth = new Navigation.lib.GravitationSource(
                5.98E24,
                6.38E6,
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0)
            );

            var me = new Navigation.lib.GravitationSource(
                68,
                2,
                new Vector3(0, 6.38E6, 0),
                new Vector3(0, 0, 0)
            );

            Assert.IsTrue(Math.Abs(me.AccelerationTo(earth) - 9.80) < 0.01);
            Assert.IsTrue(earth.AccelerationTo(me) < 0.00001);
        }

        [TestMethod]
        public void TestSolSystem()
        {
            var sol = new Navigation.lib.GravitationSource(
                1.989E+030,
                695000000,
                new Vector3(1, 1, 1),
                new Vector3(0, 0, 0)
            );

            var earth = new Navigation.lib.GravitationSource(
                5.976E+024,
                6378140,
                new Vector3(150000000000, 0, 0),
                new Vector3(0, 0, 29800)
            );

            var start = new TimeSpan(0);
            while (true)
            {
                var next = start + TimeSpan.FromHours(1);
                var deltaTime = next - start;

                earth.AccelerateTo(sol, deltaTime);
                sol.AccelerateTo(earth, deltaTime);
                earth.Update(deltaTime);
                sol.Update(deltaTime);

                start = next;
                if (start.TotalDays >= 370)
                {
                    break;
                }
            }
            Assert.AreEqual(true, earth.Position.Distance(new Vector3(150000000000, 0, 0)) < 3015200000);
        }

        [TestMethod]
        public void TestGravityDrop()
        {
            var earth = new Navigation.lib.GravitationSource(
                5.9722E24,
                6357000,
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 0)
            );

            var height = new Vector3(0, 6357000 + 54, 0);
            var book = new Navigation.lib.GravitationSource(
                1,
                0.05,
                height,
                new Vector3()
            );

            var start = new TimeSpan(0);
            while (true)
            {
                var next = start + TimeSpan.FromSeconds(0.01);
                var deltaTime = next - start;

                book.AccelerateTo(earth, deltaTime);
                book.Update(deltaTime);

                if (book.IsCollidingWith(earth)) break;

                start = next;
                if (start.TotalDays >= 365.2564)
                {
                    break;
                }
            }

            Assert.AreEqual(3.31, start.TotalSeconds);
        }
    }
}
