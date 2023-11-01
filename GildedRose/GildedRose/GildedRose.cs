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
                if (item.Name == "Sulfuras, Hand of Ragnaros")
                    continue;

                if (item.Name == "Aged Brie")
                {
                    if (item.Quality < 50)
                        item.Quality = item.Quality + 1;
                    item.SellIn = item.SellIn - 1;
                    if (item is { SellIn: < 0, Quality: < 50 })
                        item.Quality = item.Quality + 1;
                    continue;
                }

                if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                {
                    if (item.Quality < 50)
                    {
                        item.Quality = item.Quality + 1;

                        if (item.Name == "Backstage passes to a TAFKAL80ETC concert")
                        {
                            if (item is { SellIn: < 11, Quality: < 50 })
                            {
                                item.Quality = item.Quality + 1;
                            }

                            if (item is { SellIn: < 6, Quality: < 50 })
                            {
                                item.Quality = item.Quality + 1;
                            }
                        }
                    }

                    item.SellIn = item.SellIn - 1;

                    if (item.SellIn < 0)
                    {
                        item.Quality = item.Quality - item.Quality;
                    }

                    continue;
                }

                if (item.Quality > 0)
                {
                    item.Quality = item.Quality - 1;
                }

                item.SellIn = item.SellIn - 1;

                if (item is { SellIn: < 0, Quality: > 0 })
                {
                    item.Quality = item.Quality - 1;
                }
            }
        }
    }
}
