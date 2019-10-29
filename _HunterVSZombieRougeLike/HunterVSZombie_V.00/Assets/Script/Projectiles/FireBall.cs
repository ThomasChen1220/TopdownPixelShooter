using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Projectile
{
    private void Update()
    {
        move();
    }
    public override bool onHit(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            //Debug.Log(other.name);

            PlayerController player = other.GetComponentInParent<PlayerController>();
            player.damaged(1);
            return true;
        }
        if (other.CompareTag("Walls"))
        {
            Destroy(gameObject);
            return true;
        }
        //if (other.CompareTag("Enemy"))
        //{
        //    Destroy(gameObject);
        //    Debug.Log(other.name);
        //    return true;
        //}
        return false;
    }
}
