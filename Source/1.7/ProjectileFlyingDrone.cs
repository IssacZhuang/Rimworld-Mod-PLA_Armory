using System;
using UnityEngine;
using RimWorld;
using Verse;
using Verse.Sound;

namespace PLA
{
    class ProjectileFlyingDrone:ProjectileMissile
    {
		public override Quaternion ExactRotation
		{
			get
			{
				return Quaternion.identity;
			}
		}
	}
}
