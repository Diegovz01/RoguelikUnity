using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour
{
    public string nombre;

    protected virtual void Awake() {
        Anda();
        Habla();
    }

    // virtual => Se añanden a Metodos / propiedades 
    //para ser cambiados en clases que extiendan / Hereden

    private void Anda()
    {
        Debug.Log("El " + nombre + " anda.");
    }

    protected abstract void Habla();
}
