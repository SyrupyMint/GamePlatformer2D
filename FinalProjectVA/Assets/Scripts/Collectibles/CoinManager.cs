using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public int coinCount;
    public Text coinTxt;

    void Start()
    {
        
    }

    void Update()
    {
        coinTxt.text = ": " + coinCount.ToString();
    }
}