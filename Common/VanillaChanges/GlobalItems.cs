using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.UI;

namespace DungeonDelvers.Common.VanillaChanges
{
    partial class GlobalItems : GlobalItem
    {
        public override void Load()
        {
            IL_ItemSlot.Draw_SpriteBatch_ItemArray_int_int_Vector2_Color += DisableAmmoCount;
        }
        public override void SetDefaults(Item entity)
        {
            base.SetDefaults(entity);
            if (entity.ammo != AmmoID.None)
            {
                entity.consumable = false;
                entity.maxStack = 1;
            }
        }
    }
}
