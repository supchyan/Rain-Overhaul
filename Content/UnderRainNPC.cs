using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using System.Linq;

namespace RainOverhaul.Content {    
    public class UnderRainNPC:GlobalNPC {

        // Damage control of NPCs under the rain
        public static int OldLife;
        public override bool InstancePerEntity => true; 

        public override void OnSpawn(NPC npc, IEntitySource source) {
            OldLife = npc.life;
            // npc.AddBuff(ModContent.BuffType<ShelterNotification>(), int.MaxValue);
        }
        public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter) {
            binaryWriter.Write(OldLife);
		}
		public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader) {
            OldLife = binaryReader.ReadInt32();
		}

        public override void AI(NPC npc) {
            for(int i=0; i<Main.maxPlayers; i++) {
                if(Main.player[i].HasBuff<ShelterNotification>() && Main.player[i].active && !Main.player[i].dead) {
                    float rIntensity = 550*Main.maxRaining/(20.0f * 645.0f)*2.5f;
                    int fValue = (int)Math.Round(rIntensity*20);

                    OldLife -= 2*fValue;
                    npc.life = OldLife;
                    if(OldLife <= 1) {
                        npc.active = false;
                        npc.CheckActive();
                        npc.checkDead();
                    }
                }
            }
        }
    }
}