using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Contador : MonoBehaviour
{
    public int contador = 0;
    public TextMeshProUGUI textElement;
    private string mensaje = "Enemigos restantes: ";
    // Start is called before the first frame update
    void Start()
    {
        textElement = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        textElement.SetText(mensaje + contador, true);
        if (contador == 0)
        {
            endGame();
        }
    }

    void endGame()
    {
        Time.timeScale = 0;
        SceneManager.LoadScene(2);//Reemplazar por la escena requerida
    }
}
