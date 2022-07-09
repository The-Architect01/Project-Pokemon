using UnityEngine;

public class Stat {

    public int Value { get; set; }
    public float Base { get; set; }
    public float EVs { get; private set; } = 0;
    public float IV { get; set; }
    public float EV_Reward { get; set; }
    public bool IsHP { get; set; } = false;

    public void AddEVs(int newValue) {
        if(EVs >= 252 || EVs + newValue >= 252) {
            EVs = 252;
        } else {
            EVs += newValue;
        }
    }

    public void Update(int Lvl, decimal nature = 1m) {
        float Nature = (float)nature;
        if (IsHP) {
            float HP = Mathf.Ceil((((((2f * Base) + IV)) + Mathf.Floor(EVs/4f)) * Lvl)/100f);
            Value = (int)Mathf.Ceil(HP + Lvl + 10f);
        } else {
            float STAT = Mathf.Ceil((((2f * Base)+IV+(EVs/4f)))*Lvl);
            Value = (int)Mathf.Ceil(((STAT / 100) + 5) * Nature);
        }
    }

}
