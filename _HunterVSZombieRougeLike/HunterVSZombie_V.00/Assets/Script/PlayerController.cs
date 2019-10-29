using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    public float moveSpeed = 1f;
    public float walkSpeed = 0.5f;
    public float crosshairLength = 0.25f;
    public float crosshairStretch = 0.5f;
    public float arrowVelocity = 5.0f;
    public float arrowStart = 0.01f;
    public int maxHealth = 6;
    public float pushCooldown = 1f;
    //public float arrowForce = 1f;
    //public float drag = 0.1f;



    [HideInInspector]public float acceleration = 0f;
    float faceRight = 1f;
    bool aimming = false;
    //Vector3 lastMovement=new Vector3();

    public Animator topAnimator;
    public Animator legAnimator;
    public GameObject crosshair;
    public GameObject arrow;
    public GameObject shootPoint;
    public GameObject mySprite;
    public UImanager UImg;
    public GameObject airPush;
    public GameObject directImage;

    int health;
    Animator airPushAinm;
    Collider2D airPushArea;
    GameManager GM;

    float cooldownTimer;
    public GameObject coolDownBar;
    public GameObject waitBar;
    public GameObject doneBar;
    // Start is called before the first frame update
    void Start()
    {
        //Invoke("test", 1f);
        //Invoke("test", 1f); Invoke("test", 1f);
        rb = gameObject.GetComponent<Rigidbody2D>();
        health = maxHealth;
        UImg.displayHealth(health, maxHealth);
        airPushAinm = airPush.GetComponentInChildren<Animator>();
        airPushArea = airPush.GetComponentInChildren<Collider2D>();
        airPushArea.enabled = false;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        cooldownTimer = Time.time - pushCooldown;
        //coolDownBar = gameObject.transform.Find("coolDownBar").gameObject;
        //waitBar = coolDownBar.transform.Find("wait").gameObject;
        //downBar = coolDownBar.transform.Find("done").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        MoveCrossHair();
        Fire();
        push();
    }

    public void fullHealth() {
        health = maxHealth;
        UImg.displayHealth(health, maxHealth);
    }
    public void recoverHealth(int amount) {
        health += amount;
        if (health > maxHealth) health = maxHealth;
        UImg.displayHealth(health, maxHealth);
    }
    public void damaged(int damage) {
        health -= damage;
        Debug.Log("player got hit " + health);
        checkDeath();
        UImg.displayHealth(health, maxHealth);
    }
    void checkDeath() {
        if (health <= 0) {
            GM.gameEnd();
            fullHealth();
        }
    }

    void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f);
        movement = movement.normalized;
        if (movement.x > 0.001f) { faceRight = 1f; }
        else if (movement.x < -0.001f) { faceRight = -1f; }
        //set the animator parameter
        {
            topAnimator.SetFloat("Horizontal", movement.x);
            topAnimator.SetFloat("Vertical", movement.y);
            topAnimator.SetFloat("Magnitude", movement.magnitude);
            topAnimator.SetFloat("FaceRight", faceRight);

            Vector3 movement1 = movement;
            if (aimming) movement1 *= 0.1f;
            legAnimator.SetFloat("Horizontal", movement1.x);
            legAnimator.SetFloat("Vertical", movement1.y);
            legAnimator.SetFloat("Magnitude", movement1.magnitude);
            legAnimator.SetFloat("FaceRight", faceRight);
        }
        movement *= moveSpeed;
        if (aimming)
            movement *= walkSpeed;

        //transform.position = transform.position + movement * Time.deltaTime;
        rb.velocity = new Vector2(movement.x, movement.y);
        //Vector3 acc = (movement - lastMovement);
        //rb.velocity += new Vector2(acc.x, acc.y);
        //lastMovement = movement;
    }
    void MoveCrossHair()
    {
        //get the mouse input
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        //show the crosshair if aimming
        if (aimming)
        {
            //set the posi of crosshair
            Vector3 center = shootPoint.transform.position;
            Vector3 diff = mousePos - center;
            diff.z = 0;
            diff = diff.normalized + diff * crosshairStretch;
            diff *= crosshairLength;
            Vector3 aimPos = center + diff;
            aimPos.z = 0;
            crosshair.transform.position = aimPos;
            crosshair.SetActive(true);

            //set the player body pose
            diff.Normalize();
            topAnimator.SetBool("Aimming", aimming);
            topAnimator.SetFloat("AimX", diff.x);
            topAnimator.SetFloat("AimY", diff.y);
            legAnimator.SetBool("Aimming", aimming);
            legAnimator.SetFloat("AimX", diff.x);
            legAnimator.SetFloat("AimY", diff.y);
        }
        else
        {
            crosshair.SetActive(false);
            topAnimator.SetBool("Aimming", aimming);
            legAnimator.SetBool("Aimming", aimming);
        }
    }
    void Fire()
    {
        if (Input.GetButton("Aim"))
        {
            aimming = true;
        }
        else
        {
            aimming = false;
        }
        if (Input.GetButtonDown("Fire1") && aimming)
        {
            //Debug.Log("FIRE!");
            //give velocity
            Vector3 shootDirection = crosshair.transform.position - shootPoint.transform.position;
            shootDirection.Normalize();
            Vector3 startPoint = shootPoint.transform.position + arrowStart * shootDirection;

            GameObject shooting = Instantiate(arrow, startPoint, Quaternion.identity);
            Arrow arrowScript = shooting.GetComponent<Arrow>();
            arrowScript.archer = mySprite;
            arrowScript.velocity = shootDirection * arrowVelocity;
            //arrowScript.force = arrowForce;

            //give rotation
            shooting.transform.Rotate(0f, 0f, Mathf.Atan2(shootDirection.y, shootDirection.x) * Mathf.Rad2Deg);
            
            Destroy(shooting, 2f);//get rid of the arrow after time
        }
    }
    void closeAirpushArea() {
        airPushArea.enabled = false;
    }
    void showCoolDownBar(float percent) {
        coolDownBar.SetActive(true);
        float unit = 1f / (doneBar.transform.childCount+1);
        float count = 0f;
        //string show = "";
        for (int i = 0; i < doneBar.transform.childCount; i++)
        {
            count += unit;
            //show += percent > count;
            if (percent > count) doneBar.transform.GetChild(i).gameObject.SetActive(false);
            else doneBar.transform.GetChild(i).gameObject.SetActive(true);
        }
        //Debug.Log(show);
    }
    void push() {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Vector3 direction = mousePos - airPush.transform.position;
        directImage.transform.eulerAngles = new Vector3(-70f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-45f);
        if (Time.time <= cooldownTimer + pushCooldown)
        {
            float percent = 1 - (cooldownTimer + pushCooldown - Time.time) / pushCooldown;
            //Debug.Log("" + 100f * percent + "%");
            showCoolDownBar(percent);
        }
        else {
            //hideCoolDownBar
            coolDownBar.SetActive(false);
        }
        if (Input.GetKeyDown("space")&&Time.time>cooldownTimer+pushCooldown) {
            //Debug.Log("push");
            cooldownTimer = Time.time;
            airPushArea.enabled = true;
            airPush.transform.eulerAngles = new Vector3(50f, 0f, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
            airPushAinm.SetTrigger("Push");
            Invoke("closeAirpushArea", 0.2f);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GateRight")
        {
            Debug.Log("enter right gate");
            GM.switchRoom(0);
        }
        if (collision.gameObject.tag == "GateLeft")
        {
            Debug.Log("enter left gate");
            GM.switchRoom(2);
        }
        if (collision.gameObject.tag == "GateUp")
        {
            Debug.Log("enter up gate");
            GM.switchRoom(3);
        }
        if (collision.gameObject.tag == "GateDown")
        {
            Debug.Log("enter down gate");
            GM.switchRoom(1);
        }
        if (collision.gameObject.tag == "GateNextLevel")
        {
            Debug.Log("enter next Level!!");
            GM.nextLevel();
        }
    }
}