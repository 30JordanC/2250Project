using UnityEngine;

public class Weapon : Item
{
    [SerializeField] protected int damage = 10;

    public int Damage => damage;

    public virtual void Attack()
    {
        Debug.Log(itemName + " attacked and dealt " + damage + " damage.");
    }
}