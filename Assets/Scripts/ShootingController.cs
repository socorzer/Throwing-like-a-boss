using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingController : MonoBehaviour
{
    [SerializeField] Transform _aimPivot;
    [SerializeField] Transform _bulletSpawnPoint;
    [SerializeField] PlayerStateMachine _player;
    [SerializeField] float _shootDelayTime;
    [SerializeField] float _delayAfterThrow;
    string _itemName = "none";
    public void SetItemName(string itemName)
    {
        _itemName = itemName;
    }
    public void Shoot(GameObject prefab, float power)
    {
        StartCoroutine(ShootBullet(prefab, power));
    }
    IEnumerator ShootBullet(GameObject prefab, float power)
    {
        int shootAmount = 1;
        float damage = 0;
        if (_itemName != "none")
        {
            int amount = StatsReader.Instance.GetStat(_itemName).Amount;
            shootAmount = amount > 0?amount:1;
            damage = StatsReader.Instance.GetStat(_itemName).Damage;
        }
        while (shootAmount > 0)
        {
            GameObject bullet = Instantiate(prefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
            Rigidbody2D rigidbody = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = bullet.transform.up * power;
            rigidbody.AddForce(direction, ForceMode2D.Impulse);
            shootAmount--;
            _player.PlayThrowAnimation();
            if (shootAmount > 0)
                yield return new WaitForSeconds(_shootDelayTime);
            else yield return null;
        }
        _itemName = "none";
        Invoke(nameof(ThrowEnd), _delayAfterThrow);
    }
    public void ThrowEnd()
    {
        _player.ThrowSuccess();
    }
}
