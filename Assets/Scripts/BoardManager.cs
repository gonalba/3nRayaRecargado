using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using static UnityEditor.PlayerSettings;

//[ExecuteAlways]
public class BoardManager : MonoBehaviour
{
    private float interOffset = 0.5f;

    public SimpleRenderBoard simpleBoardPrefab;


    private int currentXBoard = -1;
    private int currentYBoard = -1;

    static int dim = 3;

    private SimpleRenderBoard[,] renderBoard = new SimpleRenderBoard[dim, dim];

    private DimensionalLogicBoard dimLogicBoard;

    private int turn;
    private int turnCount;



    // Start is called before the first frame update
    void Start()
    {
        dimLogicBoard = new DimensionalLogicBoard();

        turnCount = Random.Range(0, 2);
        turn = turnCount + 1;

        #region Escalado del tablero
        // escalamos tablero a la resolucion de la pantalla
        // cogemos resolucion
        int w = Screen.currentResolution.width;
        int h = Screen.currentResolution.height;

        float boardSize = (dim * dim) + (2 * interOffset);
        float lessSize;
        Debug.Log("W: " + w + " H: " + h);
        // averiguamos si esta apaisado o en vertical (w>h o h>w)
        if (w <= h)
        {
            // unidades de unity de la vertical
            lessSize = Camera.main.orthographicSize * 2;
        }
        else
        {   // unidades de unity de la horizontal
            lessSize = (Camera.main.orthographicSize * 2 * h) / w;
        }

        float scale = boardSize / lessSize;
        //float offset = -scale + 1;

        Debug.Log("scale: " + scale + " - lessSize: " + lessSize + " - boardSize: " + boardSize);

        float xs = gameObject.transform.localScale.x / scale;
        float ys = gameObject.transform.localScale.y / scale;
        float zs = gameObject.transform.localScale.z / scale;

        Vector3 scaleV = new Vector3(xs, ys, zs);

        //gameObject.transform.localScale = scaleV;
        #endregion

        #region Inicializacion tablero
        // Inicializamos el array board 
        for (int y = 0; y < dim; y++)
            for (int x = 0; x < dim; x++)
            {
                renderBoard[y, x] = Instantiate(simpleBoardPrefab);
                renderBoard[y, x].transform.SetParent(transform);
                float xpos = (x * dim) + (interOffset * x) - interOffset;
                float ypos = (y * dim) + (interOffset * y) - interOffset;
                renderBoard[y, x].transform.localPosition = new Vector3(xpos, ypos, 0);
                //board[y, x].transform.localScale = scaleV;
            }
        #endregion
    }



    /// <summary>
    /// Este método coloca una ficha del jugador en la casilla con las coordenadas pasadas por parámetro.
    /// No pasamos las coordenadas del tablero porque solo podemos colocar casillas en el tablero actual.
    /// </summary>
    /// <param name="xCell">Coordenada x de la casilla</param>
    /// <param name="yCell">Coordenada y de la casilla</param>
    /// <param name="player">0=player1 | 1=player2</param>
    private bool PlayerTurn(int xCell, int yCell, int xBoard, int yBoard, int player)
    {
        // si las coordenadas del tablero (xBoard, yBoard) son distintas del tablero
        // donde hay que poner ficha (currentXBoard, currentYBoard), exceptuando si es
        // el primer movimiento (currentXBoard, currentYBoard) = (-1, -1); y 
        // si las coordenadas de la casilla se salen de la dimension del tablero hacemos return
        if ((currentXBoard != xBoard || currentYBoard != yBoard) &&
            (currentXBoard != -1 || currentYBoard != -1) ||
            xCell < 0 || xCell > dim || yCell < 0 || yCell > dim)
            return false;


        //Debug.Log("X: " + xCell + " " + "Y: " + yCell);

        //Debug.Log("X Board: " + currentXBoard + " " + "Y Board: " + currentYBoard);

        // normalizamos las coordenadas de la casilla para que entre dentro del rango 3x3
        xCell %= dim;
        yCell %= dim;

        // vemos si se puede poner una ficha en la celda clicada
        bool aux = dimLogicBoard.TryFillCellByOnePlayer(yBoard, xBoard, yCell, xCell, player);


        // si podemos colocar la ficha 
        if (aux)
        {
            // la colocamos en la celda correspondiente
            renderBoard[yBoard, xBoard].ChangeCellToPlayer(yCell, xCell, player);


            // Cambiamos el color a azul del tablero actual si se ha ganado o empate.
            // A blanco si se puede seguir poniendo fichas
            if (dimLogicBoard.WhoWinInSimpleBoard(xBoard, yBoard) >= 0)
                renderBoard[yBoard, xBoard].ChangeColor(Color.blue);
            else
                renderBoard[yBoard, xBoard].ChangeColor(Color.white);

            if (dimLogicBoard.WhoWinInSimpleBoard(xCell, yCell) >= 0)
            {
                currentXBoard = -1;
                currentYBoard = -1;
            }
            else
            {
                renderBoard[yCell, xCell].ChangeColor(Color.green);

                currentXBoard = xCell;
                currentYBoard = yCell;
            }
        }

        return aux;
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100.0f);

            Vector2 cellCoords = new Vector2(-1, -1);
            Vector2 boardCoords = new Vector2(-1, -1);


            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hitInfo = hits[i];
                SimpleRenderBoard sb = hitInfo.transform.GetComponent<SimpleRenderBoard>();
                Cell c = hitInfo.transform.GetComponent<Cell>();

                if (sb)
                {
                    for (int j = 0; j < dim; j++)
                    {
                        for (int k = 0; k < dim; k++)
                        {
                            if (renderBoard[j, k].Equals(sb))
                            {
                                boardCoords = new Vector2(k, j);
                            }
                        }
                    }
                }
                else if (c)
                {
                    cellCoords = c.transform.localPosition;
                }

            }

            if (boardCoords.x != -1 && boardCoords.y != -1 && cellCoords.x != -1 && cellCoords.y != -1)
            {

                int xc = (int)cellCoords.x, yc = (int)cellCoords.y;
                int xb = (int)boardCoords.x, yb = (int)boardCoords.y;

                if (PlayerTurn(xc, yc, xb, yb, turn))
                {
                    turnCount = ++turnCount % 2;
                    turn = turnCount + 1;
                    Debug.Log(turn);
                }
            }
        }
    }//fin update
}
