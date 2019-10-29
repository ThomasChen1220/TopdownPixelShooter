using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Projectile
{
    private void Start()
    {
        pushable = false;
    }

    private void Update()
    {
        move();
    }
}
