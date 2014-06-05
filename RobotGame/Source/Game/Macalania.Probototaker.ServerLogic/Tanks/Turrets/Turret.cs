
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
    public class Turret : TankComponent
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

        public void ActivatePlugin(Plugin p, Vector2 targetPosition, Tank targetTank)
        {
            p.Activate(targetPosition, targetTank);
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

        public bool AddPluginButtom(Plugin plugin, int startIndex)
        {
            // Cheks if there are open slots for the plugin
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                if (Buttom[i] != null)
                    return false;
            }
            for (int i = startIndex; i < startIndex + plugin.Size; i++)
            {
                Buttom[i] = plugin;
            }

            SetPluginOriginButtom(plugin, startIndex);

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

        private void SetPluginOriginButtom(Plugin plugin, int startIndex)
        {
            Vector2 origin = new Vector2();
            origin.X = (Sprite.Texture.Width - ExtraPixelsTop) / 2 - startIndex * Globals.PluginPixelWidth;
            origin.Y = Sprite.Texture.Height / 2;
            origin.Y = -origin.Y;

            origin += plugin.OriginOfset;

            plugin.Sprite.Origin = origin;
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

        public override void OnTankDestroy()
        {
            foreach (Plugin p in Plugins)
            {
                p.OnTankDestroy();
            }
            base.OnTankDestroy();
        }

        public void FireMainGun()
        {
            List<MainGun> mgs = GetMainGuns();

            foreach (MainGun mg in mgs)
            {
                mg.FireRequest(this);
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

            foreach (Plugin p in Plugins)
            {
                p.Draw(render, camera);
            }



        }
    }
}
