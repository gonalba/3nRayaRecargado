using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// DimensionalBoard es una clase que guarda el estado de un tablero dimensional 3x3 (tablero de tableros simples)
/// </summary>
public class DimensionalLogicBoard
{

    static int dim = 3;

    /// <summary>
    /// Id del jugador ganador. Es -1 si no ha ganado nadie y 0 si el resultado es empate.
    /// </summary>
    private int playerWin = -1;

    /// <summary>
    /// Este array representa el tablero lógico 3x3
    /// </summary>
    private SimpleLogicBoard[,] board = new SimpleLogicBoard[dim, dim];




    public DimensionalLogicBoard()
    {
        // Inicializamos el array board 
        for (int y = 0; y < dim; y++)
            for (int x = 0; x < dim; x++)
                board[y, x] = new SimpleLogicBoard();
    }


    /// <summary>
    /// Comprueba si hay un ganador actual.
    /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
    /// </summary>
    private void checkWinner()
    {
        // Si en alguna fila todas las casillas son iguales y no vacías
        for (int fila = 0; fila < 3; fila++)
            if ((board[fila, 0].WhoWin() == board[fila, 1].WhoWin())
                    && (board[fila, 0].WhoWin() == board[fila, 2].WhoWin())
                    && (board[fila, 0].WhoWin() > 0))
            {
                playerWin = board[fila, 0].WhoWin();
                return;
            }

        // Lo mismo para las columnas
        for (int columna = 0; columna < 3; columna++)
            if ((board[0, columna].WhoWin() == board[1, columna].WhoWin())
                    && (board[0, columna].WhoWin() == board[2, columna].WhoWin())
                    && (board[0, columna].WhoWin() > 0))
            {
                playerWin = board[0, columna].WhoWin();
                return;
            }

        // Y finalmente miro las dos diagonales
        if ((board[0, 0].WhoWin() == board[1, 1].WhoWin())
                && (board[0, 0].WhoWin() == board[2, 2].WhoWin())
                && (board[0, 0].WhoWin() > 0))
            playerWin = board[0, 0].WhoWin();
        else if ((board[0, 2].WhoWin() == board[1, 1].WhoWin())
                && (board[0, 2].WhoWin() == board[2, 0].WhoWin())
                && (board[0, 2].WhoWin() > 0))
            playerWin = board[0, 2].WhoWin();
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
        if (board[ySimpleBorad, xSimpleBoard].WhoWin() != -1 || WhoWin() != -1)
            return false;

        bool aux = board[ySimpleBorad, xSimpleBoard].TryFillCellByOnePlayer(yCell, xCell, pId);

        checkWinner();

        return aux;
    }

    /// <summary>
    /// Método que sirve para consultar quien ha ganado la partida.
    /// </summary>
    /// <returns>
    /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
    /// El valor es -1 si todavía no ha acabado la partida.
    /// </returns>
    public int WhoWin() { return playerWin; }


    /// <summary>
    /// Método que sirve para consultar quien ha ganado en un board especifico.
    /// </summary>
    /// <returns>
    /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
    /// El valor es -1 si todavía no ha acabado la partida.
    /// </returns>
    public int WhoWinInSimpleBoard(int x, int y)
    {
        return board[y,x].WhoWin();
    }


}
