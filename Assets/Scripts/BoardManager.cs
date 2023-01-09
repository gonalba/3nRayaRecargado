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

        #region Inicializacion tablero
        // Inicializamos el array board 
        for (int y = 0; y < dim; y++)
            for (int x = 0; x < dim; x++)
            {
                // Instanciamos el prefab y lo hacemos hijo del boardmanager
                renderBoard[y, x] = Instantiate(simpleBoardPrefab);
                renderBoard[y, x].transform.SetParent(transform);

                // calculamos la posicion (posicion en el tablero + espacio entre casillas)
                float xpos = (x * dim) + (interOffset * x);
                float ypos = (y * dim) + (interOffset * y);
                renderBoard[y, x].transform.localPosition = new Vector3(xpos, ypos, 0);
                //board[y, x].transform.localScale = scaleV;
            }
        #endregion
    }

    float scaleFactorW, scaleFactorH, _scaleFactor;

    /// <summary>
    /// Aplica al boardManager un escalado y una transformacion segun la resolucion de la pantalla
    /// </summary>
    private void MapRescaling()
    {
        // resolucion de la pantalla en pixeles
        int screenPixelsWidth = Screen.width;
        int screenPixelsHeight = Screen.height;

        // resolucion de la pantalla en unidades de Unity
        float screenUnityHeight = Camera.main.orthographicSize * 2;
        float screenUnityWidth = (screenPixelsWidth * screenUnityHeight) / screenPixelsHeight;

        // tamaño del board tanto ancho como alto (3*3 + separacion entre casillas)
        float boardSize = (dim * dim) + (2 * interOffset);
        Debug.Log("W: " + screenPixelsWidth + " H: " + screenPixelsHeight);

        // calculamos el factor de escala al que hay que escalar
        scaleFactorW = screenUnityWidth / boardSize;
        scaleFactorH = screenUnityHeight / boardSize;
        _scaleFactor = Mathf.Min(scaleFactorW, scaleFactorH);

        float xs = gameObject.transform.localScale.x * _scaleFactor;
        float ys = gameObject.transform.localScale.y * _scaleFactor;
        float zs = gameObject.transform.localScale.z * _scaleFactor;

        gameObject.transform.localScale = new Vector3(xs, ys, zs);

        // Centramos la cámara
        float pos = (boardSize - 1) / 2;
        Camera.main.transform.position = new Vector3(pos * _scaleFactor,
            pos * _scaleFactor, Camera.main.transform.position.z);
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


        // normalizamos las coordenadas de la casilla para que entre dentro del rango 3x3
        xCell %= dim;
        yCell %= dim;


        // si podemos colocar la ficha 
        if (dimLogicBoard.TryFillCellByOnePlayer(yBoard, xBoard, yCell, xCell, player))
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
            return true;
        }

        return false;
    }

    int countaux = 0;
    void Update()
    {
        if (countaux++ == 0)
        {
            // escalado del tablero
            MapRescaling();
        }

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
