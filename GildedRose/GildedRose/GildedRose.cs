namespace GildedRose
{
    public class GildedRose
    {
        IList<Item> Items;

        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
        }

        public void UpdateQuality()
        {
            foreach (var item in Items)
            {
                item.UpdateQuality();
            }
        }
    }

    public class Conjured : IDegrade
    {
        public void UpdateQuality(Item item)
        {
            item.SellIn -= 1;

            item.Quality -= 2;

            if (item.SellIn < 0)
                item.Quality -= 2;

            if (item.Quality < 0)
                item.Quality = 0;
        }
    }

    public class StandardItem : IDegrade
    {
        public void UpdateQuality(Item item)
        {
            item.SellIn -= 1;

            if (item.Quality > 0)
                item.Quality -= 1;

            if (item is { SellIn: < 0, Quality: > 0 })
                item.Quality -= 1;
        }
    }

    public class BackstagePass : IDegrade
    {
        public void UpdateQuality(Item item)
        {
            item.SellIn -= 1;

            if (item is { Quality: < 50})
                item.Quality += 1;

            if (item is { SellIn: < 10, Quality: < 50 })
                item.Quality += 1;

            if (item is { SellIn: < 5, Quality: < 50 })
                item.Quality += 1;

            if (item.SellIn < 0)
                item.Quality = 0;
        }
    }

    public class AgedBrie : IDegrade
    {
        public void UpdateQuality(Item item)
        {
            item.SellIn -= 1;

            if (item.Quality < 50)
                item.Quality += 1;

            if (item is { SellIn: < 0, Quality: < 50 })
                item.Quality += 1;
}
    }

    public class Sulfuras : IDegrade
    {
        public void UpdateQuality(Item item)
        {
            return;
        }
    }

    public interface IDegrade
    {
        void UpdateQuality(Item item);
    }
}
