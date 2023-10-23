using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace RainOverhaul {
	public class RainOverhaul : Mod {
		public override void Load() {
			Ref<Effect> screenRef = new Ref<Effect>(ModContent.Request<Effect>("RainOverhaul/Content/RainFilter", AssetRequestMode.ImmediateLoad).Value);
			Filters.Scene["RainFilter"] = new Filter(new ScreenShaderData(screenRef, "RainFilter"), EffectPriority.VeryHigh);
			Filters.Scene["RainFilter"].Load();
		}
	}
}