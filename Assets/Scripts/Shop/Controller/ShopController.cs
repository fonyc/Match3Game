using Shop.Model;
using UnityEngine;

namespace Shop.Controller
{
    public class ShopController 
    {
        public ShopModel Model { get; private set; }

        public UserData UserData { get; private set; }

        public ShopController(UserData userData)
        {
            UserData = userData;
        }

        public void Initialize()
        {
            Load();
        }

        public void PurchaseItem(ShopItemModel model)
        {
            if (UserData.GetResourceAmount(model.Cost.Type) < model.Cost.Amount) return;

            UserData.RemoveResource(model.Cost);
            UserData.AddResource(model.Reward);
            UserData.Save();
        }

        public void FreeResource()
        {
            UserData.AddGold();
            UserData.Save();
        }

        private void Load()
        {
            Model = JsonUtility.FromJson<ShopModel>(Resources.Load<TextAsset>("ShopModel").text);
        }
    }

}
