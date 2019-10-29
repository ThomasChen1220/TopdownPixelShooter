using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceObject : MonoBehaviour
{
    bool debugging = false;
    public GameObject[] placing;

    private MouseController mc;
    private BoardManager bm;
    private GameManager gm;

    public bool testing = false;
    public static bool died = false;
    // Start is called before the first frame update
    void Start()
    {
        mc = GetComponent<MouseController>();
        bm = GetComponent<BoardManager>();
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(testing){
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            for (int i = 0; i < 9; i++)
            {
                if (Input.GetKeyDown("" + (i + 1)))
                {
                    Instantiate(placing[i], mousePos, Quaternion.identity);
                }
            }

            if (Input.GetKeyDown("z")) mc.enableCurser();
            if (Input.GetKeyDown("x")) mc.disableCurser();
            if (Input.GetKeyDown("k")) bm.createRegularBoard(new BoardData(new Vector3()));
            if (Input.GetKeyDown("l")) bm.deletScene();
        }
        if (died) {
            if (Input.GetKeyDown("enter")||Input.GetKeyDown("return")) {
                //restart the game
                Debug.Log("restarting");
                gm.restart();
                died = false;
            }
        }
    }
}
