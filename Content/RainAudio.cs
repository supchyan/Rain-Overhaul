using Terraria;
using Terraria.ModLoader;

namespace RainOverhaul.Content {
    // Audio replace (Rain World mode only)
    public class aRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/sRain");

        public override bool IsBiomeActive(Player player) {
            if(RainSystem.SoundCondition) {
                return true;
            } else return false;
        }
    }
    public class aDimRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/sDimRain");

        public override bool IsBiomeActive(Player player) {
            if(RainSystem.DimSoundCondition) {
                return true;
            } else return false;
        }
    }
    public sealed class aRainSoundRegister:ILoadable {
		public void Load(Mod mod) {
			MusicLoader.AddMusic(mod, "Content/Sounds/sRain");
            MusicLoader.AddMusic(mod, "Content/Sounds/sDimRain");
        }
		public void Unload() { }
	}
}