using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance()
    {
        return _instance;
    }
    #endregion

    public LevelManager _levelManager;


    private void Awake()
    {
        if (_instance != null)
        {
            TransferInformation();
            DestroyImmediate(gameObject);
            return;
        }

        _instance = this;
        Init();

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Transfiere la informacion de la instancia nueva del GameManager de la escena que se acaba de cargar a la instancia antigua
    /// </summary>
    private void TransferInformation()
    {
        _instance._levelManager = _levelManager;
    }

    /// <summary>
    /// Metodo de inicializacion de la IA, los turnos y asignacion de IDs
    /// </summary>
    private void Init()
    {
        if (_levelManager == null)
        {
            Debug.LogWarning("Board manager no asignado en el editor");
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_levelManager.WhoWin() != -1)
            _levelManager.enabled = false;
    }//fin Update()


}
