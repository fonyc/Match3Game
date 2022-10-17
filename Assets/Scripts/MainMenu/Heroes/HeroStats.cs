
[System.Serializable]
public class Stats
{
    public int ATK;
    public int DEF;
    public int HP;
    public int ManaPerHit;
    public float Progression;

    public Stats (int ATK, int DEF, int HP, int ManaPerHit, float Progression)
    {
        this.ATK = ATK;
        this.DEF = DEF;
        this.HP = HP;
        this.ManaPerHit = ManaPerHit;
        this.Progression = Progression;
    }
}
