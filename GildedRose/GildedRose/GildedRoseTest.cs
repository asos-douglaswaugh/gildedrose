using NUnit.Framework;
using NUnit.Framework.Interfaces;
using System.Diagnostics.Metrics;

namespace GildedRose
{
    [TestFixture]
    public class GildedRoseTest
    {
        private static IList<Item> _items;

        private static GildedRose _app;
        // Once the sell by date has passed, quality degrades twice as fast
        // The quality of an item is never negative
        // Aged brie actually increases in quality the older it gets
        // The quality of an item is never more than 50
        // Sulfuras being a legendary item, never has to be sold or decreases in quality; it is always 80
        // Backstage passes, like aged brie, increases in quality as its sell in value approaches
        // Quality increases by 2 when there are 10 days or less and by 3 when there are 5 days or less but drops to 0 after the concert
        // Conjured items degrade in quality twice as fast as normal items
        // Sell in should decrease by 1 each day for all items apart from Sulfuras which never decrease
        // Conjured items degrade in Quality twice as fast as normal items
        
        public static IEnumerable<TestCaseData> StandItemShouldDegradeByOneEachDayTestCases
        {
            get
            {
                yield return new TestCaseData("Standard item", 1, 1, 0, new StandardItem());
            }
        }

        [Test, TestCaseSource(nameof(StandItemShouldDegradeByOneEachDayTestCases))]
        public void A_standard_item_should_degrade_by_one_each_day(string name, int startSellIn, int startQuality, int endQuality, IDegrade type)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality, type);
        }

        public static IEnumerable<TestCaseData> OnceTheSellByDateHasPassQualityDegradesTwiceAsFastTestCases
        {
            get
            {
                yield return new TestCaseData("Standard item", -1, 2, 0, new StandardItem());
                yield return new TestCaseData("Standard item", 0, 2, 0, new StandardItem());
            }
        }

        [Test, TestCaseSource(nameof(OnceTheSellByDateHasPassQualityDegradesTwiceAsFastTestCases))]
        public void Once_the_sell_by_date_has_passed_quality_degrades_twice_as_fast(string name, int startSellIn, int startQuality, int endQuality, IDegrade type)
        {
            _items = new List<Item> { new Item { Name = name, SellIn = startSellIn, Quality = startQuality, Type = type } };
            _app = new GildedRose(_items);
            WhenTheQualitiesAreUpdatedAtTheEndOfTheDay();
            ThenTheNameShouldMatch(name);
            ThenTheQualityShouldMatch(endQuality);
        }

        [TestCase("Aged Brie", 0, 1, 3)]
        [TestCase("Aged Brie", -1, 1, 3)]
        public void Once_the_sell_by_date_has_passed_quality_degrades_twice_as_fast(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        public static IEnumerable<TestCaseData> TheQualityOfAnItemIsNeverNegativeTestSource
        {
            get
            {
                yield return new("Standard item", 1, 0, 0, new StandardItem());
            }
        }

        [Test, TestCaseSource(nameof(TheQualityOfAnItemIsNeverNegativeTestSource))]
        public void The_quality_of_an_item_is_never_negative(string name, int startSellIn, int startQuality, int endQuality, IDegrade type)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality, type);
        }

        [TestCase("Aged Brie", 1, 1, 2)]
        [TestCase("Aged Brie", 0, 1, 3, Description = "twice as fast when sellin is 0")] // potential bug
        [TestCase("Aged Brie", -1, 1, 3, Description = "twice as fast when sellin is 1")] // potential bug
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 11, 1, 2)]
        public void Aged_brie_and_backstage_passes_actually_increases_in_quality_the_older_it_gets(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Aged Brie", 1, 50, 50)]
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 1, 48, 50)]
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 10, 49, 50)]
        public void The_quality_of_an_item_is_never_more_than_50(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        public static IEnumerable<TestCaseData> TheQualityOfAnItemIsNeverMoreThan50TestSource
        {
            get
            {
                yield return new("Standard item", 1, 75, 74, new StandardItem()); // bug
            }
        }

        [Test, TestCaseSource(nameof(TheQualityOfAnItemIsNeverMoreThan50TestSource))]
        public void The_quality_of_an_item_is_never_more_than_50(string name, int startSellIn, int startQuality, int endQuality, IDegrade type)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality, type);
        }

        [TestCase("Sulfuras, Hand of Ragnaros", 1, 80, 80)]
        [TestCase("Sulfuras, Hand of Ragnaros", 0, 80, 80)]
        [TestCase("Sulfuras, Hand of Ragnaros", -1, 80, 80)]
        public void Sulfuras_being_a_legendary_item_never_has_to_be_sold_or_decreases_in_value_it_is_always_80(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 10, 1, 3)]
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 6, 1, 3)]
        public void Quality_of_backstage_passes_increases_by_2_when_there_are_10_days_or_less_to_sell_in_date(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 5, 1, 4)]
        [TestCase("Backstage passes to a TAFKAL80ETC concert", 1, 1, 4)]
        public void Quality_of_backstage_passes_increases_by_3_when_there_are_5_days_or_less_to_sell_in_date(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 0, 10, 0)] // potential bug, doesn't a sellIn of 0 mean the concert is today and the ticket still valid?
        [TestCase("Backstage passes to a TAFKAL80ETC concert", -1, 10, 0)] // shouldn't get in to this state, unless it's initialised as such
        public void Quality_of_backstage_passes_drops_to_0_after_the_concert(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        [TestCase("Backstage passes to a TAFKAL80ETC concert", 1, 0)]
        [TestCase("Aged Brie", 1, 0)]
        [TestCase("Sulfuras, Hand of Ragnaros", 1, 1)]
        [TestCase("Sulfuras, Hand of Ragnaros", 0, 0)]
        [TestCase("Sulfuras, Hand of Ragnaros", -1, -1)]
        public void Sell_in_should_decrease_by_1_each_day_for_all_items_apart_from_Sulfuras_which_never_decrease(string name, int startSellIn, int endSellIn)
        {
            GivenTheGildedRoseHasOneItemInStock(name, startSellIn, 10);
            WhenTheQualitiesAreUpdatedAtTheEndOfTheDay();
            ThenTheNameShouldMatch(name);
            ThenTheSellInShouldMatch(endSellIn);
        }

        public static IEnumerable<TestCaseData> SellInShouldDecreaseBy1EachDayForAllItemsApartFromSulfurasWhichNeverDecrease
        {
            get
            {
                yield return new("Standard item", 1, 0, new StandardItem());
            }
        }

        [Test, TestCaseSource(nameof(SellInShouldDecreaseBy1EachDayForAllItemsApartFromSulfurasWhichNeverDecrease))]
        public void Sell_in_should_decrease_by_1_each_day_for_all_items_apart_from_Sulfuras_which_never_decrease(string name, int startSellIn, int endSellIn, IDegrade type)
        {
            GivenTheGildedRoseHasOneItemInStock(name, startSellIn, 10, type);
            WhenTheQualitiesAreUpdatedAtTheEndOfTheDay();
            ThenTheNameShouldMatch(name);
            ThenTheSellInShouldMatch(endSellIn);
        }

        [TestCase("Conjured Mana Cake", 1, 2, 0)]
        [TestCase("Conjured Mana Cake", 1, 1, 0)]
        [TestCase("Conjured Mana Cake", 0, 4, 0)]
        [TestCase("Conjured Mana Cake", 0, 3, 0)]
        public void Conjured_items_degrade_in_quality_twice_as_fast_as_normal_items(string name, int startSellIn, int startQuality, int endQuality)
        {
            CreateUpdateAndAssertQuality(name, startSellIn, startQuality, endQuality);
        }

        private void CreateUpdateAndAssertQuality(string name, int startSellIn, int startQuality, int endQuality)
        {
            GivenTheGildedRoseHasOneItemInStock(name, startSellIn, startQuality);
            WhenTheQualitiesAreUpdatedAtTheEndOfTheDay();
            ThenTheNameShouldMatch(name);
            ThenTheQualityShouldMatch(endQuality);
        }

        private void CreateUpdateAndAssertQuality(string name, int startSellIn, int startQuality, int endQuality, IDegrade type)
        {
            GivenTheGildedRoseHasOneItemInStock(name, startSellIn, startQuality, type);
            WhenTheQualitiesAreUpdatedAtTheEndOfTheDay();
            ThenTheNameShouldMatch(name);
            ThenTheQualityShouldMatch(endQuality);
        }

        private void GivenTheGildedRoseHasOneItemInStock(string name, int startSellIn, int startQuality)
        {
            _items = new List<Item> { new Item { Name = name, SellIn = startSellIn, Quality = startQuality } };
            _app = new GildedRose(_items);
        }

        private static void GivenTheGildedRoseHasOneItemInStock(string name, int startSellIn, int startQuality,
            IDegrade type)
        {
            _items = new List<Item> { new Item { Name = name, SellIn = startSellIn, Quality = startQuality, Type = type } };
            _app = new GildedRose(_items);
        }

        private void WhenTheQualitiesAreUpdatedAtTheEndOfTheDay()
        {
            _app.UpdateQuality();
        }

        private void ThenTheQualityShouldMatch(int quality)
        {
            Assert.That(_items[0].Quality, Is.EqualTo(quality));
        }

        private void ThenTheNameShouldMatch(string name)
        {
            Assert.That(_items[0].Name, Is.EqualTo(name));
        }

        private void ThenTheSellInShouldMatch(int sellIn)
        {
            Assert.That(_items[0].SellIn, Is.EqualTo(sellIn));
        }
    }
}
