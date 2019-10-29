using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirPush : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Air Push hit: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("pushed"+ collision.gameObject.name);
            collision.gameObject.GetComponentInParent<Enemy>().pushed();
        }
        if (collision.gameObject.tag == "Projectile")
        {
            Debug.Log("pushed" + collision.gameObject.name);
            collision.gameObject.GetComponent<Projectile>().pushed();
        }
    }
}
