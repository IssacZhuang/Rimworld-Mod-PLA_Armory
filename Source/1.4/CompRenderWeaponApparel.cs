using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using RimWorld;
using UnityEngine;

namespace PLA
{
    public class CompRenderWeaponApparel:ThingComp
    {
		public Pawn Holder
        {
            get
            {
				return this.holder;
            }
        }

		public CompProperties_RenderWeaponApperal Props
        {
            get
            {
				return (CompProperties_RenderWeaponApperal)this.props;
            }
        }

		private Pawn holder;

        public override void Notify_Equipped(Pawn pawn)
        {
			holder = pawn;
			pawn.Map?.GetComponent<MapComponent_RenderWeaponBackpack>().Register(this);
		}

        public override void Notify_Unequipped(Pawn pawn)
        {
			holder = null;
			pawn.Map?.GetComponent<MapComponent_RenderWeaponBackpack>().Deregister(this);
		}

		public void TryUpdateMap()
        {
			this.Holder?.Map?.GetComponent<MapComponent_RenderWeaponBackpack>().Register(this);
		}

		public override void PostExposeData()
        {
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				holder = (this.parent.ParentHolder as Pawn_EquipmentTracker)?.pawn;
				if (holder != null)
				{
					holder.Map?.GetComponent<MapComponent_RenderWeaponBackpack>().Register(this);
				}
			}
		}

        public void RenderWeaponApparel()
        {
            if (this.holder == null || !this.holder.def.race.Humanlike)
            {
				return;
            }

			PawnRenderFlags pawnRenderFlags = this.GetDefaultRenderFlags(this.holder);
			pawnRenderFlags |= PawnRenderFlags.Clothes;
			pawnRenderFlags |= PawnRenderFlags.Headgear;

			ApparelProperties apparelProps = this.Props.apparel;

			Rot4 bodyFacing = holder.Rotation;
			Vector3 shellLoc = holder.DrawPos;

			Quaternion quaternion = Quaternion.AngleAxis(0f, Vector3.up);

			Mesh bodyMesh = MeshPool.humanlikeBodySet.MeshAt(bodyFacing);

			if (bodyFacing != Rot4.North)
			{
				shellLoc.y += 0.02027027f;
				shellLoc.y += 0.01f;
			}
			else
			{
				shellLoc.y += 0.023166021f;
			}

			Vector3 utilityLoc = shellLoc;
			//utilityLoc.y += ((bodyFacing == Rot4.South) ? 0.0057915053f : 0.028957527f);
			utilityLoc.y += ((bodyFacing == Rot4.South) ? -0.05f : 0.028957527f);

			string path;
			
			if (apparelProps.LastLayer == ApparelLayerDefOf.Overhead || apparelProps.LastLayer == ApparelLayerDefOf.EyeCover || apparelProps.wornGraphicData.renderUtilityAsPack || apparelProps.wornGraphicPath == BaseContent.PlaceholderImagePath || apparelProps.wornGraphicPath == BaseContent.PlaceholderGearImagePath)
			{
				path = apparelProps.wornGraphicPath;
			}
			else
			{

				path = apparelProps.wornGraphicPath + "_" + this.holder.story.bodyType.defName;
			}

			Shader shader = ShaderDatabase.Cutout;
			if (apparelProps.useWornGraphicMask)
			{
				shader = ShaderDatabase.CutoutComplex;
			}
			Graphic graphic = GraphicDatabase.Get<Graphic_Multi>(path, shader, this.Props.drawSize, this.Props.color);

			//ApparelGraphicRecord apparelGraphicRecord = apparelGraphics[i];
			if (apparelProps.LastLayer == ApparelLayerDefOf.Shell && !apparelProps.shellRenderedBehindHead)
			{
				Material material = graphic.MatAt(bodyFacing, null);
				material = (pawnRenderFlags.FlagSet(PawnRenderFlags.Cache) ? material : this.OverrideMaterialIfNeeded(material, this.holder, pawnRenderFlags.FlagSet(PawnRenderFlags.Portrait)));
				Vector3 loc = shellLoc;
				if (apparelProps.shellCoversHead)
				{
					loc.y += 0.0028957527f;
				}
				GenDraw.DrawMeshNowOrLater(bodyMesh, loc, quaternion, material, pawnRenderFlags.FlagSet(PawnRenderFlags.DrawNow));
			}

			if (apparelProps.wornGraphicData.renderUtilityAsPack)
			{
				Material material2 = graphic.MatAt(bodyFacing, null);
				material2 = (pawnRenderFlags.FlagSet(PawnRenderFlags.Cache) ? material2 : this.OverrideMaterialIfNeeded(material2, this.holder, pawnRenderFlags.FlagSet(PawnRenderFlags.Portrait)));
				if (apparelProps.wornGraphicData != null)
				{
					Vector2 vector = apparelProps.wornGraphicData.BeltOffsetAt(bodyFacing, this.holder.story.bodyType);
					Vector2 vector2 = apparelProps.wornGraphicData.BeltScaleAt(bodyFacing, this.holder.story.bodyType);
					Matrix4x4 matrix = Matrix4x4.Translate(utilityLoc) * Matrix4x4.Rotate(quaternion) * Matrix4x4.Translate(new Vector3(vector.x, 0f, vector.y)) * Matrix4x4.Scale(new Vector3(vector2.x, 1f, vector2.y));
					GenDraw.DrawMeshNowOrLater(bodyMesh, matrix, material2, pawnRenderFlags.FlagSet(PawnRenderFlags.DrawNow));
				}
				else
				{
					GenDraw.DrawMeshNowOrLater(bodyMesh, shellLoc, quaternion, material2, pawnRenderFlags.FlagSet(PawnRenderFlags.DrawNow));
				}
			}
		}

		private PawnRenderFlags GetDefaultRenderFlags(Pawn pawn)
		{
			PawnRenderFlags pawnRenderFlags = PawnRenderFlags.None;
			if (pawn.IsInvisible())
			{
				pawnRenderFlags |= PawnRenderFlags.Invisible;
			}
			if (!pawn.health.hediffSet.HasHead)
			{
				pawnRenderFlags |= PawnRenderFlags.HeadStump;
			}
			return pawnRenderFlags;
		}

		private Material OverrideMaterialIfNeeded(Material original, Pawn pawn, bool portrait = false)
		{
			Material baseMat = (!portrait && pawn.IsInvisible()) ? InvisibilityMatPool.GetInvisibleMat(original) : original;
			return this.holder.Drawer.renderer.graphics.flasher.GetDamagedMat(baseMat);
		}

		//public static List<CompRenderWeaponApparel> cachedComp = new List<CompRenderWeaponApparel>();
	}
}
