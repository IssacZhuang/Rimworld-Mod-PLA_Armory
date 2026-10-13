using System;
using UnityEngine;
using RimWorld;
using Verse;

namespace PLA
{
    class CompProperties_SecondaryVerb : CompProperties
    {
        public CompProperties_SecondaryVerb()
        {
            this.compClass = typeof(CompSecondaryVerb);
        }

        public VerbProperties verbProps = new VerbProperties();

        public string mainCommandIcon = "";
        public string mainWeaponLabel = "";
        public string secondaryCommandIcon = "";
        public string secondaryWeaponLabel = "";
        public string description = "";
    }
}
