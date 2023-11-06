using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RainOverhaul.Content {
    public class RainCircle:ModProjectile {
        public override string Texture => "RainOverhaul/Content/Texture/WIP";
        public override void SetDefaults() {
            Projectile.width = 17;
            Projectile.height = 18;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.Opacity = 1f;
        }
        public override void AI() {
            Projectile.timeLeft = 2;
            Player player = Main.player[Projectile.owner];
            for(int i=0; i<Main.maxNPCs; i++) {
                Projectile.velocity = Main.npc[i].velocity;
                Projectile.position = Main.npc[i].Center - new Vector2(Projectile.width/2f, Projectile.height/2f);
                if(!Main.npc[i].active || player.HasBuff<ShelterNotification>()) Projectile.timeLeft = 0;
            }
        }
    }
}