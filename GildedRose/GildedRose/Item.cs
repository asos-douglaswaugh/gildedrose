namespace GildedRose
{
    public class Item : IDegradable
    {
        public string Name { get; set; }
        public int SellIn { get; set; }
        public int Quality { get; set; }
        public StandardItem Type { get; set; }

        public override string ToString()
        {
            return this.Name + ", " + this.SellIn + ", " + this.Quality;
        }

        public void UpdateQuality()
        {
            Type.UpdateQuality(this);
        }
    }
}
