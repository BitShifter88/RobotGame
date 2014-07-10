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
            MainTexture = new Sprite(content.LoadYunaTexture("Textures/Tanks/Styles/Classic/main"));
            Cluder = new Sprite(content.LoadYunaTexture("Textures/Tanks/Styles/Classic/desertTankCluder"));
            CornersLeftTop = new Sprite(content.LoadYunaTexture("Textures/Tanks/Styles/Classic/cornersLeftTop"));
            CornersRightTop = new Sprite(content.LoadYunaTexture("Textures/Tanks/Styles/Classic/cornersRightTop"));
            CornersLeftBottom = new Sprite(content.LoadYunaTexture("Textures/Tanks/Styles/Classic/cornersLeftBottom"));
            MainTexture.DepthLayer = 0.2f;
            Cluder.DepthLayer = 0.21f;
        }
    }
}
