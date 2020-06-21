using System.Collections;
using System.Collections.Generic;
using System;

public class PokerHand : IComparable
{
    public enum PokerHands {RoyalFlush, StraightFlush, FourOfKind, FullHouse, Flush, Straight, ThreeOfKind, TwoPair, Pair, HighCard};

    public PokerHands pokerHand;
    public PlayingCard[] cards;

    public PokerHand(PokerHands pokerHand, PlayingCard[] cards) {
        this.pokerHand = pokerHand;
        this.cards = cards;
    }


    public int CompareTo(Object obj) {
        if ((obj == null) || ! this.GetType().Equals(obj.GetType())) {
            return 1;
        }
        PokerHand other = (PokerHand)obj;
        if (pokerHand == other.pokerHand) {
            for (int i = 0; i < cards.Length && i < other.cards.Length; i++) {
                if (other.cards[i].cardValue == cards[i].cardValue) {
                    continue;
                } else {
                    return other.cards[i].cardValue > cards[i].cardValue ? -1 : 1;
                }
            }

            return 0;
        }

        return other.pokerHand > pokerHand ? 1 : -1;
    }
}
