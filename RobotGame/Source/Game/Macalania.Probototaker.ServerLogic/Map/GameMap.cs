using Macalania.YunaEngine.GameLogic;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Map
{
    public class Layer
    {
        public byte[,] LayerTiles { get; set; }

        public Layer(int dimx, int dimy)
        {
            LayerTiles = new byte[dimx, dimy];
        }
    }

    public class GameMap : GameObject
    {
        Dictionary<int, Layer> _layers = new Dictionary<int, Layer>();
        public int DimX { get; set; }
        public int DimY { get; set; }
        public TileStyle TileStyle { get; set; }

        public static int TILE_DIM = 64;
       

        public GameMap(int dimx, int dimy, Room room) : base(room)
        {
            DimX = dimx;
            DimY = dimy;
        }
        public void CreateNextLayer()
        {
            _layers.Add(_layers.Count, new Layer(DimX, DimY));
        }
        public void SetTile(int layer, int x, int y, byte value)
        {
            _layers[layer].LayerTiles[x, y] = value;
        }

        public override void Draw(IRender render, Camera camera)
        {
            foreach (KeyValuePair<int, Layer> layer in _layers)
            {
                for (int i = 0; i < DimX; i++)
                {
                    for (int j = 0; j < DimY; j++)
                    {
                        TileStyle.TileSet.Position = new Vector2(i * TILE_DIM, j * TILE_DIM);
                        TileStyle.TileSet.Draw(render, camera, TileStyle.GetSource(layer.Value.LayerTiles[i, j]));
                    }
                }
            }
                base.Draw(render, camera);
        }

        public static GameMap LoadFromFile(string filePath, Room room)
        {
            GameMap gameMap = null;

            using (BinaryReader br = new BinaryReader(new FileStream(filePath, FileMode.Open)))
            {
                byte tileStyle = br.ReadByte();
                int dimx = br.ReadInt32();
                int dimy = br.ReadInt32();

                gameMap = new GameMap(dimx, dimy, room);
                gameMap.TileStyle = TileStyle.CreateFromType((TileStyleType)tileStyle);
                gameMap.TileStyle.Load(room.Content);

                int layers = br.ReadInt32();

                for (int layer = 0; layer < layers; layer++)
                {
                    gameMap.CreateNextLayer();

                    for (int i = 0; i < dimx; i++)
                    {
                        for (int j = 0; j < dimy; j++)
                        {
                            gameMap.SetTile(layer, i, j, br.ReadByte());
                        }
                    }
                }

                
            }

            return gameMap;
        }

        public void SaveToFile(string filePath)
        {
            using (BinaryWriter bw = new BinaryWriter(new FileStream(filePath, FileMode.Create)))
            {
                bw.Write((byte)TileStyle.Type);
                bw.Write(DimX);
                bw.Write(DimY);

                bw.Write(_layers.Count);

                foreach (KeyValuePair<int, Layer> layer in _layers)
                {
                    for (int i = 0; i < DimX; i++)
                    {
                        for (int j = 0; j < DimY; j++)
                        {
                            bw.Write(layer.Value.LayerTiles[i, j]);
                        }
                    }
                }
            }
        }
    }
}
