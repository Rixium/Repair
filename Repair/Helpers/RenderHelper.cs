using System.Text;
using Repair.Games;

namespace Repair.Helpers
{
    public class RenderHelper { 
        
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        public static string CreateNeighborString(Tile tile)
        {
            StringBuilder.Clear();

            var northTile = tile.North;
            var northEastTile = tile.NorthEast;
            var eastTile = tile.East;
            var southEast = tile.SouthEast;
            var southTile = tile.South;
            var southWest = tile.SouthWest;
            var westTile = tile.West;
            var northWest = tile.NorthWest;

            if (northTile != null && !northTile.IsDry)
                StringBuilder.Append("n_");
            if (eastTile != null && !eastTile.IsDry)
                StringBuilder.Append("e_");
            if (southTile != null && !southTile.IsDry)
                StringBuilder.Append("s_");
            if (westTile != null && !westTile.IsDry)
                StringBuilder.Append("w_");

            return StringBuilder.ToString();
        }
    }
}