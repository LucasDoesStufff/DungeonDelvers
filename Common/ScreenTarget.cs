using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Terraria.Graphics.Shaders;

namespace DungeonDelvers.Common
{
    internal class ScreenTarget : ModSystem
    {
        // MOSTLY COMPLETE JUST NEEDS TOGGLES FOR NPC/DUST/PROJECTILE RENDERING
        public static RenderTarget2D Target;

        public static int sheetSquareX;
        public static int sheetSquareY;
        bool canUsePlayer = true;

        Vector2 oldPos;
        Vector2 oldCenter;
        Vector2 oldMountedCenter;
        Vector2 oldScreen;
        Vector2 oldItemLocation;
        Vector2 positionOffset;
        public override void Load()
        {
            if(Main.dedServ)
                return;

            sheetSquareX = 300;
            sheetSquareY = 200;

            Main.QueueMainThreadAction(() =>
                Target = new RenderTarget2D(Main.graphics.GraphicsDevice, sheetSquareX, sheetSquareY, false, SurfaceFormat.Color, DepthFormat.None, 0, RenderTargetUsage.PreserveContents)
            );

            On_Main.CheckMonoliths += DrawTarget;
            On_Lighting.GetColorClamped += FullBrightPlayer;
            On_Player.GetHairColor += FullBrightHair;
        }
        public override void Unload()
        {
            On_Main.CheckMonoliths -= DrawTarget;
            On_Lighting.GetColorClamped -= FullBrightPlayer;
            On_Player.GetHairColor -= FullBrightHair;
            Main.QueueMainThreadAction(() => Target?.Dispose());
        }

        private void DrawTarget(On_Main.orig_CheckMonoliths orig)
        {
            orig();

            if(Main.gameMenu)
                return;

            if (Main.player.Any(n => n.active))
            {
                DrawPlayerTarget();
            }

            if (Main.instance.tileTarget.IsDisposed)
                return;
        }

        // THANK YOU OUR LORD AND SAVIOUR STARLIGHT RIVER
        void DrawPlayerTarget()
        {
            if (Target == null) 
                return;

            RenderTargetBinding[] oldTargets = Main.graphics.GraphicsDevice.GetRenderTargets();
            canUsePlayer = false;

            Main.graphics.GraphicsDevice.SetRenderTarget(Target);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState,
                                   DepthStencilState.None, Main.Rasterizer, null, Main.GameViewMatrix.EffectMatrix);

            Player player = Main.LocalPlayer;

            // save
            oldPos = player.position;
            oldCenter = player.Center;
            oldMountedCenter = player.MountedCenter;
            oldScreen = Main.screenPosition;
            oldItemLocation = player.itemLocation;
            int oldHeldProj = player.heldProj;

            //modify
            positionOffset = new Vector2(sheetSquareX / 2, sheetSquareY / 2);
            player.position = positionOffset;
            player.Center = oldCenter - oldPos + positionOffset;
            player.itemLocation = oldItemLocation - oldPos + positionOffset;
            player.MountedCenter = oldMountedCenter - oldPos + positionOffset;
            player.heldProj = -1;
            Main.screenPosition = Vector2.Zero;

            Main.PlayerRenderer.DrawPlayer(Main.Camera, player, player.position, player.fullRotation, player.fullRotationOrigin, 0f);

            //restore
            player.position = oldPos;
            player.Center = oldCenter;
            Main.screenPosition = oldScreen;
            player.itemLocation = oldItemLocation;
            player.MountedCenter = oldMountedCenter;
            player.heldProj = oldHeldProj;

            Main.spriteBatch.End();
            Main.graphics.GraphicsDevice.SetRenderTargets(oldTargets);
            canUsePlayer = true;
        }

        Color FullBrightPlayer(On_Lighting.orig_GetColorClamped orig, int x, int y, Color color)
        {
            if (canUsePlayer)
                return orig(x, y, color);
            else
                return color;
        }

        private Color FullBrightHair(On_Player.orig_GetHairColor orig, Player self, bool useLighting)
        {
            return orig(self, useLighting & canUsePlayer);
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Vector2 playerScreen = Main.LocalPlayer.position - Main.screenPosition;
            Vector2 offset = new Vector2(-sheetSquareX / 2 + Main.LocalPlayer.width, -Main.LocalPlayer.height / 4);
            Vector2 renderPos = new Vector2(MathF.Floor(playerScreen.X + offset.X), MathF.Floor(playerScreen.Y + offset.Y));
            //spriteBatch.Draw(Target, renderPos, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.FlipVertically, 1);
        }
    }
}
