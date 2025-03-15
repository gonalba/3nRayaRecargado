using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board.Graphics
{
    public class DimensionalVisualBoard : MonoBehaviour
    {
        public float boardsOffset = 0.5f;

        //private SimpleVisualBoard _visualBoardOfSimpleGame;
        [HideInInspector]
        public Skin.SkinPackage skin = null;

        public SimpleVisualBoard simpleBoardPrefab;
        private SimpleVisualBoard[,] _renderBoard = new SimpleVisualBoard[LevelController.DIM, LevelController.DIM];

        // Start is called before the first frame update
        void Awake()
        {
            if (simpleBoardPrefab == null)
            {
                Debug.LogError("simpleBoardPrefab no está asignado");
                return;
            }

            if (skin == null)
            {
                Debug.LogError("skin no está asignado");
                return;
            }

            simpleBoardPrefab.skin = skin;

            for (int y = 0; y < LevelController.DIM; y++)
            {
                for (int x = 0; x < LevelController.DIM; x++)
                {
                    _renderBoard[y, x] = Instantiate(simpleBoardPrefab);
                    _renderBoard[y, x].transform.SetParent(transform);
                    _renderBoard[y, x].row = y;
                    _renderBoard[y, x].col = x;

                    // calculamos la posicion (posicion en el tablero + espacio entre casillas)
                    float xpos = (x * LevelController.DIM) + (boardsOffset * x);
                    float ypos = (y * LevelController.DIM) + (boardsOffset * y);
                    _renderBoard[y, x].transform.localPosition = new Vector3(xpos, ypos, 0);
                }
            }
        }

        public void ChangeCellToPlayer(int rowBoard, int colBoard, int rowCell, int colCell, int idPlayer)
        {
            _renderBoard[rowBoard, colBoard].ChangeCellToPlayer(rowCell, colCell, idPlayer);
        }

        public void ChangeColor(int rowBoard, int colBoard, Color c)
        {
            _renderBoard[rowBoard, colBoard].ChangeColor(c);
        }

        public void AddClickListenerToCells(VisualCell.ClickAction action)
        {
            for (int y = 0; y < LevelController.DIM; y++)
                for (int x = 0; x < LevelController.DIM; x++)
                    _renderBoard[y, x].AddClickListenerToCells(action);
        }
    }
}