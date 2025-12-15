namespace Model
{
    public static class ModelFactory
    {
        private static readonly System.Random rng = new();
        // Використання одного екземпляра Random - правильно.
        // Створення нового Random кожного разу може давати однакові значення.
        
        public static Data.Item CreateItem(int x)
        {
            //TODO random for enum
            // TODO коментар але рандомізація вже реалізована нижче. Видалити.
            return new Data.Item(x, 0, GetRandomType(), GetRandomColor());
            // MAGIC NUMBER 0 - це spawn row. Краще константа або параметр.
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
        
        // Альтернатива з кешуванням:
        // private static readonly Data.ItemTypeColor[] AllColors = (Data.ItemTypeColor[])System.Enum.GetValues(typeof(Data.ItemTypeColor));
        // private static readonly Data.ItemType[] AllTypes = (Data.ItemType[])System.Enum.GetValues(typeof(Data.ItemType));
        // private static Data.ItemTypeColor GetRandomColor() => AllColors[rng.Next(AllColors.Length)];
    }
}