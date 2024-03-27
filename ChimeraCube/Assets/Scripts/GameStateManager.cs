using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField]
    KeyCode restartKey = KeyCode.R;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(restartKey))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
