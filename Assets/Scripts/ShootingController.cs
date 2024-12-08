using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] Transform _aimPivot;
    [SerializeField] Transform _bulletSpawnPoint;

    public void Shoot(GameObject prefab, float power)
    {
        GameObject bullet = Instantiate(prefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
        Vector2 direction = bullet.transform.up * power;
        rigidbody.AddForce(direction, ForceMode2D.Impulse);
    }
}
