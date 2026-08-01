using DungeonDelvers.Content.DungeonGeneration.GenerationTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDelvers.Content.DungeonGeneration
{
    internal class DungeonGenerator : ModSystem
    {
        public override void PostUpdateWorld()
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Main.tile[x, y];

                    
                    if(tile.TileType == ModContent.TileType<JigsawTile>())
                    {
                        
                    }
                }
            }
        }
    }
}
