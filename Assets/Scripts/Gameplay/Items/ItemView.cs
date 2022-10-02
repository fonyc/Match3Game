using System;
using UnityEngine;

public class ItemView : MonoBehaviour
{
    ItemController _itemController;
    [SerializeField] Transform _itemsParent;
    [SerializeField] ItemViewSlot _itemSlotPrefab;

    public void Initialize(ItemController itemController)
    {
        _itemController = itemController;

        while (_itemsParent.childCount > 0)
        {
            Transform child = _itemsParent.GetChild(0);
            child.SetParent(null);
            Destroy(child.gameObject);
        }

        foreach (BattleItemModel itemModel in _itemController.Model.itemStats)
        {
            foreach (OwnedBattleItem ownedItem in _itemController.Model.selectedItems)
            {
                if (itemModel.Id != ownedItem.Id) continue;
                ItemViewSlot item = Instantiate(_itemSlotPrefab, _itemsParent);
                item.SetData(itemModel, ownedItem.Amount, _itemController.Model.MaxItemQty, OnItemUsed);
            }
        }
    }

    public void OnItemUsed(BattleItemModel item)
    {
        ItemEffect effect = item.ItemEffect;
        _itemController.OnPlayerStatChanged(effect.Stat, effect.Amount);
        _itemController.RemovePotionFromPlayer(item.Id);
    }
}
