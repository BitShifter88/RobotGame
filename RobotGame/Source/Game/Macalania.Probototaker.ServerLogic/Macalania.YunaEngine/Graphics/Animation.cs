using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Graphics
{
    public class Animation : Sprite
    {
        public int FrameWidth { get; set; }
        public int Frames { get; set; }
        public int FramesPerSecond { get; set; }
        public int CurrentFrame { get; set; }
        public bool IsPlaying { get; set; }

        int _updatesSinceLastFrame = 0;

        public Animation(int frameWidth, int frames, int framesPerSecond, YunaTexture texture) : base(texture)
        {
            FramesPerSecond = framesPerSecond;
            Frames = frames;
            FrameWidth = frameWidth;
        }

        public void PlayAnimation()
        {
            CurrentFrame = 0;
            IsPlaying = true;
        }

        public override void CalculateBoundingSphere()
        {
            BoundingSphere = BoundingSphere.CreateFromBoundingBox(new BoundingBox(new Vector3(FrameWidth, Texture.Height, 0), new Vector3(0, 0, 0)));
            //BoundingSphereTexture = YunaMath.CreateCircleTexture((int)BoundingSphere.Radius, YunaGameEngine.Instance.GraphicsDevice);
        }

        public override void CalculateRelativeBoundingSphere()
        {
            Vector2 center = -Origin;
            center += new Vector2(FrameWidth / 2, Texture.Height / 2);
            center = YunaMath.RotateVector2(center, Rotation);

            center += Position;

            RelativeBoundingSphere = new BoundingSphere(new Vector3(center.X, center.Y, 0), BoundingSphere.Radius);
        }

        public override void Update(double dt)
        {
            if (IsPlaying)
            {
                _updatesSinceLastFrame++;

                if (_updatesSinceLastFrame >= 60/FramesPerSecond)
                {
                    _updatesSinceLastFrame = 0;
                    CurrentFrame++;
                    if (CurrentFrame + 1 == Frames)
                        IsPlaying = false;
                }
            }

            base.Update(dt);
        }

        public override void Draw(Rendering.IRender render, Camera camera)
        {
            render.Draw(Texture, Position, new Rectangle(CurrentFrame * FrameWidth, 0, FrameWidth, Texture.Height), Color, Rotation, Origin, Scale, DepthLayer);
        }
    }
}

