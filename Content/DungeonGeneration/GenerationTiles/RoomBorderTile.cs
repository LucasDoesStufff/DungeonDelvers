using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonDelvers.Content.DungeonGeneration.GenerationTiles
{
    internal class RoomBorderTile_Item : ModItem
    {
        public override string Texture => AssetDirectory.DungeonGeneration + "Items/" + Name;
        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 0;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.DefaultToPlaceableTile(ModContent.TileType<RoomBorderTile_Tile>(), 0);


            Item.consumable = false;
        }
    }
    internal class RoomBorderTile_Tile : ModTile
    {
        public override string Texture => AssetDirectory.DungeonGeneration + "Tiles/" + Name;
        public override void SetStaticDefaults()
        {
            TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileSolid[Type] = true;

            AddMapEntry(new Color(200, 200, 200));
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            noItem = true;

        }
    }
}
