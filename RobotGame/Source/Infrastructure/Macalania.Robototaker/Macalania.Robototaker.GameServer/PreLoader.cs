using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Robototaker.GameServer
{
    public static class PreLoader
    {
        public static void PreLoad(ResourceManager content)
        {
            content.LoadYunaTexture("Textures/Effects/shieldsphere");
            content.LoadYunaTexture("Textures/Effects/explosion");
            content.LoadYunaTexture("Textures/Items/starterMine");
            content.LoadYunaTexture("Textures/Projectiles/artileryProjectile");
            content.LoadYunaTexture("Textures/Projectiles/attackRocketProjectile");
            content.LoadYunaTexture("Textures/Projectiles/rocketStarterProjectile");
            content.LoadYunaTexture("Textures/Projectiles/shell");
            content.LoadYunaTexture("Textures/Tanks/Hulls/hullStarter");
            content.LoadYunaTexture("Textures/Tanks/MainGuns/mainGunStarter");
            content.LoadYunaTexture("Textures/Tanks/MainGuns/MiniMainGun");
            content.LoadYunaTexture("Textures/Tanks/MainGuns/SprayMainGun");
            content.LoadYunaTexture("Textures/Tanks/Turrets/turretBig");
            content.LoadYunaTexture("Textures/Tanks/Turrets/turretBigNew");
            content.LoadYunaTexture("Textures/Tanks/Turrets/turretStarter");

            content.LoadYunaTexture("Textures/Tanks/Tracks/tracksStarter");
            content.LoadYunaTexture("Textures/Tanks/Misc/amorStarterBottom");
            content.LoadYunaTexture("Textures/Tanks/Misc/amorStarterLeft");
            content.LoadYunaTexture("Textures/Tanks/Misc/amorStarterRight");
            content.LoadYunaTexture("Textures/Tanks/Misc/artilery");
            content.LoadYunaTexture("Textures/Tanks/Misc/battery");
            content.LoadYunaTexture("Textures/Tanks/Misc/rocketStarterRight");
            content.LoadYunaTexture("Textures/Tanks/Misc/shieldBottom");
            content.LoadYunaTexture("Textures/Tanks/Misc/shieldLeft");
            content.LoadYunaTexture("Textures/Tanks/Misc/starterAttackRocket");
            content.LoadYunaTexture("Textures/Tanks/Misc/sunPannel");
            content.LoadYunaTexture("Textures/Tanks/Misc/superArtillery");
        }
    }
}
