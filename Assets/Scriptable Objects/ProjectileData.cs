using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
[CreateAssetMenu(menuName="Projectile")]
public class ProjectileData : ScriptableObject
{
    public string projectileName = "New Projectile";
    public Sprite projectileSprite;
    public float moveSpeed;
    public float fireForce;
    public float timeToDeath;
    // public float timeToAccel;
    public float mass;
    // public int health;
}
}