using DungeonDelvers.Content.DungeonGeneration.GenerationTiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDelvers.Content.DungeonGeneration
{
    internal class InvisibleTile_Item : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 0;
            Item.useAnimation = 15;
            Item.useTime = 10;

            
            Item.DefaultToPlaceableTile(ModContent.TileType<InvisibleTile_Tile>(), 0);
            Item.consumable = false;
        }
    }
}
