

using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ObjectData;

namespace DungeonDelvers.Content.DungeonGeneration.GenerationTiles
{
    internal class JigsawTileItem : ModItem     
    {
        internal class JigsawTileItem_Loader : ILoadable
        {
            public void Load(Mod mod)
            {
                for (int i = 0; i < 4; i++)
                {
                    mod.AddContent(new JigsawTileItem(i));
                }
            }
            public void Unload()
            {
            }
        }
        private enum JigsawTileDirections
        {
            JigsawDown = 0,
            JigsawRight = 1,
            JigsawUp = 2,
            JigsawLeft = 3
        }

        protected override bool CloneNewInstances => true;
        private readonly int placeStyle;

        public override string Name => GetInternalNameFromStyle(placeStyle);

        public static string GetInternalNameFromStyle(int style)
        {

            return Enum.GetName(typeof(JigsawTileDirections), style);

            throw new Exception("Invalid style");
        }
        public JigsawTileItem(int placeStyle)
        {
            this.placeStyle = placeStyle;
        }

        public override void SetDefaults()
        {
            Item.width = 16;
            Item.height = 16;
            Item.maxStack = 999;
            Item.value = 0;
            
            Item.DefaultToPlaceableTile(ModContent.TileType<JigsawTile>(), placeStyle);
            Item.consumable = false;
        }
    }

    internal class JigsawTile : ModTile
    {

        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            TileID.Sets.DisableSmartCursor[Type] = true;
            Main.tileSolid[Type] = true;
            Main.tileLavaDeath[Type] = false;

            Main.tileNoFail[Type] = false;




            TileObjectData.newTile.CoordinateHeights = new int[4] { 16,16,16,16 };
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
