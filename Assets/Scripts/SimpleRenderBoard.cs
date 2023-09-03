using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteAlways]
public class SimpleRenderBoard : MonoBehaviour {
    public RenderCell cellPrefab;
    public BoxCollider bCollider;

    private RenderCell[,] renderBoard = new RenderCell[LevelManager.DIM(), LevelManager.DIM()];


    // Start is called before the first frame update
    void Start() {
        for (int y = 0; y < LevelManager.DIM(); y++)
            for (int x = 0; x < LevelManager.DIM(); x++) {
                renderBoard[y, x] = Instantiate(cellPrefab);
                renderBoard[y, x].transform.SetParent(transform);
                renderBoard[y, x].transform.localPosition = new Vector3(x, y, 0);
            }

        float sizeCollX = cellPrefab.GetSizeCellX() * LevelManager.DIM();
        float sizeCollY = cellPrefab.GetSizeCellY() * LevelManager.DIM();

        float centerPosX = (sizeCollX - cellPrefab.GetSizeCellX()) / 2;
        float centerPosY = (sizeCollY - cellPrefab.GetSizeCellY()) / 2;

        bCollider.size = new Vector3(sizeCollX, sizeCollY);
        bCollider.center = new Vector3(centerPosX, centerPosY);
    }

    public void ChangeCellToPlayer(int y, int x, int idPlayer) {
        renderBoard[y, x].ChangeCellToPlayer(idPlayer);
    }

    public void ChangeColor(Color c) {
        for (int y = 0; y < LevelManager.DIM(); y++)
            for (int x = 0; x < LevelManager.DIM(); x++)
                renderBoard[y, x].ChangeColor(c);
    }
}
