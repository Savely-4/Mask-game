public interface IDamageble
{
    float Hp { get;  set; }
    float MaxHp { get; set; }

    abstract void ChangeHp(int dmg);

    abstract void Die();
}
