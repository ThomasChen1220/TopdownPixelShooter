using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireSpirit : Enemy
{
    public float avoidDis = 0.5f;
    public float attackCooldown=1.5f;
    public float attackVari = 0.3f;

    public GameObject shootPoint;

    bool avoiding = false;
    float attackTimer;
    float acooldown;
    // Start is called before the first frame update
    // Start is called before the first frame update
    void Start()
    {
        load();
        attackTimer = Time.time + attackCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected override void Move()
    {
        Vector3 diff = player.transform.position - transform.position;
        Vector3 direction = diff.normalized;
        Vector2 movement = new Vector2(direction.x, direction.y) * speed;

        if (diff.magnitude <= attackDis)
        {
            movement *= attackSpeed;
            movement *= -1;
            avoiding = true;
        }
        else if (diff.magnitude <= attackDis + avoidDis && avoiding)
        {
            movement *= attackSpeed;
            movement *= -1;
        }
        else {
            avoiding = false;
        }
        rb.velocity = movement;

        if (Time.time > attackTimer) {
            attack();
        }
    }
    protected override void died()
    {
        base.died();
        attackTimer = Time.time + attackCooldown + 1000f;
    }
    protected override void attack()
    {
        base.attack();
        attackTimer = Time.time+ attackCooldown + Random.Range(-attackVari, attackVari);

        Vector3 shootDirection = player.transform.position - shootPoint.transform.position;
        shootDirection.Normalize();

        GameObject s = Instantiate(shooting, shootPoint.transform.position, Quaternion.identity);
        FireBall arrowScript = s.GetComponent<FireBall>();
        arrowScript.archer = damageBox.gameObject;
        arrowScript.velocity = shootDirection * projectileSpeed;
        //arrowScript.force = arrowForce;

        //give rotation
        s.transform.Rotate(0f, 0f, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg);
        Destroy(s, 2f);//get rid of the arrow after time
    }
}
