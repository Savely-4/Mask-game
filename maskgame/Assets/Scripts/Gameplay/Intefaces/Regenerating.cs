using System.Collections;
using UnityEngine;

public interface IRegenerating : IDamageble
{
    bool AllowRegen { get; set; }
    float RegenForSecond { get; set; }
    float  Ticrate { get; set; }

    public virtual IEnumerator Regenerate()
    {
       while (true)
       {
            if (Hp > 0f && Hp < MaxHp)
            {
               float regen = RegenForSecond * MaxHp * Ticrate;
               Hp = Mathf.Min(Hp + regen, MaxHp);
                Debug.Log(Hp);
            }
            yield return new WaitForSeconds(Ticrate);
       }
    }
}

