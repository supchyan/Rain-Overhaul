using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Audio;
using ReLogic.Utilities;
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
        private float HardIntensity;
        private float RainTransition;
        private float ShakeTransition;
        private float Extra;
        private bool PlayerInSafePlace;
        private bool TileAboveNPC;
        public static bool SoundCondition;
        public static bool DimSoundCondition;
        private SoundStyle DeathSound = new SoundStyle("RainOverhaul/Content/Sounds/sDeath");

        public override void PostUpdateTime() {

            Main.SyncRain();

            // RAIN FILTER

            Filters.Scene.Activate("RainFilter"); 
            Filters.Scene.Activate("RainShake");
            // Filters.Scene.Activate("RainVignette");

            Tile tile = Main.tile[Main.LocalPlayer.Center.ToTileCoordinates()];
            bool WallCollision = tile.WallType > WallID.None; // old stuff if u wanna get back wall collision rain condition

            for(int y = Main.screenPosition.ToTileCoordinates().Y; y < Main.LocalPlayer.Top.ToTileCoordinates().Y; y++) {
                if(Main.tile[Main.LocalPlayer.Center.ToTileCoordinates().X,y].HasTile && WallCollision) {
                    PlayerInSafePlace = true;
                    break;
                }
                else PlayerInSafePlace = false;
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
                
                KillEnemiesWithRain();
                
                if(RainCondition && !Main.LocalPlayer.dead && !Main.LocalPlayer.immune) {
                    int fValue = (int)Math.Round(HardIntensity*20);
                    if(fValue > 0) Main.LocalPlayer.AddBuff(ModContent.BuffType<ShelterNotification>(),2);
                    Main.LocalPlayer.statLife -= fValue;
                    if(Main.LocalPlayer.statLife <= 0 && Main.LocalPlayer.active) {
                        Main.LocalPlayer.statLife = 0;
                        Main.LocalPlayer.KillMe(PlayerDeathReason.ByCustomReason(Main.LocalPlayer.name + " " + Language.GetTextValue("Mods.RainOverhaul.RainDeathReason")), 9999, 0);
                        SoundEngine.PlaySound(DeathSound, Main.LocalPlayer.Center); // not syncing on server
                    }
                }
                Filters.Scene["RainFilter"].GetShader().UseOpacity(HardIntensity*RainTransition*Extra).UseIntensity(RainTransition);            
            }
        }
        private void KillEnemiesWithRain() {
            for(int i=0; i<Main.maxNPCs; i++) {
                NPC Victim = Main.npc[i];

                for(int y = Main.screenPosition.ToTileCoordinates().Y; y < Victim.Top.ToTileCoordinates().Y; y++) {
                    if(Main.tile[Victim.Center.ToTileCoordinates().X,y].HasTile) {
                        TileAboveNPC = true;
                        break;
                    }
                    else TileAboveNPC = false;
                }

                bool damageCondition = Main.IsItRaining && !TileAboveNPC;

                if(damageCondition) {
                    if(Victim.life <= 3 && Victim.active) {
                        Victim.life = 0;
                        Victim.checkDead();
                        Victim.active = false;
                    } else Victim.life -= (int)Math.Round(HardIntensity*20);
                }
            }
        }
    }
    public class aRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/sRain");

        public override bool IsBiomeActive(Player player) {
            if(RainSystem.SoundCondition) {
                return true;
            } else return false;
        }
    }
    public class aDimRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/Sounds/sDimRain");

        public override bool IsBiomeActive(Player player) {
            if(RainSystem.DimSoundCondition) {
                return true;
            } else return false;
        }
    }
    public sealed class aRainSoundRegister:ILoadable {
		public void Load(Mod mod) {
			MusicLoader.AddMusic(mod, "Content/Sounds/sRain");
            MusicLoader.AddMusic(mod, "Content/Sounds/sDimRain");
        }
		public void Unload() { }
	}
}