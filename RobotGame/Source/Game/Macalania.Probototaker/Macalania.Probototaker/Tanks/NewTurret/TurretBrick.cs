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
        // Number of pixels for the brick (both width and height)
        int _dim = 16;

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

            _origin = new Vector2(-x * _dim, -y * _dim);
        }

        public override void Draw(YunaEngine.Rendering.IRender render, Camera camera)
        {
            _tank.TurretStyle.MainTexture.Origin = _origin;
            _tank.TurretStyle.MainTexture.Position = _tank.Position; 
            _tank.TurretStyle.MainTexture.Rotation = _tank.TurretRotation + _tank.BodyRotation;
            _tank.TurretStyle.MainTexture.Draw(render, camera, new Rectangle(_x * _dim, _y * _dim, _dim, _dim));
            
        }
    }
}


