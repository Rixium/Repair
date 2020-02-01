using Microsoft.Xna.Framework;

namespace Repair.Games
{
    public interface IEntity
    {
        
        Tile Tile { get; set; }
        Tile TargetTile { get; set; }
        float MovementPercentage { get; set; }
    }
}