using System;

[Serializable]
public class OwnedBattleItem
{
    public string Name;
    public int Amount;

    public OwnedBattleItem(string name, int amount)
    {
        Name = name;
        Amount = amount;
    }
}