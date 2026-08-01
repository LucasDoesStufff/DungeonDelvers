using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDelvers.Common
{
    class KeybindHandler : ModSystem
    {
        public static ModKeybind NodeKey { get; private set; }

        public override void Load()
        {
            NodeKey = KeybindLoader.RegisterKeybind(Mod, "Parkour Node", "G");
        }

        public override void Unload()
        {
            NodeKey = null;
        }
    }
}
