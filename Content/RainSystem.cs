using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace RainOverhaul.Content {
    public class ForcedRainSync:ModPlayer {
        public override void OnEnterWorld() {
            Main.StopRain();
            Main.maxRaining = 0;
            Main.raining = false;

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

        public static int CycleTime; 

        public const int CycleClear = 0;
        public const int CycleQuake = 1;
        public const int CycleRain = 2; 

        public int CycleClearTime = 1500; // time when is clear // 108000
        public int CycleQuakeTime = 2300; // time when is quaking // 2300
        public int CycleRainTime = 1500; // time when is raining // 36000
        public int CycleState = 0; // (clear / quake / raining) check consts above ^^^
        public float CycleRainForce;
        public float CycleQuakeStrength;
        public float CycleQuakeImpulse;

        public SoundStyle sCycleSwap = new SoundStyle("RainOverhaul/Content/Sounds/sCycleSwap");
        public bool PlayCycleSound; // condition to play sound above ^^^



        // Rain logic
        public override void PostUpdateTime() {

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

            // old stuff, but returns true, when player overlaps (collides) the wall vvv
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

            // Filters.Scene["RainShake"].GetShader().UseOpacity(ShakeTransition*1.07f).UseIntensity(3.7f);

            if(!ModContent.GetInstance<RainConfigAdditions>().cRainWorld) {
                // null custom rain behavior
                CycleTime = 0;
                CycleState = CycleClear;
                
                if(RainCondition) {
                    if(RainTransition < 1f) RainTransition+=0.005f;
                
                } else {
                    if(RainTransition > 0f) RainTransition-=0.005f;
                }

                Filters.Scene["RainFilter"].GetShader().UseOpacity(Intensity*RainTransition*Extra).UseIntensity(RainTransition);
                Filters.Scene["RainShake"].GetShader().UseOpacity(0f).UseIntensity(0f);
                
            } else {

                // this controls sound effects to a rain system
                QuakeSoundCondition = CycleState == CycleQuake;
                RainSoundCondition = CommonCondition;
                DimRainSoundCondition = CommonCondition && PlayerInSafePlace;

                if(RainCondition && !Main.LocalPlayer.dead && !Main.LocalPlayer.immune) {
                    int fValue = (int)Math.Round(HardIntensity*20);
                    if(fValue > 0) Main.LocalPlayer.AddBuff(ModContent.BuffType<ShelterNotification>(),2);
                }

                CycleTime++;

                // Main.NewText(CycleTime);

                Filters.Scene["RainFilter"].GetShader().UseOpacity(CycleRainForce * 0.1f).UseIntensity(CycleRainForce);
                Filters.Scene["RainShake"].GetShader().UseOpacity(CycleQuakeImpulse).UseIntensity(3.7f);

                bool RainWorldCondition = (Main.LocalPlayer.ZoneRain || Main.LocalPlayer.ZoneForest || Main.LocalPlayer.ZoneJungle || Main.LocalPlayer.ZoneDesert || Main.LocalPlayer.ZoneCrimson || Main.LocalPlayer.ZoneCorrupt || Main.LocalPlayer.ZoneBeach || Main.LocalPlayer.ZoneHallow || Main.LocalPlayer.ZoneMeteor) && !Main.LocalPlayer.ZoneNormalSpace && !Main.LocalPlayer.ZoneSandstorm && !Main.LocalPlayer.ZoneSnow;

                // Custom rain behavior when in "RainWorld" mode
                switch(CycleState) {
                    case CycleClear: {
                        Main.StopRain();
                        Main.raining = false;

                        if(CycleQuakeImpulse > 0f) CycleQuakeImpulse -= 0.05f;
                        if(CycleRainForce > 0f) CycleRainForce -= 0.01f;

                        if(Main.maxRaining != 0f) Main.maxRaining = 0.01f;

                        if(CycleTime >= CycleClearTime) {
                            SoundEngine.PlaySound(sCycleSwap);

                            CycleTime = 0;
                            CycleState = CycleQuake;
                        }

                    } break;

                    case CycleQuake: {

                        CycleQuakeStrength = 1f + CycleTime/1000f;
                        
                        if(RainWorldCondition) CycleQuakeImpulse = ((float)Math.Sin(MathHelper.ToRadians(CycleTime/2f)))*CycleQuakeStrength;
                        else { if(CycleQuakeImpulse > 0.0f) CycleQuakeImpulse -=0.1f; }

                        if(CycleTime >= CycleQuakeTime) {
                            CycleTime = 0;
                            CycleState = CycleRain;
                        }

                    } break;

                    case CycleRain: {                        
                        Main.StartRain();
                        Main.raining = true;
                        if(RainWorldCondition) {
                            if(!PlayerInSafePlace) {
                                if(CycleQuakeImpulse < 5.07f) CycleQuakeImpulse += 0.1f;
                                else CycleQuakeImpulse -= 0.1f;

                                if(CycleRainForce < 1.0f) CycleRainForce += 0.01f;

                            } else {
                                if(CycleQuakeImpulse < 1.07f) CycleQuakeImpulse += 0.1f;
                                else CycleQuakeImpulse -= 0.1f;
                                
                                if(CycleRainForce > 0.0f) CycleRainForce -= 0.01f;
                            }
                        } else {
                            if(CycleQuakeImpulse > 0.0f) CycleQuakeImpulse -=0.1f;
                            if(CycleRainForce > 0.0f) CycleRainForce -= 0.01f;
                        }

                        if(Main.maxRaining != 0.97f) Main.maxRaining = 0.97f;

                        if(CycleTime >= CycleRainTime) {
                            SoundEngine.PlaySound(sCycleSwap);

                            CycleTime = 0;
                            CycleState = CycleClear;
                        }

                    } break;
                }

                // drawing circle with shader from above comment
                // for(int i=0; i<Main.maxNPCs; i++) {
                //     if(!Main.LocalPlayer.HasBuff<ShelterNotification>() && Main.npc[i].active && Main.npc[i].HasBuff<ShelterNotification>()) {
                //         Projectile.NewProjectile(Entity.GetSource_None(), Main.npc[i].Center, Main.npc[i].velocity, ModContent.ProjectileType<RainCircle>(), 0, 0, Main.LocalPlayer.whoAmI);
                //     }
                // }
                
                         
            }
        }
    }
}