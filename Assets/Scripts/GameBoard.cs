using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    public GameBoardSlot Ace;
    public GameBoardSlot King;
    public GameBoardSlot Queen;
    public GameBoardSlot Jack;
    public GameBoardSlot Ten;
    public GameBoardSlot KingQueen;
    public GameBoardSlot SevenEightNine;
    public GameBoardSlot Poker;
    public GameBoardSlot Jackpot;

    static GameBoardSlot[] slots;
    
    // Start is called before the first frame update
    void Start()
    {
        slots = new GameBoardSlot[] {Ace, King, Queen, Jack, Ten, KingQueen, SevenEightNine, Poker, Jackpot};
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static GameBoardSlot GetSlot(GameBoardSlot.BoardSlots slotType) {
        foreach (GameBoardSlot slot in slots) {
            if (slot.slot == slotType) {
                return slot;
            }
        }

        return null;
    }
}
