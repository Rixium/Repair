using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Repair.UI;

namespace Repair.Games
{
    public class WorldObject
    {

        public int Power = 1;
        public Origin[] Origins;
        public string[] FileName { get; set; }
        public bool CanPickup { get; set; }
        public bool Collidable { get; set; }
        public int[] DrynessRadius { get; set; }
        public bool Repairable { get; set; }
        public Animation AnimationOnRepair { get; set; }
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
        public string[] RepairRequirements { get; set; }
        
        public string DropsOnFinalStage { get; set; }
        public float DropRarity { get; set; }
        public bool HasDropped { get; set; }
        public bool Repaired { get; set; }
        public List<string> RepairedItems { get; set; } = new List<string>();
        public bool EndsLevelOnRepair { get; set; }

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
                ObjectType = ObjectType,
                Repairable = Repairable,
                AnimationOnRepair = AnimationOnRepair,
                Repaired = Repaired,
                RepairRequirements = RepairRequirements,
                EndsLevelOnRepair = EndsLevelOnRepair
            };

            tile.WorldObject = worldObject;
            
            return true;
        }

        public void Repair()
        {
            if (!Repairable) return;
            Repaired = true;
        }

        public bool Progress()
        {
            if (Stage >= TotalStages) return false;
            
            Stage++;
            return true;

        }

        public float GetDrynessEffect() => HasProgressEffect ? DrynessEffect[Stage - 1] : 0;

        public int CurrentDrynessHit = 0;
        
        public int GetDrynessRadius()
        {
            if (!HasProgressEffect) return 0;
            var totalRadius = DrynessRadius[Stage - 1];
            var extra = 0;

            if (Tile.East?.WorldObject != null && Tile.East.WorldObject.ObjectType == ObjectType)
                extra += 2;
            if (Tile.West?.WorldObject != null && Tile.West.WorldObject.ObjectType == ObjectType)
                extra += 2;
            if (Tile.North?.WorldObject != null && Tile.North.WorldObject.ObjectType == ObjectType)
                extra += 2;
            if (Tile.South?.WorldObject != null && Tile.South.WorldObject.ObjectType == ObjectType)
                extra += 2;
            if (Tile.NorthEast?.WorldObject != null && Tile.NorthEast.WorldObject.ObjectType == ObjectType)
                extra += 2;
            if (Tile.SouthWest?.WorldObject != null && Tile.SouthWest.WorldObject.ObjectType == ObjectType)
                extra += 2;
            if (Tile.SouthEast?.WorldObject != null && Tile.SouthEast.WorldObject.ObjectType == ObjectType)
                extra += 2;
            if (Tile.NorthWest?.WorldObject != null && Tile.NorthWest.WorldObject.ObjectType == ObjectType)
                extra += 2;

            if(extra > 2)
            {
                extra = (int) Math.Ceiling(extra / 2.0f);
            }

            totalRadius += extra;
            
            if (CurrentDrynessHit < totalRadius)
            {
                CurrentDrynessHit++;
            }

            return CurrentDrynessHit;
        }

        public bool Repair(string itemName)
        {
            if (!RepairRequirements.Contains(itemName)) return false;
            
            RepairedItems.Add(itemName);

            if (RepairRequirements.Length == RepairedItems.Count)
            {
                Repaired = true;
                Stage++;
            }

            return true;

        }
        
        public void AddPower()
        {
            if (Power >= 4) return;
            Power += 1;
        }

        public void Update(float delta)
        {
            if (!Repaired) return;
            AnimationOnRepair?.Update(delta);
        }
    }
}