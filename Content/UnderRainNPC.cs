using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RainOverhaul.Content {
    // Damage control of NPCs under the rain 
    public class VictimMustDie:GlobalNPC {
        public static bool NPCinSafePlace;
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            ThisTileType thisTileType = new ThisTileType();

            for(int y = Main.screenPosition.ToTileCoordinates().Y; y < npc.Top.ToTileCoordinates().Y; y++) {
                Tile tTile = Main.tile[npc.Center.ToTileCoordinates().X,y];
                if(tTile.HasTile&&!thisTileType.Exists(tTile))
                {
                    NPCinSafePlace = true;
                    break;
                }
                else NPCinSafePlace = false;
            }

            int fValue = (int)Math.Round(RainSystem.HardIntensity*20);

            bool DamageCondition = !NPCinSafePlace;
            if(DamageCondition) npc.lifeRegen = -100*fValue;
            else npc.lifeRegen = default;
        }
    }
}