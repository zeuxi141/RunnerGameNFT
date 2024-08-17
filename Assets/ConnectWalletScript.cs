using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectWalletScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject connectPrompt;
    Player player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        connectPrompt.SetActive(true); 
    }

    //Connect Wallet
    public void ConnectWallet()
    {
        player.isStart = true;
        connectPrompt.SetActive(false);
    }

    public void ShowConnectPrompt()
    {
        connectPrompt.SetActive(true);
        player.isStart = false;
    }
}
