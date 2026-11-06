
// DEPRECADED FOR NOW



/*using DungeonDelvers.Content.DungeonGeneration;


namespace DungeonDelvers.Content.Test
{
    internal class RoomDarkener : ModPlayer
    {

        List<HashSet<Point>> Rooms = new List<HashSet<Point>>();
        HashSet<Point> room = new HashSet<Point>();

        public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
        {
            int roombordertileID = ModContent.TileType<RoomBorderTile_Tile>();
            if (Main.gameMenu || Main.LocalPlayer == null)
                return;

            SpriteBatch spriteBatch = Main.spriteBatch;
            Texture2D textureFade = ModContent.Request<Texture2D>("DungeonDelvers/Content/DungeonGeneration/GenerationTiles/fade").Value;

            spriteBatch.End();

            int padding = 15;
            int startX = Math.Max(0, (int)(Main.screenPosition.X / 16f - (float)padding));
            int endX = Math.Min(Main.maxTilesX, (int)((Main.screenPosition.X + (float)Main.screenWidth) / 16f + (float)padding));
            int startY = Math.Max(0, (int)(Main.screenPosition.Y / 16f - (float)padding));
            int endY = Math.Min(Main.maxTilesY, (int)((Main.screenPosition.Y + (float)Main.screenHeight) / 16f + (float)padding));
            int width = endX - startX;
            int height = endY - startY;

            spriteBatch.Begin(0, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            HashSet<Point> screen = new HashSet<Point>();
            

            for (int x = startX; x < endX; x++)
            {
                for (int y = startY; y < endY; y++)
                {
                    Tile tile = Main.tile[x, y];

                    if(tile.TileType == ModContent.TileType<InvisibleTile_Tile>() && !room.Contains(new Point(x,y)))
                    {
                        room.UnionWith(Utilities.TileFloodFillCustom(new Point(x, y), startX, endX, startY, endY, 5000, ModContent.TileType<RoomBorderTile_Tile>()));
                        Rooms.Add(room);
                    }
                    screen.Add(new Point(x, y));
                }
            }

            if(room.Count > 0)
            {
                screen.ExceptWith(room);

                //paints outside of the room black
                foreach (var s in screen)
                {
                    //spriteBatch.Draw(textureFade, new Vector2(s.X * 16, s.Y * 16) - Main.screenPosition, 
                    //  new Rectangle(0, 4 * 16, 16, 16), Color.Black);
                }

                foreach (var i in room)
                {
                    Tile tile = Main.tile[i.X, i.Y];
                    
                    Point[] cardinalNeighbors = new Point[]
                    {

                        new Point(i.X, i.Y - 1),

                        new Point(i.X - 1, i.Y),
                        new Point(i.X + 1, i.Y),

                        new Point(i.X, i.Y + 1),

                    }; // corresponds to the image

                    byte[] cardinalNeighborVal = new byte[]
                    {
                        1,
                        2,
                        4,
                        8,
                    };

                    if (Utilities.IsSolid(tile))
                    {
                        int textureIndex = 0;
                        int[] lookup = new int[16]
                        {
                            5,  // 0:  floating
                            13, // 1:  
                            14,  // 2:  
                            15,  // 3:  up + left - out corner
                            12,  // 4:  
                            3,  // 5:  up + right - out corner
                            13,  // 6:  
                            1,  // 7:  up + left + right solid - fade down
                            4,  // 8:  
                            4,  // 9:  
                            11,  // 10: down + left
                            4,  // 11: up + left + down solid - fade right
                            7,  // 12: down + right
                            6,  // 13: up + right + down solid - fade left
                            9,  // 14: left + right + down solid - fade up
                            15,  // 15: all solid
                        };

                        for (int j = 0; j < cardinalNeighbors.Length; j++)
                        {
                            Tile t = Main.tile[cardinalNeighbors[j].X , cardinalNeighbors[j].Y ];
                            if (Utilities.IsSolid(t) || !room.Contains(cardinalNeighbors[j]))
                            {
                                textureIndex += cardinalNeighborVal[j];
                            }
                        }

                        if (textureIndex == 15)
                        {
                            Point[] diagonalNeighbor = new Point[]// indent corner fade draw
                            {
                                new Point(i.X - 1, i.Y - 1), // top-left
                                new Point(i.X + 1, i.Y - 1), // top-right
                                new Point(i.X - 1, i.Y + 1), // bottom-left
                                new Point(i.X + 1, i.Y + 1), // bottom-right
                            };

                            int[] diagonalNeighborVal = new int[]
                            {
                                1, // top-left
                                2, // top-right
                                4, // bottom-left
                                8, // bottom-right
                            };

                            int[] cornerFrames = new int[] { 10, 8, 2, 0 };
                            int diagonalIndex = 0;

                            for (int d = 0; d < diagonalNeighbor.Length; d++)
                            {
                                Tile t = Main.tile[diagonalNeighbor[d].X, diagonalNeighbor[d].Y];
                                if (room.Contains(diagonalNeighbor[d]) && !Utilities.IsSolid(t))
                                {
                                    diagonalIndex += diagonalNeighborVal[d];
                                    int cf = cornerFrames[d];
                                    int csx = (cf % 4) * 16;
                                    int csy = (cf / 4) * 16;
                                    //spriteBatch.Draw(textureFade, new Vector2(i.X * 16, i.Y * 16) - Main.screenPosition, 
                                    //    new Rectangle(csx, csy, 16, 16), Color.White);
                                }
                            }
                            //floating tiles
                            if (diagonalIndex == 0)
                            {
                                //spriteBatch.Draw(textureFade, new Vector2(i.X * 16, i.Y * 16) - Main.screenPosition, 
                                //    new Rectangle(0, 4 * 16, 16 ,16), Color.Black);
                            }
                            continue; // skip the normal draw below
                        }

                        int frame = lookup[textureIndex];

                        int sourceX = (frame % 4) * 16;
                        int sourceY = (frame / 4) * 16;
                        Rectangle sourceRect = new(sourceX, sourceY, 16, 16);

                        //this line draws the fade effect inside the room
                        //spriteBatch.Draw(textureFade, new Vector2(i.X * 16, i.Y * 16) - Main.screenPosition, sourceRect, Color.White);
                        HashSet<Point> edges = GetRoomEdges(room, roombordertileID);
                        foreach (var item in edges)
                        {
                            spriteBatch.Draw(textureFade, new Vector2(item.X * 16, item.Y * 16) - Main.screenPosition, sourceRect, Color.Red);
                        }
                    }
                }
            }
        }
        HashSet<Point> GetRoomEdges(HashSet<Point> room, int borderID)
        {
            HashSet<Point> edges = new HashSet<Point>();
            foreach (Point p in room)
            {
                Point[] neighbors = new Point[]
                {
                    new Point(p.X + 1, p.Y),
                    new Point(p.X - 1, p.Y),
                    new Point(p.X, p.Y + 1),
                    new Point(p.X, p.Y - 1)
                };
                foreach (Point n in neighbors)
                {
                    Tile t = Main.tile[n.X, n.Y];
                    if(t.TileType == borderID)
                    {
                        edges.Add(p);
                        break;
                    }
                }
            }
            return edges;
        }
    }
}*/