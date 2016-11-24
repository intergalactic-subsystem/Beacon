using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.lib.Math;

namespace Navigation.lib
{
    public class NamedGravitationSource : GravitationSource
    {
        public string Name { get; private set; }

        public NamedGravitationSource(string name, double mass, double radius, Vector3 position, Vector3 velocity) : base(mass, radius, position, velocity)
        {
            Name = name;
        }
    }
}
