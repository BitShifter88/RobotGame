using Macalania.YunaEngine;
using Macalania.YunaEngine.Rooms;
using System;

namespace Macalania.Probototaker.WorldEditor
{
#if WINDOWS || XBOX
    static class Program
    {
        static YunaGameEngine _engine;

        static void Main(string[] args)
        {
            using (_engine = new YunaGameEngine())
            {
                _engine.EngineStarted += new YunaGameEngine.EngineStartedEventHandler(OnEngineStart);
                _engine.Run();
            }
        }

        static void OnEngineStart()
        {
            Globals.Viewport = _engine.GraphicsDevice.Viewport;
            //LoadGameRoom room = new LoadGameRoom();
            Room editorRoom = new EditorRoom();
            YunaGameEngine.Instance.SetActiveRoom(editorRoom, true, YunaGameEngine.Instance.Services);
        }
    }
#endif
}

