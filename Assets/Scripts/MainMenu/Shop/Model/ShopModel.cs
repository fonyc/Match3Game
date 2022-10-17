using System.Collections.Generic;

namespace Shop.Model
{
    [System.Serializable]
    public class ShopModel
    {
        public List<ShopItemModel> Items = new List<ShopItemModel>();
        public List<ShopItemModel> IAPs = new List<ShopItemModel>();
    }
}