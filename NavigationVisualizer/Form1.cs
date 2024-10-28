using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Navigation.lib;
using Navigation.lib.Math;

namespace NavigationVisualizer
{
    public partial class Form1 : Form
    {
        private GravitationSource Sol = new Navigation.lib.GravitationSource(
            1.989E+030,
            695000000,
            new Vector3(1, 1, 1),
            new Vector3(0, 0, 0)
        );

        private GravitationSource Earth = new Navigation.lib.GravitationSource(
            5.976E+024,
            6378140,
            new Vector3(150000000000, 0, 0),
            new Vector3(0, 0, 29800)
        );

        //private GravitationSource Moon = new GravitationSource(
        //    7.34767309E+22,
        //    1737000,
        //    new Vector3(150384400000, 0, 0),
        //    new Vector3(0, 0, 30822)
        //);

        private GravitationSource Mercury = new Navigation.lib.GravitationSource(
            3.3011E+23,               // Mass of Mercury in kg
            2439700,                  // Radius of Mercury in meters
            new Vector3(57910000000, 0, 0),  // Initial position (approx. distance from Sun in meters)
            new Vector3(0, 0, 47400)         // Orbital velocity in meters per second
        );

        private GravitationSource Venus = new Navigation.lib.GravitationSource(
            4.8675E+24,               // Mass of Venus in kg
            6051800,                  // Radius of Venus in meters
            new Vector3(108200000000, 0, 0),  // Initial position (approx. distance from Sun in meters)
            new Vector3(0, 0, 35020)         // Orbital velocity in meters per second
        );

        private GravitationSource Mars = new Navigation.lib.GravitationSource(
            6.4171E+23,                // Mass of Mars in kg
            3389500,                   // Radius of Mars in meters
            new Vector3(227940000000, 0, 0),  // Initial position (approx. distance from Sun in meters)
            new Vector3(0, 0, 24077)           // Orbital velocity in meters per second
        );

        private bool? count = null;

        private DateTime start = DateTime.Now;
        private DateTime epoch;
        private double smallest;

        private Queue<Vector3> trail = new Queue<Vector3>();
        private Queue<Vector3> mercuryTrail = new Queue<Vector3>();
        private Queue<Vector3> venusTrail = new Queue<Vector3>();
        private Queue<Vector3> marsTrail = new Queue<Vector3>();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            epoch = start;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var earthm = (Earth.Position - new Vector3(150000000000, 0, 0)).Magnitude;
            if (count.HasValue && earthm < 1400000000)
                count = false;
            else if (count.HasValue == false && earthm > 300000000000)
                count = true;

            if (count.HasValue == false || count.Value)
            {
                if (count != null && count.Value)
                    smallest = earthm < smallest || smallest == 0 ? earthm : smallest;
            }
            var origin = new Vector3(Width/2.0, 0, Height/2.0);
            var max = 500000000000.0/ System.Math.Min(Width, Height);
            var sunpos = origin - Sol.Position/max;
            var earthpos = origin - Earth.Position/max;
            //var moonpos = origin - Moon.Position/max;
            var mercuryPos = origin - Mercury.Position / max;

            var venusPos = origin - Venus.Position / max;
            Rectangle venusRect = new Rectangle((int)venusPos.X, (int)venusPos.Z, 3, 3); // Slightly larger for visibility
            e.Graphics.FillEllipse(Brushes.Goldenrod, venusRect);

            Rectangle mercuryRect = new Rectangle((int)mercuryPos.X, (int)mercuryPos.Z, 2, 2);
            e.Graphics.FillEllipse(Brushes.Orange, mercuryRect);

            var marsPos = origin - Mars.Position / max;
            Rectangle marsRect = new Rectangle((int)marsPos.X, (int)marsPos.Z, 3, 3); // Slightly larger for visibility
            e.Graphics.FillEllipse(Brushes.Red, marsRect);

            trail.Enqueue(earthpos);
            if (trail.Count > 5000)
            {
                trail.Dequeue();
            }

            mercuryTrail.Enqueue(mercuryPos);
            if (mercuryTrail.Count > 1300) // Adjust for a lighter trail if needed
            {
                mercuryTrail.Dequeue();
            }

            venusTrail.Enqueue(venusPos);
            if (venusTrail.Count > 3000) // Adjust the count as needed
            {
                venusTrail.Dequeue();
            }

            marsTrail.Enqueue(marsPos);
            if (marsTrail.Count > 5000) // Adjust the count as needed
            {
                marsTrail.Dequeue();
            }

            var status = Brushes.Brown;

            if (count.HasValue == false) status = Brushes.Red;
            if (count.HasValue && count.Value) status = Brushes.Yellow;
            if (count.HasValue && !count.Value) status = Brushes.Green;

            var trailRect = new Rectangle(0, 0, 1, 1);
            foreach (var t in trail)
            {
                trailRect.X = (int)t.X;
                trailRect.Y = (int)t.Z;
                e.Graphics.DrawRectangle(Pens.Gray, trailRect);
            }

            // Draw Mercury's trail
            foreach (var t in mercuryTrail)
            {
                trailRect.X = (int)t.X;
                trailRect.Y = (int)t.Z;
                e.Graphics.DrawRectangle(Pens.Orange, trailRect);
            }

            foreach (var t in venusTrail)
            {
                trailRect.X = (int)t.X;
                trailRect.Y = (int)t.Z;
                e.Graphics.DrawRectangle(Pens.Goldenrod, trailRect);
            }

            // Draw Mars's trail
            foreach (var t in marsTrail)
            {
                trailRect.X = (int)t.X;
                trailRect.Y = (int)t.Z;
                e.Graphics.DrawRectangle(Pens.Red, trailRect);
            }

            Rectangle r = new Rectangle((int) sunpos.X, (int) sunpos.Z, 5, 5);
            Rectangle s = new Rectangle((int) earthpos.X, (int) earthpos.Z, 2, 2);
            //Rectangle m = new Rectangle((int) moonpos.X, (int) moonpos.Z, 1, 1);
            e.Graphics.FillEllipse(status, r);
            e.Graphics.FillEllipse(Brushes.Blue, s);
            //e.Graphics.DrawRectangle(Pens.Azure, m);
        }

        private void UpdatePhysics(TimeSpan deltaTime)
        {
            Earth.AccelerateTo(Sol, deltaTime);
            Sol.AccelerateTo(Earth, deltaTime);
            //Moon.AccelerateTo(Earth, deltaTime);
            //Sol.AccelerateTo(Moon, deltaTime);
            Mercury.AccelerateTo(Earth, deltaTime);
            Venus.AccelerateTo(Earth, deltaTime);
            Mars.AccelerateTo(Earth, deltaTime);

            //Moon.AccelerateTo(Earth, deltaTime);
            //Moon.AccelerateTo(Sol, deltaTime);
            //Earth.AccelerateTo(Moon, deltaTime);

            Mercury.AccelerateTo(Sol, deltaTime);
            Sol.AccelerateTo(Mercury, deltaTime);
            Earth.AccelerateTo(Mercury, deltaTime);
            Venus.AccelerateTo(Mercury, deltaTime);
            Mars.AccelerateTo(Mercury, deltaTime);

            Venus.AccelerateTo(Sol, deltaTime);
            Sol.AccelerateTo(Venus, deltaTime);
            Earth.AccelerateTo(Venus, deltaTime);
            Mars.AccelerateTo(Venus, deltaTime);

            Mars.AccelerateTo(Sol, deltaTime);
            Sol.AccelerateTo(Mars, deltaTime);
            Earth.AccelerateTo(Mars, deltaTime);
            Venus.AccelerateTo(Mars, deltaTime);
            Mercury.AccelerateTo(Mars, deltaTime);

            Earth.Update(deltaTime);
            Sol.Update(deltaTime);
            //Moon.Update(deltaTime);
            Mercury.Update(deltaTime);
            Venus.Update(deltaTime);
            Mars.Update(deltaTime);
        }

        private TimeSpan fixedTimeStep = TimeSpan.FromMinutes(1);
        private TimeSpan timeMultiplier = TimeSpan.FromDays(12); // Simulate 5 days per real second, adjust as needed
        private double accumulatedTime = 0.0;
        private TimeSpan totalElapsedTime = TimeSpan.Zero;
        private DateTime currentTime = DateTime.Now;

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = accumulatedTime
                               + " -- days: " + Math.Round(totalElapsedTime.TotalDays, 3)
                               + " -- " + (Earth.Position - new Vector3(150000000000, 0, 0)).Magnitude
                               + "m -- " + (epoch + totalElapsedTime).ToLongDateString();

            // Calculate elapsed time since the last frame
            var now = DateTime.Now;
            var deltaTime = (now - start).TotalSeconds;
            if (deltaTime < 0) deltaTime = 0;
            start = now;

            // Scale up the elapsed time for the simulation using the time multiplier
            accumulatedTime += deltaTime * timeMultiplier.TotalSeconds;

            // Update the physics simulation in fixed time steps
            while (accumulatedTime >= fixedTimeStep.TotalSeconds)
            {
                totalElapsedTime += fixedTimeStep;
                UpdatePhysics(fixedTimeStep); // Update with a fixed timestep (1 hour)
                accumulatedTime -= fixedTimeStep.TotalSeconds;
            }

            // Render the latest positions
            Refresh();
        }
    }
}
