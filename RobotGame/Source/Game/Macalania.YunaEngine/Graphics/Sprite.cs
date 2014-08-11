using Macalania.YunaEngine.Collision;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Resources;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Graphics
{
    public class Sprite
    {
        public BoundingSphere BoundingSphere { get; set; }
        public BoundingSphere RelativeBoundingSphere { get; set; }

        //public YunaTexture BoundingSphereTexture { get; set; }

        public Sprite(YunaTexture texture)
        {
            Texture = texture;
            Scale = 1;
            Color = Color.White;
            CalculateBoundingSphere();
        }

        public YunaTexture Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; }
        public Color Color { get; set; }
        public float Rotation { get; set; }
        public float Scale { get; set; }
        public float DepthLayer { get; set; }

        public void SetOriginCenter()
        {
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
        }

        public Matrix GetTransform()
        {
            Matrix blockTransform =
                    Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                    Matrix.CreateScale(Scale) *  
                    Matrix.CreateRotationZ(Rotation) *
                    Matrix.CreateTranslation(new Vector3(Position, 0.0f));

            return blockTransform;
        }

        public virtual void Update(double dt)
        {
            CalculateRelativeBoundingSphere();
        }

        public virtual void Draw(IRender render, Camera camera)
        {
            render.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Color, Rotation, Origin, Scale, DepthLayer);

            //if (YunaSettings.DrawBoundingSpheres)
            //    render.Draw(BoundingSphereTexture, new Vector2(RelativeBoundingSphere.Center.X, RelativeBoundingSphere.Center.Y),new Rectangle(0,0, BoundingSphereTexture.Width, BoundingSphereTexture.Height), Color.Red, 0, new Vector2(BoundingSphereTexture.Width/2, BoundingSphereTexture.Height/2), 1, 0.9f);
        }

        public void Draw(IRender render, Camera camera, Rectangle source)
        {
            render.Draw(Texture, Position, source, Color, Rotation, Origin, Scale, DepthLayer);

            //if (YunaSettings.DrawBoundingSpheres)
            //    render.Draw(BoundingSphereTexture, new Vector2(RelativeBoundingSphere.Center.X, RelativeBoundingSphere.Center.Y),new Rectangle(0,0, BoundingSphereTexture.Width, BoundingSphereTexture.Height), Color.Red, 0, new Vector2(BoundingSphereTexture.Width/2, BoundingSphereTexture.Height/2), 1, 0.9f);
        }

        public virtual void CalculateBoundingSphere()
        {
            BoundingSphere = BoundingSphere.CreateFromBoundingBox(new BoundingBox(new Vector3(Texture.Width, Texture.Height, 0), new Vector3(0, 0, 0)));
            //BoundingSphereTexture = YunaMath.CreateCircleTexture((int)BoundingSphere.Radius, YunaGameEngine.Instance.GraphicsDevice);
        }

        public virtual void CalculateRelativeBoundingSphere()
        {
            Vector2 center = -Origin;
            center += new Vector2(Texture.Width / 2, Texture.Height / 2);
            center = YunaMath.RotateVector2(center, Rotation);
            
            center += Position;

            RelativeBoundingSphere = new BoundingSphere(new Vector3(center.X, center.Y, 0), BoundingSphere.Radius);
        }

        public static bool CheckCollision(Sprite a, Sprite b)
        {
            if (Sprite.BoundingSphereCollision(a, b))
            {
                if (PerPixelCollision.IntersectPixels(a.GetTransform(), a.Texture.Width, a.Texture.Height, a.Texture.GetTransperancyMap(),
                    b.GetTransform(), b.Texture.Width, b.Texture.Height, b.Texture.GetTransperancyMap()) == true)
                    return true;
                return false;

            }

            return false;
        }

        static bool BoundingSphereCollision(Sprite a, Sprite b)
        {
            if (a.RelativeBoundingSphere.Intersects(b.RelativeBoundingSphere))
            {
                return true;
            }
            return false;
        }

        //static bool PerPixelCollision(Sprite a, Sprite b)
        //{
        //    // Get Color data of each Texture
        //    Color[] bitsA = new Color[a.Texture.Width * a.Texture.Height];
        //    a.Texture.GetData(bitsA);
        //    Color[] bitsB = new Color[b.Texture.Width * b.Texture.Height];
        //    b.Texture.GetData(bitsB);

        //    // Calculate the intersecting rectangle
        //    int x1 = Math.Max(a.Bounds.X, b.Bounds.X);
        //    int x2 = Math.Min(a.Bounds.X + a.Bounds.Width, b.Bounds.X + b.Bounds.Width);

        //    int y1 = Math.Max(a.Bounds.Y, b.Bounds.Y);
        //    int y2 = Math.Min(a.Bounds.Y + a.Bounds.Height, b.Bounds.Y + b.Bounds.Height);

        //    // For each single pixel in the intersecting rectangle
        //    for (int y = y1; y < y2; ++y)
        //    {
        //        for (int x = x1; x < x2; ++x)
        //        {
        //            // Get the color from each texture
        //            Color aa = bitsA[(x - a.Bounds.X) + (y - a.Bounds.Y) * a.Texture.Width];
        //            Color bb = bitsB[(x - b.Bounds.X) + (y - b.Bounds.Y) * b.Texture.Width];

        //            if (aa.A != 0 && bb.A != 0) // If both colors are not transparent (the alpha channel is not 0), then there is a collision
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    // If no collision occurred by now, we're clear.
        //    return false;
        //}

        private Rectangle bounds = Rectangle.Empty;
        public virtual Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X - Texture.Width,
                    (int)Position.Y - Texture.Height,
                    Texture.Width,
                    Texture.Height);
            }

        }
    }
}
