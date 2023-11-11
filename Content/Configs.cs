using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RainOverhaul.Content {
  public class RainConfig:ModConfig {
    public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("RainIntensity")]
        [DefaultValue(1)]
        [Range(0f, 1f)]
        public float cIntensity;
    }
    public class RainConfigAdditions:ModConfig {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("RainWorldExactly")]
        [DefaultValue(false)]
        public bool cRainWorld;
    }
    public class RainConfigDev:ModConfig {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("CircleProps")]
        [DefaultValue(8f)]
        public float cProgress;
    }
}