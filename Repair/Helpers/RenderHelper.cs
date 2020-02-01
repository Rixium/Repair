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
            var eastTile = tile.East;
            var southTile = tile.South;
            var westTile = tile.West;

            if (westTile != null && !westTile.IsDry)
                StringBuilder.Append("l");
            if (northTile != null && !northTile.IsDry)
                StringBuilder.Append("t");
            if (eastTile != null && !eastTile.IsDry)
                StringBuilder.Append("r");
            if (southTile != null && !southTile.IsDry)
                StringBuilder.Append("b");

            return StringBuilder.ToString();
        }
    }
}