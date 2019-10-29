using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine;

public class BoardData {
    public Vector3 center;
    public bool[] gates = { true, true, true, true };
    public BoardData(Vector3 _center, bool[] _gates) {
        center = _center;
        gates = _gates;
    }
    public BoardData(Vector3 _center)
    {
        center = _center;
    }
}


public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class PlaceProb
    {
        public GameObject[] objects;
        public float prob;
        public int min, max;

        public PlaceProb(GameObject[] _objects, float _prob, int _min, int _max)
        {
            objects = _objects;
            prob = _prob;
            min = _min;
            max = _max;
        }
    }
    [Serializable]
    public class Range {
        public float max;
        public float min;
        public Range(float _min, float _max)
        {
            min = _min;
            max = _max;
        }
    }

    public GameObject map;
    public GameObject[] baseScene;
    public GameObject gateNextLevel;
    public PlaceProb grass;
    public PlaceProb streetLight;
    public PlaceProb smallStone;
    public PlaceProb bigStone;
    public PlaceProb bigTree;
    public PlaceProb bushe;
    public GameObject[] enemies;
    public int[] enemyPower;
    public Range enemyCount;

    //x:-0.55 to 0.55
    //y: -0.4 to 0.15
    //blocking: x:  -0.12 0.22
    [HideInInspector] public Range xRange = new Range(-0.45f, 0.45f);
    [HideInInspector] public Range yRange = new Range(-0.3f, 0.05f);
    [HideInInspector] public Range xRangeBlocking = new Range(-0.12f, 0.22f);
    public float gridSizeX = 0.15f;
    public float gridSizeY = 0.08f;
    [HideInInspector] GameManager.Room myRoom;
    public float currDifficulty=1f;

    private List<Vector3> gridPositions;
    private int count = 0;
    private GameObject scene;
    private Transform onGround_obj;
    private Transform placable_obj;
    private Transform allenemy_obj;

    public void deletScene() {
        Destroy(scene);
    }

    public void cleanMap() {
        for (int i = 0; i < map.transform.childCount; i++)
        {
            Destroy(map.transform.GetChild(i).gameObject);
        }
    }

    public GameObject createStartRoom(BoardData bd) {
        float temp = currDifficulty;
        currDifficulty = 0f;
        GameObject result = createRegularBoard(bd, true);
        currDifficulty = temp;
        return result;
    }

    public GameObject createEndRoom(BoardData bd) {
        GameObject result = createRegularBoard(bd);
        scene.transform.Find("Gates").Find("GateBlock_up").gameObject.SetActive(false);
        Instantiate(gateNextLevel, result.transform.position, Quaternion.identity, result.transform);
        return result;
    }

    public GameObject createRegularBoard(BoardData bd, bool start=false) {
        if(start)
            scene = Instantiate(baseScene[0], bd.center, Quaternion.identity, map.transform);
        else
            scene = Instantiate(baseScene[Random.Range(1,baseScene.Length)], bd.center, Quaternion.identity, map.transform);
        onGround_obj = scene.transform.Find("OnGround");
        placable_obj = scene.transform.Find("Placable");
        allenemy_obj = scene.transform.Find("Enemies");
        GameObject gates = scene.transform.Find("Gates").gameObject;

        //get grid pos
        gridPositions = new List<Vector3>();
        count = 0;
        for (float x = xRange.min; x < xRange.max; x += gridSizeX) {
            for (float y = yRange.min; y < yRange.max; y += gridSizeY) {
                gridPositions.Add(new Vector3(x, y, 0f));
                
            }
        }
        //Debug.Log(gridPositions.Count);
        for (int i = 0; i < gridPositions.Count; i++)
        {
            Vector3 temp = gridPositions[i];
            int randomIndex = Random.Range(i, gridPositions.Count);
            gridPositions[i] = gridPositions[randomIndex];
            gridPositions[randomIndex] = temp;
        }

        placeObject(grass, true); //create grass
        //placeObject(streetLight); //create light
        //placeObject(smallStone);
        //placeObject(bigStone);    //create small and big stone
        //placeObject(bigTree);     //create tree
        //placeObject(bushe);       //create bushe
        placeEnemy(currDifficulty); //create enemies

        //close gate
        bool[] g = bd.gates;
        gates.transform.Find("Gate_right").gameObject.SetActive(g[0]);
        gates.transform.Find("Gate_down").gameObject.SetActive(g[1]);
        gates.transform.Find("Gate_left").gameObject.SetActive(g[2]);
        gates.transform.Find("Gate_up").gameObject.SetActive(g[3]);

        gates.transform.Find("GateBlock_right").gameObject.SetActive(!g[0]);
        gates.transform.Find("GateBlock_down").gameObject.SetActive(!g[1]);
        gates.transform.Find("GateBlock_left").gameObject.SetActive(!g[2]);
        gates.transform.Find("GateBlock_up").gameObject.SetActive(!g[3]);

        return scene;
    }



    void placeObject(PlaceProb placing, bool onGround=false) {
        float poss = Random.Range(0f, 1f);
        if (poss > placing.prob) return;

        int num = Random.Range(placing.min, placing.max + 1);
        while (count < gridPositions.Count - 1&& num>0)
        {
            GameObject toPlace = placing.objects[Random.Range(0, placing.objects.Length)];
            if (onGround) {
                //Debug.Log(num);
                //place
                Instantiate(toPlace, onGround_obj.position+gridPositions[count], Quaternion.identity, onGround_obj);
                num--;
                count++;
                continue;
            }
            if (gridPositions[count].x > xRangeBlocking.max || gridPositions[count].x > xRangeBlocking.max) {
                count++;
                continue;
            }
            //place
            Instantiate(toPlace, placable_obj.position+ gridPositions[count], Quaternion.identity, placable_obj);
            num--;
            count++;
        }
        if (onGround) count = 0;
    }
    void placeEnemy(float difficulty)
    {
        float num = Random.Range(enemyCount.min, enemyCount.max + 1)*difficulty;
        while (count < gridPositions.Count - 1 && num > 0)
        {
            int index = Random.Range(0, enemies.Length);
            if (num - enemyPower[index] < 0) break;
            GameObject toPlace = enemies[index];
            Instantiate(toPlace, allenemy_obj.position + gridPositions[count], Quaternion.identity, allenemy_obj);
            num-=enemyPower[index];
            count++;
        }
    }

}
