using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using RimWorld;
using Verse;

namespace PLA
{
    public class MapComponent_RenderWeaponBackpack : MapComponent
    {
        public MapComponent_RenderWeaponBackpack(Map map) : base(map)
        {
            
        }

        public override void MapComponentUpdate()
        {
            /*int length = compRenders.Count;
            for (int i = 0; i < length; i++)
            {
                if (compRenders[i].parent?.Map != null) compRenders[i].RenderWeaponApparel();
            }*/
        }

        public override void MapComponentTick()
        {
            tmpCompRenderToTransfer.Clear();
            tmpCompRenderToTransfer.AddRange(compRenders.Where(x => x.Holder?.Map != null && x.Holder.Map != this.map));
            for(int i = 0; i < tmpCompRenderToTransfer.Count; i++)
            {
                this.Deregister(tmpCompRenderToTransfer[i]);
                tmpCompRenderToTransfer[i].TryUpdateMap();
            }
        }

        public void Register(CompRenderWeaponApparel value)
        {
            compRenders.Add(value);
        }

        public void Deregister(CompRenderWeaponApparel value)
        {
            compRenders.Remove(value);
        }

        private static List<CompRenderWeaponApparel> compRenders = new List<CompRenderWeaponApparel>();
        private static List<CompRenderWeaponApparel> tmpCompRenderToTransfer = new List<CompRenderWeaponApparel>();
    }
}
