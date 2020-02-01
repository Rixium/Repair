using Repair.UI;

namespace Repair.Games
{
    public class WorldObject
    {

        public Origin[] Origins;
        public string[] FileName { get; set; }
        public bool CanPickup { get; set; }
        public bool Collidable { get; set; }
        public int DrynessRadius { get; set; }
        public float DrynessEffect { get; set; }
        public string[] ObjectName { get; set; }
        public int Stage { get; set; }
        public int TotalStages { get; set; }
        public int StageModifier { get; set; }
        public bool ProgressOnSleep { get; set; }
        public Tile Tile { get; set; }
        public bool CanUse { get; set; }

        public bool CreateInstance(Tile tile)
        {
            if (!tile.IsDry) return false;
            if (tile.WorldObject != null) return false;

            var worldObject = new WorldObject()
            {
                Origins = Origins,
                Tile = tile,
                ObjectName =  ObjectName,
                FileName = FileName,
                Collidable = Collidable,
                DrynessRadius = DrynessRadius,
                DrynessEffect = DrynessEffect,
                Stage = Stage,
                TotalStages = TotalStages,
                StageModifier =  StageModifier,
                ProgressOnSleep = ProgressOnSleep,
                CanPickup = CanPickup,
                CanUse =  CanUse
            };

            tile.WorldObject = worldObject;
            
            return true;
        }

        public void Progress()
        {
            if (Stage < TotalStages)
            {
                Stage++;
                DrynessEffect += StageModifier;
            }
        }
    }
}