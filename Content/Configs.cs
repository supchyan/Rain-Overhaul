using System.ComponentModel;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.ModLoader.Config;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Linq;


namespace RainOverhaul.Content {
    public class RainConfig:ModConfig {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("RainSettings")]
        [DefaultValue(1)]
        [Range(0f, 1f)]
        public float cIntensity;

        [Header("RainWorldExactly")]
        [DefaultValue(false)]
        public bool cRainWorld;

        [DefaultValue(typeof(Vector2), "0.04, 0.9")]
        public Vector2 cCycleIndicatorPos;
    }
    public class RainConfigDev:ModConfig {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [BackgroundColor(0, 0, 0)]
        public NPCDefinition npcDefinitionExample = new NPCDefinition(NPCID.Bunny);


        [Header("RemoveItLater")]
        [Range(-10f, 10f)]
        public float cShaderMUL;

        [Header("WeatherSettings")]
        public bool cStartRain;

        public bool cStopRain;
    }
}