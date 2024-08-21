using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWalletScript : MonoBehaviour
{
    public GameObject claimPrompt;
    // Start is called before the first frame update
    public GameObject connectPrompt;
    Player player;
    void Start()
    {
        claimPrompt.SetActive(false);
        player = GameObject.Find("Player").GetComponent<Player>();
        connectPrompt.SetActive(true); 
    }

    //Connect ket noi vi thanh cong
    public void ConnectWallet()
    {
        player.isStart = true;
        connectPrompt.SetActive(false);
    }

    // ket noi vi
    public void ShowConnectPrompt()
    {
        connectPrompt.SetActive(true);
        player.isStart = false;
    }
}
