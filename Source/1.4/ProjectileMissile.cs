using System;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;

namespace PLA
{
    class ProjectileMissile: Projectile_Explosive
    {
        public override void Tick()
        {
			if (intendedTarget.Thing != null)
			{
                this.destination = intendedTarget.Thing.DrawPos;
            }
            base.Tick();
        }
	}
}
