using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RainOverhaul.Content {
    public class ThisTileType {
        public bool Exists(Tile tTile) {
            return 
            tTile.TileType==TileID.Trees||
            tTile.TileType==TileID.Plants||tTile.TileType==TileID.Plants2||
            tTile.TileType==TileID.JunglePlants||tTile.TileType==TileID.JunglePlants2||
            tTile.TileType==TileID.HallowedPlants||tTile.TileType==TileID.HallowedPlants2;
        }
    }
}