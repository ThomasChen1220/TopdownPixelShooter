using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public UImanager uimanager;
    private BoardManager boardManager;
    private List<Room> allRoom;
    private List<Gate> expanable;
    public GameObject Player;
    public float v, h;
    public static float verDis, horDis;
    public int totalRoomInLevel = 4;
    int playerInRoom = 0;
    public static int playerScore = 0;
    int level = 1;

    public class Gate {
        public Room currRoom;
        public Vector3 expandRoom;
        public int expandRoomId = -1;
        public bool used = false;
        public Gate(Room _currRoom, Vector3 nextRoom) {
            currRoom = _currRoom;
            expandRoom = nextRoom;
            used = false;
        }
    }
    public class Room {
        public static int nextId;
        public int id;
        public Vector3 center;
        public Gate[] gates=new Gate[4];
        public Room(Vector3 _center= new Vector3()) {
            id = nextId;
            Room.nextId++;
            center = _center;

            gates[0] = new Gate(this, center + new Vector3(horDis, 0f, 0f));
            gates[1] = new Gate(this, center + new Vector3(0f, -verDis, 0f));
            gates[2] = new Gate(this, center + new Vector3(-horDis, 0f, 0f));
            gates[3] = new Gate(this, center + new Vector3(0f, verDis, 0f));
        }
        public GameObject myScene;
    }
    
    public void gameEnd() {
        int score = playerScore;
        uimanager.playDeadScene(score);
        boardManager.cleanMap();
    }

    void expandGate(Gate myGate) {
        myGate.used = true;
        Room myRoom=new Room(myGate.expandRoom);
        myGate.expandRoomId = myRoom.id;
        allRoom.Add(myRoom);
        //add the new gates to expandable
        foreach (Gate gate in myRoom.gates) {
            expanable.Add(gate);
        }
        //check for overlap
        foreach(Gate gate in expanable) {
            foreach (Gate newgate in myRoom.gates) {
                if (newgate.expandRoom == gate.currRoom.center&&gate.expandRoom==newgate.currRoom.center) {
                    newgate.used = true;
                    gate.used = true;
                    newgate.expandRoomId = gate.currRoom.id;
                    gate.expandRoomId = newgate.currRoom.id;
                }
            }
        }
        for (int i=expanable.Count-1;i>=0;i--) {
            if (expanable[i].used == true) {
                expanable.RemoveAt(i);
            }
        }
    }
    bool expandGate_end(Gate myGate)
    {
        if (myGate.expandRoom.y < myGate.currRoom.center.y)
        {
            expanable.Remove(myGate);
            return false;
        }
        myGate.used = true;
        Room myRoom = new Room(myGate.expandRoom);
        myGate.expandRoomId = myRoom.id;
        allRoom.Add(myRoom);

        foreach (Gate gate in myRoom.gates)
        {
            if (gate.expandRoom == myGate.currRoom.center)
            {
                gate.used = true;
                gate.expandRoomId = myGate.currRoom.id;
            }
        }
        //check for overlap
        for (int i = expanable.Count - 1; i >= 0; i--)
        {
            if(expanable[i].expandRoom==myRoom.center)
                expanable.RemoveAt(i);
        }
        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.verDis = v;
        GameManager.horDis = h;
        boardManager = GetComponent<BoardManager>();
        generateMap(totalRoomInLevel);
        //foreach (Room room in allRoom) {
        //    Debug.Log(room.center.x + ", " + room.center.y);
        //}
        createRooms();
    }
    public void restart() {
        //TODO
        Player.transform.position = new Vector3();
        totalRoomInLevel = 4;
        boardManager.currDifficulty = 1f;
        generateMap(totalRoomInLevel);
        boardManager.cleanMap();
        Camera.main.transform.position = new Vector3(0f, 0f, -10f);
        createRooms();
        level = 0;
        playerScore = 0;
        uimanager.deadScene.SetActive(false);
    }
    public void nextLevel() {
        Player.transform.position = new Vector3();
        totalRoomInLevel += 2;
        boardManager.currDifficulty *= 1.3f;
        generateMap(totalRoomInLevel);
        boardManager.cleanMap();
        Camera.main.transform.position = new Vector3(0f,0f,-10f);
        createRooms();
        uimanager.playLevelScene(++level);
    }

    void createRooms() {
        playerInRoom = 0;
        foreach (Room room in allRoom) {
            bool[] g = new bool[4];
            for (int i = 0; i < 4; i++) {
                g[i] = room.gates[i].used;
            }
            BoardData bd=new BoardData(room.center, g);
            if (room.id == 0)
            {
                room.myScene = boardManager.createStartRoom(bd);
                room.myScene.SetActive(true);
            }
            else if (room.id == totalRoomInLevel - 1) {
                room.myScene = boardManager.createEndRoom(bd);
                room.myScene.SetActive(false);
                setEnemies(room.myScene, false);
            }
            else
            {
                room.myScene = boardManager.createRegularBoard(bd);
                room.myScene.SetActive(false);
                setEnemies(room.myScene, false);
            }
        }
    }
    public void setEnemies(GameObject scene, bool active) {
        Transform enemies = scene.transform.Find("Enemies");
        for (int i = 0; i < enemies.childCount; i++) {
            enemies.GetChild(i).gameObject.GetComponent<Enemy>().enabled = active;
        }
    }
    IEnumerator setEnemiesInTime(GameObject scene, bool active, float sec)
    {
        yield return new WaitForSeconds(sec);
        setEnemies(scene, active);
    }

    void generateMap(int rooms, bool startLevel=false) {
        Room.nextId = 0;
        allRoom = new List<Room>();
        expanable = new List<Gate>();
        Room starter = new Room(new Vector3());
        allRoom.Add(starter);
        //add the new gates to expandable
        foreach (Gate gate in starter.gates)
        {
            expanable.Add(gate);
        }
        rooms--;
        while (rooms > 0 && expanable.Count > 0) {
            //Debug.Log(rooms);
            if (rooms == 1) {
                while (!expandGate_end(expanable[Random.Range(0, expanable.Count)])) { }
                rooms--;
                break;
            }
            expandGate(expanable[Random.Range(0, expanable.Count)]);
            rooms--;
        }

    }

    Room getRoomWithID(int id) {
        foreach (Room room in allRoom) {
            if (room.id == id) return room;
        }
        return null;
    }
    
    public void switchRoom(int direction) {
        Room playerRoom = getRoomWithID(playerInRoom);
        Gate throughGate = playerRoom.gates[direction];
        getRoomWithID(playerInRoom).myScene.SetActive(false);

        playerInRoom = throughGate.expandRoomId;
        getRoomWithID(playerInRoom).myScene.SetActive(true);
        StartCoroutine(setEnemiesInTime(getRoomWithID(playerInRoom).myScene, true, 0.75f));

        Camera.main.transform.position = throughGate.expandRoom+new Vector3(0f,0f,-10f);
        Debug.Log("player in room NO." + playerInRoom);
    }
}
