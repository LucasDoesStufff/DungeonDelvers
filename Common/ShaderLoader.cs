using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;

namespace DungeonDelvers.Common
{
    internal class ShaderLoader : ModSystem
    {
        public override void Load()
        {
            if (Main.dedServ) return;


            Asset<Effect> effect = Mod.Assets.Request<Effect>("Effects/Reflection", AssetRequestMode.ImmediateLoad);

            GameShaders.Misc[$"DungeonDelvers:Reflection"] = new MiscShaderData(effect, "Reflection");

            /*var miscShaders = Mod.GetFileNames().Where(f =>
                f.StartsWith("Effects/Compiled/Misc/", StringComparison.OrdinalIgnoreCase) &&
                f.EndsWith(".xnb", StringComparison.OrdinalIgnoreCase));

            foreach (string file in miscShaders)
            {
                string assetPath = Path.ChangeExtension(file, null);
                string shaderName = Path.GetFileNameWithoutExtension(file);

                try
                {
                    Asset<Effect> effect = Mod.Assets.Request<Effect>(assetPath, AssetRequestMode.ImmediateLoad);

                    GameShaders.Misc[$"DungeonDelvers:{shaderName}"] = new MiscShaderData(effect, shaderName);
                }
                catch (Exception)
                {
                    Mod.Logger.Error($"Shader {shaderName} failed: pass name must match file name");
                    throw;
                }
            }*/
        }
    }
}
