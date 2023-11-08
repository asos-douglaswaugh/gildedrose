namespace GildedRose
{
    public class SaleableItemFactory
    {
        public IDegradable Create(Item item)
        {
            if (item.Name == "Sulfuras, Hand of Ragnaros")
                return new SaleableItem(item, new Sulfuras());

            if (item.Name == "Aged Brie")
                return item;

            if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                return new SaleableItem(item, new BackstagePass());

            if (item.Name == "Conjured Mana Cake")
                return new SaleableItem(item, new Conjured());

            return item;
        }
    }

    public class GildedRose
    {
        IList<Item> Items;
        private List<IDegradable> _saleableItems;
        private readonly SaleableItemFactory _saleableItemFactory;

        public GildedRose(IList<Item> Items)
        {
            this.Items = Items;
            _saleableItemFactory = new SaleableItemFactory();

            _saleableItems = new List<IDegradable>();

            foreach (var item in Items)
            {
                IDegradable saleableItem;

                saleableItem = _saleableItemFactory.Create(item);

                _saleableItems.Add(saleableItem);
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

    public class Conjured : IDegrade
    {
        public void UpdateQuality(IDegradable item)
        {
            item.SellIn = item.SellIn - 1;

            item.Quality = item.Quality - 2;

            if (item.SellIn < 0)
                item.Quality = item.Quality - 2;

            if (item.Quality < 0)
                item.Quality = 0;
        }
    }

    public class StandardItem : IDegrade
    {
        public void UpdateQuality(IDegradable item)
        {
            item.SellIn = item.SellIn - 1;

            if (item.Quality > 0)
                item.Quality = item.Quality - 1;

            if (item is { SellIn: < 0, Quality: > 0 })
                item.Quality = item.Quality - 1;
        }
    }

    public class BackstagePass : IDegrade
    {
        public void UpdateQuality(IDegradable item)
        {
            item.SellIn = item.SellIn - 1;

            if (item is { Quality: < 50})
                item.Quality = item.Quality + 1;

            if (item is { SellIn: < 10, Quality: < 50 })
                item.Quality = item.Quality + 1;

            if (item is { SellIn: < 5, Quality: < 50 })
                item.Quality = item.Quality + 1;

            if (item.SellIn < 0)
                item.Quality = 0;
        }
    }

    public class AgedBrie : IDegrade
    {
        public void UpdateQuality(IDegradable item)
        {
            item.SellIn = item.SellIn - 1;

            if (item.Quality < 50)
                item.Quality = item.Quality + 1;

            if (item is { SellIn: < 0, Quality: < 50 })
                item.Quality = item.Quality + 1;
}
    }

    public class Sulfuras : IDegrade
    {
        public void UpdateQuality(IDegradable item)
        {
            return;
        }
    }

    public interface IDegrade
    {
        void UpdateQuality(IDegradable item);
    }

    public interface IDegradable
    {
        void UpdateQuality();
        string Name { get; }
        int Quality { get; set; }
        int SellIn { get; set; }
    }

    public class SaleableItem : IDegradable
    {
        private readonly Item _item;
        private readonly IDegrade _sulfuras;

        public string Name => _item.Name;
        public int Quality
        {
            get => _item.Quality;
            set => _item.Quality = value;
        }
        public int SellIn
        {
            get => _item.SellIn;
            set => _item.SellIn = value;
        }

        public SaleableItem(Item item, IDegrade sulfuras)
        {
            _item = item;
            _sulfuras = sulfuras;
        }

        public void UpdateQuality()
        {
            _sulfuras.UpdateQuality(this);
        }
    }
}
