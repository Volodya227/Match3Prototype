namespace Model
{
    public static class ModelFactory
    {
        public static Data.Item CreateItem(int x)
        {
            //TODO random for enum
            return new Data.Item(x, 0, Data.ItemType.Common1, Data.ItemTypeColor.Blue);
        }
    }
}