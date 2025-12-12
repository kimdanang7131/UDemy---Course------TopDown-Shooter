using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy data/Melee weapon Data", fileName = "New Weapon Data")]
public class Enemy_MeleeWeaponData : ScriptableObject
{
    public List<AttackData_EnemyMelee> attackList;
    public float turnSpeed = 10;
}
