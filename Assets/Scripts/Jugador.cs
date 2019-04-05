using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("http://www.google.com")] // URL documento
[DisallowMultipleComponent] // No permite multiples componentes
[RequireComponent(typeof(Rigidbody2D))] 
[RequireComponent(typeof(BoxCollider2D))] // Nos aseguramos de que lo tiene
public class Jugador : MonoBehaviour
{
    [Header("Salud")] // Titulo para agrupar
    [Space(10)] // Genera espacio
    [Range(1, 5)]
    [Tooltip("Vidas del jugador. \nEntre 1 y 5")]
    public int vidas = 3;
    [Range(1, 100)] // uso de Slider
    public int energia = 80;
    [HideInInspector] // Hacer variable publica invisible
    public int maxEnergia = 100;
    [SerializeField] // Hacer variable privada visible
    private bool tieneEspada = false;
    [Header("Otros")] // Titulo para agrupar
    [Multiline(3)] // lineas de texto
    //[TextArea(3, 6)] // lineas de texto(minimo, maximo)
    public string saludo = "¡Hola Gamer! \nJuguemos  \n Aqui ahora";
    BoxCollider2D bc2;
    Rigidbody2D rbd;
    private void Awake() {
        bc2 = GetComponent<BoxCollider2D>();
        rbd = GetComponent<Rigidbody2D>();
    }
}
