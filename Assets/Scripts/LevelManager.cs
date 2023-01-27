using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static int _DIM = 3;
    public static int DIM() { return _DIM; }


    public bool _simpleGame;
    public bool _hardDifficult;

    private AIPlayer_SimpleBoard AIPlayer;


    // id del jugador al que le toca jugar (1,2)
    private int _turn;
    // cuenta del turno (0,1)
    private int _turnCount;
    // Id del jugador
    private int player1Id;
    private int player2Id;


    #region Dimensional game logic board
    private DimensionalLogicBoard _dimLogicBoard;
    private int _cbCol = -1;
    private int _cbRow = -1;
    #endregion


    #region Simple game logic board
    private SimpleLogicBoard _logicBoardSimpleGame;
    #endregion


    #region Render board

    #region Scale attributes
    private float _scaleFactorW;
    private float _scaleFactorH;
    private float _scaleFactor;
    private int _screenPixelsWidth;
    private int _screenPixelsHeight;
    #endregion

    // Simple game render
    private SimpleRenderBoard _renderBoardSimpleGame;

    public SimpleRenderBoard simpleBoardPrefab;
    private SimpleRenderBoard[,] _renderBoard = new SimpleRenderBoard[DIM(), DIM()];

    // espacio que hay entre tableros
    private float _interOffset = 0.5f;

    /// <summary>
    /// Aplica al boardManager un escalado y una transformacion segun la resolucion de la pantalla
    /// </summary>
    private void MapRescaling()
    {
        // resolucion de la pantalla en pixeles
        _screenPixelsWidth = Screen.width;
        _screenPixelsHeight = Screen.height;

        // resolucion de la pantalla en unidades de Unity
        float screenUnityHeight = Camera.main.orthographicSize * 2;
        float screenUnityWidth = (_screenPixelsWidth * screenUnityHeight) / _screenPixelsHeight;

        // tamaño del board tanto ancho como alto (3*3 + separacion entre casillas)
        float boardSize = _simpleGame ? DIM() : (DIM() * DIM()) + (2 * _interOffset);
        Debug.Log("W: " + _screenPixelsWidth + " H: " + _screenPixelsHeight);

        // calculamos el factor de escala al que hay que escalar
        _scaleFactorW = screenUnityWidth / boardSize;
        _scaleFactorH = screenUnityHeight / boardSize;
        _scaleFactor = Mathf.Min(_scaleFactorW, _scaleFactorH);

        gameObject.transform.localScale = new Vector3(_scaleFactor, _scaleFactor, _scaleFactor);

        // Centramos la cámara
        float pos = (boardSize - 1) / 2;
        Camera.main.transform.position = new Vector3(pos * _scaleFactor,
            pos * _scaleFactor, Camera.main.transform.position.z);
    }
    #endregion


    #region Unity implementation

    // Start is called before the first frame update
    void Start()
    {

        if (!_simpleGame)
        {
            _dimLogicBoard = new DimensionalLogicBoard();

            #region Inicializacion tablero
            // Inicializamos el array board 
            for (int y = 0; y < DIM(); y++)
                for (int x = 0; x < DIM(); x++)
                {
                    // Instanciamos el prefab y lo hacemos hijo del boardmanager
                    _renderBoard[y, x] = Instantiate(simpleBoardPrefab);
                    _renderBoard[y, x].transform.SetParent(transform);

                    // calculamos la posicion (posicion en el tablero + espacio entre casillas)
                    float xpos = (x * DIM()) + (_interOffset * x);
                    float ypos = (y * DIM()) + (_interOffset * y);
                    _renderBoard[y, x].transform.localPosition = new Vector3(xpos, ypos, 0);
                }
            #endregion

        }
        else
        {
            _logicBoardSimpleGame = new SimpleLogicBoard();

            // Instanciamos el prefab y lo hacemos hijo del boardmanager
            _renderBoardSimpleGame = Instantiate(simpleBoardPrefab);
            _renderBoardSimpleGame.transform.SetParent(transform);
        }

        _turnCount = Random.Range(0, 2);
        _turn = _turnCount + 1;

        Debug.Log("Turno: " + _turn);

        player1Id = 1;
        player2Id = 2;

        AIPlayer = new AIPlayer_SimpleBoard(this, player2Id, player1Id, _hardDifficult);
    }


    void Update()
    {
        if (_screenPixelsWidth != Screen.width || _screenPixelsHeight != Screen.height)
        {
            // escalado del tablero
            MapRescaling();
        }

        Vector2 cellCoords = new Vector2(-1, -1);
        Vector2 boardCoords = new Vector2(-1, -1);

        if (_turn == player1Id && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, 100.0f);

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit hitInfo = hits[i];
                SimpleRenderBoard sb = hitInfo.transform.GetComponent<SimpleRenderBoard>();
                RenderCell c = hitInfo.transform.GetComponent<RenderCell>();

                if (sb)
                {
                    if (!_simpleGame)
                        for (int row = 0; row < LevelManager.DIM(); row++)
                        {
                            for (int col = 0; col < LevelManager.DIM(); col++)
                            {
                                if (_renderBoard[row, col].Equals(sb))
                                {
                                    boardCoords = new Vector2(col, row);
                                }
                            }
                        }
                    else
                        boardCoords.x = boardCoords.y = 0;
                }
                else if (c)
                {
                    cellCoords = c.transform.localPosition;
                }
            }
        }
        else if (_turn == player2Id)
        {
            Vector2 bCoords = AIPlayer.SelectSimpleBoard();
            boardCoords.x = bCoords.x;
            boardCoords.y = bCoords.y;

            Vector2 coords = AIPlayer.SelectCoords();
            cellCoords.x = coords.x;
            cellCoords.y = coords.y;
        }


        if (boardCoords.x != -1 && boardCoords.y != -1 && cellCoords.x != -1 && cellCoords.y != -1)
        {
            int cC = (int)cellCoords.x, rC = (int)cellCoords.y;
            int cB = (int)boardCoords.x, rB = (int)boardCoords.y;

            if (PlayerTurn(rC, cC, rB, cB, _turn))
            {
                _turnCount = ++_turnCount % 2;
                _turn = _turnCount + 1;
            }
        }
    }//fin update
    #endregion


    /// <summary>
    /// Este método coloca una ficha del jugador en la casilla con las coordenadas pasadas por parámetro.
    /// </summary>
    /// <param name="cRow">Fila de la casilla</param>
    /// <param name="cCol">Columna de la casilla</param>
    /// <param name="bRow">Fila del tablero</param>
    /// <param name="bCol">Columna del tablero</param>
    /// <param name="player">0=player1 | 1=player2</param>
    /// <returns>
    /// Devuelve TRUE si ha conseguido poner la ficha, FALSE en caso contrario
    /// </returns>
    private bool PlayerTurn(int cRow, int cCol, int bRow, int bCol, int player)
    {
        // si las coordenadas del tablero (xBoard, yBoard) son distintas del tablero
        // donde hay que poner ficha (currentXBoard, currentYBoard), exceptuando si es
        // el primer movimiento (currentXBoard, currentYBoard) = (-1, -1); y 
        // si las coordenadas de la casilla se salen de la dimension del tablero hacemos return
        if ((_cbCol != bCol || _cbRow != bRow) &&
            (_cbCol != -1 || _cbRow != -1) ||
            cCol < 0 || cCol > DIM() || cRow < 0 || cRow > DIM())
            return false;


        // si podemos colocar la ficha 
        if (!_simpleGame && _dimLogicBoard.TryFillCellByOnePlayer(bRow, bCol, cRow, cCol, player))
        {
            // la colocamos en la celda correspondiente
            _renderBoard[bRow, bCol].ChangeCellToPlayer(cRow, cCol, player);


            // Cambiamos el color a azul del tablero actual si se ha ganado o empate.
            // A blanco si se puede seguir poniendo fichas
            int whoWin = _dimLogicBoard.WhoWinInSimpleBoard(bCol, bRow);
            if (whoWin >= 0)
            {
                if (whoWin == 1) _renderBoard[bRow, bCol].ChangeColor(Color.blue);
                else if (whoWin == 2) _renderBoard[bRow, bCol].ChangeColor(Color.red);
                else _renderBoard[bRow, bCol].ChangeColor(new Color(1, 0.72f, 0.31f, 1));
            }
            else
                _renderBoard[bRow, bCol].ChangeColor(Color.white);

            if (_dimLogicBoard.WhoWinInSimpleBoard(cCol, cRow) >= 0)
            {
                _cbCol = -1;
                _cbRow = -1;
            }
            else
            {
                _renderBoard[cRow, cCol].ChangeColor(Color.green);

                _cbCol = cCol;
                _cbRow = cRow;
            }
            return true;
        }
        else if (_simpleGame && _logicBoardSimpleGame.WhoWin() == -1 && _logicBoardSimpleGame.TryFillCellByOnePlayer(cRow, cCol, player))
        {
            // la colocamos en la celda correspondiente
            _renderBoardSimpleGame.ChangeCellToPlayer(cRow, cCol, player);

            if (_logicBoardSimpleGame.WhoWin() == 1) _renderBoardSimpleGame.ChangeColor(Color.blue);
            else if (_logicBoardSimpleGame.WhoWin() == 2) _renderBoardSimpleGame.ChangeColor(Color.red);
            else if (_logicBoardSimpleGame.WhoWin() == 0) _renderBoardSimpleGame.ChangeColor(new Color(1, 0.72f, 0.31f, 1));

            return true;
        }

        return false;
    }//fin PlayerTurn()


    /// <summary>
    /// Método que selecciona el tablero logico. Si estamos jugando al dimensional, selecciona el tablero que toque. 
    /// Si hay que elegir, elige uno al azar
    /// </summary>
    /// <returns>
    /// Devuelve el tablero logico si es una partida de tres en raya normal. 
    /// Si es dimensional entonces devuelve el tablero simple actual y si no hay ninguno marcado, devuelve uno aleatorio
    /// </returns>
    public SimpleLogicBoard SelectLogicBoard(out Vector2 board)
    {
        board.x = 0;
        board.y = 0;

        if (_simpleGame)
            return _logicBoardSimpleGame;
        else if (_cbCol > -1 && _cbRow > -1)
        {
            board.x = _cbCol;
            board.y = _cbRow;
            return _dimLogicBoard.GetLogicBoard(_cbRow, _cbCol);
        }

        if (_dimLogicBoard.WhoWin() == -1)
        {
            int col, row;
            do
            {
                col = Random.Range(0, DIM());
                row = Random.Range(0, DIM());
            } while (_dimLogicBoard.GetLogicBoard(row, col).WhoWin() != -1);

            board.x = col;
            board.y = row;

            return _dimLogicBoard.GetLogicBoard(row, col);
        }

        return null;
    }


    public int WhoWin()
    {
        if (_simpleGame)
            return _logicBoardSimpleGame.WhoWin();
        return _dimLogicBoard.WhoWin();
    }
}
