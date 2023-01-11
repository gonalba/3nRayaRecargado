using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

// Código sacado de https://github.com/parzibyte/tic-tac-toe-c

public class AIPlayer
{
    //static int FILAS = BoardManager.DIM;
    //static int COLUMNAS = BoardManager.DIM;
    //static int TAMANIO_MATRIZ = FILAS * COLUMNAS;
    //static int CONTEO_PARA_GANAR = 3;

    //char[,] tablero = new char[FILAS, COLUMNAS];

    //int yDestino = -1;
    //int xDestino = -1;

    ///// <summary>
    ///// Hace que el CPU elija unas coordenadas para ganar
    ///// </summary>
    ///// <param name="jugador"></param>
    ///// <param name="tablero"></param>
    ///// <param name=""></param>
    ///// <param name="yDestino"></param>
    ///// <param name="xDestino"></param>
    //void elegirCoordenadasCpu(char jugador)
    //{
    //    Debug.Log("Estoy pensando..." + jugador);
    //    /*
    //    El orden en el que el CPU infiere las coordenadas que toma es:
    //    1. Ganar si se puede
    //    2. Hacer perder al oponente si está a punto de ganar
    //    3. Tomar el mejor movimiento del oponente (en donde obtiene el mayor puntaje)
    //    4. Tomar mi mejor movimiento (en donde obtengo mayor puntaje)
    //    5. Elegir la de la esquina superior izquierda (0,0)
    //    6. Coordenadas aleatorias
    //    */
    //    int conteoJugador, conteoOponente;
    //    char oponente = oponenteDe(jugador);
    //    // 1
    //    Vector2 pos = coordenadasParaGanar(jugador, tablero);
        
    //    if (pos.y != -1 && pos.x != -1)
    //    {
    //        Debug.Log("Ganar" + jugador);
    //        yDestino = pos.y;
    //        xDestino = pos.x;
    //        return;
    //    }
    //    // 2
    //    coordenadasParaGanar(oponente, tablero, &y, &x);
    //    if (pos.y != -1 && pos.x != -1)
    //    {
    //        Debug.Log("Tomar victoria de oponente" + jugador);
    //        yDestino = pos.y;
    //        xDestino = pos.x;
    //        return;
    //    }
    //    // 3
    //    Vector2 pos2 = coordenadasParaMayorPuntaje(jugador, tablero, &y, &x, &conteoJugador);
    //    Vector2 pos3 = coordenadasParaMayorPuntaje(oponente, tablero, &y, &x, &conteoOponente);
    //    if (conteoOponente > conteoJugador)
    //    {
    //        Debug.Log("Tomar puntaje mayor del oponente" + jugador);
    //        *yDestino = y;
    //        *xDestino = x;
    //        return;
    //    }
    //    else
    //    {
    //        Debug.Log("Tomar mi mayor puntaje" + jugador);
    //        *yDestino = y;
    //        *xDestino = x;
    //        return;
    //    }
    //    // 4
    //    if (coordenadasVacias(0, 0, tablero))
    //    {
    //        Debug.Log("Tomar columna superior izquierda" + jugador);
    //        *yDestino = 0;
    //        *xDestino = 0;
    //        return;
    //    }
    //    // 5
    //    Debug.Log("Coordenadas aleatorias" + jugador);
    //    obtenerCoordenadasAleatorias(jugador, tablero, yDestino, xDestino);
    //}


    ///// <summary>
    ///// Esta función complementa a contarSinSaberCoordenadas.
    ///// Te dice en qué X e Y el jugador [jugador] obtendrá
    ///// el mayor puntaje si pone ahí su pieza
    ///// </summary>
    ///// <param name="jugador"></param>
    ///// <param name="tableroOriginal"></param>
    ///// <param name=""></param>
    ///// <param name="yDestino"></param>
    ///// <param name="xDestino"></param>
    ///// <param name="conteo"></param>
    //void coordenadasParaMayorPuntaje(char jugador, char tableroOriginal[FILAS][COLUMNAS], int* yDestino, int* xDestino, int* conteo)
    //{

    //    char copiaTablero[FILAS][COLUMNAS] ;
    //    int y, x;
    //    int conteoMayor = 0,
    //        xConteoMayor = -1,
    //        yConteoMayor = -1;
    //    for (y = 0; y < FILAS; y++)
    //    {
    //        for (x = 0; x < COLUMNAS; x++)
    //        {
    //            clonarMatriz(tableroOriginal, copiaTablero);
    //            if (!coordenadasVacias(y, x, copiaTablero))
    //            {
    //                continue;
    //            }
    //            // Colocamos y contamos el puntaje
    //            colocarPieza(y, x, jugador, copiaTablero);
    //            int conteoTemporal = contarSinSaberCoordenadas(jugador, copiaTablero);
    //            if (conteoTemporal > conteoMayor)
    //            {
    //                conteoMayor = conteoTemporal;
    //                yConteoMayor = y;
    //                xConteoMayor = x;
    //            }
    //        }
    //    }
    //    *conteo = conteoMayor;
    //    *xDestino = xConteoMayor;
    //    *yDestino = yConteoMayor;
    //}


    ///// <summary>
    ///// Coloca dos coordenadas aleatorias y válidas en xDestino y yDestino
    ///// </summary>
    ///// <param name="jugador"></param>
    ///// <param name="tableroOriginal"></param>
    ///// <param name=""></param>
    ///// <param name="yDestino"></param>
    ///// <param name="xDestino"></param>
    //void obtenerCoordenadasAleatorias(char jugador, char tableroOriginal[FILAS][COLUMNAS], int* yDestino, int* xDestino)
    //{
    //    int x, y;
    //    do
    //    {
    //        x = aleatorio_en_rango(0, COLUMNAS - 1);
    //        y = aleatorio_en_rango(0, FILAS - 1);
    //    } while (!coordenadasVacias(y, x, tableroOriginal));
    //    *yDestino = y;
    //    *xDestino = x;
    //}


    ///// <summary>
    ///// Coloca en yDestino y xDestino las coordenadas para que jugador gane en tableroOriginal.
    ///// Si no puede ganar, colocará ambas coordenadas en -1
    ///// </summary>
    ///// <param name="jugador"></param>
    ///// <param name="tableroOriginal"></param>
    ///// <param name=""></param>
    ///// <param name="yDestino"></param>
    ///// <param name="xDestino"></param>
    //Vector2 coordenadasParaGanar(char jugador, char[,] tableroOriginal)
    //{
    //    char copiaTablero[FILAS][COLUMNAS] ;
    //    int y, x;

    //    Vector2 pos = new Vector2(-1, -1);

    //    for (y = 0; y < FILAS; y++)
    //    {
    //        for (x = 0; x < COLUMNAS; x++)
    //        {
    //            clonarMatriz(tableroOriginal, copiaTablero);
    //            if (coordenadasVacias(y, x, copiaTablero))
    //            {
    //                colocarPieza(y, x, jugador, copiaTablero);
    //                if (comprobarSiGana(jugador, copiaTablero))
    //                {
    //                    pos.y = y;
    //                    pos.x = x;
    //                    return pos;
    //                }
    //            }
    //        }
    //    }
    //    return pos;
    //}


    ///// <summary>
    /////     Esta función cuenta y te dice el mayor puntaje, pero no te dice en cuál X ni cuál Y. 
    /////     Está pensada para ser llamada desde otra función que lleva cuenta de X e Y
    ///// </summary>
    ///// <param name="jugador"></param>
    ///// <param name="copiaTablero"></param>
    ///// <param name=""></param>
    ///// <returns></returns>
    //int contarSinSaberCoordenadas(char jugador, char copiaTablero[FILAS][COLUMNAS])
    //{
    //    int conteoMayor = 0;
    //    int x, y;
    //    for (y = 0; y < FILAS; y++)
    //    {
    //        for (x = 0; x < COLUMNAS; x++)
    //        {
    //            // Colocamos y contamos el puntaje
    //            int conteoTemporal;
    //            conteoTemporal = contarHaciaArriba(x, y, jugador, copiaTablero);
    //            if (conteoTemporal > conteoMayor)
    //            {
    //                conteoMayor = conteoTemporal;
    //            }
    //            conteoTemporal = contarHaciaArribaDerecha(x, y, jugador, copiaTablero);
    //            if (conteoTemporal > conteoMayor)
    //            {
    //                conteoMayor = conteoTemporal;
    //            }

    //            conteoTemporal = contarHaciaDerecha(x, y, jugador, copiaTablero);
    //            if (conteoTemporal > conteoMayor)
    //            {
    //                conteoMayor = conteoTemporal;
    //            }

    //            conteoTemporal = contarHaciaAbajoDerecha(x, y, jugador, copiaTablero);
    //            if (conteoTemporal > conteoMayor)
    //            {
    //                conteoMayor = conteoTemporal;
    //            }
    //        }
    //    }
    //    return conteoMayor;
    //}

    ///*
    //Funciones de conteo. Simplemente cuentan cuántas piezas del mismo jugador están
    //alineadas
    //*/
    //int contarHaciaArriba(int x, int y, char jugador, char tablero[FILAS][COLUMNAS])
    //{
    //    int yInicio = (y - CONTEO_PARA_GANAR >= 0) ? y - CONTEO_PARA_GANAR + 1 : 0;
    //    int contador = 0;
    //    for (; yInicio <= y; yInicio++)
    //    {
    //        if (tablero[yInicio][x] == jugador)
    //        {
    //            contador++;
    //        }
    //        else
    //        {
    //            contador = 0;
    //        }
    //    }
    //    return contador;
    //}

    //int contarHaciaDerecha(int x, int y, char jugador, char tablero[FILAS][COLUMNAS])
    //{
    //    int xFin = (x + CONTEO_PARA_GANAR < COLUMNAS) ? x + CONTEO_PARA_GANAR - 1 : COLUMNAS - 1;
    //    int contador = 0;
    //    for (; x <= xFin; x++)
    //    {
    //        if (tablero[y][x] == jugador)
    //        {
    //            contador++;
    //        }
    //        else
    //        {
    //            contador = 0;
    //        }
    //    }
    //    return contador;
    //}

    //int contarHaciaArribaDerecha(int x, int y, char jugador, char tablero[FILAS][COLUMNAS])
    //{
    //    int xFin = (x + CONTEO_PARA_GANAR < COLUMNAS) ? x + CONTEO_PARA_GANAR - 1 : COLUMNAS - 1;
    //    int yInicio = (y - CONTEO_PARA_GANAR >= 0) ? y - CONTEO_PARA_GANAR + 1 : 0;
    //    int contador = 0;
    //    while (x <= xFin && yInicio <= y)
    //    {
    //        if (tablero[y][x] == jugador)
    //        {
    //            contador++;
    //        }
    //        else
    //        {
    //            contador = 0;
    //        }
    //        x++;
    //        y--;
    //    }
    //    return contador;
    //}

    //int contarHaciaAbajoDerecha(int x, int y, char jugador, char tablero[FILAS][COLUMNAS])
    //{
    //    int xFin = (x + CONTEO_PARA_GANAR < COLUMNAS) ? x + CONTEO_PARA_GANAR - 1 : COLUMNAS - 1;
    //    int yFin = (y + CONTEO_PARA_GANAR < FILAS) ? y + CONTEO_PARA_GANAR - 1 : FILAS - 1;
    //    int contador = 0;
    //    while (x <= xFin && y <= yFin)
    //    {
    //        if (tablero[y][x] == jugador)
    //        {
    //            contador++;
    //        }
    //        else
    //        {
    //            contador = 0;
    //        }
    //        x++;
    //        y++;
    //    }
    //    return contador;
    //}


    ///// <summary>
    ///// Indica si el tablero está vacío en las coordenadas indicadas
    ///// </summary>
    ///// <param name="y"></param>
    ///// <param name="x"></param>
    ///// <param name="tablero"></param>
    ///// <param name=""></param>
    ///// <returns></returns>
    //int coordenadasVacias(int y, int x, char tablero[FILAS][COLUMNAS])
    //{
    //    return tablero[y][x] == ESPACIO_VACIO;
    //}
}
