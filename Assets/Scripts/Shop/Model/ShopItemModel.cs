namespace Shop.Model
{
    [System.Serializable]
    public class ShopItemModel
    {
        public string Id;
        public string Title;
        public string Image;
        public ResourceItem Cost;
        public ResourceItem Reward;
    }
}