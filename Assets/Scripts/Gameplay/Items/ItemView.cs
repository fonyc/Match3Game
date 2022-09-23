using UnityEngine;

public class ItemView : MonoBehaviour
{
    ItemController _controller;
    [SerializeField] Transform _itemsParent;
    [SerializeField] ItemViewSlot _itemSlotPrefab;

    public void Initialize(ItemController _itemController, UserData userData)
    {
        _controller = _itemController;

        while (_itemsParent.childCount > 0)
        {
            Transform child = _itemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (BattleItemModel itemModel in _controller.Model.itemStats)
        {
            foreach (OwnedBattleItem ownedItem in _controller.Model.selectedItems)
            {
                if (itemModel.Id != ownedItem.Id) continue;
                ItemViewSlot item = Instantiate(_itemSlotPrefab, _itemsParent);
                item.SetData(itemModel, ownedItem.Amount, _controller.Model.MaxItemQty, OnItemUsed);
            }
        }
    }

    public void OnItemUsed(BattleItemModel item)
    {
        Debug.Log("Item: " + item.Name + " was used!");
    }
}
