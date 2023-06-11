using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private bool isPrologueSeen = false;
    private string currentScene = "Title";
    public string prefix = "";
    public void GameStart()
    {
        if(isPrologueSeen) MoveToIngame();
        else MoveToPrologueCutscene();
    }
    public void MoveToIngame()
    {
        ChangeScene("Ingame");
    }
    public void MoveToPrologueCutscene()
    {
        StartCoroutine(ChangePrologueScene());
    }
    public void MoveToTitle()
    {
        ChangeScene("Title");
    }
    public void MoveToTutorial()
    {
        ChangeScene("Tutorial");
    }

    public void GameOver()
    {
        if(currentScene != "Ingame") return;
        StartCoroutine(ChangeGameOverScene(prefix + "GameOver"));
    }
    public void GameClear()
    {
        if(currentScene != "Ingame") return;
        StartCoroutine(ChangeGameOverScene(prefix + "GameClear"));
    }

    private void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(prefix + sceneName);
        currentScene = sceneName;
    }

    private IEnumerator ChangePrologueScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();
        string toLoadSceneName = prefix + "Prologue";
        GameObject titleBGMRunner = GameObject.Find(Constants.TITLE_BGM_RUNNER);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(toLoadSceneName, LoadSceneMode.Additive);
        yield return new WaitUntil(() => asyncLoad.isDone);

        if(titleBGMRunner != null)
        {
            SceneManager.MoveGameObjectToScene(titleBGMRunner, SceneManager.GetSceneByName(toLoadSceneName));
        }
        SceneManager.MoveGameObjectToScene(titleBGMRunner, SceneManager.GetSceneByName(toLoadSceneName));
        SceneManager.UnloadSceneAsync(thisScene);

        currentScene = "Prologue";
        isPrologueSeen = true;
    }
    private IEnumerator ChangeGameOverScene(string toLoadScene)
    {
        IScoreManager scoreManager = ScoreManager.Instance;
        ScoreData score = scoreManager.scoreData;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(toLoadScene);
        yield return new WaitUntil(() => asyncLoad.isDone);
        GameObject resultUIObj = GameObject.Find(Constants.RESULT_UI_OBJECT);
        IResultManager resultManager = resultUIObj.GetComponent<IResultManager>();
        resultManager.SetScoreData(score);
        currentScene = toLoadScene;
    }
}