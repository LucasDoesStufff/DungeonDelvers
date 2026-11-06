using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ObjectData;

namespace DungeonDelvers.Content.DungeonGeneration.GenerationTiles
{
    internal class InvisibleTile_Item : ModItem
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

            
            Item.DefaultToPlaceableTile(ModContent.TileType<InvisibleTile_Tile>(), 0);
            Item.consumable = false;
        }
    }

    internal class InvisibleTile_Tile : ModTile
    {
        public override string Texture => AssetDirectory.DungeonGeneration + "Tiles/" + Name;
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;

            Main.tileNoFail[Type] = false;




            TileObjectData.newTile.CoordinateHeights = new int[4] { 16, 16, 16, 16 };
            TileObjectData.newTile.CoordinateWidth = 16;


            TileObjectData.newTile.StyleHorizontal = true;


            TileObjectData.newTile.StyleWrapLimit = 4;


            TileObjectData.addTile(Type);
        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            noItem = true;

        }
    }
}
