using System.Collections.Generic;

[System.Serializable]
public class LevelModelItem
{
    public int Level;
    public string Enemy;
    public List<ResourceItem> Rewards = new();
}