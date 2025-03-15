using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace Board.Graphics
{
    //[ExecuteAlways]
    public class VisualCell : MonoBehaviour
    {
        public SpriteRenderer empty = null;
        public SpriteRenderer x = null;
        public SpriteRenderer o = null;

        public BoxCollider bCollider;

        [HideInInspector]
        public SimpleVisualBoard board = null;
        public int row = -1;
        public int col = -1;

        public ClickAction clickAction;

        private void Start()
        {
            if (empty == null || x == null || o == null || board == null || row == -1 || col == -1)
            {
                Debug.LogError("Cell no tiene asignados los gameobjects");
                return;
            }

            bCollider.size = empty.size;
        }

        public Vector2Int GetCoodrs() { return new Vector2Int(col, row); }

        public void ChangeCellToPlayer(int idPlayer)
        {
            if (idPlayer == 1)
            {
                empty.gameObject.SetActive(false);
                x.gameObject.SetActive(true);
                o.gameObject.SetActive(false);
            }
            else if (idPlayer == 2)
            {
                empty.gameObject.SetActive(false);
                x.gameObject.SetActive(false);
                o.gameObject.SetActive(true);
            }

        }

        public void ChangeColor(Color c)
        {
            empty.material.color = c;
            x.material.color = c;
            o.material.color = c;
        }

        /// <summary>
        /// Devuelve la dimension (coordenada Y) de la casilla. 
        /// La casilla ocupa 1 unidad pero dimension de los sprites de las casillas
        /// son 0,9. Esto lo hacemos para crear el espacio entre casillas
        /// </summary>
        /// <returns></returns>
        public float GetSizeCellY()
        {
            return 1;
            //return empty.size.y;
        }

        /// <summary>
        /// Devuelve la dimension (coordenada X) de la casilla. 
        /// La casilla ocupa 1 unidad pero dimension de los sprites de las casillas
        /// son 0,9. Esto lo hacemos para crear el espacio entre casillas
        /// </summary>
        /// <returns></returns>
        public float GetSizeCellX()
        {
            return 1;
            //return empty.size.y;
        }

        /// <summary>
        /// Asigna los gráficos de las celdas
        /// </summary>
        /// <param name="skin"></param>
        public void SetSkin(Skin.SkinPackage skin)
        {
            empty.sprite = skin.empty;
            x.sprite = skin.x;
            o.sprite = skin.o;
        }

        public delegate void ClickAction(VisualCell cell);
        public void AddClickListenerToCells(ClickAction action)
        {
            Debug.Log("Añadimos la accion");
            clickAction += action;
        }

        private void OnMouseUpAsButton()
        {
            clickAction.Invoke(this);
        }
    }
}