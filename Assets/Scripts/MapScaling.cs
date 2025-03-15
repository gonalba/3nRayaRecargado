using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScaling : MonoBehaviour
{

    private int _screenPixelsWidth = 0;
    private int _screenPixelsHeight = 0;

    public float boardSize = 0;

    public LevelController levelController;


    // Start is called before the first frame update
    void Start()
    {
        if (levelController == null)
        {
            Debug.LogError("levelController no inicializado");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_screenPixelsWidth != Screen.width || _screenPixelsHeight != Screen.height)
        {
            MapRescaling();
        }
    }

    /// <summary>
    /// Aplica al LevelController un escalado y una transformacion segun la resolucion de la pantalla
    /// </summary>
    private void MapRescaling()
    {
        // resolucion de la pantalla en pixeles
        _screenPixelsWidth = Screen.width;
        _screenPixelsHeight = Screen.height;

        // resolucion de la pantalla en unidades de Unity
        float screenUnityHeight = Camera.main.orthographicSize * 2;
        float screenUnityWidth = (_screenPixelsWidth * screenUnityHeight) / _screenPixelsHeight;

        // tamaño del board tanto ancho como alto (3*3 + separacion entre casillas)
        Debug.Log("W: " + _screenPixelsWidth + " H: " + _screenPixelsHeight);

        // calculamos el factor de escala al que hay que escalar
        float scaleFactorW = screenUnityWidth / boardSize;
        float scaleFactorH = screenUnityHeight / boardSize;
        float scaleFactor = Mathf.Min(scaleFactorW, scaleFactorH);

        levelController.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // Centramos la cámara
        float pos = (boardSize - 1) / 2;
        Camera.main.transform.position = new Vector3(pos * scaleFactor, pos * scaleFactor, Camera.main.transform.position.z);
        //Vector3 newPos = new Vector3(pos * _scaleFactor, pos * _scaleFactor, Camera.main.transform.position.z);

        // Movemos la cámara
        //Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPos, Time.deltaTime * smoothLevel);
        //SmoothLookAt();
    }


    /// <summary>
    /// Permite definir la cantidad de suavizado.
    /// Cuanto más grande, más rápido va. Con 0 no se mueve...
    /// </summary>
    //public float smoothLevel = 1.0f;

    /// <summary>
    /// Cambia la rotación (con el factor de suavizado) para que se mire
    /// hacia el jugador.
    /// </summary>
    /// (Extraído de un tutorial de Unity.)
    //void SmoothLookAt()
    //{
    //    // Create a vector from the camera towards the player.
    //    Vector3 relPlayerPosition = Camera.main.transform.position - Camera.main.transform.position + 0.5f * Vector3.up;

    //    // Create a rotation based on the relative position of the player being the forward vector.
    //    Quaternion lookAtRotation = Quaternion.LookRotation(relPlayerPosition, Vector3.up);

    //    // Lerp the camera's rotation between it's current rotation and the rotation that looks at the player.
    //    Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, lookAtRotation, smoothLevel * Time.deltaTime);
    //}
}
