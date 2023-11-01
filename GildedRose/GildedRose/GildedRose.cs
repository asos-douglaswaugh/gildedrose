namespace GildedRose
{
    public class GildedRose
    {
        IList<Item> Items;
        private List<SaleableItem> _saleableItems;

        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;

            _saleableItems = new List<SaleableItem>();

            foreach (var item in Items)
            {
                if (item.Name == "Sulfuras, Hand of Ragnaros")
                    _saleableItems.Add(new SaleableItem(item, new Sulfuras()));
                else if (item.Name == "Aged Brie")
                    _saleableItems.Add(new SaleableItem(item, new AgedBrie()));
                else if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                    _saleableItems.Add(new SaleableItem(item, new BackstagePass()));
                else
                    _saleableItems.Add(new SaleableItem(item, new StandardItem()));
            }
        }

        public void UpdateQuality()
        {
            foreach (var item in _saleableItems)
            {
                item.UpdateQuality();
            }
        }
    }

    public class StandardItem : IDegradable
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality > 0)
                item.Quality = item.Quality - 1;

            if (item is { SellIn: < 1, Quality: > 0 })
                item.Quality = item.Quality - 1;

            item.SellIn = item.SellIn - 1;
        }
    }

    public class BackstagePass : IDegradable
    {
        public void UpdateQuality(Item item)
        {
            if (item is { Quality: < 50})
                item.Quality = item.Quality + 1;

            if (item is { SellIn: < 11, Quality: < 50 })
                item.Quality = item.Quality + 1;

            if (item is { SellIn: < 6, Quality: < 50 })
                item.Quality = item.Quality + 1;

            if (item.SellIn < 1)
                item.Quality = 0;

            item.SellIn = item.SellIn - 1;
        }
    }

    public class AgedBrie : IDegradable
    {
        public void UpdateQuality(Item item)
        {
            if (item.Quality < 50)
                item.Quality = item.Quality + 1;

            if (item is { SellIn: < 1, Quality: < 50 })
                item.Quality = item.Quality + 1;

            item.SellIn = item.SellIn - 1;
        }
    }

    public class Sulfuras : IDegradable
    {
        public void UpdateQuality(Item item)
        {
            return;
        }
    }

    public interface IDegradable
    {
        void UpdateQuality(Item item);
    }

    public class SaleableItem
    {
        private readonly Item _item;
        private readonly IDegradable _sulfuras;

        public SaleableItem(Item item, IDegradable sulfuras)
        {
            _item = item;
            _sulfuras = sulfuras;
        }

        public void UpdateQuality()
        {
            _sulfuras.UpdateQuality(_item);
        }
    }
}
