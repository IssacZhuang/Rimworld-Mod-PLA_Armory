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
                this.Destination.x = intendedTargetThing.DrawPos.x;
                this.Destination.y = intendedTargetThing.DrawPos.z;
            }
            else
            {
                this.Destination.x = intendedTarget.Cell.x;
                this.Destination.y = intendedTarget.Cell.z;
            }
            base.Tick();
        }
    }
}
