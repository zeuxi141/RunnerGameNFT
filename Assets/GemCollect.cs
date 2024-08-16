using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemCollect : MonoBehaviour
{
    public int gems = 0;
    [SerializeField] private TMPro.TextMeshProUGUI gemScoreText;
  
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
}
