using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Effects;

namespace RainOverhaul.Content {
    public class Visuals : ModSystem {

        private float Intensity;
        private float RainTransition;
        private float Extra;

        public override void PreUpdateTime() {

            // RAIN FILTER

            if (Main.netMode != NetmodeID.Server && !Filters.Scene["RainFilter"].IsActive()) 
            Filters.Scene.Activate("RainFilter"); 

            if(Main.netMode != NetmodeID.Server && Filters.Scene["RainFilter"].IsActive()) {

                Tile tile = Main.tile[Main.LocalPlayer.Center.ToTileCoordinates()];
                bool WallCollision = tile.WallType > WallID.None;

                if(Main.raining && !WallCollision && 
                !Main.LocalPlayer.ZoneSandstorm && !Main.LocalPlayer.ZoneSnow && 
                !Main.LocalPlayer.ZoneNormalSpace && Main.LocalPlayer.position.Y < 5430f) 
                {

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

                if(Main.LocalPlayer.ZoneBeach || Main.LocalPlayer.ZoneJungle)
                Extra = 1.4f;
                else
                Extra = 1f;

                Intensity = (550 * Main.maxRaining / (20.0f * 645.0f));

                Filters.Scene["RainFilter"].GetShader().UseOpacity(Intensity*RainTransition*Extra).UseIntensity(RainTransition);

            }
        }
    }
}