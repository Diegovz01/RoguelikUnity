using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _BoardManager : MonoBehaviour
{
    public int columns = 8;
    public int rows = 8;

    public GameObject[] floorTiles, outerWallTiles, wallTiles, foodTiles, enemyTiles; // Losetas
    public GameObject exit;
    Transform boardHolder;
    List<Vector2> gridPositions = new List<Vector2>();

    void InitializeList()
    {
        gridPositions.Clear(); // Limpiar lista 

        for (int x = 1; x < columns -1; x++)
        {
            for (int y = 1; y < rows -1; y++)
            {
                gridPositions.Add(new Vector2(x, y));
            }
        }
    }

    Vector2 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count); // Count => Largo en List
        Vector2 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex); // Removemos de la lista (Evitamos que se encime)
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1); // +1 => para incluir valor maximo en Range(); 
        for (int i = 0; i < objectCount; i++)
        {
            Vector2 randomPosition = RandomPosition();
            GameObject tileChoice = GetRandomInArray(tileArray);
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetUpScene(int level)
    {
        BoardSetup();
        InitializeList();
        LayoutObjectAtRandom(wallTiles, 5, 9);
        LayoutObjectAtRandom(foodTiles, 1, 5);
        int enemyCount = (int)Mathf.Log(level, 2); // Función logaritmica
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(exit, new Vector2(columns - 1, rows -1), Quaternion.identity);
    }

    private void BoardSetup()
    {
        boardHolder = new GameObject("Board").transform; // Creamos GameObject, necesitamos acceder a su Transform
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = - 1; y < rows + 1; y++)
            {
                GameObject toInstantiate;

                if(x == -1 || y == -1 || x == columns || y == rows)
                {
                    toInstantiate = GetRandomInArray(outerWallTiles); // Obtenemos el objeto random.
                }else
                {
                    toInstantiate = GetRandomInArray(floorTiles);
                }
                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y), Quaternion.identity); // Intanciamos el objeto random.
                instance.transform.SetParent(boardHolder);
            }
        }
    }

    // No repetir codigo.
    GameObject GetRandomInArray(GameObject[] array)
    {
        return array[Random.Range(0, array.Length)]; // Length => nos ayuda con el largo del array
    }
}
