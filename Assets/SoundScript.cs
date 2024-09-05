using UnityEngine;

public class SoundScript : MonoBehaviour
{
    Player player;
    public AudioSource BackgroundSound;
    public AudioSource SoundEffect;

    public AudioClip backgroundOnMenu;
    public AudioClip backgroundGameStart;
    public AudioClip jump1Audio; // Âm thanh nhảy
    public AudioClip deathAudio;
    public AudioClip collectGold;

    bool isJumping = false; // Biến cờ để xác định người chơi đang nhảy
    private bool hasDied = false; // Cờ để kiểm tra đã chết

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleBackgroundMusic();
        HandleJumpSound();
        //HandleRunSound();
        HandleDeathSound();
    }

    private void HandleBackgroundMusic()
    {
        // Phát nhạc nền khi trò chơi bắt đầu
        if (player.isStart && !player.isDead)
        {
            hasDied = false; // Reset cờ chết khi trò chơi đang diễn ra
            if (BackgroundSound.clip != backgroundGameStart)
            {
                BackgroundSound.clip = backgroundGameStart;
                BackgroundSound.loop = true;
                BackgroundSound.Play();
            }
        }
        else if (!player.isDead)
        {
            if (BackgroundSound.clip != backgroundOnMenu)
            {
                BackgroundSound.clip = backgroundOnMenu;
                BackgroundSound.loop = true;
                BackgroundSound.Play();
            }
        }
    }

    private void HandleJumpSound()
    {
        // Kiểm tra trạng thái giữ phím Space để xác định nhảy
        if (Input.GetKeyDown(KeyCode.Space) && player.isGrounded)
        {
            isJumping = true; // Bắt đầu nhảy

            if (jump1Audio != null)
            {
                // Reset pitch trước khi phát âm thanh nhảy
                //SoundEffect.pitch = 1f;
                SoundEffect.PlayOneShot(jump1Audio); // Phát âm thanh nhảy
            }
            else
            {
                Debug.LogError("jump1Audio is not assigned.");
            }
        }

        // Kiểm tra người chơi đã tiếp đất sau khi nhảy
        if (player.isGrounded && isJumping)
        {
            isJumping = false; // Người chơi đã chạm đất, kết thúc nhảy
        }
    }

    //private void HandleRunSound()
    //{
    //    // Phát âm thanh chạy khi player.isGrounded = true, không nhảy, không chết và đang chơi
    //    if (player.isGrounded && !isJumping && !player.isDead && player.isStart)
    //    {
    //        /*float speedFactor = Mathf.Clamp(player.currentSpeed / 100f, 0.1f, 1f);*/ // Tăng tốc độ lặp khi tốc độ tăng
    //        //SoundEffect.pitch = 1f + Time.fixedDeltaTime;

    //        if (SoundEffect.clip != runAudio)
    //        {
    //            SoundEffect.clip = runAudio;
    //            SoundEffect.loop = true;
    //            SoundEffect.Play();
    //        }
    //    }
    //}

    private void HandleDeathSound()
    {
        // Phát âm thanh khi chết dừng nhạc nền
        if (player.isDead && !hasDied)
        {
            SoundEffect.Stop(); // Dừng âm thanh hiện tại trước khi phát âm thanh chết
            if (deathAudio != null)
            {
                SoundEffect.clip = deathAudio;
                SoundEffect.loop = false;
                SoundEffect.Play();
                BackgroundSound.Stop(); // Dừng nhạc nền
                hasDied = true; // Đặt cờ đã chết để âm thanh chỉ phát một lần
            }
            else
            {
                Debug.LogError("deathAudio is not assigned.");
            }
        }
    }
}
