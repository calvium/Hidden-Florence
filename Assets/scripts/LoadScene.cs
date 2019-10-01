using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    public Animator animator;
    private string nextScene;

    public void Load(string sceneName) {
        if (animator)
        {
            animator.SetTrigger("end");
            nextScene = sceneName;
        } else {
            SceneManager.LoadScene(sceneName);
        }

    }

    public void DoTransition() {
        SceneManager.LoadScene(nextScene);
        nextScene = null;
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
