using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SimpleBoard es la clase engargada de guardar el estado un tablero 3x3. 
/// Puedes rellenar una casilla por un juador y consultar quien ha ganado.
/// </summary>
public class SimpleLogicBoard
{
    /// <summary>
    /// Id del jugador ganador. Es -1 si no ha ganado nadie y 0 si el resultado es empate.
    /// </summary>
    private int _playerWin = -1;

    /// <summary>
    /// Este array representa el tablero lógico 3x3
    /// </summary>
    private int[,] _board = new int[BoardManager.DIM(), BoardManager.DIM()];




    public SimpleLogicBoard()
    {
        // Inicializamos el array board 
        for (int y = 0; y < BoardManager.DIM(); y++)
            for (int x = 0; x < BoardManager.DIM(); x++)
                _board[y, x] = 0;
    }





    /// <summary>
    /// Comprueba si hay un ganador en el tablero actual.
    /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
    /// </summary>
    /// <returns> 
    /// Devuelve el id del jugados que ha ganado. Si hay empate devuelve 0 y si todavia se puede seguir jugando devuelve -1
    /// </returns>
    private int checkWinner()
    {
        // Si en alguna fila todas las casillas son iguales y no vacías
        for (int fila = 0; fila < BoardManager.DIM(); fila++)
            if ((_board[fila, 0] == _board[fila, 1])
                    && (_board[fila, 0] == _board[fila, 2])
                    && (_board[fila, 0] != 0))
            {
                return _board[fila, 0];
            }

        // Lo mismo para las columnas
        for (int columna = 0; columna < BoardManager.DIM(); columna++)
            if ((_board[0, columna] == _board[1, columna])
                    && (_board[0, columna] == _board[2, columna])
                    && (_board[0, columna] != 0))
            {
                return _board[0, columna];
            }

        // Y finalmente miro las dos diagonales
        if ((_board[0, 0] == _board[1, 1])
                && (_board[0, 0] == _board[2, 2])
                && (_board[0, 0] != 0))
        {
            return _board[0, 0];
        }
        else if ((_board[0, 2] == _board[1, 1])
                && (_board[0, 2] == _board[2, 0])
                && (_board[0, 2] != 0))
        {
            return _board[0, 2];
        }

        // Miro si ha habido empate: si hay alguna casilla vacia, entonces no hay empate
        for (int i = 0; i < BoardManager.DIM(); i++)
            for (int j = 0; j < BoardManager.DIM(); j++)
            {
                if (_board[i, j] == 0)
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
    public bool TryFillCellByOnePlayer(int y, int x, int pId)
    {
        if (_board[y, x] != 0)
            return false;

        _board[y, x] = pId;

        _playerWin = checkWinner();

        return true;
    }


    /// <summary>
    /// Método que sirve para consultar quien ha ganado el board.
    /// </summary>
    /// <returns>
    /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
    /// El valor es -1 si todavía no ha acabado la partida.
    /// </returns>
    public int WhoWin() { return _playerWin; }


    public int GetValueofCell(int x, int y) { return _board[y, x]; }
}
