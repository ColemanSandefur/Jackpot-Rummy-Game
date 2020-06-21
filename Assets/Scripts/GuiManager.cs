using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiManager : MonoBehaviour
{
    public static int chipCount = 0;
    public static int curBid = 0;
    GameObject bottomBar;
    GameObject ChipCount;
    GameObject CurBid;
    GameObject bidMenu;
    static byte[] bidMenuActive = new byte[] {0, 0};
    
    static int curCall = 0;
    public GameObject pokerBiddingDisplay;
    static byte[] pokerBidMenuActive = new byte[] {0, 0};
    Text callDisplay;


    // Start is called before the first frame update
    void Start()
    {
        bottomBar = transform.Find("BottomBar").gameObject;
        ChipCount = bottomBar.transform.Find("ChipCount").gameObject;
        CurBid = transform.Find("BiddingDisplay").Find("CurBid").gameObject;
        bidMenu = transform.Find("BiddingDisplay").gameObject;
        callDisplay = pokerBiddingDisplay.transform.Find("CallDisplay").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        ChipCount.GetComponent<Text>().text = chipCount + " chips";
        CurBid.GetComponent<Text>().text = curBid + " chips";
        callDisplay.text = curCall + " chips";

        if (bidMenuActive[0] == 1) {
            bidMenu.SetActive(bidMenuActive[1] == 1);
            bidMenuActive[0] = 0;
        }

        if (pokerBidMenuActive[0] == 1) {
            pokerBiddingDisplay.SetActive(pokerBidMenuActive[1] == 1);
            pokerBidMenuActive[0] = 0;
        }
    }

    public static void SetChips(int chips) {
        chipCount = chips;
    }

    public static void SetCurBid(int bid) {
        curBid = bid;
    }

    public static void SetBidMenuActive(bool active) {
        bidMenuActive[0] = 1;
        bidMenuActive[1] = (byte)((active) ? 1 : 0);
    }

    public static void SetCurCall(int call) {
        curCall = call;
    }
    public static void SetPokerBidMenuActive(bool active) {
        pokerBidMenuActive[0] = 1;
        pokerBidMenuActive[1] = (byte)((active) ? 1 : 0);
    }
}
