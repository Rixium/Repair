namespace Repair.Games
{
    public class Slot
    {

        public Item Item;
        public int Count;

        public bool Add(Item item, int count)
        {
            if (Item == item)
            {
                Count += count;
                return true;
            }

            if (Item != null) return false;
            
            Count = count;
            Item = item;
            return true;
        }
    }
}