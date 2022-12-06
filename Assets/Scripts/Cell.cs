using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public GameObject empty = null;
    public GameObject x = null;
    public GameObject o = null;

    private void Start()
    {
        if (empty == null || x == null || o == null)
        {
            Debug.LogError("Cell no tiene asignados los gameobjects");
            return;
        }

        empty.SetActive(true);
        x.SetActive(false);
        o.SetActive(false);
    }
}
