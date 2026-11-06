using DungeonDelvers.Common;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;

namespace DungeonDelvers.Content.Test
{
    public class ShaderTestProjectile : ModProjectile
    {
        public override string Texture => AssetDirectory.Debug + "ShaderTestMirror";
        static Asset<Texture2D> normalMapTex;
        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 100;
            Projectile.aiStyle = -1;
            Projectile.tileCollide = false;
            Projectile.friendly = false;
            Projectile.timeLeft = int.MaxValue;
        }
        public override void Load()
        {
            normalMapTex = ModContent.Request<Texture2D>(
                "DungeonDelvers/Assets/Debug/ShaderTestMirror_NormalMap",
                AssetRequestMode.ImmediateLoad);
        }
        public override void AI()
        {
            Projectile.velocity = Vector2.Zero;
            Projectile.timeLeft = 2;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            SpriteBatch sb = Main.spriteBatch;
            Texture2D tex = TextureAssets.Projectile[Type].Value;

            Vector2 pos = Projectile.Center - Main.screenPosition;
            Vector2 origin = tex.Size() / 2f;

            var data = new DrawData(tex, pos, null, Color.White, Projectile.rotation,
                                    origin, Projectile.scale, SpriteEffects.None, 0);

            sb.End();
            sb.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState,
                     DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            var shader = GameShaders.Misc["DungeonDelvers:Reflection"];

            Vector2 pos1 = (Projectile.Center - Main.screenPosition) - (tex.Size() * Projectile.scale) / 2;
            Vector2 offset = new Vector2(-ScreenTarget.sheetSquareX / 2 + Main.LocalPlayer.width, -ScreenTarget.sheetSquareY / 1.93f);

            shader.Shader.Parameters["uReflectorScreenPosition"].SetValue(Utilities.FloorVec2F(pos1));
            shader.Shader.Parameters["uReflectorScreenSize"].SetValue(tex.Size() * Projectile.scale);
            shader.Shader.Parameters["uPlayerScreenPosition"].SetValue(Utilities.FloorVec2F(Main.LocalPlayer.position - Main.screenPosition + offset));
            shader.Shader.Parameters["uSheetSize"].SetValue(new Vector2(ScreenTarget.sheetSquareX, ScreenTarget.sheetSquareY));

            shader.Apply(data);
            Main.graphics.GraphicsDevice.Textures[1] = ScreenTarget.Target;
            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;

            Main.graphics.GraphicsDevice.Textures[2] = normalMapTex.Value;
            Main.graphics.GraphicsDevice.SamplerStates[2] = SamplerState.PointClamp;

            sb.Draw(tex, Utilities.FloorVec2F(Projectile.Center - Main.screenPosition), null, Color.White,
                    Projectile.rotation, tex.Size() / 2f, Projectile.scale, SpriteEffects.None, 0f);

            sb.End();
            sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                     DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.TransformationMatrix);

            return false; // skip vanilla drawing
        }
    }
}
