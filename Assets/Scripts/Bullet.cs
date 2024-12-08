using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask _mask;
    bool isHit;
    float damage;
    public void SetDamage(float damage)
    {
        this.damage = damage;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isHit)return;
        Destroy(gameObject, 0.2f);

        if ((_mask & (1 << collision.gameObject.layer)) != 0)
        {
            PlayerStateMachine player = collision.gameObject.GetComponentInParent<PlayerStateMachine>();
            if (damage != 0)
            {
                player.TakeDamage(damage);
            }
            else
            {
                if (collision.gameObject.CompareTag("Head"))
                {
                    player.TakeDamage(true);
                }
                else
                {
                    player.TakeDamage(false);
                }
            }
            isHit = true;
        }
    }
}
