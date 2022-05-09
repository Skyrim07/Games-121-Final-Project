using System;
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

    public Action onScreenFaded;

    public AudioSource src;
    public AudioSource src2;
    public AudioClip menu;
    public AudioClip w1;
    public AudioClip die;
    public AudioClip undo;
    public AudioClip reset;
    public AudioClip nlevel;
    public AudioClip win;


    private bool playonce;

    //References
    [SerializeField] Animator transitionAnim; //The black screen animator

    private void Start()
    {
        curLevel = 1;
        
        src.Play();
    }

    public void WalkingSound()
    {
        src2.PlayOneShot(w1, 1f);
    }

    public void NewLevelSound()
    {
        src2.PlayOneShot(nlevel, 1f);
    }

    public void WinSound()
    {
        src2.PlayOneShot(win, 1f);
    }

    public void ResetSound()
    {
        src2.PlayOneShot(reset, 1f);
    }

    public void UndoSound()
    {
        src2.PlayOneShot(undo, 0.7f);
    }

    public void DieSound()
    {
        if (playonce)
        {
            src2.PlayOneShot(die, 1f);
        }

        playonce = false;
    }

    private void Update()
    {
        playonce = true;
        if (Input.GetKeyDown(KeyCode.K))
        {
            LoadNextLevel();

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
            SceneManager.LoadScene(curLevel);
            
            //SceneManager.LoadScene($"Level{level}"); 

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

        //CommonUtils.InvokeAction(0.5f, () =>
        //{
        //    onScreenFaded.Invoke();
        //});
    }

}
