using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.lib.Math;

namespace Navigation.lib
{
    /// <summary>
    /// A star in the middle of a system
    /// </summary>
    public class Star : NamedGravitationSource
    {
        public List<Planet> Planets { get; private set; }

        private Star(string name, double mass, double radius, Vector3 position, Vector3 velocity)
            : base(name, mass, radius, position, velocity)
        {
            Planets = new List<Planet>();
        }

        internal void AddPlanet(Planet planet)
        {
            Planets.Add(planet);
        }

        public static Star Create(string name, double mass, double radius)
        {
            return new Star(name, mass, radius, new Vector3(1, 1, 1), new Vector3(0, 0, 0));
        }
    }
}
