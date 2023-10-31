using Terraria;
using Terraria.ModLoader;

namespace RainOverhaul.Content {    
    public class UnderRainNPC:GlobalNPC {

        // Damage control of NPCs under the rain
        public bool LifeLost;
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc) {
            LifeLost = false;
        }
        public override void AI(NPC npc) {
            if(ModContent.GetInstance<RainConfigAdditions>().cRainWorld) {
                if(Main.LocalPlayer.HasBuff<ShelterNotification>() && Main.LocalPlayer.active && !Main.LocalPlayer.dead) {
                    npc.AddBuff(ModContent.BuffType<ShelterNotification>(), 2);
                }
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if(LifeLost) {
                npc.lifeRegen -= 200;
            }
        }
    }
}