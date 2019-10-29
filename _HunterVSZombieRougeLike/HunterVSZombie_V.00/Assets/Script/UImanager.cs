using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    public GameObject deadScene;
    public GameObject levelScene;
    public GameObject[] hearts;
    public GameObject healthBar;
    public float distance = 120f;
    public GameObject loseHealthEffect;
    public Text scoreText;
    public Text levelText;

    void Start() {
    }

    public void playLevelScene(int level) {
        levelScene.SetActive(true);
        levelText.text = "Entering Level " + level;
        StartCoroutine(mySetActive(levelScene,false,2));
    }
    IEnumerator mySetActive(GameObject obj, bool setTo, int sec)
    {
        yield return new WaitForSeconds(sec);
        obj.SetActive(setTo);
    }

    public void playDeadScene(int score)
    {
        Debug.Log(score);
        deadScene.SetActive(true);
        scoreText.text = "Your Score: "+score;
        PlaceObject.died = true;
    }
    public void displayHealth(int health, int maxHealth)
    {
        bool lose = true;
        if (health > maxHealth) {
            Debug.Log("error in display health, UImanager");
            return;
        }
        if (health == maxHealth) lose = false;

        foreach (Transform child in healthBar.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        Vector3 position = healthBar.transform.position;
        Vector3 placePos=new Vector3();
        while (health > 0) {
            if (health == 1) {
                health--;
                maxHealth-=2;
                Instantiate(hearts[1], position, healthBar.transform.rotation, healthBar.transform);
                placePos = position;
                position.x += distance;
                break;
            }
            else if (health == 2) {
                placePos = position;
                placePos.x += distance;
            }
            health -= 2;
            maxHealth -= 2;
            Instantiate(hearts[0], position, healthBar.transform.rotation, healthBar.transform);
            position.x += distance;
        }
        if (lose) {
            placePos = Camera.main.ScreenToWorldPoint(placePos);
            placePos.z = 0;
            Instantiate(loseHealthEffect, placePos, Quaternion.identity);
        }
        while (maxHealth > 0) {
            Debug.Log(maxHealth);
            if (maxHealth == 1)
            {
                maxHealth -= 1;
                Instantiate(hearts[3], position, healthBar.transform.rotation, healthBar.transform);
                break;
            }
            maxHealth -= 2;
            Instantiate(hearts[2], position, healthBar.transform.rotation, healthBar.transform);
            position.x += distance;
        }
    }
}
