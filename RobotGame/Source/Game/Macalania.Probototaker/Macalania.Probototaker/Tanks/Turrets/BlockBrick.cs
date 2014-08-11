using Macalania.YunaEngine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    class BlockBrick : TurretComponent
    {
        public TurretModule Owner { get; set; }
        Tank _tank;
        Vector2 _origin;
        int _dim = 16;

        public BlockBrick(TurretModule module, Tank tank)
        {
            _tank = tank;
            Owner = module;
        }

        public override void SetLocation(int x, int y)
        {
            base.SetLocation(x, y);

            x = x - 16;
            y = y - 16;

            _origin = new Vector2(-x * _dim, -y * _dim);
        }

        public override void Load(YunaEngine.Resources.ResourceManager content)
        {
            Sprite = new Sprite(content.LoadYunaTexture("Textures/Garage/blockBrick"));
            base.Load(content);
        }

        public override void Draw(YunaEngine.Rendering.IRender render, YunaEngine.Graphics.Camera camera)
        {
#if DEBUG
            render.Draw(Sprite.Texture, _tank.Position, _origin, new Color(1f, 1f, 1f, 0.1f));
#endif
            base.Draw(render, camera);
        }
    }
}
