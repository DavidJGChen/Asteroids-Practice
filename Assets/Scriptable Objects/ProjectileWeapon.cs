using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public abstract class ProjectileWeapon : ScriptableObject
{
    public string weaponName = "New Weapon";
    public Sprite projectileSprite;
    public abstract void Initialize(GameObject obj);
    public abstract void Shoot();
}
}