using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Reset();
    }

    void Reset()
    {
        // reset the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            // This reloads the currently active scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
