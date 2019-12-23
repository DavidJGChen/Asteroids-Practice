using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dawid {
public class ProjectileTriggerable : MonoBehaviour
{
    public float fireForce;
    public float duration;
    public GameObject projectile;
    public Transform projectileSpawn;

    public void Shoot() {
        var clonedProjectile = ObjectPooler.SharedInstance.GetPooledObject(projectile.tag);
        clonedProjectile.transform.position = projectileSpawn.position;
        clonedProjectile.transform.rotation = Quaternion.identity;
        clonedProjectile.SetActive(true);

        clonedProjectile.GetComponent<Rigidbody2D>().AddForce(projectileSpawn.transform.up * fireForce);
    }
}
}