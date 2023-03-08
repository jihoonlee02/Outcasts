using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
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
    [SerializeField] private Pawn[] m_pawnsToControl;

    [Header("Important Game Comoponents")]
    [SerializeField] private GameObject m_visualCanvas;

    [Header("Level Management")]
    [SerializeField] private RoomManager m_roomManager;
    [SerializeField] private LevelManager m_levelManager;

    public RoomManager CurrRoomManager => m_roomManager;
    private Queue<string> m_scenes;
    [SerializeField] private string m_currScene;
    public Pawn[] PlayerPawns => m_pawnsToControl;

    [Header("Dev Settings")]
    [SerializeField] private string[] initialScenesToEnqueue;

    private void Awake()
    {
        m_currScene = SceneManager.GetActiveScene().ToString();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (m_visualCanvas == null) { Debug.LogError("Error: VisualCanvas is Missing as an instance in GameManger!"); }
        AddSceneToQueue(initialScenesToEnqueue);
        foreach (Pawn pawn in m_pawnsToControl)
            pawn.gameObject.SetActive(false);
    }

    private int count = 0;
    public void SetPlayerControllerToPawn(PlayerInput pi)
    {
        //Coupled
        if (count > 1) return;
        pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        m_pawnsToControl[count].gameObject.SetActive(true);
        count++;
    }

    public void OpenUpPlayerSelecitonMenu(PlayerInput pi)
    {
        //Coupled
        if (m_pawnsToControl.Length - count == 1) SetPlayerControllerToPawn(pi);
        if (m_pawnsToControl.Length - count <= 0) return;
        m_visualCanvas.SetActive(true);
        pi.GetComponent<PlayerController>().ControlPawn(m_pawnsToControl[count]);
        pi.gameObject.name = m_pawnsToControl[count].name + " Player";
        m_pawnsToControl[count].gameObject.SetActive(true);

    }

    #region Scene Management

    public void TransitionToNextScene()
    {
        m_currScene = m_scenes.Dequeue();
        SceneManager.LoadSceneAsync(m_currScene != null ? m_currScene : "Hub");
    }

    public void LoadToScene(string scene)
    {
        SceneManager.LoadSceneAsync(scene != null ? scene : "Hub");
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
}
