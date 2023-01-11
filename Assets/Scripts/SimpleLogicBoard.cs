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
    private int playerWin = -1;

    /// <summary>
    /// Este array representa el tablero lógico 3x3
    /// </summary>
    private int[,] board = new int[BoardManager.DIM(), BoardManager.DIM()];




    public SimpleLogicBoard()
    {
        // Inicializamos el array board 
        for (int y = 0; y < BoardManager.DIM(); y++)
            for (int x = 0; x < BoardManager.DIM(); x++)
                board[y, x] = 0;
    }


    /// <summary>
    /// Comprueba si hay un ganador en el tablero actual.
    /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
    /// </summary>
    private void checkWinner()
    {
        // Si en alguna fila todas las casillas son iguales y no vacías
        for (int fila = 0; fila < BoardManager.DIM(); fila++)
            if ((board[fila, 0] == board[fila, 1])
                    && (board[fila, 0] == board[fila, 2])
                    && (board[fila, 0] != 0))
            {
                playerWin = board[fila, 0];
                return;
            }

        // Lo mismo para las columnas
        for (int columna = 0; columna < BoardManager.DIM(); columna++)
            if ((board[0, columna] == board[1, columna])
                    && (board[0, columna] == board[2, columna])
                    && (board[0, columna] != 0))
            {
                playerWin = board[0, columna];
                return;
            }

        // Y finalmente miro las dos diagonales
        if ((board[0, 0] == board[1, 1])
                && (board[0, 0] == board[2, 2])
                && (board[0, 0] != 0))
        {
            playerWin = board[0, 0];
            return;
        }
        else if ((board[0, 2] == board[1, 1])
                && (board[0, 2] == board[2, 0])
                && (board[0, 2] != 0))
        {
            playerWin = board[0, 2];
            return;
        }

        // Miro si ha habido empate
        for (int i = 0; i < BoardManager.DIM(); i++)
            for (int j = 0; j < BoardManager.DIM(); j++)
            {
                if (board[i, j] == 0)
                    return;
            }

        playerWin = 0;

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
        if (board[y, x] != 0)
            return false;

        board[y, x] = pId;

        checkWinner();

        return true;
    }

    /// <summary>
    /// Método que sirve para consultar quien ha ganado el board.
    /// </summary>
    /// <returns>
    /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
    /// El valor es -1 si todavía no ha acabado la partida.
    /// </returns>
    public int WhoWin() { return playerWin; }


}
