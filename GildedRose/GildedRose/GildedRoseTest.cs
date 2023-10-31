using NUnit.Framework;

namespace GildedRose
{
    [TestFixture]
    public class GildedRoseTest
    {
        // Once the sell by date has passed, quality degrades twice as fast
        // The quality of an item is never negative
        // Aged brie actually increases in quality the older it gets
        // The quality of an item is never more than 50
        // Sulfuras being a legendary item, never has to be sold or decreases in quality
        // Backstage passes, like aged brie, increases in quality as its sell in value approaches
        // Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but drops to 0 after the concert
        // Conjured items degrade in quality twice as fast as normal items

        [TestCase("Standard item", 1, 1, 0)]
        public void A_standard_item_should_degrade_by_one_each_day(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssert(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Standard item", -1, 2, 0)]
        [TestCase("Standard item", 0, 2, 0)]
        public void Once_the_sell_by_date_has_passed_quality_degrades_twice_as_fast(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssert(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Standard item", 1, 0, 0)]
        public void The_quality_of_an_item_is_never_negative(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssert(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Aged Brie", 1, 1, 2)]
        [TestCase("Aged Brie", 0, 1, 3, Description = "twice as fast when sellin is 0")] // potential bug
        [TestCase("Aged Brie", -1, 1, 3, Description = "twice as fast when sellin is 1")] // potential bug
        public void Aged_brie_actually_increases_in_quality_the_older_it_gets(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssert(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Aged Brie", 1, 50, 50)]
        [TestCase("Standard item", 1, 75, 74)] // bug
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 1, 48, 50)]
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 10, 49, 50)]
        public void The_quality_of_an_item_is_never_more_than_50(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssert(name, startSellIn, startQuality, endQuality);
        }

        private static void CreateUpdateAndAssert(string name, int startSellIn, int startQuality, int endQuality)
        {
            IList<Item> Items = new List<Item> { new Item { Name = name, SellIn = startSellIn, Quality = startQuality } };
            GildedRose app = new GildedRose(Items);
            app.UpdateQuality();
            Assert.That(Items[0].Name, Is.EqualTo(name));
            Assert.That(Items[0].Quality, Is.EqualTo(endQuality));
        }
    }
}
