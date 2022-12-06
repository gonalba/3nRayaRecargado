using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public float interOffset = 0.5f;

    public GameObject simpleBoardPrefab;


    private int currentXBoard;
    private int currentYBoard;

    static int dim = 3;

    private GameObject[,] board = new GameObject[dim, dim];

    private DimensionalLogicBoard dimBoard;


    // Start is called before the first frame update
    void Start()
    {
        dimBoard = new DimensionalLogicBoard();

        // Inicializamos el array board 
        for (int y = 0; y < dim; y++)
            for (int x = 0; x < dim; x++)
            {
                board[y, x] = Instantiate(simpleBoardPrefab);
                board[y, x].transform.SetParent(transform);
                float xpos = (x * dim) + (interOffset * x) - interOffset;
                float ypos = (y * dim) + (interOffset * y) - interOffset;
                board[y, x].transform.localPosition = new Vector3(xpos, ypos, 0);
            }
    }

    public void PlayerTurn(int x, int y, int player)
    {
        dimBoard.TryFillCellByOnePlayer(currentYBoard, currentXBoard, y, x, player);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 cellPosSelected = Input.mousePosition;
            Vector3 pos = Camera.main.ScreenToWorldPoint(cellPosSelected);

            PlayerTurn(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 1);


        }
    }
}
