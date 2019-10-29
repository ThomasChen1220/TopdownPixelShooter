using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    //public float speed = 0.5f;
    //public float attackSpeed = 2f;
    //public float coolDownSpeed = 0.5f;
    //public float attackDis = 0.5f;
    //public float coolDown = 0.5f;
    //public int maxHealth = 4;

    //public GameObject player;
    //public Animator animator;
    //public Rigidbody2D rb;

    //int health;
    //BoxCollider2D damageBox;

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
}