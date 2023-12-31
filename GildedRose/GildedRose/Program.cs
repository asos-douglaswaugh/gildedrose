﻿namespace GildedRose
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("OMGHAI!");

            IList<Item> Items = new List<Item>{
                new()
                {
                    Name = "+5 Dexterity Vest",
                    SellIn = 10,
                    Quality = 20,
                    Type = new StandardItem()
                },
                new()
                {
                    Name = "Aged Brie",
                    SellIn = 2,
                    Quality = 0,
                    Type = new AgedBrie()
                },
                new()
                {
                    Name = "Elixir of the Mongoose",
                    SellIn = 5,
                    Quality = 7,
                    Type = new StandardItem()
                },
                new()
                {
                    Name = "Sulfuras, Hand of Ragnaros",
                    SellIn = 0,
                    Quality = 80,
                    Type = new Sulfuras()
                },
                new()
                {
                    Name = "Sulfuras, Hand of Ragnaros",
                    SellIn = -1,
                    Quality = 80,
                    Type = new Sulfuras()
                },
                new()
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 15,
                    Quality = 20,
                    Type = new BackstagePass()
                },
                new()
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 10,
                    Quality = 49,
                    Type = new BackstagePass()
                },
                new()
                {
                    Name = "Backstage passes to a TAFKAL80ETC concert",
                    SellIn = 5,
                    Quality = 49,
                    Type = new BackstagePass()
                },
                new()
                {
                    Name = "Conjured Mana Cake",
                    SellIn = 3,
                    Quality = 6,
                    Type = new Conjured()
                }
            };

            var app = new GildedRose(Items);


            for (var i = 0; i < 31; i++)
            {
                Console.WriteLine("-------- day " + i + " --------");
                Console.WriteLine("name, sellIn, quality");
                for (var j = 0; j < Items.Count; j++)
                {
                    System.Console.WriteLine(Items[j]);
                }
                Console.WriteLine("");
                app.UpdateQuality();
            }
        }
    }
}