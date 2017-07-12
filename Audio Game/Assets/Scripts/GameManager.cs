using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// @author Ryan Oh
/// 
/// 
/// </summary>
public class GameManager : MonoBehaviour {

    FloorVisualizer floorVisualizer;

    void Start()
    {
        InitVisualizers();

    }

    void InitVisualizers()
    {
        floorVisualizer = gameObject.AddComponent<FloorVisualizer>();
    }
}
