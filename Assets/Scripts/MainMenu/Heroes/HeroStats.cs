
[System.Serializable]
public class Stats
{
    public int ATK;
    public int DEF;
    public int HP;
    public float Progression;

    public Stats (int ATK, int DEF, int HP, float Progression)
    {
        this.ATK = ATK;
        this.DEF = DEF;
        this.HP = HP;
        this.Progression = Progression;
    }
}
