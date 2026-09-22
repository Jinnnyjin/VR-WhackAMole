using UnityEngine;

// 게임 상태에 따라 BGM 재생/정지 및 게임오버 효과음을 담당
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip gameOverClip;

    private AudioSource bgmSource;

    private void Awake()
    {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
    }

    private void OnEnable()
    {
        gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            if (bgmClip == null)
                return;

            bgmSource.clip = bgmClip;
            bgmSource.Play();
        }
        else if (state == GameState.GameOver)
        {
            bgmSource.Stop();

            if (gameOverClip != null)
                AudioSource.PlayClipAtPoint(gameOverClip, transform.position);
        }
    }
}
