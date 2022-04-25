using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using SKCell;
public class LevelManager : MonoSingleton<LevelManager>
{
    //The number of levels we want to create
    const int MAX_LEVEL = 5;
    //The bool used to control animators
    const string ANIM_APPEAR_BOOL = "Appear";

    //Current playing level, start from 1
    public int curLevel;

    //References
    [SerializeField] Animator transitionAnim; //The black screen animator

    private void Start()
    {
        curLevel = 1;
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadLevel(2);
        }

    }

    // Added by Jack
    // For easy use in GridItem.cs
    public void LoadNextLevel()
    {
        Scene curScene = SceneManager.GetActiveScene();
        int curIndex = curScene.buildIndex;
        if(curIndex < SceneManager.sceneCountInBuildSettings - 1)
        {
            LoadLevel(curIndex + 1);
        }
    }

    public void LoadLevel(int level)
    {
        //Start transition
        SetTransition(true);

        //Wait for 1 sec to let the transition screen fade in
        CommonUtils.InvokeAction(1f, () =>
        {
            curLevel = level;

            SceneManager.LoadScene($"Level{level}"); //Actually load the level

            //Turn off the black screen after the scene is loaded
            CommonUtils.InvokeAction(1f, () =>
            {
                SetTransition(false);
            });
        });
    }

    /// <summary>
    /// Set the between-scene transition screen (fading black screen etc.)
    /// </summary>
    /// <param name="appear">if true, put the black screen on</param>
    public void SetTransition(bool appear)
    {
        transitionAnim.SetBool(ANIM_APPEAR_BOOL, appear);
    }

}
