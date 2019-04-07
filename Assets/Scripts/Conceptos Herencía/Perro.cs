using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perro : Animal
{
    protected override void Habla()
    {
        Debug.Log("Guau");
    }
}
