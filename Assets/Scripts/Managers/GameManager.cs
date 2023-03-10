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

    private void Awake()
    {
        m_currScene = SceneManager.GetActiveScene().name;
        

        m_scenes = new Queue<string>();

        foreach (var scene in initialScenesToEnqueue)
        {
            m_scenes.Enqueue(scene);
        }
    }

    private void Start()
    {
        if (m_visualCanvas == null) { Debug.LogError("Error: VisualCanvas is Missing as an instance in GameManger!"); }
        AddSceneToQueue(initialScenesToEnqueue);

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



    #region Scene Management

    public void TransitionToNextScene()
    {
        m_currScene = m_scenes.Dequeue();
        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(m_visualCanvas);
        DontDestroyOnLoad(m_tinker);
        DontDestroyOnLoad(m_ashe);
        m_doorTransition.CloseDoors();
        StartCoroutine(DelaySceneLoad(2f));

    }

    public void LoadToScene(string scene)
    {
        m_doorTransition.CloseDoors();
        StartCoroutine(DelaySceneLoad(2f));
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

    #endregion

    private IEnumerator DelaySceneLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadSceneAsync(m_currScene != null ? m_currScene : "Hub");
    }
}
