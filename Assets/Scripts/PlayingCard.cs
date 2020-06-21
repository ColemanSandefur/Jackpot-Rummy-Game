using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingCard : MonoBehaviour
{
    public enum CardValues {Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King, Ace};
    public enum CardSuits {Clubs, Diamonds, Hearts, Spades};

    public CardValues cardValue;
    public CardSuits cardSuit;
    public byte cardByte;

    public PlayingCard(CardValues value, CardSuits suit) {
        cardByte = PlayingCardManager.valsToByte((byte)value, (byte)suit);
        cardValue = value;
        cardSuit = suit;
    }

    public PlayingCard(byte cardByte) {
        this.cardByte = cardByte;
        byte[] vals = PlayingCardManager.byteToVals(cardByte);
        cardValue = (CardValues)vals[0];
        cardSuit = (CardSuits)vals[1];
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string ToString() {
        return $"{cardValue.ToString()} of {cardSuit.ToString()}";
    }
}
