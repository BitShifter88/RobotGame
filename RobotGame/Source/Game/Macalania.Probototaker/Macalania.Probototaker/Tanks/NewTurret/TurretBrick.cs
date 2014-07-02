using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.NewTurret
{
    public class TurretBrick : TurretComponent
    {
        Tank _tank;
        Vector2 _origin;

        public TurretBrick(Tank tank)
        {
            CanAttachBottom = true;
            CanAttachLeft = true;
            CanAttachRight = true;
            CanAttachTop = true;

            _tank = tank;
        }

        public override void SetLocation(int x, int y)
        {
            base.SetLocation(x, y);

            x = x - 32;
            y = y - 32;

            _origin = new Vector2( - x * 8,  - y * 8);
        }

        public override void Draw(YunaEngine.Rendering.IRender render, Camera camera)
        {
            _tank.TurretStyle.MainTexture.Origin = _origin;
            _tank.TurretStyle.MainTexture.Position = _tank.Position; //+ new Vector2(_x * 8, _y * 8);
            _tank.TurretStyle.MainTexture.Rotation = _tank.TurretRotation + _tank.BodyRotation;
            _tank.TurretStyle.MainTexture.Draw(render, camera, new Rectangle(_x * 8, _y * 8 , 8, 8));
            
        }
    }
}
