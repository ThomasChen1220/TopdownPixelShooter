using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.5f;
    public float attackSpeed = 2f;
    public float coolDownSpeed = 0.5f;
    public float attackDis = 0.5f;
    public int maxHealth = 4;
    public int scorePoint = 100;

    public GameObject player;
    public Animator animator;
    public Rigidbody2D rb;
    public GameObject shockedStars;
    public Transform headTop;

    public GameObject shooting;
    public float projectileSpeed;

    protected int health;
    protected BoxCollider2D damageBox;

    protected void load ()
    {
        player = GameObject.Find("Player");
        animator = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        health = maxHealth;
        damageBox = gameObject.GetComponentInChildren<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        load();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        Vector3 diff = player.transform.position - transform.position;
        Vector3 direction = diff.normalized;
        Vector2 movement = new Vector2(direction.x, direction.y) * speed;

        deside(ref movement, diff);

        rb.velocity = movement;
    }
    protected virtual void deside(ref Vector2 movement, Vector3 diff) {
        if (getCurrentClipName() == "shocked")
        {
            movement *= 0.001f;
        }
        else if (getCurrentClipName() == "rest")
        {
            movement *= 0.001f;
        }
        else if (getCurrentClipName() == "cooldown")
        {
            movement *= coolDownSpeed;
        }
        else if (getCurrentClipName().Split('_')[0] == "attack")
        {
            movement *= attackSpeed;
        }
        else if (Mathf.Abs(diff.magnitude) <= attackDis)
        {
            attack();
        }
    }
    protected virtual void attack() {
        animator.SetTrigger("Attack");
    }
    public virtual void damaged(int damage)
    {
        if (getCurrentClipName() == "shocked"){
            damage *= 5;
        }
        health -= damage;
        animator.SetTrigger("Hit");
        if (health <= 0)
        {
            died();
        }
    }
    protected virtual void died() {
        animator.SetBool("Died", true);
        damageBox.enabled = false;
        gameObject.GetComponent<Collider2D>().enabled = false;
        speed = 0f;
        GameManager.playerScore += scorePoint;
        Destroy(gameObject, 1.5f);
    }
    public virtual void pushed() {
        string[] name= getCurrentClipName().Split('_');
        if (name.Length>1 && name[1] == "pushable") {
            Debug.Log("I am pushed! Ahhh!?~~~~");
            shocked();
        }
    }
    protected virtual void shocked() {
        GameObject stars = Instantiate(shockedStars, headTop.position, transform.rotation, headTop);
        animator.SetTrigger("Shocked");
        Destroy(stars, 1f);
    }
    public float getCurrentClipTime()
    {
        AnimatorClipInfo[] info = animator.GetCurrentAnimatorClipInfo(0);
        //Debug.Log(info[0].clip.name);
        //Debug.Log("clip length: " + info[0].clip.length);
        return info[0].clip.length;
    }
    public string getCurrentClipName()
    {
        AnimatorClipInfo[] info = animator.GetCurrentAnimatorClipInfo(0);
        //Debug.Log(info[0].clip.name);
        //Debug.Log("clip length: " + info[0].clip.length);
        return info[0].clip.name;
    }
}