using Macalania.YunaEngine.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macalania.YunaEngine.Rooms
{
    public class RoomManager
    {
        static public RoomManager Instance { get; set; }
        private Room _activeRoom;

        public RoomManager()
        {
            Instance = this;
        }

#if !SERVER
        public void SetActiveRoom(Room room, bool load, IServiceProvider services)
        {
            if (_activeRoom != null)
                _activeRoom.Unload();
            if (load)
            {
                room.Inizialize();
                room.Load(services);
            }
            _activeRoom = room;
        }
#endif

#if SERVER
        public void SetActiveRoom(Room room, bool load, ResourceManager content)
        {
            if (_activeRoom != null)
                _activeRoom.Unload();
            if (load)
            {
                room.Inizialize();
                room.Load(content);
            }
            _activeRoom = room;
        }
#endif

        public Room GetActiveRoom()
        {
            return _activeRoom;
        }
    }
}
