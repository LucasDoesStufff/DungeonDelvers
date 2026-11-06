using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using static tModPorter.ProgressUpdate;

namespace DungeonDelvers.Common.ModPlayers
{
    public partial class Delver : ModPlayer
    {

        public override void ProcessTriggers(TriggersSet triggersSet)
        {
            
        }
        
        public override void UpdateLifeRegen()
        {
            Vector2 mouseUV = new Vector2(Main.mouseX, Main.mouseY) /
                  new Vector2(Main.screenWidth, Main.screenHeight);
            

            if (KeybindHandler.ShaderOpen.JustPressed)
            {
                Projectile.NewProjectile(Player.GetSource_FromThis(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<Content.Test.ShaderTestProjectile>(), 0, 0f, Player.whoAmI);
                Main.NewText("shaderopened");
            }
            if (KeybindHandler.ShaderClose.JustPressed)
            {
                for (int i = 0; i < Main.maxProjectiles; i++)
                {
                    Projectile p = Main.projectile[i];
                    if (p.active && p.type == ModContent.ProjectileType<Content.Test.ShaderTestProjectile>())
                    {
                        p.Kill();
                    }
                }
                Main.NewText("shaderclosed");
            }
        }
    }
}
