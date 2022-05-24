using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicControllerScript : MonoBehaviour
{
    public static MainMenuMusicControllerScript instance; 

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); 

        if (instance == null) 
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
