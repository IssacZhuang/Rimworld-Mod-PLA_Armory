using System;
using Verse;
using UnityEngine;
using CombatExtended;
using RimWorld;

namespace PLA.CE
{
    public class ProjectileGuidedBulletCE: BulletCE
    {
        public float Distance
        {
            get
            {
                return distance;
            }
        }

        public Vector3 Origin
        {
            get { return launcherPos; }
        }
        private Vector3 Direction
        {
            get {
                dicrectionInt.x = TargetPos.x - launcherPos.x;
                dicrectionInt.y = TargetPos.y - launcherPos.y;
            
                return dicrectionInt.normalized;
            }
        }

        public Vector3 TargetPos
        {
            get
            {
                targetPosInt.x = intendedTargetThing.DrawPos.x;
                targetPosInt.y = intendedTargetThing.DrawPos.z;
                return targetPosInt;
            }
        }

        private Vector3 GuidedDestination
        {
            get
            {
                return launcherPos + (Direction * distance);
            }
        }

        private float distance;
        private Vector3 targetPosInt;
        private Vector3 dicrectionInt;
        private Vector3 launcherPos;
        public ProjectileGuidedBulletCE()
        {
            targetPosInt = new Vector3();
            dicrectionInt = new Vector3();
            this.launcherPos = new Vector3();
        }
        
        public override void Launch(Thing launcher, Vector2 origin, Thing equipment = null)
        {

            this.launcherPos.x = origin.x;
            this.launcherPos.y = origin.y;
            //base.Launch(launcher, origin, shotAngle, shotRotation, shotHeight, shotSpeed);
            base.Launch(launcher, origin, equipment);
            double x = this.Destination.x - origin.x;
            double y = this.Destination.y - origin.y;
            distance = (float)Math.Sqrt(x * x + y * y);

            //Log.Message("origin: " + launcherPos + ", direction: " + Direction + ", distance" + distance);
        }

        public override void Tick()
        {
            //Log.Message("Dest:" + destinationInt + ", Guided:" + GuidedDestination + ", Origin:" + origin + ", Dis:" + distance + ", Direct:" + Direction);
            if (intendedTargetThing!=null)
            {
                this.destinationInt.x = GuidedDestination.x;
                this.destinationInt.y = GuidedDestination.y;
            }
            base.Tick();
        }

    }
}
