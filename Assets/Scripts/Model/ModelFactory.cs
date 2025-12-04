namespace Model
{
    public static class ModelFactory
    {
        private static readonly System.Random rng = new();
        public static Data.Item CreateItem(int x)
        {
            //TODO random for enum
            return new Data.Item(x, 0, GetRandomType(), GetRandomColor());
        }
        private static Data.ItemTypeColor GetRandomColor()
        {
            int count = System.Enum.GetValues(typeof(Data.ItemTypeColor)).Length;
            return (Data.ItemTypeColor)rng.Next(0, count);
        }
        private static Data.ItemType GetRandomType()
        {
            int count = System.Enum.GetValues(typeof(Data.ItemType)).Length;
            return (Data.ItemType)rng.Next(0, count);
        }
    }
}