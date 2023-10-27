using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Linq;
using Terraria.ID;

namespace RainOverhaul.Content {    
    public class UnderRainNPC:GlobalNPC {

        // Damage control of NPCs under the rain
        public bool LifeLost;
        public override bool InstancePerEntity => true;
        public override void ResetEffects(NPC npc) {
            LifeLost = false;
        }
        public override void AI(NPC npc) {
            if(Main.LocalPlayer.HasBuff<ShelterNotification>() && Main.LocalPlayer.active && !Main.LocalPlayer.dead) {
                npc.AddBuff(ModContent.BuffType<ShelterNotification>(), 2);
                Projectile.NewProjectile(Entity.GetSource_None(), npc.Center, Vector2.Zero, ModContent.ProjectileType<RainCircle>(), 1, 1, npc.whoAmI);
            }
        }
        public override void UpdateLifeRegen(NPC npc, ref int damage) {
            if(LifeLost) {
                npc.lifeRegen -= 200;
            }
        }
    }
}