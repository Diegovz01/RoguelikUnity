using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gato : Animal
{
    protected override void Awake()
    {
        // override => sobrescribir metodos de clases
        // base => llamar metodos de la clase heredada

        Debug.Log("Mensaje 1");
        base.Awake();
        Debug.Log("Mensaje 2");
    }

    protected override void Habla()
    {
        Debug.Log("Miuau");
    }
}
