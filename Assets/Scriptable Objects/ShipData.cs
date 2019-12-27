using UnityEngine;

namespace Dawid {
[CreateAssetMenu(menuName="Ships")]
public class ShipData : ScriptableObject
{
    public string shipName = "New Ship";
    public Sprite shipSprite;
    public float moveSpeed;
    public float rotateSpeed;
    public float timeToAccel;
    public float mass;
    public int health;
    public ProjectileData defaultProjectile;
}
}