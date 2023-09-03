using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// SimpleBoard es la clase engargada de guardar el estado un tablero 3x3. 
/// Puedes rellenar una casilla por un juador y consultar quien ha ganado.
/// </summary>
public class SimpleLogicBoard {
    /// <summary>
    /// Id del jugador ganador. Es -1 si no ha ganado nadie y 0 si el resultado es empate.
    /// </summary>
    private int _playerWin = -1;

    /// <summary>
    /// Este array representa el tablero lógico 3x3
    /// </summary>
    private int[,] _board = new int[LevelManager.DIM(), LevelManager.DIM()];


    public SimpleLogicBoard() {
        // Inicializamos el array board 
        for (int r = 0; r < LevelManager.DIM(); r++)
            for (int c = 0; c < LevelManager.DIM(); c++)
                _board[r, c] = 0;
    }


    /// <summary>
    /// Comprueba si hay un ganador en el tablero actual.
    /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
    /// </summary>
    /// <returns> 
    /// Devuelve el id del jugados que ha ganado. Si hay empate devuelve 0 y si todavia se puede seguir jugando devuelve -1
    /// </returns>
    private int checkWinner() {
        // Si en alguna fila todas las casillas son iguales y no vacías
        for (int row = 0; row < LevelManager.DIM(); row++)
            if ((_board[row, 0] == _board[row, 1]) && (_board[row, 0] == _board[row, 2]) && (_board[row, 0] != 0))
                return _board[row, 0];

        // Lo mismo para las columnas
        for (int col = 0; col < LevelManager.DIM(); col++)
            if ((_board[0, col] == _board[1, col]) && (_board[0, col] == _board[2, col]) && (_board[0, col] != 0)) {
                return _board[0, col];
            }

        // Y finalmente miro las dos diagonales
        if ((_board[0, 0] == _board[1, 1]) && (_board[0, 0] == _board[2, 2]) && (_board[0, 0] != 0))
            return _board[0, 0];
        else if ((_board[0, 2] == _board[1, 1]) && (_board[0, 2] == _board[2, 0]) && (_board[0, 2] != 0))
            return _board[0, 2];

        // Miro si ha habido empate: si hay alguna casilla vacia, entonces no hay empate
        for (int row = 0; row < LevelManager.DIM(); row++)
            for (int col = 0; col < LevelManager.DIM(); col++)
                if (_board[row, col] == 0)
                    return -1;

        return 0;

    }


    /// <summary>
    /// Player p try to fill the cell with coords (row,col) 
    /// </summary>
    /// <param name="row">Cell row</param>
    /// <param name="col">Cell column</param>
    /// <param name="pId">Player id</param>
    /// <returns>
    /// True if player fill the cell (row,col). False otherwise
    /// </returns>
    public bool TryFillCellByOnePlayer(int row, int col, int pId) {
        if (_board[row, col] != 0) return false;

        _board[row, col] = pId;
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


    /// <summary>
    /// Metodo que permite acceder al valor de la celda [row,col]
    /// </summary>
    /// <param name="row">Cell row</param>
    /// <param name="col">Cell column</param>
    /// <returns>
    /// Devuelve un INT que representa el valor de la celda [row,col]
    /// </returns>
    public int GetValueofCell(int row, int col) { return _board[row, col]; }
}
