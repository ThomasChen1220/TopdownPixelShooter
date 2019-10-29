using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    Vector3 movementBeforeAttack = new Vector3();
    GameObject stars;
    public Animator forceWave;
    bool waving = false;
    public Collider2D damageArea;
    // Start is called before the first frame update
    void Start()
    {
        load();
        attackDis *= gameObject.transform.Find("sprites").localScale.x;
        damageArea.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    protected override void Move()
    {
        base.Move();
        if (rb.velocity.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else {
            gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    public override void damaged(int damage)
    {
        if (getCurrentClipName() == "shocked")
        {
            damage = 1;
            animator.SetTrigger("Hit");
        }
        else {
            damage = 0;
        }
        health -= damage;
        if (health <= 0)
        {
            died();
        }
    }
    protected override void shocked()
    {
        stars = Instantiate(shockedStars, headTop.position, transform.rotation, headTop);
        animator.SetTrigger("Shocked");
        Destroy(stars, 2f);
    }
    protected override void deside(ref Vector2 movement, Vector3 diff)
    {
        if (getCurrentClipName() == "shocked")
        {
            movement *= 0.001f;
        }
        else if (getCurrentClipName() == "hit") {
            movement *= 0.001f;
            Destroy(stars);
        }
        else if (getCurrentClipName() == "rest")
        {
            movement *= 0.001f;
        }
        else if (getCurrentClipName().Split('_')[0] == "cooldown")
        {
            movement *= coolDownSpeed;
        }
        else if (getCurrentClipName().Split('_')[0] == "attack")
        {
            movement = movementBeforeAttack;
            if (getCurrentClipName().Split('_')[1] == "damage")
            {
                if (!waving)
                {
                    forceWave.SetTrigger("wave");
                    waving = true;
                }
                damageArea.enabled = true;
            }
            else {
                damageArea.enabled = false;
            }
        }
        else if (Mathf.Abs(diff.magnitude) <= attackDis)
        {
            attack();
            movementBeforeAttack = movement * attackSpeed;
            waving = false;
        }
        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
    }

}
