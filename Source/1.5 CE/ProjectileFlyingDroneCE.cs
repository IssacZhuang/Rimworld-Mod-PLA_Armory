using System;
using Verse;
using UnityEngine;
using CombatExtended;
using RimWorld;
namespace PLA.CE
{
    class ProjectileFlyingDroneCE : ProjectileGuidedBulletCE
    {
        private Vector3 CurrentPos
        {
            get
            {
                currentPosInt.x = ExactPosition.x;
                currentPosInt.y = ExactPosition.z;
                return currentPosInt;
            }
        }
        private Vector3 currentPosInt;

        public ProjectileFlyingDroneCE()
        {
            currentPosInt = new Vector3();
        }
        public override void Launch(Thing launcher, Vector2 origin, float shotAngle, float shotRotation, float shotHeight = 0, float shotSpeed = -1, Thing equipment = null, float distance = -1)
        {
            base.Launch(launcher, origin, shotAngle, shotRotation, shotHeight, shotSpeed);

        }


        private bool readyToExplode = false;
        public override void Tick()
        {
            base.Tick();
            if (intendedTargetThing != null)
            {
                readyToExplode = Vector3.Distance(CurrentPos, Origin) > Vector3.Distance(TargetPos, Origin);
            }
            else
            {
                readyToExplode = Vector3.Distance(CurrentPos, Origin) > Vector3.Distance(intendedTarget.Cell.ToVector3(), Origin);
            }

            if (readyToExplode)
            {
                Impact(null);
            }

        }

        public override void Impact(Thing hitThing)
        {
            base.Impact(hitThing);
        }

        public override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            Vector3 offset = new Vector3(0, 0, 2);
            offset.z += (float)Math.Sin((float)this.ticksToImpact / 12f) * 0.1f;
            Vector3 position = new Vector3(this.ExactPosition.x, this.def.Altitude - 0.01f, this.ExactPosition.z - Mathf.Lerp(this.shotHeight, 0f, this.FlightTicks / this.startingTicksToImpact));
            if (this.def.projectile.shadowSize > 0f)
            {
                Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), position, this.ExactRotation, ProjectileFlyingDroneCE.CustomShadowMaterial, 0);
            }
            Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), position + offset, Quaternion.identity, this.def.DrawMatSingle, 0);

            base.Comps_PostDraw();
        }

        // public override void Draw()
        // {
        //     Vector3 offset = new Vector3(0, 0, 2);
        //     offset.z += (float)Math.Sin((float)this.ticksToImpact / 12f) * 0.1f;
        //     Vector3 position = new Vector3(this.ExactPosition.x, this.def.Altitude - 0.01f, this.ExactPosition.z - Mathf.Lerp(this.shotHeight, 0f, this.FlightTicks / this.startingTicksToImpact));
        //     if (this.def.projectile.shadowSize > 0f)
        //     {
        //         Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), position, this.ExactRotation, ProjectileFlyingDroneCE.CustomShadowMaterial, 0);
        //     }
        //     Graphics.DrawMesh(MeshPool.GridPlane(this.def.graphicData.drawSize), position + offset, Quaternion.identity, this.def.DrawMatSingle, 0);

        //     base.Comps_PostDraw();

        // }

        private static readonly Material CustomShadowMaterial = MaterialPool.MatFrom("Things/Skyfaller/SkyfallerShadowCircle", ShaderDatabase.Transparent);
	}
}
