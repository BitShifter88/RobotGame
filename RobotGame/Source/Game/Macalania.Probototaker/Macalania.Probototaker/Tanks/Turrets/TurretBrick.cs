using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    public enum BrickType
    {
        NoCorners,
        LeftTop,
        RightTop,
        LeftBottom,
        RightBottom,
        Top,
        Bottom,
        Right,
        Left,
    }

    public class TurretBrick : TurretComponent
    {
        Tank _tank;
        Vector2 _origin;
        public BrickType BrickType { get; set; }
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

            x = x - 16;
            y = y - 16;

            _origin = new Vector2(-x * _dim, -y * _dim);
        }

        public override void Draw(YunaEngine.Rendering.IRender render, Camera camera)
        {
            Sprite brickTexture = null;
            Rectangle sideTextureSource = _tank.TurretStyle.GetSidesSource(BrickType);

            if (BrickType == Turrets.BrickType.NoCorners)
            {
                brickTexture = _tank.TurretStyle.MainTexture;
                
            }
            else if (BrickType == Turrets.BrickType.LeftTop)
            {
                brickTexture = _tank.TurretStyle.CornersLeftTop;
            }
            else if (BrickType == Turrets.BrickType.RightTop)
            {
                brickTexture = _tank.TurretStyle.CornersRightTop;
            }
            else if (BrickType == Turrets.BrickType.LeftBottom)
            {
                brickTexture = _tank.TurretStyle.CornersLeftBottom;
            }
            else if (BrickType == Turrets.BrickType.RightBottom)
            {
                brickTexture = _tank.TurretStyle.CornersRightBottom;
            }
            else
                brickTexture = _tank.TurretStyle.MainTexture;

            // Draws main part of the brick
            brickTexture.Origin = _origin;
            brickTexture.Position = _tank.Position;
            brickTexture.Rotation = _tank.TurretRotation + _tank.BodyRotation;
            brickTexture.Draw(render, camera, new Rectangle(_x * _dim, _y * _dim, _dim, _dim));

            _tank.TurretStyle.Sides.Origin = _origin;
            _tank.TurretStyle.Sides.Position = _tank.Position;
            _tank.TurretStyle.Sides.Rotation = _tank.TurretRotation + _tank.BodyRotation;
            if (BrickType != Turrets.BrickType.NoCorners)
                _tank.TurretStyle.Sides.Draw(render, camera, sideTextureSource);


            // Draws clutter
            _tank.TurretStyle.Cluder.Origin = _origin;
            _tank.TurretStyle.Cluder.Position = _tank.Position;
            _tank.TurretStyle.Cluder.Rotation = _tank.TurretRotation + _tank.BodyRotation;
            _tank.TurretStyle.Cluder.Draw(render, camera, new Rectangle(_x * _dim, (_y - _tank.Turret.YCordForTopBrick) * _dim, _dim, _dim));
            
        }
    }
}


