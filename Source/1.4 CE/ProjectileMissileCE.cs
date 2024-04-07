using System;
using Verse;
using UnityEngine;
using CombatExtended;
using RimWorld;

namespace PLA.CE
{
    class ProjectileMissileCE: BulletCE
    {
        public override void Tick()
        {
            //Log.Message(this.StartingTicksToImpact.ToString());
            if (this.intendedTargetThing != null)
            {
                this.destinationInt.x = intendedTargetThing.DrawPos.x;
                this.destinationInt.y = intendedTargetThing.DrawPos.z;
                this.Destination = destinationInt;
            }
            else
            {
                this.destinationInt.x = intendedTarget.Cell.x;
                this.destinationInt.y = intendedTarget.Cell.z;
                this.Destination = destinationInt;
            }
            base.Tick();
        }
    }
}
