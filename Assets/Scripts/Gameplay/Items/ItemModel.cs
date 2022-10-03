using System.Collections.Generic;

public class ItemModel 
{
    public List<BattleItemModel> itemStats = new();
    public List<OwnedBattleItem> selectedItems = new();
    public int MaxItemQty;
}
