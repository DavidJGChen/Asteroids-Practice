using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
[CreateAssetMenu(menuName="Asteroid")]
public class AsteroidData : ScriptableObject
{
    public string asteroidType = "New Asteroid";
    public Sprite[] asteroidSprites;
    public float minForce;
    public float maxForce;
    public float mass;
    public int health;
    public int splitAmount;
    public AsteroidData nextAsteroid;
}
}