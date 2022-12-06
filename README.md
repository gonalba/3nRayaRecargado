# 3nRayaRecargado
Este es un proyecto en desarrollo de una versión del tradicional juego tres en raya o tateti. Por lo que he visto por internet lo llaman tres en raya/tateti recargado. 

## Reglas

### Reglas básicas

El tateti recargado es una evolución del tateti original que da profundidad al juego y permite alargar la partida. La descripción rápida es que es un tateti de tatetis. Comenzamos por un tablero 3x3 donde en cada casilla hay otro tateti 3x3. Si se gana un tateti pequeño entonces la casilla del tablero grande es marcada con tu símbolo. Gana quien consigue rellenar tres casillas consecutivas (vertical, horizontal y diagonal) de tablero grande.

Para dar emoción, no se puede elegir el cuadrante para marcar, sino que es el contrario el que dicta en qué cuadrante te tocará marcar en tu turno. Ejemplo: La primera persona en colocar ficha elige el cuadrante y casilla donde marcar. La casilla donde marque determina el cuadrante del siguiente jugador y así sucesivamente.

### Posibles reglas a añadir

#### Bloqueo de cuadrante

El jugador en su turno tiene la opción de colocar ficha o de bloquear un cuadrante (cualquier cuadrante o el cuadrante en el que está, habría que pensarlo). Tras bloquear un cuadrante, el siguiente jugador puede elegir el cuadrante donde poner ficha. Solo puede haber un cuadrante bloqueado en la partida. Para desbloquear el cuadrante, el jugador debe "gastar" un turno en desbloquerlo sin poner ficha, al igual que sucede para bloquerlo. Tras debloquear el cuadrante, el siguiente jugador puede elegir el cuadrante donde poner ficha.

#### Eliminación de cuadrante

Se puede resetear un cuadrante cualquiera pero solo en el momento en el que el propio jugador gana uno. De esta manera, al ganar un cuadrante se da la opción al jugador de marcar el cuadrante ganado o eliminar uno del contrario.

## Versiones de Unity y Visual Studio

Unity 2021.3.15f1 LTS  
Visual Studio Communnity 2022 17.4.2
