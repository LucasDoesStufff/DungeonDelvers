using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace DungeonDelvers.Common
{
    class MouseHandler : ModSystem
    {
        public Rectangle Mouse = new Rectangle(0, 0, 10, 10);
        public override void PostUpdateEverything()
        {
            Mouse.X = Main.mouseX;
            Mouse.Y = Main.mouseY;
        }
    }
}
