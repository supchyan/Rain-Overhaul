using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using Terraria.Localization;

namespace RainOverhaul.Content {
    public class PlayerTools:ModPlayer {
        SoundStyle sEnter = new SoundStyle("RainOverhaul/Content/Sounds/sEnter");
        public override void OnEnterWorld() {
            if(ModContent.GetInstance<RainConfigServer>().cRainWorld) SoundEngine.PlaySound(sEnter);
            
            RainSystem.CycleState = RainSystem.CycleClear;

            if (Main.netMode == NetmodeID.Server && Main.maxRaining == 0) {
                Main.StartRain();
                Main.SyncRain();
            }
        }
    }
    public class RainSystem:ModSystem {
        private float OldMaxRaining;
        private float MaxRainingTransition;
        private float Intensity;
        public static float HardIntensity;
        private float RainTransition;
        private float Extra;
        private bool PlayerInSafePlace;
        public static bool QuakeSoundCondition;
        public static bool RainSoundCondition;
        public static bool DimRainSoundCondition;

        public const int CycleClear = 0;
        public const int CycleQuake = 1;
        public const int CycleRain = 2; 

        // Game time's pick states for "RainWorld" mode
        public const int CycleClearTimeEnd = 51700; // time when quake cycle starts 
        public const int CycleQuakeTimeEnd = 53999; // time when rain cycle starts 
        public const int CycleRainTimeEnd = 16200; // time when clear cycle starts

        public static int CycleState; // (clear / quake / raining) check consts above ^
        public float CycleRainForce;
        public float CycleQuakeStrength;
        public float CycleQuakeImpulse;

        // Rain logic
        public override void PostUpdateTime() {
            // Main.NewText(Main.windSpeedCurrent);

            // Affects vanilla rain state in game
            if(ModContent.GetInstance<RainConfigDev>().cStartRain) {
                Main.StartRain();
                ModContent.GetInstance<RainConfigDev>().cStartRain = false;
            }
            if(ModContent.GetInstance<RainConfigDev>().cStopRain) {
                Main.StopRain();
                ModContent.GetInstance<RainConfigDev>().cStopRain = false;
            }
            
            // Sync rain with server all time 
            Main.SyncRain();

            Filters.Scene.Activate("RainFilter"); 
            Filters.Scene.Activate("RainShake");
            // Returns true, when player overlaps (collides) the wall v
            // Tile tTile = Main.tile[Main.LocalPlayer.Center.ToTileCoordinates()];
            // bool WallCollision = tTile.WallType > WallID.None;
            
            RainTile RT = new RainTile();

            for(int y = Main.screenPosition.ToTileCoordinates().Y; y < Main.LocalPlayer.Top.ToTileCoordinates().Y; y++) {
                Tile tTile = Main.tile[Main.LocalPlayer.Center.ToTileCoordinates().X,y];
                if(tTile.HasTile&&!RT.CantProtectVanilla(tTile)) {
                    PlayerInSafePlace = true;
                    break;

                } else PlayerInSafePlace = false;
            }

            bool CommonCondition = Main.LocalPlayer.ZoneRain && !Main.LocalPlayer.ZoneNormalSpace && !Main.LocalPlayer.ZoneSandstorm && !Main.LocalPlayer.ZoneSnow;
            bool RainCondition = !PlayerInSafePlace && CommonCondition;

            if(Main.LocalPlayer.ZoneBeach||Main.LocalPlayer.ZoneJungle) Extra = 1.4f;
            else Extra = 1f;

            if(OldMaxRaining != Main.maxRaining) {
                if(MaxRainingTransition<1f) MaxRainingTransition+=0.01f;
                OldMaxRaining = MathHelper.Lerp(OldMaxRaining, Main.maxRaining, MaxRainingTransition);
            } else {
                MaxRainingTransition = 0f;
            }

            Intensity = 550*OldMaxRaining/(20.0f * 645.0f)*ModContent.GetInstance<RainConfig>().cIntensity;
            HardIntensity = 550*OldMaxRaining/(20.0f * 645.0f)*2.5f;

            if(!ModContent.GetInstance<RainConfigServer>().cRainWorld) {
                // Nulling custom rain behavior
                CycleState = CycleClear;
                
                if(RainCondition) {
                    if(RainTransition < 1f) RainTransition+=0.005f;
                
                } else {
                    if(RainTransition > 0f) RainTransition-=0.005f;
                }

                Filters.Scene["RainFilter"].GetShader().UseOpacity(Intensity*RainTransition*Extra).UseIntensity(RainTransition).UseProgress(-Main.windSpeedCurrent*4.0f);
                Filters.Scene["RainShake"].GetShader().UseOpacity(0f).UseIntensity(0f);
                
            } else {
                // This controls sound effects in the rain system
                QuakeSoundCondition = CycleState == CycleQuake;
                RainSoundCondition = CommonCondition;
                DimRainSoundCondition = CommonCondition && PlayerInSafePlace;

                if(RainCondition && !Main.LocalPlayer.dead && !Main.LocalPlayer.immune) {
                    int fValue = (int)Math.Round(HardIntensity*20);
                    if(fValue > 0) Main.LocalPlayer.AddBuff(ModContent.BuffType<ShelterNotification>(),2);
                }

                Filters.Scene["RainFilter"].GetShader().UseOpacity(CycleRainForce * 0.1f).UseIntensity(CycleRainForce).UseProgress(-Main.windSpeedCurrent*4.0f);
                Filters.Scene["RainShake"].GetShader().UseOpacity(CycleQuakeImpulse).UseIntensity(3.7f);

                bool RainWorldCondition = (Main.LocalPlayer.ZoneRain || Main.LocalPlayer.ZoneForest || Main.LocalPlayer.ZoneJungle || Main.LocalPlayer.ZoneDesert || Main.LocalPlayer.ZoneCrimson || Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneBeach || Main.LocalPlayer.ZoneHallow || Main.LocalPlayer.ZoneMeteor) && !Main.LocalPlayer.ZoneNormalSpace && !Main.LocalPlayer.ZoneSandstorm && !Main.LocalPlayer.ZoneSnow;

                // Custom rain behavior when in "RainWorld" mode
                switch(CycleState) {
                    case CycleClear: {
                        Main.raining = false;

                        if(CycleQuakeImpulse > 0f) CycleQuakeImpulse -= 0.05f;
                        if(CycleRainForce > 0f) CycleRainForce -= 0.01f;

                        if(Main.maxRaining != 0f) Main.maxRaining = 0f;

                        // Cycle state swap
                        if(Main.time >= CycleClearTimeEnd && Main.IsItDay()) {
                            CycleState = CycleQuake;
                        }
                        if(Main.time < CycleRainTimeEnd && !Main.IsItDay()) {
                            CycleState = CycleRain;
                        }

                    } break;

                    case CycleQuake: {
                        Main.raining = false;

                        CycleQuakeStrength = 1f + (float)(Main.time - CycleClearTimeEnd) / (float)(Main.dayLength - CycleClearTimeEnd);
                        
                        if(RainWorldCondition) CycleQuakeImpulse = ((float)Math.Sin(MathHelper.ToRadians((float)(Main.time - CycleClearTimeEnd) / 2f))) * CycleQuakeStrength;
                        else { if(CycleQuakeImpulse > 0.0f) CycleQuakeImpulse -= 0.1f; } // if player left certain biome, stop the quake
                        if(CycleRainForce > 0.0f) CycleRainForce -= 0.01f;
                        if(Main.maxRaining != 0f) Main.maxRaining = 0f;

                        // Cycle state swap
                        if((Main.time < CycleClearTimeEnd && Main.IsItDay()) || (Main.time >= CycleRainTimeEnd && !Main.IsItDay())) {
                            CycleState = CycleClear;
                        }
                        if(Main.time < CycleRainTimeEnd && !Main.IsItDay()) {
                            CycleState = CycleRain;
                        }

                    } break;

                    case CycleRain: {
                        if (!Main.raining) Main.StartRain();

                        if(RainWorldCondition) {
                            if(!PlayerInSafePlace) {
                                if(CycleQuakeImpulse != 5.07f) CycleQuakeImpulse = 5.07f;
                                if(CycleRainForce < 1.0f) CycleRainForce += 0.01f;

                            } else {
                                if(CycleQuakeImpulse < 1.07f) CycleQuakeImpulse += 0.1f;
                                else CycleQuakeImpulse -= 0.1f;
                                
                                if(CycleRainForce > 0.0f) CycleRainForce -= 0.01f;
                            }
                        } else {
                            if(CycleQuakeImpulse > 0.0f) CycleQuakeImpulse -= 0.1f;
                            if(CycleRainForce > 0.0f) CycleRainForce -= 0.01f;
                        }

                        if(Main.maxRaining != 0.97f) Main.maxRaining = 0.97f;

                        // Cycle state swap
                        if((Main.time >= CycleRainTimeEnd && !Main.IsItDay()) || (Main.time < CycleClearTimeEnd && Main.IsItDay())) {
                            CycleState = CycleClear;
                        }
                        if(Main.time >= CycleClearTimeEnd && Main.IsItDay()) {
                            CycleState = CycleQuake;
                        }

                    } break;
                }
            }
        }
    }
}