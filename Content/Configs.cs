using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RainOverhaul.Content {
  public class RainConfig:ModConfig {
    public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("RainIntensity")]
        [DefaultValue(1)]
        [DrawTicks]
        [Increment(0.25f)]
        [Range(0f, 1f)]
        public float cIntensity;

        [Header("RainSound")]
        [DefaultValue(false)]
        public bool cRainSound;
    }
    public class RainConfigAdditions:ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("RainWorldExactly")]
        [DefaultValue(false)]
        public bool cRainWorld;
    }
}