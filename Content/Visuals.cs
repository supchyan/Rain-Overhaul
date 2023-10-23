using System;
﻿using System.ComponentModel;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.ModLoader.Config;



namespace RainOverhaul.Content {
    public class startThis:ModPlayer {
        public override void OnEnterWorld() {
            if (Main.netMode == NetmodeID.Server && Main.maxRaining == 0) {
                Main.StartRain();
                Main.SyncRain();
            }
        }
    }
    public class Visuals:ModSystem {
        private float Intensity;
        private float HardIntensity;
        private float RainTransition;
        private float Extra;
        private bool TileAbovePlayer;

        public override void PostUpdateTime() {

            Main.SyncRain();

            // RAIN FILTER

            Filters.Scene.Activate("RainFilter"); 

            Tile tile = Main.tile[Main.LocalPlayer.Center.ToTileCoordinates()];
            bool WallCollision = tile.WallType > WallID.None;

            for(int y = Main.screenPosition.ToTileCoordinates().Y; y < Main.LocalPlayer.Top.ToTileCoordinates().Y; y++) {
                if(Main.tile[Main.LocalPlayer.Center.ToTileCoordinates().X,y].HasTile) {
                    TileAbovePlayer = true;
                    break;
                }
                else TileAbovePlayer = false;
            }

            // bool FullCondition = WallCollision && TileAbove;

            bool rainCondition =
                Main.IsItRaining && !TileAbovePlayer &&
                !Main.LocalPlayer.ZoneSandstorm && !Main.LocalPlayer.ZoneSnow && 
                !Main.LocalPlayer.ZoneNormalSpace; // Main.LocalPlayer.position.Y < WorldFactorY

            if(rainCondition) {
                if(RainTransition < 1f) RainTransition+=0.01f;
            } else {
                if(RainTransition > 0f) RainTransition-=0.01f;
            }

            if(Main.LocalPlayer.ZoneBeach||Main.LocalPlayer.ZoneJungle) Extra = 1.4f;
            else Extra = 1f;

            Intensity = 550*Main.maxRaining/(20.0f * 645.0f)*ModContent.GetInstance<RainConfig>().cIntensity;
            HardIntensity = 550*Main.maxRaining/(20.0f * 645.0f)*2.5f;           

            if(!ModContent.GetInstance<RainConfigAdditions>().cRainWorld) {
                Filters.Scene["RainFilter"].GetShader().UseOpacity(Intensity*RainTransition*Extra).UseIntensity(RainTransition);
            } else {
                if(rainCondition && !Main.LocalPlayer.dead && !Main.LocalPlayer.immune) {
                    Main.LocalPlayer.statLife -= (int)Math.Round(HardIntensity*20);
                    if(Main.LocalPlayer.statLife <= 0 && Main.LocalPlayer.active) {
                        Main.LocalPlayer.statLife = 0;
                        Main.LocalPlayer.KillMe(PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + " " + Language.GetTextValue("Mods.RainOverhaul.RainDeathReason")), 9999, 0);
                    } 
                    if(Main.LocalPlayer.velocity.Y != 0) Main.LocalPlayer.velocity.Y += HardIntensity*50;
                }
                Filters.Scene["RainFilter"].GetShader().UseOpacity(HardIntensity*RainTransition*Extra).UseIntensity(RainTransition);
            }
        }
    }
    public class RainConfig:ModConfig {
		public override ConfigScope Mode => ConfigScope.ClientSide;

		[DefaultValue(1)]
        [DrawTicks]
        [Increment(0.25f)]
        [Range(0f, 1f)]
        [Header("RainIntensity")]
		public float cIntensity;
    }
    public class RainConfigAdditions:ModConfig {
		public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("RainWorldExactly")]
        [DefaultValue(false)]
		public bool cRainWorld;
    }
}