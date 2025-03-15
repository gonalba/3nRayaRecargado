using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Board.Graphics {
    //[ExecuteAlways]
    public class SimpleVisualBoard : MonoBehaviour {
        public VisualCell cellPrefab = null;
        //public BoxCollider bCollider;

        [HideInInspector]
        public Skin.SkinPackage skin = null;

        private VisualCell[,] _renderBoard = new VisualCell[LevelController.DIM, LevelController.DIM];

        public int row = -1;
        public int col = -1;

        public Vector2Int GetCoodrs() { return new Vector2Int(col, row); }


        // Start is called before the first frame update
        void Awake() {
            if (cellPrefab == null) {
                Debug.LogError("cellPrefab no está asignado");
                return;
            }

            if (skin == null) {
                Debug.LogError("skin no está asignado");
                return;
            }

            cellPrefab.SetSkin(skin);

            for (int y = 0; y < LevelController.DIM; y++) { 
                for (int x = 0; x < LevelController.DIM; x++) {
                    _renderBoard[y, x] = Instantiate(cellPrefab);
                    _renderBoard[y, x].transform.SetParent(transform);
                    _renderBoard[y, x].transform.localPosition = new Vector3(x, y, 0);
                    _renderBoard[y, x].board = this;
                    _renderBoard[y, x].row = y;
                    _renderBoard[y, x].col = x;
                    Debug.Log("Board: " + _renderBoard[y, x].board);
                }
            }
            float sizeCollX = cellPrefab.GetSizeCellX() * LevelController.DIM;
            float sizeCollY = cellPrefab.GetSizeCellY() * LevelController.DIM;

            float centerPosX = (sizeCollX - cellPrefab.GetSizeCellX()) / 2;
            float centerPosY = (sizeCollY - cellPrefab.GetSizeCellY()) / 2;

            //bCollider.size = new Vector3(sizeCollX, sizeCollY);
            //bCollider.center = new Vector3(centerPosX, centerPosY);
        }

        public void ChangeCellToPlayer(int y, int x, int idPlayer) {
            _renderBoard[y, x].ChangeCellToPlayer(idPlayer);
        }

        public void ChangeColor(Color c) {
            for (int y = 0; y < LevelController.DIM; y++)
                for (int x = 0; x < LevelController.DIM; x++)
                    _renderBoard[y, x].ChangeColor(c);
        }

        public void AddClickListenerToCells(VisualCell.ClickAction action)
        {
            for (int y = 0; y < LevelController.DIM; y++)
                for (int x = 0; x < LevelController.DIM; x++)
                    _renderBoard[y, x].AddClickListenerToCells(action);
        }
    }
}