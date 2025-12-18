using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Respawn")]
    public Transform playerSpawn;

    bool paused;
    PlayerController player;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        FindPlayer();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();

        if (Input.GetKeyDown(KeyCode.F2))
            RestartScene();
    }

    public void OnPlayerDied()
    {
        // Respawn simple después de 1s
        Invoke(nameof(RespawnPlayer), 1f);
    }

    void RespawnPlayer()
    {
        FindPlayer();
        if (player == null || playerSpawn == null) return;

        player.RespawnAt(playerSpawn.position);
    }

    void FindPlayer()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.GetComponent<PlayerController>();
    }

    void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0f : 1f;
    }

    void RestartScene()
    {
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
}
