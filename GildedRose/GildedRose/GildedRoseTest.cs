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
            IList<Item> Items = new List<Item> { new Item { Name = name, SellIn = startSellIn, Quality = startQuality } };
            GildedRose app = new GildedRose(Items);
            app.UpdateQuality();
            Assert.That(Items[0].Name, Is.EqualTo(name));
            Assert.That(Items[0].Quality, Is.EqualTo(endQuality));
        }

        [TestCase("Standard item", -1, 2, 0)]
        [TestCase("Standard item", 0, 2, 0)]
        public void Once_the_sell_by_date_has_passed_quality_degrades_twice_as_fast(string name, int startSellIn, int startQuality, int endQuality)
        {
            IList<Item> Items = new List<Item> { new Item { Name = name, SellIn = startSellIn, Quality = startQuality } };
            GildedRose app = new GildedRose(Items);
            app.UpdateQuality();
            Assert.That(Items[0].Name, Is.EqualTo(name));
            Assert.That(Items[0].Quality, Is.EqualTo(endQuality));
        }
    }
}
