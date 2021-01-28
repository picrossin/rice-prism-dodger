using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreenInputManager : MonoBehaviour
{
    [SerializeField] private Animator transition;

    private bool _started;
    
    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown("joystick button 7")) && !_started)
        {
            StartCoroutine(StartGame());
        }
    }

    private IEnumerator StartGame()
    {
        transition.SetTrigger("End");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
