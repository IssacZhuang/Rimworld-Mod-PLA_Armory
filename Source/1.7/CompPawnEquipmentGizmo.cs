using System;
using UnityEngine;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace PLA
{
	class CompPawnEquipmentGizmo : ThingComp
	{
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			Pawn pawn = this.parent as Pawn;
			ThingWithComps equip = (pawn != null) ? pawn.equipment.Primary : null;
			if (equip != null && !equip.AllComps.NullOrEmpty<ThingComp>())
			{
				foreach (ThingComp comp in equip.AllComps)
				{
					CompSecondaryVerb scondaeryVerb = comp as CompSecondaryVerb;
					if (scondaeryVerb == null)
					{
						continue;
					}
					foreach (Gizmo gizmo in scondaeryVerb.CompGetGizmosExtra())
					{
						yield return gizmo;
					}
					yield break;
				}
			}
			yield break;
		}
	}
}
