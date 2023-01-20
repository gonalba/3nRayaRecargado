using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DimensionalBoard es una clase que guarda el estado de un tablero dimensional 3x3 (tablero de tableros simples)
/// </summary>
public class DimensionalLogicBoard
{
    /// <summary>
    /// Id del jugador ganador. Es -1 si no ha ganado nadie y 0 si el resultado es empate.
    /// </summary>
    private int _playerWin = -1;

    /// <summary>
    /// Este array representa el tablero lógico 3x3
    /// </summary>
    private SimpleLogicBoard[,] _board = new SimpleLogicBoard[LevelManager.DIM(), LevelManager.DIM()];




    public DimensionalLogicBoard()
    {
        // Inicializamos el array board 
        for (int y = 0; y < LevelManager.DIM(); y++)
            for (int x = 0; x < LevelManager.DIM(); x++)
                _board[y, x] = new SimpleLogicBoard();
    }


    /// <summary>
    /// Comprueba si hay un ganador actual.
    /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
    /// </summary>
    /// <returns> 
    /// Devuelve el id del jugados que ha ganado. Si hay empate devuelve 0 y si todavia se puede seguir jugando devuelve -1
    /// </returns>
    private int checkWinner()
    {
        // Si en alguna fila todas las casillas son iguales y no vacías
        for (int fila = 0; fila < LevelManager.DIM(); fila++)
            if ((_board[fila, 0].WhoWin() == _board[fila, 1].WhoWin())
                    && (_board[fila, 0].WhoWin() == _board[fila, 2].WhoWin())
                    && (_board[fila, 0].WhoWin() > 0))
            {
                return _board[fila, 0].WhoWin();
            }

        // Lo mismo para las columnas
        for (int columna = 0; columna < LevelManager.DIM(); columna++)
            if ((_board[0, columna].WhoWin() == _board[1, columna].WhoWin())
                    && (_board[0, columna].WhoWin() == _board[2, columna].WhoWin())
                    && (_board[0, columna].WhoWin() > 0))
            {
                return _board[0, columna].WhoWin();
            }

        // Y finalmente miro las dos diagonales
        if ((_board[0, 0].WhoWin() == _board[1, 1].WhoWin())
                && (_board[0, 0].WhoWin() == _board[2, 2].WhoWin())
                && (_board[0, 0].WhoWin() > 0))
            return _board[0, 0].WhoWin();
        else if ((_board[0, 2].WhoWin() == _board[1, 1].WhoWin())
                && (_board[0, 2].WhoWin() == _board[2, 0].WhoWin())
                && (_board[0, 2].WhoWin() > 0))
            return _board[0, 2].WhoWin();

        for (int i = 0; i < LevelManager.DIM(); i++)
            for (int j = 0; j < LevelManager.DIM(); j++)
            {
                if (_board[i, j].WhoWin() == -1)
                    return -1;
            }

        return 0;
    }


    /// <summary>
    /// Player p try to fill the cell with coords xy 
    /// </summary>
    /// <param name="x">Column</param>
    /// <param name="y">Row</param>
    /// <param name="pId">Player id</param>
    /// <returns>
    /// True if player fill the cell xy. False otherwise
    /// </returns>
    public bool TryFillCellByOnePlayer(int ySimpleBorad, int xSimpleBoard, int yCell, int xCell, int pId)
    {
        if (_board[ySimpleBorad, xSimpleBoard].WhoWin() != -1 || WhoWin() != -1)
            return false;

        bool aux = _board[ySimpleBorad, xSimpleBoard].TryFillCellByOnePlayer(yCell, xCell, pId);

        _playerWin = checkWinner();

        return aux;
    }

    /// <summary>
    /// Método que sirve para consultar quien ha ganado la partida.
    /// </summary>
    /// <returns>
    /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
    /// El valor es -1 si todavía no ha acabado la partida.
    /// </returns>
    public int WhoWin() { return _playerWin; }


    /// <summary>
    /// Método que sirve para consultar quien ha ganado en un board especifico.
    /// </summary>
    /// <returns>
    /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
    /// El valor es -1 si todavía no ha acabado la partida.
    /// </returns>
    public int WhoWinInSimpleBoard(int x, int y)
    {
        return _board[y, x].WhoWin();
    }

    public SimpleLogicBoard GetLogicBoard(int x, int y) { return _board[y, x]; }
}
