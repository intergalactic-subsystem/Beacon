using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Navigation.lib.Math;

namespace Navigation.lib
{
    public class Planet : NamedGravitationSource
    {
        public Star SystemStar { get; private set; }
        public double Atmosphere { get; private set; }
        public List<Moon> Moons { get; private set; }

        private Vector3 findLagrange(double direction, double start)
        {
            // walk the gravity potential between this planet and the star, looking for L1ish
            Func<double, Vector3> move = percentage => Position.Interpolate(SystemStar.Position, percentage, true);
            var percent = start;
            var body = new GravitationSource(100, 10, move(percent), new Vector3(0, 0, 0));
            var toStar = body.ForceTo(SystemStar);
            var toPlanet = body.ForceTo(this) + Moons.Sum((moon => body.ForceTo(moon)));
            var movedToStar = false;
            var movedToPlanet = false;

            while (System.Math.Abs(toStar - toPlanet) > 0.01)
            {
                if (toStar > toPlanet)
                {
                    body.Position = move(percent += direction);
                    movedToStar = true;
                }
                else if (toStar < toPlanet)
                {
                    body.Position = move(percent -= direction);
                    movedToPlanet = true;
                }

                toStar = body.ForceTo(SystemStar);
                toPlanet = body.ForceTo(this);

                if (!movedToStar || !movedToPlanet) continue;

                direction *= 0.5;
                movedToPlanet = false;
                movedToStar = false;
            }

            return body.Position;
        }

        private Vector3? _l1;
        public Vector3 L1
        {
            get
            {
                if (_l1 != null) return _l1.Value;

                _l1 = findLagrange(-0.2, 0.0);

                return _l1.Value;
            }
        }

        private Vector3? _l2;

        public Vector3 L2
        {
            get
            {
                if (_l2 != null) return _l2.Value;

                _l2 = findLagrange(0.2, 0.0);

                return _l2.Value;
            }
        }

        private Planet(Star star, string name, double mass, double radius, double atmostphere, Vector3 position, Vector3 velocity) : base(name, mass, radius, position, velocity)
        {
            SystemStar = star;
            Atmosphere = atmostphere;
            Moons = new List<Moon>();
            star.AddPlanet(this);
        }

        internal void AddMoon(Moon moon)
        {
            Moons.Add(moon);
        }

        public static Planet Create(string name, Star star, double mass, double radius, double distance, double atmosphere = 0)
        {
            return new Planet(star, name, mass, radius, atmosphere, new Vector3(distance, 0, 0), new Vector3(0, 0, 0));
        }
    }
}
