using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RainOverhaul.Content {
    public class RainTile {
        /// <summary>
		/// List of vanilla tiles, that can't protect entity from the rain. 
		/// </summary>
        public bool CantProtectVanilla(Tile tTile) {
            return 
            tTile.TileType==TileID.Trees||
            tTile.TileType==TileID.Plants||tTile.TileType==TileID.Plants2||
            tTile.TileType==TileID.JunglePlants||tTile.TileType==TileID.JunglePlants2||
            tTile.TileType==TileID.HallowedPlants||tTile.TileType==TileID.HallowedPlants2||
            tTile.TileType==TileID.Bamboo||tTile.TileType==TileID.Banners||tTile.TileType==TileID.Anvils||tTile.TileType==TileID.Beds||
            tTile.TileType==TileID.Benches||tTile.TileType==TileID.Bookcases||tTile.TileType==TileID.Books||
            tTile.TileType==TileID.Bookcases||tTile.TileType==TileID.Bottles||tTile.TileType==TileID.Bowls||
            tTile.TileType==TileID.Cactus||tTile.TileType==TileID.Campfire||tTile.TileType==TileID.Candelabras||
            tTile.TileType==TileID.Candles||tTile.TileType==TileID.Chairs||tTile.TileType==TileID.Chandeliers||
            tTile.TileType==TileID.ChineseLanterns||tTile.TileType==TileID.CookingPots||
            tTile.TileType==TileID.CorruptPlants||tTile.TileType==TileID.CorruptVines||tTile.TileType==TileID.CorruptThorns||
            tTile.TileType==TileID.CrimsonPlants||tTile.TileType==TileID.CrimsonThorns||tTile.TileType==TileID.CrimsonVines||
            tTile.TileType==TileID.DemonAltar||tTile.TileType==TileID.Dressers||tTile.TileType==TileID.DyePlants||
            tTile.TileType==TileID.Furnaces||tTile.TileType==TileID.HangingLanterns||tTile.TileType==TileID.Heart||
            tTile.TileType==TileID.ImmatureHerbs||tTile.TileType==TileID.Kegs||tTile.TileType==TileID.Lampposts||
            tTile.TileType==TileID.Lamps||tTile.TileType==TileID.Loom||tTile.TileType==TileID.Pianos||
            tTile.TileType==TileID.PiggyBank||tTile.TileType==TileID.Pots||tTile.TileType==TileID.Pumpkins||
            tTile.TileType==TileID.Rope||tTile.TileType==TileID.Saplings||tTile.TileType==TileID.Sawmill||
            tTile.TileType==TileID.Signs||tTile.TileType==TileID.Sinks||tTile.TileType==TileID.SkyMill||tTile.TileType==TileID.SkullLanterns||
            tTile.TileType==TileID.Spikes||tTile.TileType==TileID.Switches||tTile.TileType==TileID.Tables||tTile.TileType==TileID.Tables2||
            tTile.TileType==TileID.TargetDummy||tTile.TileType==TileID.TeaKettle||tTile.TileType==TileID.Toilets||tTile.TileType==TileID.Tombstones||
            tTile.TileType==TileID.Torches||tTile.TileType==TileID.VineFlowers||tTile.TileType==TileID.VineRope||tTile.TileType==TileID.Vines||
            tTile.TileType==TileID.WarTable||tTile.TileType==TileID.WarTableBanner||tTile.TileType==TileID.WaterCandle||
            tTile.TileType==TileID.WebRope||tTile.TileType==TileID.WorkBenches||tTile.TileType==TileID.Sunflower||
            tTile.TileType==TileID.Platforms;
        }
    }
}