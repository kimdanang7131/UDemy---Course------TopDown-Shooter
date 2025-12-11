using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy data/Melee weapon Data", fileName = "New Weapon Datas")]
public class Enemy_MeleeWeaponData : ScriptableObject
{
    public List<AttackData_EnemyMelee> attackList;
    public float turnSpeed = 10;
}
