using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.lib.Math;

namespace Navigation.lib
{
    public class Moon : NamedGravitationSource
    {
        public Planet Body { get; private set; }

        private Moon(Planet planet, string name, double mass, double radius, Vector3 position, Vector3 velocity) : base(name, mass, radius, position, velocity)
        {
            planet.AddMoon(this);
            Body = planet;
        }

        public static Moon Create(string name, Planet planet, double mass, double radius, double distance)
        {
            return new Moon(planet, name, mass, radius, new Vector3(planet.Position.X + distance, 0, 0),
                new Vector3(0, 0, 0));
        }
    }
}
