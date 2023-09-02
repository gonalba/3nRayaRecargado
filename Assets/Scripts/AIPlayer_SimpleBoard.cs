using System;
using UnityEngine;

/// <summary>
/// IA para jugar en un tablero simple. 
///  - Algoritmo IA facil sacado de https://github.com/parzibyte/tic-tac-toe-c
///  - Algoritmo IA dificil basado en un algoritmo generado por chatGPT
/// </summary>
public class AIPlayer_SimpleBoard
{
    #region Hard difficult AI arrays
    private int[] weights = new int[9] { 3, 2, 3, 2, 4, 2, 3, 2, 3 };
    private int[] rows = new int[9] { 0, 0, 0, 1, 1, 1, 2, 2, 2 };
    private int[] cols = new int[9] { 0, 1, 2, 0, 1, 2, 0, 1, 2 };
    #endregion

    private bool _hardDifficult = false;

    private int CONTEO_PARA_GANAR = 3;
    private int _playerId;
    private int _opponentId;
    private Vector2 _nextPosition;
    private LevelManager _boardManager;

    private SimpleLogicBoard _currentLogicBoard;

    public AIPlayer_SimpleBoard(LevelManager bm, int playerId, int opponentId, bool hardDifficult)
    {
        _boardManager = bm;
        _playerId = playerId;
        _opponentId = opponentId;
        _hardDifficult = hardDifficult;
    }


    /// <summary>
    /// Método que selecciona un tablero logico. Si estamos jugando al dimensional, selecciona el tablero que toque. 
    /// Si hay que elegir, elige uno al azar. Devuelve las coordenadas del tablero elegido.
    /// </summary>
    /// <returns>
    /// Devuelve un VECTOR2 que contiene las coordenadas [X(columnas),Y(filas)] del tablero logico seleccionado.
    /// </returns>
    public Vector2 SelectSimpleBoard()
    {
        Vector2 boardCoords = new Vector2();
        _currentLogicBoard = _boardManager.GetLogicBoard(out boardCoords);

        return boardCoords;
    }


    /// <summary>
    /// Metodo de salida que dice que casilla tiene que marcar la IA
    /// </summary>
    /// <returns>
    /// Devuelve la posicion de la casilla a marcar
    /// </returns>
    public Vector2 SelectCoords()
    {
        //La forma en la que la IA tiene que calcular la mejor coordenada es:
        //1.Ganar si se puede
        if (WinIfCan(_playerId)) return _nextPosition;

        //2.Hacer perder al oponente si está a punto de ganar
        if (WinIfCan(_opponentId)) return _nextPosition;

        //3.Tomar el mejor movimiento del oponente(en donde obtiene el mayor puntaje)
        //o tomar mi mejor movimiento(en donde obtengo mayor puntaje)
        if (BestMovement()) return _nextPosition;

        //5.Elegir la de la esquina superior izquierda(0,0)
        if (UpperLeftCorner()) return _nextPosition;

        //6.Coordenadas aleatorias
        RandomCell();
        return _nextPosition;
    }


    /// <summary>
    /// Metodo que comprueba si puede ganar en el siguiente movimiento
    /// </summary>
    /// <param name="player">Id del jugador</param>
    /// <returns>
    /// Devuelve TRUE si puede ganar, FALSE en caso contrario
    /// </returns>
    private bool WinIfCan(int player)
    {
        for (int c = 0; c < LevelManager.DIM(); c++)
        {
            for (int r = 0; r < LevelManager.DIM(); r++)
            {
                if (IsEmptyCoords(r, c))
                {
                    if (CanWinInCell(player, r, c))
                    {
                        _nextPosition = new Vector2(c, r);
                        return true;
                    }
                }
            }
        }

        return false;
    }


    /// <summary>
    /// Metodo para consultar si una casilla esta vacia
    /// </summary>
    /// <param name="r">Fila</param>
    /// <param name="c">Columna</param>
    /// <returns>
    /// Devuelve TRUE si la casilla esta vacia, FALSE en caso contrario.
    /// </returns>
    public bool IsEmptyCoords(int r, int c)
    {
        return _currentLogicBoard.GetValueofCell(r, c) == 0;
    }


    /// <summary>
    /// Metodo que comprueba si se gana colocando ficha en las coordenadas (x,y) pasadas por parámetro
    /// </summary>
    /// <param name="player">Id del jugador del que queremos consultar la mejor posicion</param>
    /// <param name="r">Columnas</param>
    /// <param name="c">Filas</param>
    /// <returns>
    /// Devuelve TRUE si gana al colocar la ficha en las coordenadas pasadas por parámetro, FALSE en caso contrario
    /// </returns>
    private bool CanWinInCell(int player, int r, int c)
    {
        int value = player;

        int valueR1 = _currentLogicBoard.GetValueofCell((r + 1) % LevelManager.DIM(), c);
        int valueR2 = _currentLogicBoard.GetValueofCell((r + 2) % LevelManager.DIM(), c);

        int valueC1 = _currentLogicBoard.GetValueofCell(r, (c + 1) % LevelManager.DIM());
        int valueC2 = _currentLogicBoard.GetValueofCell(r, (c + 2) % LevelManager.DIM());

        // Si en la fila todas las casillas son iguales y no vacías
        if ((value == valueR1) && (value == valueR2) && (value != 0))
        {
            return true;
        }

        // Lo mismo para las columnas
        if ((value == valueC1) && (value == valueC2) && (value != 0))
        {
            return true;
        }


        int valueD1 = _currentLogicBoard.GetValueofCell((r + 1) % LevelManager.DIM(), (c + 1) % LevelManager.DIM());
        int valueD2 = _currentLogicBoard.GetValueofCell((r + 2) % LevelManager.DIM(), (c + 2) % LevelManager.DIM());

        // Y finalmente miro las dos diagonales

        // Diagonal 0,0|1,1|2,2
        if (Math.Abs(r - c) == 0 && (value == valueD1) && (value == valueD2) && (value != 0))
        {
            return true;
        }

        // Diagonal 2,0|1,1|0,2

        // Calculamos las coordenadas de la diagonal
        int r1Inv = (r + 1) % LevelManager.DIM();
        int c1Inv = c - 1;
        if (c1Inv == -1)
            c1Inv = LevelManager.DIM() - 1;

        int r2Inv = (r + 2) % LevelManager.DIM();
        int c2Inv = c - 2;
        if (c2Inv == -1)
            c2Inv = LevelManager.DIM() - 1;
        else if (c2Inv == -2)
            c2Inv = LevelManager.DIM() - 2;

        // accedemos al valor de las coordenadas calculadas
        int valueD1Inv = _currentLogicBoard.GetValueofCell(r1Inv, c1Inv);
        int valueD2Inv = _currentLogicBoard.GetValueofCell(r2Inv, c2Inv);

        // comprobamos si todos los valores coinciden
        if ((Math.Abs(r - c) == 2 || (r == 1 && c == 1))
            && (value == valueD1Inv) && (value == valueD2Inv) && (value != 0))
        {
            return true;
        }

        return false;
    }


    /// <summary>
    /// Metodo que comprueba el movimiento con mayor puntuacion para cada uno de los jugadores.
    /// Puntuación: 
    /// 0 -> si la casilla no tiene opciones de hacer 3 en raya.
    /// 1 -> si la casilla tiene opciones de 3 en raya y tiene una casilla puesta.
    /// 2 -> si la casilla tiene opciones de 3 en raya y tiene dos casilla puesta.
    /// </summary>
    /// <returns>
    /// Si el movimiento tiene una puntuacion inferior a 2 entonces devuelve FALSE. Devuelve TRUE en caso contrario.
    /// </returns>
    private bool BestMovement()
    {
        int playerBestScore = BestMovementTo(_playerId, _opponentId);
        Vector2 playerBestPos = _nextPosition;
        int opponentBestScore = BestMovementTo(_opponentId, _playerId);

        if (playerBestScore > opponentBestScore)
            _nextPosition = playerBestPos;

        if (opponentBestScore < 2)
            return false;

        return true;
    }


    /// <summary>
    /// Metodo que sirve para consultar la puntuacion del mejor movimiento para el jugador con id playerId. 
    /// Tambien asigna en el atributo de clase _nextPosition la mejor posicion para poner la ficha para el jugador con id playerId.
    /// </summary>
    /// <param name="player">Id del jugador del que queremos consultar la mejor posicion</param>
    /// <param name="opponent">Id del opponente</param>
    /// <returns>
    /// Devuelve un INT que representa la puntuacion del mejor movimiento
    /// </returns>
    private int BestMovementTo(int player, int opponent)
    {
        int conteoMayor = 0, cConteoMayor = -1, rConteoMayor = -1;

        for (int r = 0; r < LevelManager.DIM(); r++)
        {
            for (int c = 0; c < LevelManager.DIM(); c++)
            {
                if (IsEmptyCoords(r, c))
                {
                    // Colocamos y contamos el puntaje
                    int conteoTemporal;
                    if (_hardDifficult)
                        conteoTemporal = ScoreWithNewCoordsHardMode(player, opponent, r, c);
                    else
                        conteoTemporal = ScoreWithNewCoordsEasyMode(player, opponent, r, c);

                    if (conteoTemporal > conteoMayor)
                    {
                        conteoMayor = conteoTemporal;
                        rConteoMayor = r;
                        cConteoMayor = c;
                    }
                }
            }
        }

        _nextPosition.x = cConteoMayor;
        _nextPosition.y = rConteoMayor;

        return conteoMayor;
    }


    #region Hard mode AI
    /// <summary>
    /// Metodo que calcula la puntuacion del tablero suponiendo que en la posicion (newX,newY) ponemos una ficha del jugador con id player
    /// </summary>
    /// <param name="player">Id del jugador</param>
    /// <param name="opponent">Id del oponente</param>
    /// <param name="newR">Fila de la casilla que simula estar ocupada por el player</param>
    /// <param name="newC">Columna de la casilla que simula estar ocupada por el player</param>
    /// <returns>
    /// Devuelve un INT que representa la puntuacion del tablero suponiendo que en la posicion (newX,newY) hay una ficha del jugador con id player 
    /// </returns>
    int ScoreWithNewCoordsHardMode(int player, int opponent, int newR, int newC)
    {
        int score = 0;
        for (int i = 0; i < 9; i++)
        {
            #region puntuacion en las filas
            int lineScoreR = 0;
            for (int j = 0; j < 3; j++)
            {
                int r = rows[i];
                int c = (cols[i] + j) % 3;

                if (_currentLogicBoard.GetValueofCell(r, c) == player || (r == newR && c == newC))
                    lineScoreR++;
                else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
                {
                    lineScoreR--;
                }
            }
            score += weights[i] * lineScoreR;
            #endregion

            #region puntuacion en las columnas
            int lineScoreC = 0;
            for (int j = 0; j < 3; j++)
            {
                int r = (rows[i] + j) % 3;
                int c = cols[i];

                if (_currentLogicBoard.GetValueofCell(r, c) == player || (r == newR && c == newC))
                    lineScoreC++;
                else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
                {
                    lineScoreC--;
                }
            }
            score += weights[i] * lineScoreC;
            #endregion

            #region puntuacion en las diagonales
            int diag = Mathf.Abs(rows[i] - cols[i]);

            #region diagonal normal (00|11|22)
            if (diag == 0)
            {
                int diagScoreN = 0;
                for (int j = 0; j < 3; j++)
                {
                    int r = (rows[i] + j) % 3;
                    int c = (cols[i] + j) % 3;

                    if (_currentLogicBoard.GetValueofCell(r, c) == player || (r == newR && c == newC))
                        diagScoreN++;
                    else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
                    {
                        diagScoreN--;
                    }
                }
                score += weights[i] * diagScoreN;
            }
            #endregion

            #region diagonal inversa (20|11|02)
            if (diag == 2)
            {
                int diagScoreI = 0;
                for (int j = 0; j < 3; j++)
                {
                    int r = (rows[i] + j) % 3;
                    int c = (cols[i] - j) > -1 ? cols[i] - j : cols[i] - j + 3;

                    if (_currentLogicBoard.GetValueofCell(r, c) == player || (r == newR && c == newC))
                        diagScoreI++;
                    else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
                    {
                        diagScoreI--;
                    }
                }
                score += weights[i] * diagScoreI;
            }
            #endregion
            #endregion
        }
        return score;
    }
    #endregion


    #region Easy mode AI
    /// <summary>
    /// Metodo que calcula la puntuacion del tablero suponiendo que en la posicion (newX,newY) ponemos una ficha del jugador con id player
    /// </summary>
    /// <param name="player">Id del juegador del que vamos a calcular la puntuacion</param>
    /// <param name="opponent">Id del oponente que ayuda a calcular la puntuacion</param>
    /// <param name="newR">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newC">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que representa la puntuacion del tablero suponiendo que en la posicion (newX,newY) hay una ficha del jugador con id player
    /// </returns>
    private int ScoreWithNewCoordsEasyMode(int player, int opponent, int newR, int newC)
    {
        int conteoMayor = 0;
        for (int r = 0; r < LevelManager.DIM(); r++)
        {
            for (int c = 0; c < LevelManager.DIM(); c++)
            {
                // Colocamos y contamos el puntaje
                int conteoTemporal;
                conteoTemporal = CountUp(player, opponent, r, c, newR, newC);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                }
                conteoTemporal = CountUpRight(player, opponent, r, c, newR, newC);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                }

                conteoTemporal = CountRight(player, opponent, r, c, newR, newC);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                }

                conteoTemporal = CountBottomRight(player, opponent, r, c, newR, newC);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                }
            }
        }
        return conteoMayor;
    }


    /// <summary>
    /// Metodo que cuenta numero de fichas seguidas del jugador playerId hacia arriba empezando por la posicion (x,y)
    /// </summary>
    /// <param name="player">Id del jugador</param>
    /// <param name="opponent">Id del oponente</param>
    /// <param name="r">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="c">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newR">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newC">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia arriba empezando por la posicion (x,y)
    /// </returns>
    private int CountUp(int player, int opponent, int r, int c, int newR, int newC)
    {
        int cInicio = (c - CONTEO_PARA_GANAR >= 0) ? c - CONTEO_PARA_GANAR + 1 : 0;
        int contador = 0;

        for (; cInicio <= c; cInicio++)
        {
            if (_currentLogicBoard.GetValueofCell(r, cInicio) == player || (r == newR && cInicio == newC))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
            {
                contador = 0;
            }
        }
        return contador;
    }


    /// <summary>
    /// Metodo que cuenta numero de fichas seguidas del jugador playerId hacia la derecha empezando por la posicion (x,y)
    /// </summary>
    /// <param name="player">Id del jugador</param>
    /// <param name="opponent">Id del oponente</param>
    /// <param name="r">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="c">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newR">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newC">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia la derecha empezando por la posicion (x,y)
    /// </returns>
    private int CountRight(int player, int opponent, int r, int c, int newR, int newC)
    {
        int xFin = (r + CONTEO_PARA_GANAR < LevelManager.DIM()) ? r + CONTEO_PARA_GANAR - 1 : LevelManager.DIM() - 1;
        int contador = 0;

        for (; r <= xFin; r++)
        {
            if (_currentLogicBoard.GetValueofCell(r, c) == player || (r == newR && c == newC))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
            {
                contador = 0;
            }
        }
        return contador;
    }


    /// <summary>
    /// Metodo que cuenta numero de fichas seguidas del jugador playerId hacia arriba a la derecha empezando por la posicion (x,y)
    /// </summary>
    /// <param name="player">Id del jugador</param>
    /// <param name="opponent">Id del oponente</param>
    /// <param name="r">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="c">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newR">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newC">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia arriba a la derecha empezando por la posicion (x,y)
    /// </returns>
    private int CountUpRight(int player, int opponent, int r, int c, int newR, int newC)
    {
        int rFin = (r + CONTEO_PARA_GANAR < LevelManager.DIM()) ? r + CONTEO_PARA_GANAR - 1 : LevelManager.DIM() - 1;
        int cInicio = (c - CONTEO_PARA_GANAR >= 0) ? c - CONTEO_PARA_GANAR + 1 : 0;
        int contador = 0;

        while (r <= rFin && cInicio <= c)
        {
            if (_currentLogicBoard.GetValueofCell(r, c) == player || (r == newR && c == newC))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
            {
                contador = 0;
            }
            r++;
            c--;
        }
        return contador;
    }


    /// <summary>
    /// Metodo que cuenta numero de fichas seguidas del jugador playerId hacia abajo a la derecha empezando por la posicion (x,y)
    /// </summary>
    /// <param name="player">Id del jugador</param>
    /// <param name="opponent">Id del oponente</param>
    /// <param name="r">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="c">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newR">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newC">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia abajo a la derecha empezando por la posicion (x,y)
    /// </returns>
    private int CountBottomRight(int player, int opponent, int r, int c, int newR, int newC)
    {
        int rFin = (r + CONTEO_PARA_GANAR < LevelManager.DIM()) ? r + CONTEO_PARA_GANAR - 1 : LevelManager.DIM() - 1;
        int cFin = (c + CONTEO_PARA_GANAR < LevelManager.DIM()) ? c + CONTEO_PARA_GANAR - 1 : LevelManager.DIM() - 1;
        int contador = 0;

        while (r <= rFin && c <= cFin)
        {
            if (_currentLogicBoard.GetValueofCell(r, c) == player || (r == newR && c == newC))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(r, c) == opponent)
            {
                contador = 0;
            }
            r++;
            c++;
        }
        return contador;
    }
    #endregion


    /// <summary>
    /// Metodo que asigna al atributo de clase _nextPosition la posicion (0,0) si esta libre
    /// </summary>
    /// <returns>
    /// Devuelve TRUE si la posicion (0,0) esta libre. FALSE en caso contrario
    /// </returns>
    private bool UpperLeftCorner()
    {
        if (_currentLogicBoard.GetValueofCell(0, 0) == 0)
        {
            _nextPosition.x = 0;
            _nextPosition.y = 0;
            return true;
        }
        return false;
    }


    /// <summary>
    /// Metodo que asigna una casilla vacia aleatoria al atributo de clase _nextPosition
    /// </summary>
    private void RandomCell()
    {
        int aux = UnityEngine.Random.Range(0, LevelManager.DIM() - 1);

        for (int row = (aux + 1) % LevelManager.DIM(); row != aux; row = (row + 1) % LevelManager.DIM())
            for (int col = (aux + 1) % LevelManager.DIM(); col != aux; col = (col + 1) % LevelManager.DIM())
            {
                if (_currentLogicBoard.WhoWin() == -1 && IsEmptyCoords(row, col))
                {
                    _nextPosition.y = row;
                    _nextPosition.x = col;
                    break;
                }
            }
    }

}
