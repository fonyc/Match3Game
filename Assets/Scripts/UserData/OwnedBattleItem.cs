using System;

[Serializable]
public class OwnedBattleItem
{
    public string Id;
    public string Type;
    public int Amount;

    public OwnedBattleItem(string id, string type, int amount)
    {
        Id = id;
        Type = type;
        Amount = amount;
    }
}