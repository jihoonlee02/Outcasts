using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    //[SerializeField] private Pawn[] m_pawnsToControl;
    //public Pawn[] PlayerPawns => m_pawnsToControl;
    [SerializeField] private Pawn m_tinker;
    [SerializeField] private Pawn m_ashe;
    public Pawn Tinker => m_tinker;
    public Pawn Ashe => m_ashe;

    [Header("UI Comoponents")]
    [SerializeField] private GameObject m_visualCanvas;
    [SerializeField] private CharacterSelection m_characterSelection;
    [SerializeField] private DoorTransition m_doorTransition;
    [SerializeField] private GameObject m_pauseMenu;

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

    private void Awake()
    {
        m_currScene = SceneManager.GetActiveScene().name;
        
        m_scenes = new Queue<string>();

        AddSceneToQueue(initialScenesToEnqueue);
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
        //Coupled
        //if (count > 1) return;
        //pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        //pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        //m_pawnsToControl[count].gameObject.SetActive(true);
        //count++;
    }

    public void GivePawn(PlayerInput pi, Pawn pawn)
    {
        if (pawn == m_ashe) control_ashe = true;
        if (pawn == m_tinker) control_tinker = true;
        pi.GetComponent<PlayerController>().ControlPawn(pawn);
        
    }

    public void HandlePlayerControllerExit(PlayerInput pi)
    {
        var returnPawn = pi.GetComponent<PlayerController>().ControlledPawn;
        if (returnPawn == m_tinker)
        {
            control_tinker = false;
        }
        else if (returnPawn == m_ashe)
        {
            control_ashe = false;
        }

        pi.GetComponent<PlayerController>().ControlPawn(null);
    }

    public void OpenUpPlayerSelecitonMenu(PlayerInput pi)
    {
        m_characterSelection.Display();
        m_characterSelection.Select_Tinker.onClick.AddListener(delegate { GivePawn(pi, m_tinker); m_characterSelection.Hide(); });
        m_characterSelection.Select_Ashe.onClick.AddListener(delegate { GivePawn(pi, m_ashe); m_characterSelection.Hide(); });
        //Coupled
        //if (m_pawnsToControl.Length - count <= 1) SetPlayerControllerToPawn(pi);
        //if (m_pawnsToControl.Length - count <= 0) return;
        //m_visualCanvas.SetActive(true);
        //pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        //pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        //m_pawnsToControl[count].gameObject.SetActive(true);

    }

    public void TogglePause()
    {
        if (isPaused) UnPauseGame();
        else PauseGame();
    }
    public void PauseGame()
    {
        isPaused = true;
        m_pauseMenu.SetActive(true);
    }

    public void UnPauseGame()
    {
        isPaused = false;
        m_pauseMenu.SetActive(false);
    }



    #region Scene Management


    public void LoadToScene(string scene)
    {
        m_currScene = scene;
        //Dev Bs only
        if (m_currScene == "Hub")
        {
            ClearSceneQueue();
            AddSceneToQueue(new string[] { "Level1", "Level3" });
        }
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_visualCanvas);
        DontDestroyOnLoad(m_tinker);
        DontDestroyOnLoad(m_ashe);
        m_doorTransition.CloseDoors();
        StartCoroutine(DelaySceneLoad(2f));
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

    #endregion

    private IEnumerator DelaySceneLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadSceneAsync(m_currScene != null ? m_currScene : "Hub");
    }
}
