using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DimensionalBoard es una clase que guarda el estado de un tablero dimensional 3x3 (tablero de tableros simples)
/// </summary>
public class DimensionalLogicBoard {
    /// <summary>
    /// Id del jugador ganador. Es -1 si no ha ganado nadie y 0 si el resultado es empate.
    /// </summary>
    private int _playerWin = -1;

    /// <summary>
    /// Este array representa el tablero lógico 3x3
    /// </summary>
    private SimpleLogicBoard[,] _board = new SimpleLogicBoard[LevelManager.DIM(), LevelManager.DIM()];


    public DimensionalLogicBoard() {
        // Inicializamos el array board 
        for (int r = 0; r < LevelManager.DIM(); r++)
            for (int c = 0; c < LevelManager.DIM(); c++)
                _board[r, c] = new SimpleLogicBoard();
    }


    /// <summary>
    /// Comprueba si hay un ganador actual.
    /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
    /// </summary>
    /// <returns> 
    /// Devuelve el id del jugados que ha ganado. Si hay empate devuelve 0 y si todavia se puede seguir jugando devuelve -1
    /// </returns>
    private int checkWinner() {
        // Si en alguna fila todas las casillas son iguales y no vacías
        for (int row = 0; row < LevelManager.DIM(); row++)
            if ((_board[row, 0].WhoWin() == _board[row, 1].WhoWin()) && (_board[row, 0].WhoWin() == _board[row, 2].WhoWin()) && (_board[row, 0].WhoWin() > 0)) 
                return _board[row, 0].WhoWin();

        // Lo mismo para las columnas
        for (int col = 0; col < LevelManager.DIM(); col++)
            if ((_board[0, col].WhoWin() == _board[1, col].WhoWin()) && (_board[0, col].WhoWin() == _board[2, col].WhoWin()) && (_board[0, col].WhoWin() > 0))
                return _board[0, col].WhoWin();

        // Y finalmente miro las dos diagonales
        if ((_board[0, 0].WhoWin() == _board[1, 1].WhoWin()) && (_board[0, 0].WhoWin() == _board[2, 2].WhoWin()) && (_board[0, 0].WhoWin() > 0))
            return _board[0, 0].WhoWin();
        else if ((_board[0, 2].WhoWin() == _board[1, 1].WhoWin()) && (_board[0, 2].WhoWin() == _board[2, 0].WhoWin()) && (_board[0, 2].WhoWin() > 0))
            return _board[0, 2].WhoWin();

        for (int i = 0; i < LevelManager.DIM(); i++)
            for (int j = 0; j < LevelManager.DIM(); j++) 
                if (_board[i, j].WhoWin() == -1)
                    return -1;
        return 0;
    }


    /// <summary>
    /// Player p try to fill the cell with coords (row,col) 
    /// </summary>
    /// <param name="sbRow">Simple Board Row</param>
    /// <param name="sbCol">Simple Board Column</param>
    /// <param name="cellRow">Cell Row</param>
    /// <param name="cellCol">Cell Row</param>
    /// <param name="player">Player ID</param>
    /// <returns>
    /// True if player fill the cell (row,col). False otherwise
    /// </returns>
    public bool TryFillCellByOnePlayer(int sbRow, int sbCol, int cellRow, int cellCol, int player) {
        if (_board[sbRow, sbCol].WhoWin() != -1 || WhoWin() != -1)
            return false;

        bool aux = _board[sbRow, sbCol].TryFillCellByOnePlayer(cellRow, cellCol, player);

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
    /// <param name="row">Simple board row</param>
    /// <param name="col">Simple board column</param>
    /// <returns>
    /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
    /// El valor es -1 si todavía no ha acabado la partida.
    /// </returns>
    public int WhoWonInSimpleBoard(int row, int col) { return _board[row, col].WhoWin(); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="row">Simple board row</param>
    /// <param name="col">Simple board column</param>
    /// <returns></returns>
    public SimpleLogicBoard GetLogicBoard(int row, int col) { return _board[row, col]; }
}
