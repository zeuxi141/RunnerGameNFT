using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Thirdweb;
using Unity.VisualScripting;

public class TokenScript : MonoBehaviour
{
    Player player;
    public GameObject NotClaimedState;
    public GameObject ClaimingState;
    public GameObject ClaimedState;

    private string Address;

    [Header("Gem collect info")]        
    [SerializeField] private TMPro.TextMeshProUGUI gemsEarnText;
    public GemCollect gemCollect;

    [SerializeField] private TMPro.TextMeshProUGUI tokenBalanceText;
    private int gemsToClaim;
    private const string DROP_ERC20_CONTRACT = "0xf035705cA72e6e726f0f6C2B5D130a55fA6360a7";


    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        NotClaimedState.SetActive(true);
        ClaimingState.SetActive(false);
        ClaimedState.SetActive(false);
    }

    private void Update()
    {
        gemsEarnText.text = "Gems Earned: " + gemCollect.gems.ToString();
        gemsToClaim = gemCollect.gems;

    }

    public async void GetTokenBalance()
    {
        try
        {
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            Contract contract = ThirdwebManager.Instance.SDK.GetContract(DROP_ERC20_CONTRACT);
            var data = await contract.ERC20.BalanceOf(Address);
            tokenBalanceText.text = "$GEM: " + data.displayValue;
        }
        catch (System.Exception)
        {
            Debug.Log("Error getting token balance");
        }
    }

    //Reset balance when disconnect wallet
    public void ResetBalance()
    {
        tokenBalanceText.text = "$GEM: 0";
    }

    public async void MintERC20()
    {
        try
        {
            Debug.Log("Minting ERC20");
            Contract contract = ThirdwebManager.Instance.SDK.GetContract(DROP_ERC20_CONTRACT);
            NotClaimedState.SetActive(false);
            ClaimingState.SetActive(true);
            var result = await contract.ERC20.Mint(gemsToClaim.ToString());
            Debug.Log("Minted ERC20");
            GetTokenBalance();
            ClaimingState.SetActive(false);
            ClaimedState.SetActive(true);

        }
        catch (System.Exception)
        {
            Debug.Log("Error minting token");
        }
    }

    public async void ClaimToken()
    {
        try
        {
            Address = await ThirdwebManager.Instance.SDK.Wallet.GetAddress();
            Debug.Log("Claiming ERC20 with gemToClaim: " + gemsToClaim.ToString());

            var contract = ThirdwebManager.Instance.SDK.GetContract(DROP_ERC20_CONTRACT);

            NotClaimedState.SetActive(false);
            ClaimingState.SetActive(true);

            var result = await contract.ERC20.ClaimTo(Address, gemsToClaim.ToString());
            Debug.Log("Claim successful: " + result);

            ClaimingState.SetActive(false);
            ClaimedState.SetActive(true);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error claiming token: " + ex.Message);
            Debug.LogError("Stack Trace: " + ex.StackTrace);
            ClaimingState.SetActive(false);
            NotClaimedState.SetActive(true);
        }
    }


}
