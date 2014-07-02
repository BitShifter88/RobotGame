using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks
{
    public class ClasicStyle : VisualTurretStyle
    {
        public ClasicStyle(ResourceManager content)
        {
            MainTexture = new Sprite(content.LoadYunaTexture("Textures/Tanks/Turrets/turretBigNew"));
            MainTexture.DepthLayer = 0.2f;
        }
    }
}
