using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Newtonsoft.Json;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.ID;

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

        [BackgroundColor(0, 0, 0)]
        public NPCDefinition npcDefinitionExample = new NPCDefinition(NPCID.Bunny);

        [Header("TestFeatures")]
		public Vector2 cCycleIndicatorPos;

        [Header("RemoveItLater")]
        [Range(-10f, 10f)]
        public float cShaderMUL;

        [Header("WeatherSettings")]
        public bool cStartRain;

        public bool cStopRain;
    }
}