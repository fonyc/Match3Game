using System;

[Serializable]
public class OwnedHero
{
    public string Name;
    public int Level;

    public OwnedHero(string name, int level)
    {
        Name = name;
        Level = level;
    }
}