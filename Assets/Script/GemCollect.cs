using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollect : MonoBehaviour
{
    public int gems = 0;

    Player player;

    public GameObject claimPrompt;

    [SerializeField] private TMPro.TextMeshProUGUI gemScoreText;


    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);
            gems++;
            Debug.Log("Gems count: "+gems);
            gemScoreText.text = gems.ToString();
        }
    }

    void Update()
    {
        if (player.isDead)
        {
            claimPrompt.SetActive(true);
        }
    }
}
