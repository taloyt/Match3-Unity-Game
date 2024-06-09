using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public event Action<eStateGame> StateChangedAction = delegate { };

    public enum eLevelMode
    {
        TIMER,
        MOVES
    }
    eLevelMode currentLevelMode = eLevelMode.TIMER;

    public enum eStateGame
    {
        SETUP,
        MAIN_MENU,
        GAME_STARTED,
        PAUSE,
        GAME_OVER,
    }
    private eStateGame m_state;
    public eStateGame State
    {
        get { return m_state; }
        private set
        {
            m_state = value;

            StateChangedAction(m_state);
        }
    }


    public GameSettings m_gameSettings;
    public List<BoardSkin> m_boardSkins;
    public int m_boardSkinIndex = 0;
    public GameObject PrefabBoardController;

    private BoardController m_boardController;

    [SerializeField]
    private UIMainManager m_uiMenu;

    private LevelCondition m_levelCondition;

    private void Awake()
    {
        State = eStateGame.SETUP;
        m_uiMenu.Setup(this);
    }

    void Start()
    {
        State = eStateGame.MAIN_MENU;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_boardController != null) m_boardController.Update();
    }


    internal void SetState(eStateGame state)
    {
        State = state;

        if(State == eStateGame.PAUSE)
        {
            DOTween.PauseAll();
        }
        else
        {
            DOTween.PlayAll();
        }
    }

    public void LoadLevel(eLevelMode mode)
    {
        currentLevelMode = mode;
        GameObject boardController = Instantiate(PrefabBoardController);
        m_boardController = boardController.GetComponent<BoardController>();
        m_boardController.StartGame(this, m_gameSettings, m_boardSkins[m_boardSkinIndex]);

        if (mode == eLevelMode.MOVES)
        {
            m_levelCondition = this.gameObject.AddComponent<LevelMoves>();
            m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), m_boardController);
        }
        else if (mode == eLevelMode.TIMER)
        {
            m_levelCondition = this.gameObject.AddComponent<LevelTime>();
            m_levelCondition.Setup(m_gameSettings.LevelMoves, m_uiMenu.GetLevelConditionView(), this);
        }

        m_levelCondition.ConditionCompleteEvent += GameOver;

        State = eStateGame.GAME_STARTED;
    }

    public void GameOver()
    {
        StartCoroutine(WaitBoardController());
    }

    public void SetBoardSkin(Dropdown dropdown) {
        m_boardSkinIndex = dropdown.value;
    }

    internal void ClearLevel()
    {
        if (m_boardController)
        {
            m_boardController.Clear();
            Destroy(m_boardController.gameObject);
            m_boardController = null;
        }
    }

    public void RestartGame()
    {
        StartCoroutine(WaitRestartGame());
    }

    private WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
    private WaitForSeconds wait1s = new WaitForSeconds(1);
    private IEnumerator WaitBoardController()
    {
        while (m_boardController.IsBusy)
        {
            yield return waitForEndOfFrame;
        }

        yield return wait1s;

        State = eStateGame.GAME_OVER;

        if (m_levelCondition != null)
        {
            m_levelCondition.ConditionCompleteEvent -= GameOver;

            Destroy(m_levelCondition);
            m_levelCondition = null;
        }
    }

    private IEnumerator WaitRestartGame()
    {
        while (m_boardController.IsBusy)
        {
            yield return waitForEndOfFrame;
        }

        ClearLevel();
        LoadLevel(currentLevelMode);
    }
}
