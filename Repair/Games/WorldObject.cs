using Repair.UI;

namespace Repair.Games
{
    public class WorldObject
    {

        public Origin[] Origins;
        public string[] FileName { get; set; }
        public bool CanPickup { get; set; }
        public bool Collidable { get; set; }
        public int[] DrynessRadius { get; set; }
        public float[] DrynessEffect { get; set; }
        public string[] ObjectName { get; set; }
        public int Stage { get; set; }
        public int TotalStages { get; set; }
        public int StageModifier { get; set; }
        public bool ProgressOnSleep { get; set; }
        public Tile Tile { get; set; }
        public bool CanUse { get; set; }
        public bool HasProgressEffect { get; set; }
        public string PlaceSound { get; set; }
        public string UseSound { get; set; }
        public int ObjectType { get; set; }
        
        public string DropsOnFinalStage { get; set; }
        public float DropRarity { get; set; }
        public bool HasDropped { get; set; }

        public bool CreateInstance(Tile tile)
        {
            if (!tile.IsDry) return false;
            if (tile.DroppedItem != null) return false;
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
                CanUse =  CanUse,
                HasProgressEffect = HasProgressEffect,
                PlaceSound = PlaceSound,
                UseSound = UseSound,
                DropsOnFinalStage = DropsOnFinalStage,
                DropRarity = DropRarity,
                ObjectType = ObjectType
            };

            tile.WorldObject = worldObject;
            
            return true;
        }

        public bool Progress()
        {
            if (Stage >= TotalStages) return false;
            
            Stage++;
            return true;

        }

        public float GetDrynessEffect() => HasProgressEffect ? DrynessEffect[Stage - 1] : 0;

        public int GetDrynessRadius()
        {
            if (!HasProgressEffect) return 0;
            var totalRadius = DrynessRadius[Stage - 1];

            if (Tile.East?.WorldObject != null && Tile.East.WorldObject.ObjectType == ObjectType) totalRadius += 2;
            if (Tile.North?.WorldObject != null && Tile.North.WorldObject.ObjectType == ObjectType) totalRadius += 2;
            if (Tile.South?.WorldObject != null && Tile.South.WorldObject.ObjectType == ObjectType) totalRadius += 2;
            if (Tile.West?.WorldObject != null && Tile.West.WorldObject.ObjectType == ObjectType) totalRadius += 2;

            return totalRadius;
        } 
    }
}