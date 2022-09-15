using System;

[Serializable]
public class OwnedHero
{
    public string Id;
    public string Type;
    public int Level;

    public OwnedHero(string id,string type, int level)
    {
        Id = id;
        Type = type;
        Level = level;
    }
}