using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;
using Terraria.DataStructures;
using Terraria.Graphics.Effects;
using Terraria.Audio;
using ReLogic.Utilities;

namespace RainOverhaul.Content {
    public class ForcedRainSync:ModPlayer {
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
        private bool TileAboveNPC;
        public static bool SoundCondition;
        public static bool DimSoundCondition;

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

            bool RainCondition = !TileAbovePlayer && Main.LocalPlayer.ZoneRain && !Main.LocalPlayer.ZoneNormalSpace;

            SoundCondition = Main.LocalPlayer.ZoneRain && !Main.LocalPlayer.ZoneNormalSpace && ModContent.GetInstance<RainConfigAdditions>().cRainWorld; 
            
            DimSoundCondition = TileAbovePlayer && Main.LocalPlayer.ZoneRain && !Main.LocalPlayer.ZoneNormalSpace && ModContent.GetInstance<RainConfigAdditions>().cRainWorld;

            if(RainCondition) {
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
                KillEnemiesWithRain();
                
                if(RainCondition && !Main.LocalPlayer.dead && !Main.LocalPlayer.immune) {
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
                    } else Victim.life -= (int)Math.Round(HardIntensity*20);
                }
            }
        }
    }
    public class aRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/sRain");

        public override bool IsBiomeActive(Player player) {
            if(Visuals.SoundCondition) {
                return true;
            } else return false;
        }
    }
    public class aDimRainSound:ModBiome {
        public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
        public override int Music => MusicLoader.GetMusicSlot(Mod, "Content/sDimRain");

        public override bool IsBiomeActive(Player player) {
            if(Visuals.DimSoundCondition) {
                return true;
            } else return false;
        }
    }
    public sealed class aRainSoundRegister:ILoadable {
		public void Load(Mod mod) {
			MusicLoader.AddMusic(mod, "Content/sRain");
            MusicLoader.AddMusic(mod, "Content/sDimRain");
        }
		public void Unload() { }
	}
}