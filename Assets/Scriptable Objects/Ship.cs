using UnityEngine;

namespace Dawid {
[CreateAssetMenu(menuName="Ships")]
public class Ship : ScriptableObject
{
    public string shipName = "New Ship";
    public Sprite shipSprite;
    public float moveSpeed;
    public float rotateSpeed;
    public float timeToAccel;
    public float mass;
    public int health;
}
}