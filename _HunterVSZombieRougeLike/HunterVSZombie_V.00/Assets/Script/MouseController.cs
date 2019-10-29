using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;

    private void Start()
    {
        Cursor.visible = false;
        disableCurser();
        cursor = Instantiate(cursor) as GameObject;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        cursor.transform.position = mousePos;
    }
    public void disableCurser() {
        cursor.SetActive(false);
    }
    public void enableCurser() {
        cursor.SetActive(true);
    }
}