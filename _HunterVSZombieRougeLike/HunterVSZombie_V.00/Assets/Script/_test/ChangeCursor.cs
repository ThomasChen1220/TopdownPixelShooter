using UnityEngine;
using System.Collections;

public class ChangeCursor : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite defaultCursor;

    private void Start()
    {
        Cursor.visible = false;
    }
}