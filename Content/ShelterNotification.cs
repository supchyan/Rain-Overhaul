using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using ReLogic.Utilities;


namespace RainOverhaul.Content {
    public class ShelterNotification:ModBuff {
        
        // Buff notification and "heavy rain's" consequences
        public override string Texture => "RainOverhaul/Content/Textures/ShelterIcon";
        public override void SetStaticDefaults() {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex) {   
            float rIntensity = 550*Main.maxRaining/(20.0f * 645.0f)*2.5f;
            if(player.mount._type != 12) {
                if(player.velocity.Y != 0) player.velocity.Y += rIntensity*20;
                player.mount.Dismount(player);
            }            
        }
        public override void Update(NPC npc, ref int buffIndex) {
            npc.TryGetGlobalNPC<UnderRainNPC>(out UnderRainNPC Mmm);
            Mmm.LifeLost = true;
        }
    }
}