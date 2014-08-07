using Macalania.Probototaker.Tanks.Plugins.MainGuns;
using Macalania.YunaEngine.Graphics;
using Macalania.YunaEngine.Rendering;
using Macalania.YunaEngine.Rooms;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.Probototaker.Tanks.Turrets
{
    public class Turret
    {
        private TurretComponent[,] _turretComponents;
        private List<TurretModule> _modules = new List<TurretModule>();
        public int YCordForTopBrick { get; set; }
        Tank _tank;

        public Turret(Tank tank)
        {
            _tank = tank;
            _turretComponents = new TurretComponent[32, 32];

        }

        public void FireMainGun()
        {
            List<MainGunNew> mgs = GetMainGuns();

            foreach (MainGunNew mg in mgs)
            {
                mg.FireRequest(this);
            }
        }

        public TurretComponent GetTurretComponent(int x, int y)
        {
            return _turretComponents[x, y];
        }

        public void DetermineTurretBricks()
        {
            for (int i = 1; i < 31; i++)
            {
                for (int j = 1; j < 31; j++)
                {
                    if (_turretComponents[i, j] != null && _turretComponents[i, j].GetType() == typeof(TurretBrick))
                    {
                        if (
                            _turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() == typeof(TurretBrick))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.NoCorners;
                        }
                        else if (
                            _turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() == typeof(TurretBrick) &&
                           ( _turretComponents[i, j - 1] == null || (_turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() != typeof(TurretBrick))) &&
                            _turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() == typeof(TurretBrick) &&
                           
                            (_turretComponents[i - 1, j] == null || (_turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() != typeof(TurretBrick))) )
                           
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.LeftTop;
                        }
                        else if (
                            (_turretComponents[i + 1, j] == null || (_turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() != typeof(TurretBrick))) &&

                            _turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() == typeof(TurretBrick) &&
                            (_turretComponents[i, j - 1] == null || (_turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() != typeof(TurretBrick))) &&

                            _turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() == typeof(TurretBrick))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.RightTop;
                        }
                        else if (
                            _turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() == typeof(TurretBrick) &&
                            (_turretComponents[i, j + 1] == null || (_turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() != typeof(TurretBrick))) &&
                            (_turretComponents[i - 1, j] == null || (_turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() != typeof(TurretBrick))))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.LeftBottom;
                        }
                        else if (
                            _turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() == typeof(TurretBrick) &&
                            (_turretComponents[i, j + 1] == null || (_turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() != typeof(TurretBrick))) &&
                            (_turretComponents[i + 1, j] == null || (_turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() != typeof(TurretBrick))))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.RightBottom;
                        }
                        else if (
                            _turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() == typeof(TurretBrick) &&
                            (_turretComponents[i, j - 1] == null || (_turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() != typeof(TurretBrick))) &&
                            _turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() == typeof(TurretBrick) &&

                            _turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() == typeof(TurretBrick))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.Top;
                        }
                        else if (
                            _turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() == typeof(TurretBrick) &&
                            (_turretComponents[i, j + 1] == null || (_turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() != typeof(TurretBrick))) &&
                            _turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() == typeof(TurretBrick))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.Bottom;
                        }
                        else if (
                            _turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() == typeof(TurretBrick) &&
                            (_turretComponents[i - 1, j] == null || (_turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() != typeof(TurretBrick))))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.Left;
                        }
                        else if (
                            _turretComponents[i - 1, j] != null && _turretComponents[i - 1, j].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j - 1] != null && _turretComponents[i, j - 1].GetType() == typeof(TurretBrick) &&
                            _turretComponents[i, j + 1] != null && _turretComponents[i, j + 1].GetType() == typeof(TurretBrick) &&
                            (_turretComponents[i + 1, j] == null || (_turretComponents[i + 1, j] != null && _turretComponents[i + 1, j].GetType() != typeof(TurretBrick))))
                        {
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.Right;
                        }
                        else
                            ((TurretBrick)_turretComponents[i, j]).BrickType = BrickType.NoCorners;
                    }
                }
            }
        }

        private List<MainGunNew> GetMainGuns()
        {
            List<MainGunNew> mainGuns = new List<MainGunNew>();

            foreach (TurretModule p in _modules)
            {
                if (p.GetType().IsSubclassOf(typeof(MainGunNew)))
                {
                    mainGuns.Add((MainGunNew)p);
                }
            }

            return mainGuns;
        }

        public void OnTankDestroy()
        {
            foreach (TurretModule module in _modules)
            {
                module.OnTankDestroy();
            }
        }

        public List<TurretModule> GetModules()
        {
            return _modules;
        }

        public TurretModule GetModuleAtLocation(int x, int y)
        {
            if (x < 0 || y < 0 || x >= 32 || y >= 32)
                return null;

            if (_turretComponents[x, y] == null)
                return null;
            if (_turretComponents[x, y].GetType() == typeof(BlockBrick))
            {
                BlockBrick block = (BlockBrick)_turretComponents[x, y];
                return block.Owner;
            }
            else
                return null;
        }

        public void RemoveModule(TurretModule module)
        {
            if (_modules.Contains(module) == false)
                throw new Exception("Module not found");

            _modules.Remove(module);

            RemoveBlockBricks(module);
        }

        public void RemoveTurretBrick(int x, int y)
        {
            _turretComponents[x, y] = null;
        }

        public void AddTurretModule(TurretModule module, int x, int y)
        {
            if (x < 0 || y < 0 || x >= 32 || y >= 32)
                return;
            if (CanAddTurretModule(module, x, y) == false)
                throw new Exception("Could not add turret module");

            
            module.SetLocation(x, y);
            module.AddComponents(this);
            module.SetPositionAfterTank();

            _modules.Add(module);
            AddBlockBricks(module, x, y);
        }

        private void RemoveBlockBricks(TurretModule module)
        {
            for (int i = 0; i < 32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (_turretComponents[i,j] != null && _turretComponents[i,j].GetType() == typeof(BlockBrick))
                    {
                        BlockBrick blockBrick = (BlockBrick)_turretComponents[i, j];
                        if (blockBrick.Owner == module)
                            _turretComponents[i, j] = null;
                    }
                }
            }
        }

        private void AddBlockBricks(TurretModule module, int x, int y)
        {
            foreach (Point brick in module.GetRotatedFreeSpace())
            {
                BlockBrick bb = new BlockBrick(module, _tank);
                bb.SetLocation(x + brick.X, y + brick.Y);
                bb.Load(RoomManager.Instance.GetActiveRoom().Content);
                _turretComponents[x + brick.X, y + brick.Y] = bb;
            }
        }

        public bool CanAddTurretModule(TurretModule module, int x, int y)
        {
            if (x < 0 || y < 0 || x >= 32 || y >= 32)
                return false;
            // The module may require that turret bricks are in place at different locations.
            foreach (Point brick in module.GetRotatedRequiredBricks())
            {
                TurretComponent comp = _turretComponents[x + brick.X, y + brick.Y];
                if (comp == null || comp.GetType() != typeof(TurretBrick))
                    return false;
            }

            // The module may require free space
            foreach (Point brick in module.GetRotatedFreeSpace())
            {
                TurretComponent comp = _turretComponents[x + brick.X, y + brick.Y];
                if (comp != null)
                    return false;
            }

            return true;
        }

        public void AddTurretComponent(TurretComponent component, int x, int y)
        {
            if (_turretComponents[x, y] != null)
                throw new Exception("Can't add turret component");
            _turretComponents[x, y] = component;
            component.SetLocation(x, y);

            CalculateTopBrik();
            //DetermineTurretBricks();
        }

        private void CalculateTopBrik()
        {
            int best = 32;

            for (int i = 0; i <32; i++)
            {
                for (int j = 0; j < 32; j++)
                {
                    if (_turretComponents[i, j] != null && _turretComponents[i, j].GetType() == typeof(TurretBrick) && j < best)
                        best = j;
                }
            }

            YCordForTopBrick = best;
        }

        public bool CanAddTurretComponent(TurretComponent component, int x, int y)
        {
            if (x < 1 || y < 1 || x >= 31 || y >= 31)
                return false;
            if ((_turretComponents[x + 1, y] != null && _turretComponents[x + 1, y].GetType() == typeof(TurretBrick)) ||
                (_turretComponents[x - 1, y] != null && _turretComponents[x - 1, y].GetType() == typeof(TurretBrick)) ||
                (_turretComponents[x, y + 1] != null && _turretComponents[x, y + 1].GetType() == typeof(TurretBrick)) ||
                (_turretComponents[x, y - 1] != null && _turretComponents[x, y - 1].GetType() == typeof(TurretBrick)))
            {
                if (_turretComponents[x, y] == null)
                    return true;
            }
            return false;
        }

        public void Update(double dt)
        {
            foreach (TurretModule module in _modules)
            {
                module.Update(dt);
            }
        }

        public void Draw(IRender render, Camera camera)
        {
            foreach (TurretComponent tc in _turretComponents)
            {
                if (tc == null)
                    continue;
                tc.Draw(render, camera);
            }

            foreach (TurretModule module in _modules)
            {
                module.Draw(render, camera);
            }
        }
    }
}
