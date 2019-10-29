using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDetect : MonoBehaviour
{
    public int damage = 1;
    public Animator animator=null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("collision" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("hit player!");
            collision.gameObject.GetComponentInParent<PlayerController>().damaged(damage);
            if(animator!=null)
                animator.SetTrigger("HitPlayer");
        }
    }
}
