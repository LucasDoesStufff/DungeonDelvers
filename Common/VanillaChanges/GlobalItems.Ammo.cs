using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;


namespace DungeonDelvers.Common.VanillaChanges
{
    partial class GlobalItems : GlobalItem
    {
        void DisableAmmoCount(ILContext il)
        {
            var c = new ILCursor(il);

            // Find the read of item.useAmmo; stop right AFTER it,
            // when its value is sitting on the stack.
            if (!c.TryGotoNext(MoveType.After, i => i.MatchLdfld<Item>(nameof(Item.useAmmo))))
                throw new System.Exception("Could not find Item.useAmmo in ItemSlot.Draw");

            // Throw the real value away and push 0 instead.
            // The comparison "useAmmo > 0" becomes "0 > 0", which is always false.
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4_0);
        }
    }
}
