using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayingCardSorter : MonoBehaviour
{
    public enum Sorts {
        byteOrder,
        ValueOrderSort,
        ByteOrderValRev
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static List<PlayingCard> Sort(List<PlayingCard> hand, Sorts sort) {
        switch (sort) {
            case Sorts.byteOrder: return ByteOrderSort(hand);
            case Sorts.ValueOrderSort: return ValueOrderSort(hand);
            case Sorts.ByteOrderValRev: return ByteOrderValRev(hand);
            default: return hand;
        }
    }

    private static List<PlayingCard> ByteOrderSort(List<PlayingCard> hand) {
        return hand.OrderBy(card => (int) card.cardByte).ToList();
    }

    private static List<PlayingCard> ValueOrderSort(List<PlayingCard> hand) {
        return hand.OrderBy(card => (int) card.cardValue).ThenBy(card => (int) card.cardSuit).ToList();
    }

    private static List<PlayingCard> ByteOrderValRev(List<PlayingCard> hand) {
        return hand.OrderBy(card => (int) card.cardSuit).ThenByDescending(card => (int) card.cardValue).ToList();
    }
}
