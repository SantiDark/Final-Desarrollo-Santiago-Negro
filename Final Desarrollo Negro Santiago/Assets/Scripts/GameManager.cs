using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public PlayerController player;

    Vector3 playerSpawnPos;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (!player)
            player = FindFirstObjectByType<PlayerController>();

        if (player)
            playerSpawnPos = player.transform.position;
    }

    void Update()
    {
        // Respawn del jugador con valores iniciales
        if (Input.GetKeyDown(KeyCode.F1))
        {
            RespawnPlayer();
        }
    }

    public void OnPlayerDied()
    {
        // "Muere" => lo desactivamos (más seguro que Destroy para poder respawnear)
        if (player)
            player.gameObject.SetActive(false);
    }

    void RespawnPlayer()
    {
        if (!player) return;

        player.gameObject.SetActive(true);
        player.RespawnAt(playerSpawnPos);
    }
}
