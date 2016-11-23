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

        private GravitationSource Moon = new GravitationSource(
            7.34767309E+22,
            1737000,
            new Vector3(150384400000, 0, 0),
            new Vector3(0, 0, 30822)
        );

        private bool? count = null;

        private DateTime start = DateTime.Now;
        private DateTime epoch;
        private double smallest;

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
            var deltaTime = TimeSpan.FromHours(1);
            var earthm = (Earth.Position - new Vector3(150000000000, 0, 0)).Magnitude;
            if (count.HasValue && earthm < 1400000000)
                count = false;
            else if (count.HasValue == false && earthm > 300000000000)
                count = true;

            if (count.HasValue == false || count.Value)
            {
                start += deltaTime;
                if (count != null && count.Value)
                    smallest = earthm < smallest || smallest == 0 ? earthm : smallest;
            }
            var origin = new Vector3(Width/2.0, 0, Height/2.0);
            var max = 500000000000.0/ System.Math.Min(Width, Height);
            var sunpos = origin - Sol.Position/max;
            var earthpos = origin - Earth.Position/max;
            //var moonpos = origin - Moon.Position/max;

            Earth.AccelerateTo(Sol, deltaTime);
            Sol.AccelerateTo(Earth, deltaTime);
            //Moon.AccelerateTo(Earth, deltaTime);
            //Moon.AccelerateTo(Sol, deltaTime);
            Earth.Update(deltaTime);
            Sol.Update(deltaTime);
            //Moon.Update(deltaTime);

            Pen status = Pens.Brown;

            if (count.HasValue == false) status = Pens.Red;
            if (count.HasValue && count.Value) status = Pens.Yellow;
            if (count.HasValue && !count.Value) status = Pens.Green;

            Rectangle r = new Rectangle((int) sunpos.X, (int) sunpos.Z, 5, 5);
            Rectangle s = new Rectangle((int) earthpos.X, (int) earthpos.Z, 2, 2);
            //Rectangle m = new Rectangle((int) moonpos.X, (int) moonpos.Z, 1, 1);
            e.Graphics.DrawRectangle(status, r);
            e.Graphics.DrawRectangle(Pens.Blue, s);
            //e.Graphics.DrawRectangle(Pens.Azure, m);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = smallest
                               + " -- days: " + Math.Round((start - epoch).TotalDays, 3)
                               + " -- " + (Earth.Position - new Vector3(150000000000, 0, 0)).Magnitude
                               + "m -- " + start.ToLongDateString();
            Refresh();
        }
    }
}
