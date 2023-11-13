using Terraria;
using Terraria.ModLoader;

namespace RainOverhaul.Content {    
    public class UnderRainNPC:GlobalNPC {

        // Damage control of NPCs under the rain
        public bool LifeLost;
        public bool NPCinSafePlace = true;
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc) {
            LifeLost = false;
            NPCinSafePlace = true;
        }
        public override void AI(NPC npc) {
            RainTile RT = new RainTile();
            if(!npc.townNPC) {
                for(int y = Main.screenPosition.ToTileCoordinates().Y; y < npc.Top.ToTileCoordinates().Y; y++) {
                    Tile tTile = Main.tile[npc.Center.ToTileCoordinates().X,y];
                    if(tTile.HasTile&&!RT.CantProtectVanilla(tTile)) {
                        NPCinSafePlace = true;
                        break;

                    } else {
                        NPCinSafePlace = false;
                    }
                }
            } else {
                for(int y = 0; y < npc.Top.ToTileCoordinates().Y; y++) {
                    Tile tTile = Main.tile[npc.Center.ToTileCoordinates().X,y];
                    if(tTile.HasTile&&!RT.CantProtectVanilla(tTile)) {
                        NPCinSafePlace = true;
                        break;

                    } else {
                        NPCinSafePlace = false;
                    }
                }
            }
            
            bool CommonCondition = Main.LocalPlayer.ZoneRain && !Main.LocalPlayer.ZoneNormalSpace && !Main.LocalPlayer.ZoneSandstorm && !Main.LocalPlayer.ZoneSnow;
            if(ModContent.GetInstance<RainConfig>().cRainWorld&&Main.raining&&!NPCinSafePlace&&(Main.LocalPlayer.Center-npc.Center).Length()<1500&&CommonCondition) {
                npc.AddBuff(ModContent.BuffType<ShelterNotification>(), 2);
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if(LifeLost) {
                npc.lifeRegen -= 200;
            }
        }
    }
}