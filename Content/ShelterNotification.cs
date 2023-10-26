using System;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using ReLogic.Utilities;


namespace RainOverhaul.Content {
    public class ShelterNotification:ModBuff {

        // Buff notification and heavy rain's consequences
        public override string Texture => "RainOverhaul/Content/Texture/ShelterIcon";
        private float rIntensity;
        private SoundStyle DeathSound = new SoundStyle("RainOverhaul/Content/Sounds/sDeath");
        public SlotId TUUM;
        public override void SetStaticDefaults() {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex) {
            rIntensity = 550*Main.maxRaining/(20.0f * 645.0f)*2.5f;
            int fValue = (int)Math.Round(rIntensity*20);
            
            if(player.statLife>10) player.lifeRegen -= 100*fValue;
            else {
                player.KillMe(PlayerDeathReason.ByCustomReason(player.name + " " + Language.GetTextValue("Mods.RainOverhaul.RainDeathReason")), 9999, 0);
                TUUM = SoundEngine.PlaySound(DeathSound with {Volume=1.2f,MaxInstances=3,SoundLimitBehavior = SoundLimitBehavior.ReplaceOldest}, player.Center);
                
            }

            if(player.velocity.Y < 0) player.velocity.Y += rIntensity*5;
            player.mount.Dismount(player);
        }
        public override void Update(NPC npc, ref int buffIndex) {
            rIntensity = 550*Main.maxRaining/(20.0f * 645.0f)*2.5f;
            int fValue = (int)Math.Round(rIntensity*20);

            npc.lifeRegen -= 200*fValue;
        }
    }
}