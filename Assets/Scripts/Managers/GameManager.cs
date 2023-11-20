using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] private TinkerPawn m_tinker;
    [SerializeField] private AshePawn m_ashe;
    private PlayerController m_tinkerPC;
    private PlayerController m_ashePC;
    private SoloController m_SC;
    public TinkerPawn Tinker => m_tinker;
    public AshePawn Ashe => m_ashe;
    public PlayerController TinkerPC => m_tinkerPC;
    public PlayerController AshePC => m_ashePC;
    public SoloController SC => m_SC;

    [Header("UI Comoponents")]
    [SerializeField] private GameObject m_visualCanvas;
    [SerializeField] private CharacterSelection m_characterSelection;
    [SerializeField] private CharacterSelection m_characterSelection2;
    [SerializeField] private DoorTransition m_doorTransition;
    [SerializeField] private CanvasGroup m_fadeTransition;
    [SerializeField] private TextMeshProUGUI m_onScreenMessage; 

    public DoorTransition DoorTransition => m_doorTransition;
    public CanvasGroup FadeTransition => m_fadeTransition;

    [Header("Level Management")]
    [SerializeField] private Transform m_levelThings;
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
    private void Awake()
    {
        // Just in case the gameManager finds itself in the same scene
        if (instance != null && FindObjectOfType<GameManager>() != null)
        {
            Destroy(gameObject);
            return;
        }
        m_currScene = SceneManager.GetActiveScene().name;
        isPaused = false;

        m_scenes = new Queue<string>();

        AddSceneToQueue(initialScenesToEnqueue);

        SceneManager.activeSceneChanged += OnSceneChange;
    }

    private void Start()
    {
        if (m_visualCanvas == null) { Debug.LogError("Error: VisualCanvas is Missing as an instance in GameManger!"); }

        // Initial Unaffected Dont Destroys
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_visualCanvas);
        DontDestroyOnLoad(transform.parent);
    }

    public void HandlePlayerControllerEnter(PlayerInput pi)
    {
        DontDestroyOnLoad(pi);

        // Player Controller Moment
        var pc = pi.GetComponent<PlayerController>();

        if (pc != null)
        { 
            if (m_tinkerPC == null && m_ashePC == null)
            {
                OpenUpPlayerSelecitonMenu(pi);
            }
            else if (m_ashePC == null)
            {
                m_ashePC = pc;
                GivePawn(m_ashePC, m_ashe);
            }
            else if (m_tinkerPC == null)
            {
                m_tinkerPC = pc;
                GivePawn(m_tinkerPC, m_tinker);
            }
            return;
        } 
        
        // Solo Controller Moment
        var sc = pi.GetComponent<SoloController>();

        if (sc != null)
        {
            sc.ControlTnAPawns((TinkerPawn)m_tinker, (AshePawn)m_ashe);
            m_SC = sc;   
        }
    }

    public void GivePawn(PlayerController pc, Pawn pawn)
    {
        pc.PlayerInput.actions["Pause"].Disable();
        pc.ControlPawn(pawn);
        pc.EnablePawnControl();
    }

    public void HandlePlayerControllerExit(PlayerInput pi)
    {
        var pc = pi.GetComponent<PlayerController>();

        if (pc != null)
        {
            if (pc.ControlledPawn == m_tinker)
            {
                m_ashePC = null;
                m_characterSelection.Select_Tinker.interactable = true;
            }
            else if (pc.ControlledPawn == m_ashe)
            {
                m_tinkerPC = null;
                m_characterSelection.Select_Ashe.interactable = true;
            }

            pc.ControlPawn(null);
            Destroy(pc.gameObject);
            return;
        }

        var sc = pi.GetComponent<SoloController>();
        
        if (sc != null)
        {
            // TODO: Fill out details corresponding to solo controller input
            m_SC = null;
            Destroy(sc.gameObject);
        }
    }

    public void OpenUpPlayerSelecitonMenu(PlayerInput pi)
    {
        var pc = pi.GetComponent<PlayerController>();
        // Thank you for being nice PC
        //

        //This could have been just done through an array, but as we are only having two pawns to keep track of
        //This will do fine no matter its icckyniss
        if (pi.playerIndex == 0)
        {
            m_characterSelection.Display(false, pi.playerIndex + 1);
            m_characterSelection.Select_Tinker.onClick.AddListener(delegate {
                m_tinkerPC = pi.GetComponent<PlayerController>(); 
                GivePawn(m_tinkerPC, m_tinker); 
            });
            m_characterSelection.Select_Ashe.onClick.AddListener(delegate {
                m_ashePC = pi.GetComponent<PlayerController>();
                GivePawn(m_ashePC, m_ashe);
            });

            pc.GetComponentInChildren<MultiplayerEventSystem>().playerRoot = m_characterSelection.gameObject;
            pc.GetComponentInChildren<MultiplayerEventSystem>().SetSelectedGameObject(m_characterSelection.Select_Tinker.gameObject);
        }
        else
        {
            m_characterSelection2.Display(true, pi.playerIndex + 1);
            m_characterSelection2.Select_Tinker.onClick.AddListener(delegate {
                m_tinkerPC = pi.GetComponent<PlayerController>();
                GivePawn(m_tinkerPC, m_tinker);
            });
            m_characterSelection2.Select_Ashe.onClick.AddListener(delegate {
                m_ashePC = pi.GetComponent<PlayerController>();
                GivePawn(m_ashePC, m_ashe);
            });

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
        m_tinkerPC?.DisablePawnControl();
        m_ashePC?.DisablePawnControl();
        //m_SC?.DisablePawnControl();
        UIManager.Instance.OpenPauseMenu(pc);
    }

    public void UnPauseGame()
    {
        isPaused = false;
        m_tinkerPC?.EnablePawnControl();
        m_ashePC?.EnablePawnControl();
        //m_SC?.EnablePawnControl();
        UIManager.Instance.ClosePauseMenu();
    }

    #region Scene Management


    public void LoadToScene(string scene)
    {
        m_currScene = scene;
        m_doorTransition.CloseDoors();
        m_ashe.IsLifting = false;
        m_tinker.IsHeld = false;
        m_tinker.transform.SetParent(m_levelThings, true);
        m_ashe.transform.SetParent(m_levelThings, true);
        StartCoroutine(LoadSceneWithDelay(1.2f));
    }
    public void TransitionToNextScene()
    {
        LoadToScene(m_currScene = m_scenes.Count > 0 ? m_scenes.Dequeue() : "Hub");
    }
    public void QuitToMainMenu()
    {
        UnPauseGame();
        if (m_ashePC != null)
        {
            //Debug.Log("Ashe Pre: " + m_ashePC.ControlledPawn.name);
            //Debug.Log("Tinker Pre: " + m_tinkerPC.ControlledPawn.name);
            //Debug.Log("Destroied Ashe");
            m_ashePC.ControlPawn(null);
            //Destroy(m_ashePC.gameObject);
            //Debug.Log("Ashe: " + m_ashePC != null ? m_ashePC.ControlledPawn.name : "");
            //Debug.Log("Tinker: " + m_tinkerPC != null ? m_tinkerPC.ControlledPawn.name : "");
            m_ashePC = null;
        }
        if (m_tinkerPC != null)
        {
            Debug.Log("Destroied Tinker");
            //Destroy(m_tinkerPC.gameObject);
            m_tinkerPC.ControlPawn(null);
            m_tinkerPC = null;
        }
        if (m_SC != null)
        {
            Destroy(m_SC.gameObject);
            m_SC = null;
        }
        LoadToScene("MainMenu");
    }
    public void QuitToDesktop()
    {
        Application.Quit();
    }

    public void ReloadCurrentScene()
    {
        UnPauseGame();
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
    public void DisplayOnScreenMessage(string text)
    {
        m_onScreenMessage.text = text;
        m_onScreenMessage.GetComponent<Animator>().Play("TextScroll");
    }
    private void OnSceneChange(Scene current, Scene next)
    {
        //Dev Bs only
        Debug.Log("Scene Change is invoked");
        DialogueManager.Instance.StopDialogue();
        if (m_currScene == "hub")
        {
            ChestTracker.Instance.ResetChestCount();
        }

        if (next.name == "MainMenu") {
            Destroy(transform.parent.gameObject);
            return;
        }
        DontDestroyOnLoad(m_tinker);
        DontDestroyOnLoad(m_ashe);
    }

    private IEnumerator LoadSceneWithDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadSceneAsync(m_currScene);
        m_tinker.transform.SetParent(m_levelThings, true);
        m_ashe.transform.SetParent(m_levelThings, true);
        //This doesn't do what you think it does
        //if (!SceneManager.GetSceneByName(m_currScene).IsValid())
        //{
        //    SceneManager.LoadSceneAsync("Hub");
        //} 
    } 

    #endregion
}
