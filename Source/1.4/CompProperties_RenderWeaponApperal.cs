using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;
using UnityEngine;

namespace PLA
{
    public class CompProperties_RenderWeaponApperal :CompProperties
    {
        public ApparelProperties apparel;
        public Vector2 drawSize = Vector2.one;
        public Color color = Color.white;

        public CompProperties_RenderWeaponApperal()
        {
            this.compClass=typeof(CompRenderWeaponApparel);
        }

        public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
        {

            if (apparel == null)
            {
                yield return "CompProperties_RenderWeaponApperal has no prop: apparel";
            }
            else if (apparel.wornGraphicData == null)
            {
                apparel.wornGraphicData = new WornGraphicData();
            }
        }
    }
}
