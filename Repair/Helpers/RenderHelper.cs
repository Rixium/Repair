using System.Text;
using Repair.Games;

namespace Repair.Helpers
{
    public class RenderHelper { 
        
        public static int CalculateNeighbourBit(Tile tile)
        {
            var northTile = tile.North;
            var eastTile = tile.East;
            var southTile = tile.South;
            var westTile = tile.West;

            var sum = 0;

            if (northTile != null && northTile.IsDry)
                sum += 1 * 1;
            if (eastTile != null && eastTile.IsDry)
                sum += 1 * 4;
            if (southTile != null && southTile.IsDry)
                sum += 1 * 8;
            if (westTile != null && westTile.IsDry)
                sum += 1 * 2;

            return sum;
        }
    }
}