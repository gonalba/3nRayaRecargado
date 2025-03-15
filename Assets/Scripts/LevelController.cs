using Board.Graphics;
using Board.Logic;
using IA;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static readonly int DIM = 3;

    public bool _simpleGame;
    public bool _hardDifficult;

    public Skin.SkinPackage skinPackage;

    private AIPlayer_SimpleBoard AiPlayer;


    // id del jugador al que le toca jugar (1,2)
    private int _turn;
    // cuenta del turno (0,1)
    private int _turnCount;

    private DimensionalTicTacToe _boardOfDimensionalGame;
    private int _currentBoardCol = -1;
    private int _currentBoardRow = -1;


    private SimpleTicTacToe _boardOfSimpleGame;

    public DimensionalVisualBoard dimensionalBoardPrefab;
    private DimensionalVisualBoard _dimensionalVisualBoard;

    public SimpleVisualBoard simpleBoardPrefab;
    private SimpleVisualBoard _simpleVisualBoard;

    public MapScaling mapScaling;




    #region Unity implementation

    // Start is called before the first frame update
    void Start()
    {
        InitBoard();

        _turnCount = 1;// Random.Range(0, 2);
        _turn = _turnCount + 1;

        Debug.Log("Turno: " + _turn);

        if (mapScaling == null)
            Debug.LogError("mapScaling no inicializado: " + mapScaling);

        mapScaling.boardSize = _simpleGame ? DIM : (DIM * DIM) + (2 * _dimensionalVisualBoard.boardsOffset);

        //AiPlayer = new AIPlayer_SimpleBoard(this, player2Id, player1Id, _hardDifficult);
    }
    private void InitBoard()
    {
        if (!_simpleGame)
        {
            _boardOfDimensionalGame = new DimensionalTicTacToe();
            Debug.Log("Asignamos el skin");
            dimensionalBoardPrefab.skin = skinPackage;

            _dimensionalVisualBoard = Instantiate(dimensionalBoardPrefab);
            _dimensionalVisualBoard.transform.SetParent(transform);
            _dimensionalVisualBoard.AddClickListenerToCells(CellClicked);
        }
        else
        {
            _boardOfSimpleGame = new SimpleTicTacToe();

            Debug.Log("Asignamos el skin");
            simpleBoardPrefab.skin = skinPackage;

            // Instanciamos el prefab y lo hacemos hijo del boardmanager
            _simpleVisualBoard = Instantiate(simpleBoardPrefab);
            _simpleVisualBoard.transform.SetParent(transform);
            _simpleVisualBoard.row = 0;
            _simpleVisualBoard.col = 0;
            _simpleVisualBoard.AddClickListenerToCells(CellClicked);
        }
    }

    public void CellClicked(VisualCell cell)
    {
        bool isTurnCorrect = PlayerTurn(cell.row, cell.col, cell.board.row, cell.board.col, _turn);
        if (isTurnCorrect)
        {
            _turnCount = ++_turnCount % 2;
            _turn = _turnCount + 1;
        }
    }
    #endregion


    /// <summary>
    /// Este método coloca una ficha del jugador en la casilla con las coordenadas pasadas por parámetro.
    /// </summary>
    /// <param name="cellRow">Fila de la casilla</param>
    /// <param name="cellCol">Columna de la casilla</param>
    /// <param name="boardRow">Fila del tablero</param>
    /// <param name="boardCol">Columna del tablero</param>
    /// <param name="player">0=player1 | 1=player2</param>
    /// <returns>
    /// Devuelve TRUE si ha conseguido poner la ficha, FALSE en caso contrario
    /// </returns>
    private bool PlayerTurn(int cellRow, int cellCol, int boardRow, int boardCol, int playerId)
    {
        CellContent player;
        if (playerId == 1) player = CellContent.PLAYER1; else player = CellContent.PLAYER2;

        // si las coordenadas del tablero (xBoard, yBoard) son distintas del tablero
        // donde hay que poner ficha (currentXBoard, currentYBoard), exceptuando si es
        // el primer movimiento (currentXBoard, currentYBoard) = (-1, -1); y 
        // si las coordenadas de la casilla se salen de la dimension del tablero hacemos return
        if ((_currentBoardCol != boardCol || _currentBoardRow != boardRow) &&
            (_currentBoardCol != -1 || _currentBoardRow != -1) ||
            cellCol < 0 || cellCol > DIM || cellRow < 0 || cellRow > DIM)
            return false;


        // (tablero dimensional) si podemos colocar la ficha 
        if (!_simpleGame && _boardOfDimensionalGame.TryFillCellByOnePlayer(boardRow, boardCol, cellRow, cellCol, player))
        {
            // la colocamos en la celda correspondiente
            _dimensionalVisualBoard.ChangeCellToPlayer(boardRow, boardCol, cellRow, cellCol, playerId);


            // Cambiamos el color a azul del tablero actual si se ha ganado o empate.
            // A blanco si se puede seguir poniendo fichas
            CellContent whoWin = _boardOfDimensionalGame.WhoWinInSimpleBoard(boardRow, boardCol);
            Debug.Log("");
            if (whoWin != CellContent.EMPTY)
            {
                if (whoWin == CellContent.PLAYER1) _dimensionalVisualBoard.ChangeColor(boardRow, boardCol, Color.blue);
                else if (whoWin == CellContent.PLAYER2) _dimensionalVisualBoard.ChangeColor(boardRow, boardCol, Color.red);
                else _dimensionalVisualBoard.ChangeColor(boardRow, boardCol, new Color(1, 0.72f, 0.31f, 1));
            }
            else
                _dimensionalVisualBoard.ChangeColor(boardRow, boardCol, Color.white);

            if (_boardOfDimensionalGame.WhoWinInSimpleBoard(cellRow, cellCol) != CellContent.EMPTY)
            {
                _currentBoardCol = -1;
                _currentBoardRow = -1;
            }
            else
            {
                _dimensionalVisualBoard.ChangeColor(cellRow, cellCol, Color.green);

                _currentBoardCol = cellCol;
                _currentBoardRow = cellRow;
            }
            return true;
        }
        else if (_simpleGame && _boardOfSimpleGame.WhoWin() != CellContent.EMPTY || _boardOfSimpleGame.WhoWin() != CellContent.TIE 
            && _boardOfSimpleGame.TryFillCellByPlayer(cellRow, cellCol, player))
        {
            // la colocamos en la celda correspondiente
            _simpleVisualBoard.ChangeCellToPlayer(cellRow, cellCol, playerId);

            CellContent winner = _boardOfSimpleGame.WhoWin();
            if (winner == CellContent.PLAYER1) _simpleVisualBoard.ChangeColor(Color.blue);//todo: integrate colors in the scriptableObject
            else if (winner == CellContent.PLAYER2) _simpleVisualBoard.ChangeColor(Color.red);
            else if (winner == CellContent.TIE) _simpleVisualBoard.ChangeColor(new Color(1, 0.72f, 0.31f, 1));

            return true;
        }

        return false;
    }


    /// <summary>
    /// Método que selecciona el tablero logico. Si estamos jugando al dimensional, selecciona el tablero que toque. 
    /// Si hay que elegir, elige uno al azar
    /// </summary>
    /// <param name="board">Parametros de salida que contiene las coordenadas correspondientes al tablero</param>
    /// <returns>
    /// Devuelve el tablero logico si es una partida de tres en raya normal. 
    /// Si es dimensional entonces devuelve el tablero simple actual y si no hay ninguno marcado, devuelve uno aleatorio
    /// </returns>
    public SimpleTicTacToe GetLogicBoard(out Vector2 board)
    {
        board.x = 0;
        board.y = 0;

        // si estamos en el tablero simple, lo devolvemos
        if (_simpleGame)
            return _boardOfSimpleGame;
        // si estamos en tablero dimensional, devolvemos el tablero activo actualmente
        else if (_currentBoardCol > -1 && _currentBoardRow > -1)
        {
            board.x = _currentBoardCol;
            board.y = _currentBoardRow;
            return _boardOfDimensionalGame.GetLogicBoard(_currentBoardRow, _currentBoardCol);
        }

        // si no hay ningun tablero activo lo elegimos al azar
        if (_boardOfDimensionalGame.WhoWin() == CellContent.EMPTY)
        {
            int col, row;
            int aux1 = Random.Range(0, DIM);
            int aux2 = Random.Range(0, DIM);

            for (row = (aux1 + 1) % DIM; row != aux1; row = (row + 1) % DIM)
                for (col = (aux2 + 1) % DIM; col != aux2; col = (col + 1) % DIM)
                {
                    if (_boardOfDimensionalGame.GetLogicBoard(row, col).WhoWin() == CellContent.EMPTY)
                    {
                        board.x = col;
                        board.y = row;
                        return _boardOfDimensionalGame.GetLogicBoard(row, col);
                    }
                }
        }

        return null;
    }


    public CellContent WhoWin()
    {
        if (_simpleGame)
            return _boardOfSimpleGame.WhoWin();
        return _boardOfDimensionalGame.WhoWin();
    }
}
