
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    class Turret : TankComponent
    {
        public List<Plugin> Plugins { get; set; }
        public Plugin[] Top { get; protected set; }
        public Plugin[] Buttom { get; protected set; }
        public Plugin[] Left { get; protected set; }
        public Plugin[] Right { get; protected set; }

        public Turret()
        {
            Plugins = new List<Plugin>();
        }

        public bool AddPluginTop(Plugin plugin, int startIndex)
        {
            // Cheks if there are open slots for the plugin
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                if (Top[i] != null)
                    return false;
            }
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                Top[i] = plugin;
            }

            SetPluginOriginTop(plugin);

            Plugins.Add(plugin);

            return true;
        }

        private void SetPluginOriginTop(Plugin plugin)
        {
            Vector2 origin = new Vector2();
            origin.Y = plugin.Sprite.Texture.Height + Sprite.Texture.Height / 2;
            origin.X = (Sprite.Texture.Width - 20) / 2;
            plugin.Sprite.Origin = origin;
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;

            foreach (Plugin p in Plugins)
            {
                p.Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;
                p.Update(dt);
            }
        }

        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);

            List<Plugin> drawn = new List<Plugin>();

            foreach (Plugin p in Top)
            {
                if (drawn.Contains(p))
                    continue;
                else
                {
                    p.Draw(render, camera);
                    drawn.Add(p);
                }
            }

        }
    }
}
