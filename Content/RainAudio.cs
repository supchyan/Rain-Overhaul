using Terraria;
using Terraria.Audio;
using Terraria.ModLoader;
using System;

using Terraria.ID;

using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;


namespace RainOverhaul.Content {
    
    // Audio replace (Rain World mode only)
    // public class aQuakeSound:ModBiome {
    //     public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
    //     public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/sQuake");

    //     public override bool IsBiomeActive(Player player) {
    //         if(RainSystem.CycleState == quake) {
    //             return true;
    //         } else return false;
    //     }
    // }
    public class aRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/sRain");

        public override bool IsBiomeActive(Player player) {
            if(RainSystem.RainSoundCondition) {
                return true;
            } else return false;
        }
    }
    public class aDimRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/sDimRain");
        public override bool IsBiomeActive(Player player) {
            if(RainSystem.DimRainSoundCondition) {
                return true;
            } else return false;
        }
    }
    public sealed class aRainSoundRegister:ILoadable {
		public void Load(Mod mod) {
            MusicLoader.AddMusic(mod, "Content/Sounds/sQuake");
			MusicLoader.AddMusic(mod, "Content/Sounds/sRain");
            MusicLoader.AddMusic(mod, "Content/Sounds/sDimRain");
        }
		public void Unload() { }
	}
}