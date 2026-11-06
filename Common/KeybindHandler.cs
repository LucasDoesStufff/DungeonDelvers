using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDelvers.Common
{
    class KeybindHandler : ModSystem
    {
        public static ModKeybind ShaderOpen { get; private set; }
        public static ModKeybind ShaderClose { get; private set; }

        public override void Load()
        {
            ShaderOpen = KeybindLoader.RegisterKeybind(Mod, "Open Shader debug", "G");
            ShaderClose = KeybindLoader.RegisterKeybind(Mod, "Close Shader debug", "H");
        }

        public override void Unload()
        {
            ShaderOpen = null;
            ShaderClose = null;
        }
    }
}
