using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [HideInInspector] public Vector2 velocity;
    [HideInInspector] public GameObject archer;
    [HideInInspector] public float force;
    protected bool pushable=true;
    private void Update()
    {
        move();
    }
    protected virtual void move() {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 newPosition = currentPosition + velocity * Time.deltaTime;

        RaycastHit2D[] hits = Physics2D.LinecastAll(currentPosition, newPosition);
        foreach (RaycastHit2D hit in hits)
        {
            GameObject other = hit.collider.gameObject;
            if (other != archer)
            {
                if (onHit(other)) break;
            }
        }

        transform.position = newPosition;
    }
    public virtual void pushed() {
        if (pushable) Destroy(gameObject);
    }
    public virtual bool onHit(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            //Debug.Log(other.name);
            return true;
        }
        if (other.CompareTag("Walls"))
        {
            Destroy(gameObject);
            return true;
        }
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            //Debug.Log(other.name);

            Enemy enemy = other.GetComponentInParent<Enemy>();
            enemy.damaged(1);
            return true;
        }
        return false;
    }
}
