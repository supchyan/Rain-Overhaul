using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;

namespace RainOverhaul.Content {
    public class ForcedRainSync:ModPlayer {
        public override void OnEnterWorld() {
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
        private float ShakeTransition;
        private float Extra;
        private bool PlayerInSafePlace;
        public static bool SoundCondition;
        public static bool DimSoundCondition;

        // Rain logic
        public override void PostUpdateTime() {
            
            // Sync rain with server all time 
            Main.SyncRain();

            Filters.Scene.Activate("RainFilter"); 
            Filters.Scene.Activate("RainShake");

            // this should draws above entities under the rain,
            // when player in the save place
            // Filters.Scene.Activate("RainCircle"); 

            // old stuff, but returns true, when player is standing 
            // before wall (collides it with it's body k?)
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
            bool ShakeCondition = CommonCondition && ModContent.GetInstance<RainConfigAdditions>().cRainWorld;

            SoundCondition = CommonCondition && ModContent.GetInstance<RainConfigAdditions>().cRainWorld; 
            DimSoundCondition = ShakeCondition && PlayerInSafePlace;

            if(RainCondition) {
                if(RainTransition < 1f) RainTransition+=0.01f;
            
            } else {
                if(RainTransition > 0f) RainTransition-=0.01f;
            }
            if(ShakeCondition) {
                if(ShakeTransition < 1f) ShakeTransition+=0.01f;
            
            } else {
                if(ShakeTransition > 0f) ShakeTransition-=0.01f;
            }

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

            Filters.Scene["RainShake"].GetShader().UseOpacity(ShakeTransition*1.07f).UseIntensity(3.7f);

            if(!ModContent.GetInstance<RainConfigAdditions>().cRainWorld) {
                Filters.Scene["RainFilter"].GetShader().UseOpacity(Intensity*RainTransition*Extra).UseIntensity(RainTransition);
            
            } else {
                if(RainCondition && !Main.LocalPlayer.dead && !Main.LocalPlayer.immune) {
                    int fValue = (int)Math.Round(HardIntensity*20);
                    if(fValue > 0) Main.LocalPlayer.AddBuff(ModContent.BuffType<ShelterNotification>(),2);
                }

                // drawing circle with shader from above comment
                // for(int i=0; i<Main.maxNPCs; i++) {
                //     if(!Main.LocalPlayer.HasBuff<ShelterNotification>() && Main.npc[i].active && Main.npc[i].HasBuff<ShelterNotification>()) {
                //         Projectile.NewProjectile(Entity.GetSource_None(), Main.npc[i].Center, Main.npc[i].velocity, ModContent.ProjectileType<RainCircle>(), 0, 0, Main.LocalPlayer.whoAmI);
                //     }
                // }
                
                Filters.Scene["RainFilter"].GetShader().UseOpacity(HardIntensity*RainTransition*Extra).UseIntensity(RainTransition);            
            }
        }
    }
}