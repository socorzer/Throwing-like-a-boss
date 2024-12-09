using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] LayerMask _mask;
    bool isHit;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(isHit)return;
        Destroy(gameObject, 0.2f);
        if ((_mask & (1 << collision.gameObject.layer)) != 0)
        {
            PlayerStateMachine player = collision.gameObject.GetComponentInParent<PlayerStateMachine>();
            if (collision.gameObject.CompareTag("Head"))
            {
                player.TakeDamage(true);
            }
            else
            {
                player.TakeDamage(false);
            }
            isHit = true;
        }
    }
}
