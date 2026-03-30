using UnityEngine;

public class Weapon : Item
{
    [SerializeField] private int damage = 10;

    public int Damage => damage;

    public virtual void Attack()
    {
        Debug.Log(ItemName + " attacked and dealt " + damage + " damage.");
    }
}