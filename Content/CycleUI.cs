using Terraria;
using Terraria.UI;
using Terraria.Audio;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System;

namespace RainOverhaul.Content {
    public class CycleUIState : UIState {
        private UIElement Container;
		private UIImage cClear;
        private UIImage cQuake;
        private UIImage cRain;
		private int LocalTimeForAnimation;
		private int OldCycleState = -1;
		public SoundStyle sCycleSwap = new SoundStyle("RainOverhaul/Content/Sounds/sCycleSwap");
        public override void OnInitialize() {
            cClear = new UIImage(ModContent.Request<Texture2D>("RainOverhaul/Content/Textures/Cycles/cClear"));
            cQuake = new UIImage(ModContent.Request<Texture2D>("RainOverhaul/Content/Textures/Cycles/cQuake"));
            cRain = new UIImage(ModContent.Request<Texture2D>("RainOverhaul/Content/Textures/Cycles/cRain"));

			cClear.Color = Color.White * 0f;
			cQuake.Color = Color.White * 0f;
			cRain.Color = Color.White * 0f;

            Container = new UIElement();

            Container.Append(cClear);
            Container.Append(cQuake);
            Container.Append(cRain);

            Append(Container);
        }
        public override void Update(GameTime gameTime) {
            float xPos = Main.screenWidth * ModContent.GetInstance<RainConfig>().cCycleIndicatorPos.X;
            float yPos = Main.screenHeight * ModContent.GetInstance<RainConfig>().cCycleIndicatorPos.Y;
            float IconWidth = 64f;
            float IconHeight = 64f;

            if(xPos > Main.screenWidth - IconWidth) xPos = Main.screenWidth - (int)IconWidth;
            if(yPos > Main.screenHeight - IconHeight) yPos = Main.screenHeight - (int)IconHeight;

            IconRect(cClear, xPos, yPos, IconWidth,IconHeight);
            IconRect(cQuake, xPos, yPos, IconWidth,IconHeight);
            IconRect(cRain, xPos, yPos, IconWidth,IconHeight);

			LocalTimeForAnimation++;
			if(OldCycleState != RainSystem.CycleState) {
				OldCycleState = RainSystem.CycleState;
				LocalTimeForAnimation = 1;

				SoundEngine.PlaySound(sCycleSwap);
			}

			float e = 2.71828f;
			float IconScale = 0.7f + LocalTimeForAnimation/960f;
			Color IconOpacity = Color.White*((float)Math.Pow(1f/(LocalTimeForAnimation/40f), e));

			switch (RainSystem.CycleState) { 
				case RainSystem.CycleClear: {
					cClear.Color = IconOpacity;
					cClear.ImageScale = IconScale;

					cQuake.Color = Color.White*0f;
					cRain.Color = Color.White*0f;
				} break;

				case RainSystem.CycleQuake: {
					cQuake.Color = IconOpacity;
					cQuake.ImageScale = IconScale;

					cClear.Color = Color.White*0f;
					cRain.Color = Color.White*0f;
				} break;

				case RainSystem.CycleRain: {
					cRain.Color = IconOpacity;
					cRain.ImageScale = IconScale;

					cClear.Color = Color.White*0f;
					cQuake.Color = Color.White*0f;
				} break;
			}
        }

        public override void Draw(SpriteBatch spriteBatch) {
			if (ModContent.GetInstance<RainConfigServer>().cRainWorld) {
				base.Draw(spriteBatch);
			}
		}
        private void IconRect(UIImage icon, float x, float y, float width, float height) {
            icon.Left.Set(x, 0f);
            icon.Top.Set(y, 0f);
            icon.Width.Set(width, 0f);
            icon.Height.Set(height, 0f);
        }
    }
    class UISystem : ModSystem
	{
		private UserInterface userInterface;

		internal CycleUIState UIstate;
		public void ShowMyUI() {
			userInterface?.SetState(UIstate);
		}
		public void HideMyUI() {
			userInterface?.SetState(null);
		}
		public override void Load() {
			if (!Main.dedServ) {
				UIstate = new();
				userInterface = new();
				userInterface.SetState(UIstate);
			}
		}
		public override void UpdateUI(GameTime gameTime) {
			userInterface?.Update(gameTime);
		}
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers) {
			int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
			if (resourceBarIndex != -1) {
				layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
					"RainOverhaul: Cycles",
					delegate {
						userInterface.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}
	}
}