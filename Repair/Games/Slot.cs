namespace Repair.Games
{
    public class Slot
    {

        public Item Item;
        public int Count;

        public bool Add(Item item, int count)
        {

            if (Item != null)
            {
                if (!Item.ItemName.Equals(item.ItemName)) 
                    return false;
                
                Count += count;
                return true;

            }
            
            Count = count;
            Item = item;
            return true;
        }

        public void Remove(int count)
        {
            Count -= count;

            if (Count <= 0)
            {
                Item = null;
            }
        }
    }
}