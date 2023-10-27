using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace RainOverhaul.Content {
    public class RainCircle:ModProjectile {
        public override string Texture => "RainOverhaul/Content/Texture/RedSquare";
        public override void SetDefaults() {
            Projectile.width = 128;
            Projectile.height = 128;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 2;
        }
        public override void AI() {
            Projectile.netImportant = true;
            Projectile.netUpdate = true;

            NPC npc = Main.npc[Projectile.owner];
            
            Projectile.position = npc.Center - new Vector2(Projectile.width/2f, Projectile.height/2f);
        }
    }
}