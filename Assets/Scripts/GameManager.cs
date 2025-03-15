using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    #region Singleton
    private static GameManager _instance;
    public static GameManager Instance() { return _instance; }
    #endregion

    public LevelController _levelController;


    private void Awake() {
        if (_instance != null) {
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
    private void TransferInformation() {
        _instance._levelController = _levelController;
        Debug.Log("transfer information");
    }

    /// <summary>
    /// Metodo de inicializacion de la IA, los turnos y asignacion de IDs
    /// </summary>
    private void Init() {
        if (_levelController == null) {
            Debug.LogWarning("Board manager no asignado en el editor");
        }

        string gameMode = PlayerPrefs.GetString("GameMode"); 
        Debug.Log("Modo de juego seleccionado: " + gameMode); 

        ConfigureGameMode(gameMode);
    }

    private void ConfigureGameMode(string gameMode)
    {
        if (gameMode == "Normal") 
        {
            Debug.Log("Normal");
            _levelController._simpleGame = true;
        }
        else if (gameMode == "Reloaded")
        {
            Debug.Log("Reloaded");
            _levelController._simpleGame = false;
        }
        Debug.Log("simple game: " + _levelController._simpleGame);
        PlayerPrefs.DeleteKey("GameMode");
    }


    // Update is called once per frame
    void Update() {
        if (_levelController.WhoWin() != Board.Logic.CellContent.EMPTY)
            _levelController.enabled = false;
    }//fin Update()
}
