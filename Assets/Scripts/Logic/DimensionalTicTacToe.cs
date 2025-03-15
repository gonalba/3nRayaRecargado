using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board.Logic
{
    /// <summary>
    /// DimensionalBoard es una clase que guarda el estado de un tablero dimensional 3x3 (tablero de tableros simples)
    /// </summary>
    public class DimensionalTicTacToe
    {
        /// <summary>
        /// Id del jugador ganador. Es -1 si no ha ganado nadie y 0 si el resultado es empate.
        /// </summary>
        private CellContent _winner = CellContent.EMPTY;

        /// <summary>
        /// Este array representa el tablero lógico 3x3
        /// </summary>
        private SimpleTicTacToe[,] _board = new SimpleTicTacToe[LevelController.DIM, LevelController.DIM];




        public DimensionalTicTacToe()
        {
            // Inicializamos el array board 
            for (int r = 0; r < LevelController.DIM; r++)
                for (int c = 0; c < LevelController.DIM; c++)
                    _board[r, c] = new SimpleTicTacToe();
        }


        /// <summary>
        /// Comprueba si hay un ganador actual.
        /// Codigo sacado de http://www.errordesintaxis.es/verfuente.php?fuente=228
        /// </summary>
        /// <returns> 
        /// Devuelve el id del jugados que ha ganado. Si hay empate devuelve 0 y si todavia se puede seguir jugando devuelve -1
        /// </returns>
        private CellContent checkWinner()
        {
            // Si en alguna fila todas las casillas son iguales y no vacías
            for (int row = 0; row < LevelController.DIM; row++)
            {
                if (_board[row, 0].WhoWin() != CellContent.EMPTY && _board[row, 0].WhoWin() != CellContent.TIE &&
                    _board[row, 0].WhoWin() == _board[row, 1].WhoWin() && _board[row, 0].WhoWin() == _board[row, 2].WhoWin())
                    return _board[row, 0].WhoWin();
            }

            // Lo mismo para las columnas
            for (int col = 0; col < LevelController.DIM; col++)
            {
                if (_board[0, col].WhoWin() != CellContent.EMPTY && _board[0, col].WhoWin() != CellContent.TIE
                    && _board[0, col].WhoWin() == _board[1, col].WhoWin() && _board[0, col].WhoWin() == _board[2, col].WhoWin())
                    return _board[0, col].WhoWin();
            }

            // Y finalmente miro las dos diagonales
            if (_board[0, 0].WhoWin() != CellContent.EMPTY && _board[0, 0].WhoWin() != CellContent.TIE 
                && _board[0, 0].WhoWin() == _board[1, 1].WhoWin() && _board[0, 0].WhoWin() == _board[2, 2].WhoWin())
                return _board[0, 0].WhoWin();
            else if (_board[0, 2].WhoWin() != CellContent.EMPTY && _board[0, 2].WhoWin() != CellContent.TIE 
                     && _board[0, 2].WhoWin() == _board[1, 1].WhoWin() && _board[0, 2].WhoWin() == _board[2, 0].WhoWin())
                return _board[0, 2].WhoWin();

            for (int i = 0; i < LevelController.DIM; i++)
            {
                for (int j = 0; j < LevelController.DIM; j++)
                {
                    if (_board[i, j].WhoWin() == CellContent.EMPTY)
                        return CellContent.EMPTY;
                }
            }
            return CellContent.TIE;
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
        public bool TryFillCellByOnePlayer(int sbRow, int sbCol, int cellRow, int cellCol, CellContent player)
        {
            if (_winner != CellContent.EMPTY || sbRow >= LevelController.DIM || sbCol >= LevelController.DIM || _board[sbRow, sbCol].WhoWin() != CellContent.EMPTY)
                return false;

            bool aux = _board[sbRow, sbCol].TryFillCellByPlayer(cellRow, cellCol, player);

            _winner = checkWinner();

            return aux;
        }

        /// <summary>
        /// Método que sirve para consultar quien ha ganado la partida.
        /// </summary>
        /// <returns>
        /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
        /// El valor es -1 si todavía no ha acabado la partida.
        /// </returns>
        public CellContent WhoWin() { return _winner; }


        /// <summary>
        /// Método que sirve para consultar quien ha ganado en un board especifico.
        /// </summary>
        /// <param name="row">Simple board row</param>
        /// <param name="col">Simple board column</param>
        /// <returns>
        /// Devuelve el id del juegador que ha ganado. El valor es 0 si el resultado es empate. 
        /// El valor es -1 si todavía no ha acabado la partida.
        /// </returns>
        public CellContent WhoWinInSimpleBoard(int row, int col)
        {
            return _board[row, col].WhoWin();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="row">Simple board row</param>
        /// <param name="col">Simple board column</param>
        /// <returns></returns>
        public SimpleTicTacToe GetLogicBoard(int row, int col) { return _board[row, col]; }
    }
}