using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class RenderCell : MonoBehaviour {
    public SpriteRenderer empty = null;
    public SpriteRenderer x = null;
    public SpriteRenderer o = null;

    public BoxCollider bCollider;

    private void Start() {
        if (empty == null || x == null || o == null) {
            Debug.LogError("Cell no tiene asignados los gameobjects");
            return;
        }

        //empty.gameObject.SetActive(true);
        //x.gameObject.SetActive(false);
        //o.gameObject.SetActive(false);

        bCollider.size = empty.size;
    }

    public void ChangeCellToPlayer(int idPlayer) {
        if (idPlayer == 1) {
            empty.gameObject.SetActive(false);
            x.gameObject.SetActive(true);
            o.gameObject.SetActive(false);
        } else if (idPlayer == 2) {
            empty.gameObject.SetActive(false);
            x.gameObject.SetActive(false);
            o.gameObject.SetActive(true);
        }

    }

    public void ChangeColor(Color c) {
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
    public float GetSizeCellY() { return 1; } //return empty.size.y; }

    /// <summary>
    /// Devuelve la dimension (coordenada X) de la casilla. 
    /// La casilla ocupa 1 unidad pero dimension de los sprites de las casillas
    /// son 0,9. Esto lo hacemos para crear el espacio entre casillas
    /// </summary>
    /// <returns></returns>
    public float GetSizeCellX() { return 1; } //return empty.size.y; }
}
