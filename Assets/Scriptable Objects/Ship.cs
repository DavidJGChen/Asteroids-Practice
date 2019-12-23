using UnityEngine;

namespace Dawid {
public abstract class Ship : ScriptableObject
{
    public string shipName = "New Ship";
    public Sprite shipSprite;
    public float moveSpeed;
    public float rotateSpeed;
    public float timeToAccel;
    public int health;

    public abstract void Initialize(GameObject obj);
    public abstract void Shoot();
}
}