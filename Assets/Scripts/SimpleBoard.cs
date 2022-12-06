using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBoard : MonoBehaviour
{
    static int dim = 3;

    public GameObject cellPrefab;

    private GameObject[,] board = new GameObject[dim, dim];


    // Start is called before the first frame update
    void Start()
    {
        for (int y = 0; y < dim; y++)
            for (int x = 0; x < dim; x++)
            {
                board[y, x] = Instantiate(cellPrefab);
                board[y, x].transform.SetParent(transform);
                board[y, x].transform.localPosition = new Vector3(x, y, 0);
            }
    }
}
