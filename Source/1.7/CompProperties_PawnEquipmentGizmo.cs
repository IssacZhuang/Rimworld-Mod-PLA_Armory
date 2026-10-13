using System;
using UnityEngine;
using RimWorld;
using Verse;

namespace PLA
{
    class CompProperties_PawnEquipmentGizmo : CompProperties
    {
        public CompProperties_PawnEquipmentGizmo()
        {
            this.compClass = typeof(CompPawnEquipmentGizmo);
        }
    }
}
