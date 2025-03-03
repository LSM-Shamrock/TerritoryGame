using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stage : MonoBehaviour
{


    public void StageClear()
    {
        var curScene = SceneManager.GetActiveScene();
        switch (curScene.name)
        {
            case "Stage1":
                SceneManager.LoadScene("Stage2");
                break;
            case "Stage2":
                SceneManager.LoadScene("BossStage");
                break;
        }
    }
    public void GameOver()
    {
        FindObjectOfType<GameOverUI>(true).Show();
    }
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Exit()
    {
        SceneManager.LoadScene("Menu");
    }
}
