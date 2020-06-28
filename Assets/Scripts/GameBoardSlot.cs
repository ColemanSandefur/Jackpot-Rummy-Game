using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameBoardSlot : MonoBehaviour
{
    public enum BoardSlots {Ace, King, Queen, Jack, Ten, KingQueen, SevenEightNine, Poker, Jackpot}

    public BoardSlots slot;
    int chips = 0;
    bool updateChips = true;

    public GameBoardSlot(BoardSlots slot, int chips, GameObject boardObject) {
        this.slot = slot;
        this.chips = chips;
    }

    void Update()
    {
        if (updateChips) {
            updateChips = false;
            Debug.Log($"num of children: {transform.childCount}");
            transform.Find("Ammount").Find("Text").gameObject.GetComponent<TextMeshProUGUI>().SetText(chips + "");
        }
    }

    public int getChips() {return chips;}
    public void addChips(int amt) { chips += amt; updateChips = true; }
    public void setChips(int amt) { chips = amt; updateChips = true; }
}
