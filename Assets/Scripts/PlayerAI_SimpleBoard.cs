using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

/// <summary>
/// IA para resolver un tablero simple. Algoritmo sacado de https://github.com/parzibyte/tic-tac-toe-c
/// </summary>
public class PlayerAI_SimpleBoard
{
    private int CONTEO_PARA_GANAR = 3;
    private int _playerId;
    private Vector2 _nextPosition;
    private BoardManager _boardManager;

    private SimpleLogicBoard _currentLogicBoard;
    public PlayerAI_SimpleBoard(BoardManager bm, int playerId)
    {
        _boardManager = bm;
        _playerId = playerId;
    }


    /// <summary>
    /// Metodo de salida que dice que casilla tiene que marcar la IA
    /// </summary>
    /// <returns>
    /// Devuelve la posicion de la casilla a marcar
    /// </returns>
    public Vector2 GetCoords()
    {
        _currentLogicBoard = _boardManager.GetLogicBoard();

        //La forma en la que la IA tiene que calcular la mejor coordenada es:
        //1.Ganar si se puede
        if (WinIfCan(_playerId)) return _nextPosition;

        //2.Hacer perder al oponente si está a punto de ganar
        int opponentId = ((_playerId + 1) % 3) + 1;
        if (WinIfCan(opponentId)) return _nextPosition;

        //3.Tomar el mejor movimiento del oponente(en donde obtiene el mayor puntaje)
        //o tomar mi mejor movimiento(en donde obtengo mayor puntaje)
        if (BestMovement(_playerId, opponentId)) return _nextPosition;

        //5.Elegir la de la esquina superior izquierda(0,0)
        if (UpperLeftCorner()) return _nextPosition;

        //6.Coordenadas aleatorias
        RandomCell();
        return _nextPosition;
    }


    /// <summary>
    /// Metodo que comprueba si puede ganar en el siguiente movimiento
    /// </summary>
    /// <returns>
    /// Devuelve TRUE si puede ganar, FALSE en caso contrario
    /// </returns>
    private bool WinIfCan(int playerId)
    {
        for (int x = 0; x < BoardManager.DIM(); x++)
        {
            for (int y = 0; y < BoardManager.DIM(); y++)
            {
                if (IsEmptyCoords(x, y))
                {
                    if (CanWinInCell(playerId, x, y))
                    {
                        _nextPosition = new Vector2(x, y);
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
    /// <param name="x">Columna</param>
    /// <param name="y">Fila</param>
    /// <returns>
    /// Devuelve TRUE si la casilla esta vacia, FALSE en caso contrario.
    /// </returns>
    public bool IsEmptyCoords(int x, int y)
    {
        return _currentLogicBoard.GetValueofCell(x, y) == 0;
    }


    /// <summary>
    /// Metodo que comprueba si se gana colocando ficha en las coordenadas (x,y) pasadas por parámetro
    /// </summary>
    /// <param name="x">Columnas</param>
    /// <param name="y">Filas</param>
    /// <returns>
    /// Devuelve TRUE si gana al colocar la ficha en las coordenadas pasadas por parámetro, FALSE en caso contrario
    /// </returns>
    private bool CanWinInCell(int playerId, int x, int y)
    {
        int value = playerId;

        int valueX1 = _currentLogicBoard.GetValueofCell((x + 1) % BoardManager.DIM(), y);
        int valueX2 = _currentLogicBoard.GetValueofCell((x + 2) % BoardManager.DIM(), y);

        int valueY1 = _currentLogicBoard.GetValueofCell(x, (y + 1) % BoardManager.DIM());
        int valueY2 = _currentLogicBoard.GetValueofCell(x, (y + 2) % BoardManager.DIM());

        // Si en la fila todas las casillas son iguales y no vacías
        if ((value == valueX1) && (value == valueX2) && (value != 0))
        {
            return true;
        }

        // Lo mismo para las columnas
        if ((value == valueY1) && (value == valueY2) && (value != 0))
        {
            return true;
        }


        int valueXY1 = _currentLogicBoard.GetValueofCell((x + 1) % BoardManager.DIM(), (y + 1) % BoardManager.DIM());
        int valueXY2 = _currentLogicBoard.GetValueofCell((x + 2) % BoardManager.DIM(), (y + 2) % BoardManager.DIM());

        // Y finalmente miro las dos diagonales

        // Diagonal 0,0|1,1|2,2
        if (Math.Abs(x - y) == 0 && (value == valueXY1) && (value == valueXY2) && (value != 0))
        {
            return true;
        }

        // Diagonal 2,0|1,1|0,2

        // Calculamos las coordenadas de la diagonal
        int x1Inv = (x + 1) % BoardManager.DIM();
        int y1Inv = y - 1;
        if (y1Inv == -1)
            y1Inv = BoardManager.DIM() - 1;

        int x2Inv = (x + 2) % BoardManager.DIM();
        int y2Inv = y - 2;
        if (y2Inv == -1)
            y2Inv = BoardManager.DIM() - 1;
        else if (y2Inv == -2)
            y2Inv = BoardManager.DIM() - 2;

        // accedemos al valor de las coordenadas calculadas
        int valueXY1Inv = _currentLogicBoard.GetValueofCell(x1Inv, y1Inv);
        int valueXY2Inv = _currentLogicBoard.GetValueofCell(x2Inv, y2Inv);

        // comprobamos si todos los valores coinciden
        if ((Math.Abs(x - y) == 2 || (x == 1 && y == 1))
            && (value == valueXY1Inv) && (value == valueXY2Inv) && (value != 0))
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
    /// <param name="playerId"></param>
    /// <param name="opponentId"></param>
    /// <returns>
    /// Si el movimiento tiene una puntuacion inferior a 2 entonces devuelve FALSE. Devuelve TRUE en caso contrario.
    /// </returns>
    private bool BestMovement(int playerId, int opponentId)
    {
        int playerBestScore = BestMovementTo(playerId);
        Vector2 playerBestPos = _nextPosition;
        int opponentBestScore = BestMovementTo(opponentId);

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
    /// <param name="playerId">Id del jugador del que queremos consultar la mejor posicion</param>
    /// <returns>
    /// Devuelve un INT que representa la puntuacion del mejor movimiento
    /// </returns>
    private int BestMovementTo(int playerId)
    {
        int conteoMayor = 0, xConteoMayor = -1, yConteoMayor = -1;

        for (int x = 0; x < BoardManager.DIM(); x++)
        {
            for (int y = 0; y < BoardManager.DIM(); y++)
            {
                if (!IsEmptyCoords(x, y))
                {
                    continue;
                }
                // Colocamos y contamos el puntaje
                int conteoTemporal = ScoreWithNewCoords(playerId, x, y);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                    yConteoMayor = y;
                    xConteoMayor = x;
                }
            }
        }

        _nextPosition.x = xConteoMayor;
        _nextPosition.y = yConteoMayor;

        return conteoMayor;
    }


    /// <summary>
    /// Metodo que calcula la puntuacion del tablero suponiendo que en la posicion (newX, newY) ponemos una ficha del jugador con id playerId
    /// </summary>
    /// <param name="playerId">Id del juegador del que vamos a calcular la puntuacion</param>
    /// <param name="newX">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newY">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que representa la puntuacion del tablero suponiendo que en la posicion (newX, newY) ponemos una ficha del jugador con id playerId
    /// </returns>
    private int ScoreWithNewCoords(int playerId, int newX, int newY)
    {
        int conteoMayor = 0;
        int x, y;
        for (y = 0; y < BoardManager.DIM(); y++)
        {
            for (x = 0; x < BoardManager.DIM(); x++)
            {
                // Colocamos y contamos el puntaje
                int conteoTemporal;
                conteoTemporal = CountUp(playerId, x, y, newX, newY);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                }
                conteoTemporal = CountUpRight(playerId, x, y, newX, newY);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                }

                conteoTemporal = CountRight(playerId, x, y, newX, newY);
                if (conteoTemporal > conteoMayor)
                {
                    conteoMayor = conteoTemporal;
                }

                conteoTemporal = CountBottomRight(playerId, x, y, newX, newY);
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
    /// <param name="playerId">Id del jugador</param>
    /// <param name="x">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="y">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newX">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newY">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia arriba empezando por la posicion (x,y)
    /// </returns>
    private int CountUp(int playerId, int x, int y, int newX, int newY)
    {
        int yInicio = (y - CONTEO_PARA_GANAR >= 0) ? y - CONTEO_PARA_GANAR + 1 : 0;
        int contador = 0;

        int opponentId = ((playerId + 1) % 3) + 1;

        for (; yInicio <= y; yInicio++)
        {
            if (_currentLogicBoard.GetValueofCell(x, yInicio) == playerId || (x == newX && yInicio == newY))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(x, y) == opponentId)
            {
                contador = 0;
            }
        }
        return contador;
    }


    /// <summary>
    /// Metodo que cuenta numero de fichas seguidas del jugador playerId hacia la derecha empezando por la posicion (x,y)
    /// </summary>
    /// <param name="playerId">Id del jugador</param>
    /// <param name="x">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="y">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newX">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newY">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia la derecha empezando por la posicion (x,y)
    /// </returns>
    private int CountRight(int playerId, int x, int y, int newX, int newY)
    {
        int xFin = (x + CONTEO_PARA_GANAR < BoardManager.DIM()) ? x + CONTEO_PARA_GANAR - 1 : BoardManager.DIM() - 1;
        int contador = 0;

        int opponentId = ((playerId + 1) % 3) + 1;

        for (; x <= xFin; x++)
        {
            if (_currentLogicBoard.GetValueofCell(x, y) == playerId || (x == newX && y == newY))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(x, y) == opponentId)
            {
                contador = 0;
            }
        }
        return contador;
    }


    /// <summary>
    /// Metodo que cuenta numero de fichas seguidas del jugador playerId hacia arriba a la derecha empezando por la posicion (x,y)
    /// </summary>
    /// <param name="playerId">Id del jugador</param>
    /// <param name="x">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="y">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newX">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newY">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia arriba a la derecha empezando por la posicion (x,y)
    /// </returns>
    private int CountUpRight(int playerId, int x, int y, int newX, int newY)
    {
        int xFin = (x + CONTEO_PARA_GANAR < BoardManager.DIM()) ? x + CONTEO_PARA_GANAR - 1 : BoardManager.DIM() - 1;
        int yInicio = (y - CONTEO_PARA_GANAR >= 0) ? y - CONTEO_PARA_GANAR + 1 : 0;
        int contador = 0;

        int opponentId = ((playerId + 1) % 3) + 1;

        while (x <= xFin && yInicio <= y)
        {
            if (_currentLogicBoard.GetValueofCell(x, y) == playerId || (x == newX && y == newY))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(x, y) == opponentId)
            {
                contador = 0;
            }
            x++;
            y--;
        }
        return contador;
    }


    /// <summary>
    /// Metodo que cuenta numero de fichas seguidas del jugador playerId hacia abajo a la derecha empezando por la posicion (x,y)
    /// </summary>
    /// <param name="playerId">Id del jugador</param>
    /// <param name="x">Coordenada X de la posicion de la que partimos para contar</param>
    /// <param name="y">Coordenada Y de la posicion de la que partimos para contar</param>
    /// <param name="newX">Coordenada X de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <param name="newY">Coordenada Y de la posicon donde simulamos que hay una ficha del jugador con id playerId</param>
    /// <returns>
    /// Devuelve un INT que corresponde al numero de fichas seguidas del jugador playerId hacia abajo a la derecha empezando por la posicion (x,y)
    /// </returns>
    private int CountBottomRight(int playerId, int x, int y, int newX, int newY)
    {
        int xFin = (x + CONTEO_PARA_GANAR < BoardManager.DIM()) ? x + CONTEO_PARA_GANAR - 1 : BoardManager.DIM() - 1;
        int yFin = (y + CONTEO_PARA_GANAR < BoardManager.DIM()) ? y + CONTEO_PARA_GANAR - 1 : BoardManager.DIM() - 1;
        int contador = 0;

        int opponentId = ((playerId + 1) % 3) + 1;

        while (x <= xFin && y <= yFin)
        {
            if (_currentLogicBoard.GetValueofCell(x, y) == playerId || (x == newX && y == newY))
            {
                contador++;
            }
            else if (_currentLogicBoard.GetValueofCell(x, y) == opponentId)
            {
                contador = 0;
            }
            x++;
            y++;
        }
        return contador;
    }


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
        int x = 0;
        int y = 0;
        do
        {
            x = UnityEngine.Random.Range(0, BoardManager.DIM() - 1);
            y = UnityEngine.Random.Range(0, BoardManager.DIM() - 1);
        } while (_currentLogicBoard.WhoWin() == -1 && !IsEmptyCoords(x, y));
    }

}
