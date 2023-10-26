using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace RainOverhaul.Content {    
    public class VictimMustDie:GlobalNPC {
        public static bool NPCinSafePlace;
        public static bool DamageCondition;
        public static int fValue;

        // Damage control of NPCs under the rain 
        public override void AI(NPC npc) {
            for(int y = Main.screenPosition.ToTileCoordinates().Y; y < npc.Top.ToTileCoordinates().Y; y++) {
                Tile tTile = Main.tile[npc.Center.ToTileCoordinates().X,y];
                if(tTile.HasTile&&RainTile.CanProtect(tTile))
                {
                    NPCinSafePlace = true;
                    break;
                }
                else NPCinSafePlace = false;
            }

            fValue = (int)Math.Round(RainSystem.HardIntensity*20);

            DamageCondition = !NPCinSafePlace && Main.raining;
            if(DamageCondition) npc.AddBuff(ModContent.BuffType<ShelterNotification>(), 2);
        }
    }
}