using Terraria;
using Terraria.ModLoader;
using Terraria.Audio;

namespace RainOverhaul.Content {
    public class ShelterNotification:ModBuff {

        public override string Texture => "RainOverhaul/Content/Texture/ShelterIcon";
        private float rIntensity;
        public override void SetStaticDefaults() {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }
        public override void Update(Player player, ref int buffIndex) {
            rIntensity = 550*Main.maxRaining/(20.0f * 645.0f)*2.5f;
            if(player.velocity.Y < 0) player.velocity.Y += rIntensity*5;
            player.mount.Dismount(player);
        }
    }
}