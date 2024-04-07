using System;
using UnityEngine;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace PLA
{
    class CompSecondaryVerb:ThingComp
    {
		public CompProperties_SecondaryVerb Props
		{
			get
			{
				return (CompProperties_SecondaryVerb)this.props;
			}
		}

        public bool IsSecondaryVerbSelected { 
			get {
				return this.isSecondaryVerbSelected;
			} 
		}

        private CompEquippable EquipmentSource
        {
            get
            {
				if (compEquippableInt != null)
                {
					return this.compEquippableInt;
                }
				this.compEquippableInt = this.parent.TryGetComp<CompEquippable>();
				if (compEquippableInt == null)
				{
					Log.ErrorOnce(this.parent.LabelCap + " has CompSecondaryVerb but no CompEquippable", 50020);
					
				}
				return this.compEquippableInt;
			}
        }

		public Pawn CasterPawn
		{
			get
			{
				return this.Verb.caster as Pawn;
			}
		}


		private Verb Verb
		{
			get
			{
				if (this.verbInt == null)
				{
					this.verbInt = this.EquipmentSource.PrimaryVerb;
				}
				return this.verbInt;
			}
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			//Log.Message("1");
			if (this.CasterPawn != null && !this.CasterPawn.Faction.Equals(Faction.OfPlayer))
			{
				yield break;
			}
			//Log.Message("2");

			string commandIcon = IsSecondaryVerbSelected ? this.Props.secondaryCommandIcon : this.Props.mainCommandIcon;

			if (commandIcon == "")
			{
				commandIcon = "UI/Buttons/Reload";
			}

			Command_Action switchSecondaryLauncher = new Command_Action
			{
				action = new Action(this.SwitchVerb),
				defaultLabel = IsSecondaryVerbSelected ? this.Props.secondaryWeaponLabel : this.Props.mainWeaponLabel,
				defaultDesc = this.Props.description,
				icon = ContentFinder<Texture2D>.Get(commandIcon, false),
				//tutorTag = "Switch between rifle and grenade launcher"
			};
			yield return switchSecondaryLauncher;

			yield break;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();

			Scribe_Values.Look<bool>(ref this.isSecondaryVerbSelected, "PLA_useSecondaryVerb", false);

			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.PostAmmoDataLoaded();
			}
		}

		private void SwitchVerb()
        {
            if (!IsSecondaryVerbSelected)
            {
				this.EquipmentSource.PrimaryVerb.verbProps = this.Props.verbProps;
				this.isSecondaryVerbSelected = true;
				return;
			}
			this.EquipmentSource.PrimaryVerb.verbProps = this.parent.def.Verbs[0];
			this.isSecondaryVerbSelected = false;
		}
		private void PostAmmoDataLoaded()
        {
            if (isSecondaryVerbSelected)
            {
				this.EquipmentSource.PrimaryVerb.verbProps = this.Props.verbProps;
			}
        }


		private Verb verbInt = null;
		private CompEquippable compEquippableInt;
		private bool isSecondaryVerbSelected;
	}
}
