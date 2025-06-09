using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class HpOnObject : MonoBehaviour
{
    static public HpOnObject hpOnObj;
    public float hp;
    public float maxHp;
    public float regenRate;

    float time;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HpRegeneration(float regenNum)
    {
        hp += (maxHp / 100) * regenRate * Time.deltaTime;
        hp = Mathf.Clamp(hp, 0, maxHp);
    }
    public void ChangeHp(float dmg, float durations,bool isBurnDmg = false)
    {
        if (hp - dmg < 0) Destroy(gameObject);
        else
        {
            if (isBurnDmg)StartCoroutine(BurnDamage(durations, dmg));
            else hp -= dmg;
        }
    }
    private IEnumerator BurnDamage(float durations,float dmg)
    {
        time = 0;
        while(time < durations)
        {
            hp -= dmg * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
    }
}
