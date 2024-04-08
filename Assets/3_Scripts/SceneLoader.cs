using System.Collections;
using System.Collections.Generic;
using Tayx.Graphy.Fps;
using Tayx.Graphy.Utils.NumString;
using Tayx.Graphy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private bool SdkCallback1Result = false;
    [SerializeField] private bool SdkCallback2Result = false;

    public void Start()
    {
        StartCoroutine(StartGameScene());
    }

    IEnumerator StartGameScene()
    {
        AsyncOperation loadOp = SceneManager.LoadSceneAsync("GameScene");
        loadOp.allowSceneActivation = false;

        yield return SdkCallback();

        StartCoroutine(SdkCallback1());
        StartCoroutine(SdkCallback2());

        while (loadOp.progress < 0.9f && !SdkCallback1Result && !SdkCallback2Result)
            yield return null;
        loadOp.allowSceneActivation = true;
    }

    IEnumerator SdkCallback()
    {
        yield return null;
    }
    IEnumerator SdkCallback1()
    {
        yield return null;
        SdkCallback1Result = true;
    }
    IEnumerator SdkCallback2()
    {
        yield return null;
        SdkCallback2Result = true;
    }
}
