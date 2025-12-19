using UnityEngine;

public abstract class BossAttack : ScriptableObject
{
    public string attackName;
    public float baseDamage;

    public abstract void ExecuteAttack(Transform boss, Transform firePoint);
    public virtual void ScaleWithLevel(int level) { }
}
