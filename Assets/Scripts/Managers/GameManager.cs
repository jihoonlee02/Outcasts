using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    Debug.LogError("GameManager Prefab is required in level scene!");
                }
            }

            return instance;
        }
    }
    #endregion 

    [Header("Player Pawn Management")]
    [SerializeField] private PlayerInputManager m_pim;
    [SerializeField] private Pawn m_tinker;
    [SerializeField] private Pawn m_ashe;
    public Pawn Tinker => m_tinker;
    public Pawn Ashe => m_ashe;

    [Header("UI Comoponents")]
    [SerializeField] private GameObject m_visualCanvas;
    [SerializeField] private CharacterSelection m_characterSelection;
    [SerializeField] private CharacterSelection m_characterSelection2;
    [SerializeField] private DoorTransition m_doorTransition;

    public DoorTransition DoorTransition => m_doorTransition;

    [Header("Level Management")]
    [SerializeField] private RoomManager m_roomManager;
    [SerializeField] private LevelManager m_levelManager;
    
    public RoomManager CurrRoomManager => m_roomManager;
    public LevelManager LevelManager
    {
        get { return m_levelManager; }
        set { m_levelManager = value; }
    }
    private Queue<string> m_scenes;
    [SerializeField] private string m_currScene;
    

    [Header("Dev Settings")]
    [SerializeField] private string[] initialScenesToEnqueue;
    

    private bool isPaused = false;
    private bool isTesting = false;
    public bool IsTesting
    {
        set { isTesting = value; }
    }
    private void Awake()
    {
        m_currScene = SceneManager.GetActiveScene().name;
        
        m_scenes = new Queue<string>();

        AddSceneToQueue(initialScenesToEnqueue);

        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void Start()
    {
        if (m_visualCanvas == null) { Debug.LogError("Error: VisualCanvas is Missing as an instance in GameManger!"); }

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_visualCanvas);
        DontDestroyOnLoad(m_tinker);
        DontDestroyOnLoad(m_ashe);
    }

    private bool control_tinker = false;
    private bool control_ashe = false;

    public void HandlePlayerControllerEnter(PlayerInput pi)
    {
        DontDestroyOnLoad(pi);
        if (!control_tinker && !control_ashe)
        {
            OpenUpPlayerSelecitonMenu(pi);
        } 
        else if (!control_ashe)
        {
            GivePawn(pi, m_ashe);
        } 
        else if (!control_tinker) 
        {
            GivePawn(pi, m_tinker);
        }
    }

    public void GivePawn(PlayerInput pi, Pawn pawn)
    {
        if (pawn == m_ashe)
        {
            control_ashe = true;
        }
        else if (pawn == m_tinker) 
        { 
            control_tinker = true;
        }
        pawn.PC = pi.GetComponent<PlayerController>();
        pawn.PC.ControlPawn(pawn);
        pawn.PC.EnablePawnControl();
        if (isTesting) pawn.PC.OnTesting();
    }

    public void HandlePlayerControllerExit(PlayerInput pi)
    {
        var returnPawn = pi.GetComponent<PlayerController>().ControlledPawn;
        if (returnPawn == m_tinker)
        {
            control_tinker = false;
            m_characterSelection.Select_Tinker.interactable = true;
        }
        else if (returnPawn == m_ashe)
        {
            control_ashe = false;
            m_characterSelection.Select_Ashe.interactable = true;
        }

        pi.GetComponent<PlayerController>().ControlPawn(null);
    }

    public void OpenUpPlayerSelecitonMenu(PlayerInput pi)
    {
        var pc = pi.GetComponent<PlayerController>();
        // Thank you for being nice PC
        pc.PlayerInput.actions["Pause"].Disable();

        //This could have been just done through an array, but as we are only having two pawns to keep track of
        //This will do fine no matter its icckyniss
        if (pi.playerIndex == 0)
        {
            m_characterSelection.Display(false, pi.playerIndex + 1);
            m_characterSelection.Select_Tinker.onClick.AddListener(delegate { GivePawn(pi, m_tinker); });
            m_characterSelection.Select_Ashe.onClick.AddListener(delegate { GivePawn(pi, m_ashe); });

            pc.GetComponentInChildren<MultiplayerEventSystem>().playerRoot = m_characterSelection.gameObject;
            pc.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(m_characterSelection.Select_Tinker.gameObject);
        }
        else
        {
            m_characterSelection2.Display(true, pi.playerIndex + 1);
            m_characterSelection2.Select_Tinker.onClick.AddListener(delegate { GivePawn(pi, m_tinker); });
            m_characterSelection2.Select_Ashe.onClick.AddListener(delegate { GivePawn(pi, m_ashe); });

            pc.GetComponentInChildren<MultiplayerEventSystem>().playerRoot = m_characterSelection2.gameObject;
            pc.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(m_characterSelection2.Select_Tinker.gameObject);
        }
    }

    public void TogglePause(PlayerController pc = null)
    {
        Debug.Log(isPaused);
        if (isPaused) UnPauseGame();
        else PauseGame(pc);
    }
    public void PauseGame(PlayerController pc = null)
    {
        isPaused = true;
        Tinker.PC?.DisablePawnControl();
        Ashe.PC?.DisablePawnControl();
        UIManager.Instance.OpenPauseMenu(pc);
    }

    public void UnPauseGame()
    {
        isPaused = false;
        Tinker.PC?.EnablePawnControl();
        Ashe.PC?.EnablePawnControl();
        UIManager.Instance.ClosePauseMenu();
    }

    #region Scene Management


    public void LoadToScene(string scene)
    {
        m_currScene = scene;
        m_doorTransition.CloseDoors();
        StartCoroutine(LoadSceneWithDelay(1.2f));
    }
    public void TransitionToNextScene()
    {
        LoadToScene(m_currScene = m_scenes.Count > 0 ? m_scenes.Dequeue() : "Hub");
    }

    public void ReloadCurrentScene()
    {
        LoadToScene(m_currScene);
    }

    public void AddSceneToQueue(string scene)
    {
        m_scenes.Enqueue(scene);
    }

    public void AddSceneToQueue(string[] scenes)
    {
        foreach (string scene in scenes)
            m_scenes.Enqueue(scene);
    }

    public void ClearSceneQueue()
    {
        m_scenes.Clear();
    }

    private void OnSceneChange(Scene current, Scene next)
    {
        //Dev Bs only
        if (m_currScene == "Hub")
        {
            ClearSceneQueue();
            AddSceneToQueue(initialScenesToEnqueue);
        }
        DontDestroyOnLoad(transform.parent);
    }

    private IEnumerator LoadSceneWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadSceneAsync(m_currScene != null ? m_currScene : "Hub");
    } 

    #endregion
}
