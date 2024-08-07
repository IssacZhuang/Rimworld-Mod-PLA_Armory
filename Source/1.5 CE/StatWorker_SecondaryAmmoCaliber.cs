﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using CombatExtended;
using CombatExtended.Compatibility;

namespace PLA.CE
{
    class StatWorker_SecondaryAmmoCaliber:StatWorker
    {
        private ThingDef GunDef(StatRequest req)
        {
            var def = req.Def as ThingDef;

            if (def?.building?.IsTurret ?? false)
                def = def.building.turretGunDef;

            return def;
        }

        private Thing Gun(StatRequest req)
        {
            return (req.Thing as Building_Turret)?.GetGun() ?? req.Thing;
        }

        public override bool ShouldShowFor(StatRequest req)
        {
            if (!base.ShouldShowFor(req)) return false;

            CompProperties_SecondaryAmmo compAmmoProp = GunDef(req)?.GetCompProperties<CompProperties_SecondaryAmmo>();

            if (compAmmoProp != null && !compAmmoProp.showSecondaryAmmoStat) return false;

            AmmoSetDef ammoSet = compAmmoProp?.secondaryAmmoProps.ammoSet;
            if (ShouldDisplayAmmoSet(ammoSet))
            {
                return true;
            }
            return false;
        }

        public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
        {
            var ammoSet = GunDef(statRequest)?.GetCompProperties<CompProperties_SecondaryAmmo>()?.secondaryAmmoProps.ammoSet;
            if (ShouldDisplayAmmoSet(ammoSet))
            {
                foreach (var ammoType in ammoSet.ammoTypes)
                {
                    yield return new Dialog_InfoCard.Hyperlink(ammoType.ammo);
                }
            }
        }

        public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var ammoSet = GunDef(req)?.GetCompProperties<CompProperties_SecondaryAmmo>()?.secondaryAmmoProps.ammoSet;
            if (ShouldDisplayAmmoSet(ammoSet))
            {
                // Append various ammo stats
                stringBuilder.AppendLine(ammoSet.LabelCap + "\n");
                foreach (var cur in ammoSet.ammoTypes)
                {
                    string label = string.IsNullOrEmpty(cur.ammo.ammoClass.LabelCapShort) ? (string)cur.ammo.ammoClass.LabelCap : cur.ammo.ammoClass.LabelCapShort;
                    stringBuilder.AppendLine(label + ":\n" + cur.projectile.GetProjectileReadout(Gun(req)));   //Is fine handling req.Thing == null, then it sets mult = 1
                }
            }
            else
            {
                var projectiles = GunDef(req)?.Verbs?.Where(x => x.defaultProjectile != null).Select(x => x.defaultProjectile);

                foreach (var cur in projectiles)
                    stringBuilder.AppendLine(cur.LabelCap + ":\n" + cur.GetProjectileReadout(Gun(req)));
            }

            return stringBuilder.ToString().TrimEndNewlines();
        }

        public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
        {
            var ammoSet = GunDef(optionalReq)?.GetCompProperties<CompProperties_SecondaryAmmo>()?.secondaryAmmoProps.ammoSet;
            if (ShouldDisplayAmmoSet(ammoSet))
            {
                return ammoSet?.LabelCap;
            }
            else
            {
                var projectiles = GunDef(optionalReq)?.Verbs?.Where(x => x.defaultProjectile != null).Select(x => x.defaultProjectile);
                return projectiles.First().LabelCap + (projectiles.Count() > 1 ? "(+" + (projectiles.Count() - 1) + ")" : "");
            }
        }

        private bool ShouldDisplayAmmoSet(AmmoSetDef ammoSet)
        {
            return ammoSet != null && AmmoUtility.IsAmmoSystemActive(ammoSet);
        }
    }
}
