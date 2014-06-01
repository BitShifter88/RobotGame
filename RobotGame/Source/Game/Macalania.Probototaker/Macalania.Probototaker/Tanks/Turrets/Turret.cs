﻿
using Macalania.Probototaker.Tanks.Plugins;
using Macalania.Probototaker.Tanks.Plugins.MainGuns;
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

        public int ExtraPixelsTop { get; protected set; }
        public int ExtraPixelsSide { get; set; }
        public int ExtraPixelsButtom { get; set; }


        public Turret()
        {
            Plugins = new List<Plugin>();
        }

        private List<MainGun> GetMainGuns()
        {
            List<MainGun> mainGuns = new List<MainGun>();

            foreach (Plugin p in Plugins)
            {
                if (p.GetType().IsSubclassOf(typeof(MainGun)))
                {
                    mainGuns.Add((MainGun)p);
                }
            }

            return mainGuns;
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

            SetPluginOriginTop(plugin, startIndex);

            Plugins.Add(plugin);

            plugin.PluginPosition = startIndex;

            return true;
        }

        public bool AddPluginRightSide(Plugin plugin, int startIndex)
        {
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                if (Right[i] != null)
                    return false;
            }
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                Right[i] = plugin;
            }

            SetPluginOriginRight(plugin, startIndex);

            Plugins.Add(plugin);
            plugin.PluginPosition = startIndex;

            return true;
        }

        public bool AddPluginLeftSide(Plugin plugin, int startIndex)
        {
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                if (Left[i] != null)
                    return false;
            }
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                Left[i] = plugin;
            }

            SetPluginOriginLeft(plugin, startIndex);

            Plugins.Add(plugin);
            plugin.PluginPosition = startIndex;

            return true;
        }

        private void SetPluginOriginRight(Plugin plugin, int startIndex)
        {
            Vector2 origin = new Vector2();
            origin.X = -(Sprite.Texture.Width / 2);
            origin.Y = (Sprite.Texture.Height - ExtraPixelsSide) / 2 - startIndex * Globals.PluginPixelWidth;
            plugin.Sprite.Origin = origin;
        }


        private void SetPluginOriginLeft(Plugin plugin, int startIndex)
        {
            Vector2 origin = new Vector2();
            origin.X = Sprite.Texture.Width / 2 + plugin.Sprite.Texture.Width;
            origin.Y = (Sprite.Texture.Height - ExtraPixelsSide) / 2 - startIndex * Globals.PluginPixelWidth;
            plugin.Sprite.Origin = origin;
        }

        private void SetPluginOriginTop(Plugin plugin, int startIndex)
        {
            Vector2 origin = new Vector2();
            origin.X = (Sprite.Texture.Width - ExtraPixelsTop) / 2 - startIndex * Globals.PluginPixelWidth;
            origin.Y = plugin.Sprite.Texture.Height + Sprite.Texture.Height / 2;
            plugin.Sprite.Origin = origin;
        }

        public void FireMainGun()
        {
            List<MainGun> mgs = GetMainGuns();

            foreach (MainGun mg in mgs)
            {
                mg.Fire(this);
            }
        }

        public override void Update(double dt)
        {
            base.Update(dt);

            Sprite.Rotation = Tank.TurretRotation + Tank.BodyRotation;

            foreach (Plugin p in Plugins)
            {
                p.Update(dt);
            }
        }

        public override void Draw(IRender render, Camera camera)
        {
            base.Draw(render, camera);

            List<Plugin> drawn = new List<Plugin>();

            foreach (Plugin p in Top)
            {
                if (p == null)
                    continue;
                if (drawn.Contains(p))
                    continue;
                else
                {
                    p.Draw(render, camera);
                    drawn.Add(p);
                }
            }

            drawn.Clear();

            foreach (Plugin p in Left)
            {
                if (p == null)
                    continue;
                if (drawn.Contains(p))
                    continue;
                else
                {
                    p.Draw(render, camera);
                    drawn.Add(p);
                }
            }

            foreach (Plugin p in Right)
            {
                if (p == null)
                    continue;
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
