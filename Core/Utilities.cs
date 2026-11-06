using DungeonDelvers.Content.DungeonGeneration.GenerationTiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;


namespace DungeonDelvers.Core
{
    internal static class Utilities
    {
        /// <summary>
        /// Uses mathF function to floor down vec2s
        /// </summary>
        /// <param name="v"></param>
        /// <returns>The floored down vec2s</returns>
        public static Vector2 FloorVec2F(Vector2 v) => new Vector2(MathF.Floor(v.X), MathF.Floor(v.Y));

        public static void MultipleShot(Player player, IEntitySource source, Vector2 position, Vector2 velocity, int type, int damage, float knockback, int numberProjectiles, float rotationAmount, bool randomSpread)
        {
            position += Vector2.Normalize(new Vector2(velocity.X, velocity.Y)) * 45f;
            float rotation = MathHelper.ToRadians(rotationAmount);
            Vector2 perturbedSpeed;
            for (int i = 0; i < numberProjectiles; i++)
            {
                switch (randomSpread)
                {
                    case true:
                        perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedByRandom(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .4f;
                        break;
                    case false:
                        perturbedSpeed = new Vector2(velocity.X, velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .4f;
                        break;
                }
                Projectile.NewProjectile(source, position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockback, player.whoAmI);
            }
        }
        
        /// <summary>
        /// creates a dust ring
        /// </summary>
        /// <param name="center"></param>
        /// <param name="radius"></param>
        /// <param name="dustID"></param>
        /// <param name="dustCount"></param>
        /// <param name="dustSpeed"></param>
        public static void DustRing(Vector2 center, float radius, int dustID, int dustCount = 5, float dustSpeed = 2f)
        {
            for (int i = 0; i < dustCount; i++)
            {
                Vector2 dir = Utils.NextVector2CircularEdge(Main.rand, radius, radius);
                Vector2 val = center + dir;
                Vector2 dustVel = Utils.RotatedBy(new Vector2(dustSpeed, 0f), (double)(Utils.ToRotation(dir) + (float)Math.PI / 2f), default(Vector2));
                Dust.NewDustPerfect(val, dustID, (Vector2?)dustVel, 200, default(Color), 1f).noGravity = true;
            }
        }
        
        /// <summary>
        /// checks if the player is on the ground
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsGrounded(this Player player)
        {
            return player.velocity.Y >= 0f && Collision.SolidCollision(player.BottomLeft, 32, 8, true);
        }

        /// <summary>
        /// Checks if the given position is currently in the players POV
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static bool OnScreen(Vector2 pos)
        {
            return pos.X > -16 && pos.X < Main.screenWidth + 16 && pos.Y > -16 && pos.Y < Main.screenHeight + 16;
        }

        public static bool IsSolid(Tile tile) => tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType] && !Main.tileSolidTop[tile.TileType];

        /// <summary>
        /// Runs a flood fill algorithm at the given position and between given boundaries, stops at solid tiles.
        /// </summary>
        /// <param name="startPoint">The 'seed' of flood fill</param>
        /// <param name="startX"></param>
        /// <param name="endX"></param>
        /// <param name="startY"></param>
        /// <param name="endY"></param>
        /// <param name="safetyLimit">The maximum number of tiles to fill to prevent infinite loops</param>
        /// <returns>a hashset containing all the tile coordinates inside it. Does not fill the holes</returns>
        public static HashSet<Point> TileFloodFillSolid(Point startPoint, int startX, int endX, int startY, int endY, int safetyLimit)
        {
            HashSet<Point> newRoom = new HashSet<Point>();
            Queue<Point> q = new Queue<Point>();
            int SafetyLimit = safetyLimit;
            int count = 0;

            q.Enqueue(startPoint);
            newRoom.Add(startPoint);

            while (q.Count > 0)
            {
                count++;
                if (count > safetyLimit) break;

                Point p = q.Dequeue();

                Point[] neighbors = new Point[]
                {
                    new Point(p.X + 1, p.Y),
                    new Point(p.X - 1, p.Y),
                    new Point(p.X, p.Y + 1),
                    new Point(p.X, p.Y - 1),

                    new Point(p.X + 1, p.Y +1),
                    new Point(p.X + 1, p.Y -1),
                    new Point(p.X - 1, p.Y + 1),
                    new Point(p.X - 1, p.Y - 1)
                };

                foreach (Point n in neighbors)
                {
                    if (newRoom.Contains(n)) continue;

                    if (n.X < startX || n.X >= endX || n.Y < startY || n.Y >= endY) continue;

                    Tile t = Main.tile[n.X, n.Y];
                    bool isSolid = t.HasTile && !t.IsActuated
                                   && Main.tileSolid[t.TileType]
                                   && !Main.tileSolidTop[t.TileType] && t.TileType != ModContent.TileType<InvisibleTile_Tile>();
                    newRoom.Add(n);
                    if (!isSolid)
                    {
                        q.Enqueue(n);
                    }
                }
            }

            return newRoom;
        }
        public static HashSet<Point> TileFloodFillCustom(Point startPoint, int startX, int endX, int startY, int endY, int safetyLimit, int BorderTileID)
        {
            HashSet<Point> newRoom = new HashSet<Point>();
            Queue<Point> q = new Queue<Point>();
            int SafetyLimit = safetyLimit;
            int count = 0;

            q.Enqueue(startPoint);
            newRoom.Add(startPoint);

            while (q.Count > 0)
            {
                count++;
                if (count > safetyLimit) break;

                Point p = q.Dequeue();

                Point[] neighbors = new Point[]
                {
                    new Point(p.X + 1, p.Y),
                    new Point(p.X - 1, p.Y),
                    new Point(p.X, p.Y + 1),
                    new Point(p.X, p.Y - 1),

                    new Point(p.X + 1, p.Y +1),
                    new Point(p.X + 1, p.Y -1),
                    new Point(p.X - 1, p.Y + 1),
                    new Point(p.X - 1, p.Y - 1)
                };

                foreach (Point n in neighbors)
                {
                    if (newRoom.Contains(n)) continue;

                    if (n.X < startX || n.X >= endX || n.Y < startY || n.Y >= endY) continue;

                    Tile t = Main.tile[n.X, n.Y];

                    newRoom.Add(n);
                    //works with any other id other than the border tile id for some reason
                    if (!(t.HasTile && t.TileType == BorderTileID))
                    {
                        q.Enqueue(n);
                    }
                }
            }

            return newRoom;
        }
    }
}