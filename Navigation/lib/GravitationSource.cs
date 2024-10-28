using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Navigation.lib.Math;

namespace Navigation.lib
{
    /// <summary>
    /// Creates a large blob of mass... All things derive from this
    /// </summary>
    public class GravitationSource
    {
        public const double G = 6.67408E-11;

        public double Mass { get; private set; }
        public double Radius { get; private set; }
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public Vector3 Acceleration { get; set; }
        private Vector3 _nextAcceleration;

        /// <summary>
        /// Creates a body of mass, in the spherical sense
        /// </summary>
        /// <param name="mass">The mass of the object</param>
        /// <param name="radius">The radius of the mass</param>
        /// <param name="position">The absolute position of the object</param>
        /// <param name="velocity">The velocity of the object</param>
        public GravitationSource(double mass, double radius, Vector3 position, Vector3 velocity)
        {
            Mass = mass;
            Radius = radius;
            Position = position;
            Velocity = velocity;
        }

        /// <summary>
        /// Determine if there is a collision with another body...
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        public bool IsCollidingWith(GravitationSource body)
        {
            var distance = Position.Distance(body.Position);

            return distance - Radius - body.Radius <= 0;
        }

        /// <summary>
        /// Get's the amount of force from this body to another
        /// </summary>
        /// <param name="body">The body to perform calculations on</param>
        /// <returns>The Newtons of force</returns>
        public double ForceTo(GravitationSource body)
        {
            var distance = Position.Distance(body.Position);
            return (G*Mass*body.Mass)/(System.Math.Pow(distance, 2));
        }

        /// <summary>
        /// Gets the amount of acceleration to a body of mass
        /// </summary>
        /// <param name="body">The body to perform calculations on</param>
        /// <returns>The acceleration in m/s/s</returns>
        public double AccelerationTo(GravitationSource body)
        {
            return ForceTo(body)/Mass;
        }

        /// <summary>
        /// Changes the Velocity vector...
        /// </summary>
        /// <param name="force">The amount of force to apply</param>
        /// <param name="direction">The direction the force is pushing the body</param>
        /// <param name="deltaTime">The amount of time since the last calculation</param>
        public void Accelerate(double force, Vector3 direction, TimeSpan deltaTime)
        {
            var normal = direction.Normalize();

            var accel = force/Mass;
            // turn the vector into an acceleration vector
            normal *= accel;
            _nextAcceleration += normal;
        }

        /// <summary>
        /// Accelerates to another body, based on gravitational effects
        /// </summary>
        /// <param name="body">The body to accelerate towards</param>
        /// <param name="deltaTime">The amount of time since the last calculation</param>
        public void AccelerateTo(GravitationSource body, TimeSpan deltaTime)
        {
            var force = ForceTo(body);
            var direction = body.Position - Position;
            Accelerate(force, direction, deltaTime);
        }

        /// <summary>
        /// Updates the position based on current velocity
        /// </summary>
        /// <param name="deltaTime">The amount of time since the last calculation</param>
        public void Update(TimeSpan deltaTime)
        {
            var dt = deltaTime.TotalSeconds;
            Position += Velocity * dt + 0.5 * Acceleration * dt * dt;

            // Update velocity half-step with the current acceleration
            var newVelocity = Velocity + 0.5 * Acceleration * dt;

            // Update acceleration to the next calculated acceleration (from previous Accelerate calls)
            Acceleration = _nextAcceleration;
            _nextAcceleration = new Vector3();

            // Complete the velocity update with the new acceleration
            Velocity = newVelocity + 0.5 * Acceleration * dt;
        }
    }
}
