
using UnityEngine;
using Verse;

namespace PLA
{
    public class Verb_LaunchProjectileStaticWithIcon : Verb_LaunchProjectileStatic
    {

        private Texture2D _commandIconCache;
        public override Texture2D UIIcon
        {
            get
            {
                if (verbProps.commandIcon != null)
                {
                    if (_commandIconCache == null)
                    {
                        _commandIconCache = ContentFinder<Texture2D>.Get(verbProps.commandIcon);
                    }
                    return _commandIconCache;
                }
                return base.UIIcon;
            }
        }
    }
}
