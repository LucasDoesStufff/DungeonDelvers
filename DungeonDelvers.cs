global using DungeonDelvers.Core;
global using Terraria;
global using Terraria.ModLoader;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;


namespace DungeonDelvers
{
	
	public class DungeonDelvers : Mod
	{
        public override void Load()
        {
            if(Main.netMode != NetmodeID.Server)
            {
                //Asset<Effect> test = this.Assets.Request<Effect>("Content/Shader/test");

                //Filters.Scene["ArmorBasic"] = new Filter(new ScreenShaderData(test, "ArmorBasic"), EffectPriority.VeryHigh);
                //Filters.Scene["ArmorBasic"].Load();
            }
        }
        
	}
}
