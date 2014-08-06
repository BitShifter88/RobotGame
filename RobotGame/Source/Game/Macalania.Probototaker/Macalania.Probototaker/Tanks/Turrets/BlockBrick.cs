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
        Sprite _sprite;
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
            _sprite = new Sprite(content.LoadYunaTexture("Textures/Garage/blockBrick"));
            base.Load(content);
        }

        public override void Draw(YunaEngine.Rendering.IRender render, YunaEngine.Graphics.Camera camera)
        {
            render.Draw(_sprite.Texture, _tank.Position, _origin, new Color(255, 255, 255, 100));
            base.Draw(render, camera);
        }
    }
}
