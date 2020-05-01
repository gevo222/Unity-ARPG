using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Gets instance of Player
public class Player : MonoBehaviour
{

    #region Singleton

    public static Player instance;

    void Awake()
    {
        instance = this;
    }

    #endregion

}