using UnityEngine;

public class HpOnObject : MonoBehaviour
{
    static public HpOnObject hpOnObj;
    public float hp;
    public float maxHp;
    public float regenRate;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void HpRegeneration(float regenNum)
    {
        hp += (maxHp / 100) * regenRate * Time.deltaTime;
        hp = Mathf.Clamp(hp, 0, maxHp);
    }
    void ChangeHp(float dmg,bool isBurnDmg = false)
    {
        if (hp - dmg < 0)
            Destroy(gameObject);
        else
        {
            if (isBurnDmg)
                hp -= dmg * Time.deltaTime;
            else
                hp -= dmg;
        }
    }
}
