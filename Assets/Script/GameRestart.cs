using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRestart : MonoBehaviour
{
    // Start is called before the first frame update
    public void RestartButton()
    {
        SceneManager.LoadScene("FirstSence");
    }

    public void Quit()
    {
        Debug.Log("Game is exiting...");
        Application.Quit();
    }
}
