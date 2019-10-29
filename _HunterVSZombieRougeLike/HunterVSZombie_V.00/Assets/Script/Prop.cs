using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    public int healthEffect = 1;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player_Foot")
        {
            Debug.Log("recover player");
            collision.gameObject.GetComponent<PlayerController>().recoverHealth(healthEffect);
            Destroy(gameObject);
        }
    }
}
