using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
﻿using System.ComponentModel;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;
using Terraria.ModLoader.Config;
using Terraria.Localization;

namespace RainOverhaul.Content {
    public class Visuals:ModSystem {
        private float Intensity;
        private float RainTransition;
        private float Extra;
        private float WorldFactorY;

        public override void PreUpdateTime() {
            
            // WORLD PROPS

            if(Main.maxTilesX == 4200) WorldFactorY = 4400f;
            else if(Main.maxTilesX == 6400) WorldFactorY = 6600f;
            else if(Main.maxTilesX == 8400) WorldFactorY = 8800f;

            // RAIN FILTER

            Filters.Scene.Activate("RainFilter"); 

            if(Filters.Scene["RainFilter"].IsActive()) {

                Tile tile = Main.tile[Main.LocalPlayer.Center.ToTileCoordinates()];
                bool WallCollision = tile.WallType > WallID.None;

                if(Main.raining && !WallCollision && 
                !Main.LocalPlayer.ZoneSandstorm && !Main.LocalPlayer.ZoneSnow && 
                !Main.LocalPlayer.ZoneNormalSpace && Main.LocalPlayer.position.Y < WorldFactorY) {
                    RainTransition+=0.01f;
                    if(RainTransition > 1f)
                    RainTransition = 1f;
                    if(Main.rainTime <= 1000) {
                        Main.maxRain = (int)MathHelper.Lerp(0, Main.maxRain, (float)Main.rainTime/1000);
                        Main.maxRaining = MathHelper.Lerp(0, Main.maxRaining, (float)Main.rainTime/1000);
                    }
                }
                else {
                    RainTransition-=0.01f;
                    if(RainTransition < 0f)
                    RainTransition = 0f;
                }

                if(Main.LocalPlayer.ZoneBeach || Main.LocalPlayer.ZoneJungle) Extra = 1.4f;
                else Extra = 1f;

                Intensity = 550 * Main.maxRaining / (20.0f * 645.0f) * ModContent.GetInstance<RainConfig>().Intensity;

                Filters.Scene["RainFilter"].GetShader().UseOpacity(Intensity*RainTransition*Extra).UseIntensity(RainTransition);
            }
        }
    }
    public class RainConfig:ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;
        // [BackgroundColor(255, 0, 255)]
		[DefaultValue(1)]
        [DrawTicks]
        [Increment(0.25f)]
        [Range(0f, 1f)]
        [Header("RainIntensity")]
		public float Intensity;
    }
}