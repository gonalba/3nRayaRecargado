using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Board.Logic 
{
    public enum CellContent { PLAYER1, PLAYER2, TIE, EMPTY }

    /// <summary>
    /// SimpleBoard es la clase engargada de guardar el estado un tablero 3x3. 
    /// Puedes rellenar una casilla por un juador y consultar quien ha ganado.
    /// </summary>
    public class SimpleTicTacToe
    {
        /// <summary>
        /// Id del jugador ganador. Es -1 si no ha ganado nadie y 0 si el resultado es empate.
        /// </summary>
        private CellContent _winner = CellContent.EMPTY;

        /// <summary>
        /// Este array representa el tablero lógico 3x3
        /// </summary>
        private CellContent[,] _board = new CellContent[LevelController.DIM, LevelController.DIM];




        public SimpleTicTacToe()
        {
            // Inicializamos el array board 
            for (int r = 0; r < LevelController.DIM; r++)
                for (int c = 0; c < LevelController.DIM; c++)
                    _board[r, c] = CellContent.EMPTY;
        }





        /// <summary>
        /// Comprueba si hay un ganador en el tablero actual.
        /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
        /// </summary>
        /// <returns> 
        /// Devuelve el id del jugados que ha ganado. Si hay empate devuelve 0 y si todavia se puede seguir jugando devuelve -1
        /// </returns>
        private CellContent checkWinner()
        {
            // Si en alguna fila todas las casillas son iguales y no vacías
            for (int row = 0; row < LevelController.DIM; row++)
                if (_board[row, 0] != CellContent.EMPTY && _board[row, 0] != CellContent.TIE 
                    && _board[row, 0] == _board[row, 1] && _board[row, 0] == _board[row, 2])
                {
                    return _board[row, 0];
                }

            // Lo mismo para las columnas
            for (int col = 0; col < LevelController.DIM; col++)
                if (_board[0, col] != CellContent.EMPTY && _board[0, col] != CellContent.TIE 
                    && _board[0, col] == _board[1, col] && _board[0, col] == _board[2, col])
                {
                    return _board[0, col];
                }

            // Y finalmente miro las dos diagonales
            if (_board[0, 0] != CellContent.EMPTY && _board[0, 0] != CellContent.TIE
                && _board[0, 0] == _board[1, 1] && _board[0, 0] == _board[2, 2])
            {
                return _board[0, 0];
            }
            else if (_board[0, 2] != CellContent.EMPTY && _board[0, 2] != CellContent.TIE 
                     && _board[0, 2] == _board[1, 1] && _board[0, 2] == _board[2, 0])
            {
                return _board[0, 2];
            }

            // Miro si ha habido empate: si hay alguna casilla vacia, entonces no hay empate
            for (int row = 0; row < LevelController.DIM; row++)
                for (int col = 0; col < LevelController.DIM; col++)
                {
                    if (_board[row, col] == CellContent.EMPTY)
                        return CellContent.EMPTY;
                }

            return CellContent.TIE;
        }


        /// <summary>
        /// Player p try to fill the cell with coords (row,col) 
        /// </summary>
        /// <param name="row">Cell row</param>
        /// <param name="col">Cell column</param>
        /// <param name="playerId">Player id</param>
        /// <returns>
        /// True if player fill the cell (row,col). False otherwise
        /// </returns>
        public bool TryFillCellByPlayer(int row, int col, CellContent playerId)
        {
            if (_winner != CellContent.EMPTY || row >= LevelController.DIM || col >= LevelController.DIM || _board[row, col] != CellContent.EMPTY)
                return false;

            _board[row, col] = playerId;

            _winner = checkWinner();

            Debug.Log("Winner = " + _winner);
            Debug.Log("Board: " + _board);

            return true;
        }


        /// <summary>
        /// Método que sirve para consultar quien ha ganado el board.
        /// </summary>
        /// <returns>
        /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
        /// El valor es -1 si todavía no ha acabado la partida.
        /// </returns>
        public CellContent WhoWin() { return _winner; }


        /// <summary>
        /// Metodo que permite acceder al valor de la celda [row,col]
        /// </summary>
        /// <param name="row">Cell row</param>
        /// <param name="col">Cell column</param>
        /// <returns>
        /// Devuelve un INT que representa el valor de la celda [row,col]
        /// </returns>
        public CellContent GetValueOfCell(int row, int col) { return _board[row, col]; }

    }
}