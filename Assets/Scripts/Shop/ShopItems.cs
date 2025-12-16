namespace Shop
{
    [System.Serializable]
    public class Character : ShopItemBase
    {
        
    }

    [System.Serializable]
    public class Rope : ShopItemBase
    {
        
    }

    [System.Serializable]
    public class ShopItems
    {
        public Character[] characters;
        public Rope[] ropes;
    }

    public class ShopItemBase
    {
        public int id;
        public string name;
        public int isBought;
    }
}