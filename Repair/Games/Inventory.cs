namespace Repair.Games
{
    public class Inventory
    {

        public Slot[] Slots;

        public Inventory()
        {
            Slots = new[]
            {
                new Slot(),
                new Slot(),
                new Slot()
            };
        }

        public bool AddItem(Item item, int count = 1)
        {
            foreach(var slot in Slots)
            {
                var result = slot.Add(item, count);

                if (result) return true;
            }

            return false;
        }

        public Slot GetSlot(int slot)
        {
            return Slots[slot];
        }
    }
}