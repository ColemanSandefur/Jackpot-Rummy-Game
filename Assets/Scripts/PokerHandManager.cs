using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokerHandManager : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        // List<PlayingCard> cards = new List<PlayingCard>();

        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Ten, PlayingCard.CardSuits.Clubs));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Clubs));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Queen, PlayingCard.CardSuits.Clubs));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.King, PlayingCard.CardSuits.Clubs));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Ace, PlayingCard.CardSuits.Clubs));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Ten, PlayingCard.CardSuits.Spades));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Spades));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Queen, PlayingCard.CardSuits.Spades));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.King, PlayingCard.CardSuits.Spades));
        // // cards.Add(new PlayingCard(PlayingCard.CardValues.Ace, PlayingCard.CardSuits.Spades));
        // cards.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Clubs));
        // cards.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Spades));
        // cards.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Diamonds));
        // cards.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Hearts));

        // List<PlayingCard> cards2 = new List<PlayingCard>();

        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.Ten, PlayingCard.CardSuits.Clubs));
        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Clubs));
        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.Queen, PlayingCard.CardSuits.Clubs));
        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.King, PlayingCard.CardSuits.Clubs));
        // cards2.Add(new PlayingCard(PlayingCard.CardValues.Ace, PlayingCard.CardSuits.Clubs));
        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.Ten, PlayingCard.CardSuits.Spades));
        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.Jack, PlayingCard.CardSuits.Spades));
        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.Queen, PlayingCard.CardSuits.Spades));
        // // cards2.Add(new PlayingCard(PlayingCard.CardValues.King, PlayingCard.CardSuits.Spades));
        // cards2.Add(new PlayingCard(PlayingCard.CardValues.Ace, PlayingCard.CardSuits.Spades));
        // cards2.Add(new PlayingCard(PlayingCard.CardValues.Ace, PlayingCard.CardSuits.Diamonds));
        // cards2.Add(new PlayingCard(PlayingCard.CardValues.Ace, PlayingCard.CardSuits.Hearts));

        // PokerHand ph1 = FindBestPokerHand(cards);
        // PokerHand ph2 = FindBestPokerHand(cards2);

        // List<PokerHand> pokerHands = new List<PokerHand>();
        // // pokerHands.Add(ph2);
        // pokerHands.Add(ph1);
        // pokerHands.Add(ph2);

        // pokerHands.Sort();

        // foreach(PokerHand p in pokerHands) {
        //     // Debug.Log($"Type: {p.pokerHand} Top Card: {p.cards[0]}");
        // }

        // // Debug.Log(ph1.CompareTo(ph2));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static PokerHand FindBestPokerHand(List<PlayingCard> cards) {
        PlayingCard[] hand = null;
        List<PlayingCard> tmpCards = new List<PlayingCard>(cards);

        if ((hand = IsRoyalFlush(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.RoyalFlush, hand);
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsStraightFlush(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.StraightFlush, hand);
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsFourOfKind(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.FourOfKind, FillHand(hand, tmpCards));
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsFullHouse(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.FullHouse, hand);
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsFlush(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.Flush, hand);
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsStraight(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.Straight, hand);
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsThreeOfKind(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.ThreeOfKind, hand);
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsTwoPair(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.TwoPair, FillHand(hand, tmpCards));
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = OnePair(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.Pair, FillHand(hand, tmpCards));
        }
        tmpCards = new List<PlayingCard>(cards);
        if ((hand = IsHighCard(tmpCards)) != null) {
            return new PokerHand(PokerHand.PokerHands.HighCard, FillHand(hand, tmpCards));
        }

        return null;
    }

    public static PlayingCard[] IsRoyalFlush(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.byteOrder);

        List<PlayingCard> hand = new List<PlayingCard>();

        PlayingCard.CardSuits curSuit = cards[0].cardSuit;
        foreach (PlayingCard card in cards) {
            if (card.cardSuit != curSuit) {
                hand.Clear();
            }

            if ((PlayingCard.CardValues)hand.Count + 8 == card.cardValue) {
                hand.Add(card);
            } else {
                hand.Clear();
                continue;
            }

            if (hand.Count == 5) {
                return hand.ToArray();
            }
        }

        return null;
    }

    public static PlayingCard[] IsStraightFlush(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.ByteOrderValRev);
        
        List<PlayingCard> curHand = new List<PlayingCard>();
        PlayingCard[] bestHand = null;

        PlayingCard.CardSuits curSuit = cards[0].cardSuit;
        PlayingCard lastCard = null;

        foreach (PlayingCard card in cards) {
            if (lastCard is null) {
                curHand.Add(card);
                lastCard = card;
            } else if (card.cardSuit != curSuit) {
                curSuit = card.cardSuit;
                curHand.Clear();
                curHand.Add(card);
            } else if (((int) card.cardValue) + 1 == (int) lastCard.cardValue) {
                curHand.Add(card);
            } else if ((int) card.cardValue != (int) lastCard.cardValue) {
                curHand.Clear();
            }

            if (curHand.Count == 5) {
                if (bestHand == null || curHand[0].cardValue > bestHand[0].cardValue) {
                    bestHand = curHand.ToArray();
                } else {
                    curHand.Clear();
                }
            }

            lastCard = card;
        }

        if (bestHand == null) {
            return null;
        }
        return bestHand;
    }

    public static PlayingCard[] IsFourOfKind(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.ValueOrderSort);
        cards.Reverse();
        
        List<PlayingCard> hand = new List<PlayingCard>();

        PlayingCard.CardSuits curSuit = cards[0].cardSuit;
        PlayingCard curCard = null;
        foreach (PlayingCard card in cards) {
            if (curCard is null) {
                curCard = card;
                hand.Add(card);
            } else if (card.cardValue != curCard.cardValue) {
                curCard = card;
                hand.Clear();
            } else {
                hand.Add(card);
            }

            if (hand.Count == 4) {
                return hand.ToArray();
            }
        }

        return null;
    }

    public static PlayingCard[] IsFullHouse(List<PlayingCard> cards) {
        PlayingCard[] hand = new PlayingCard[5];
        int index = 0;

        //Get three of kind
        PlayingCard[] tmpThreeOfKind = IsThreeOfKind(cards);
        if (!(tmpThreeOfKind is null)) {
            for (int i = cards.Count - 1; i > -1; i--) {
                for (int j = 0; j < tmpThreeOfKind.Length; j++) {
                    if (cards[i].cardByte == tmpThreeOfKind[j].cardByte) {
                        hand[index] = cards[i];
                        index++;
                        cards.RemoveAt(i);
                        break;
                    }
                }
            }
        } else {
            return null;
        }

        //Get Pair
        PlayingCard[] tmpTwoOfKind = OnePair(cards);
        if (!(tmpTwoOfKind is null)) {
            for (int i = cards.Count - 1; i > -1; i--) {
                for (int j = 0; j < tmpTwoOfKind.Length; j++) {
                    if (cards[i].cardByte == tmpTwoOfKind[j].cardByte) {
                        hand[index] = cards[i];
                        index++;
                        cards.RemoveAt(i);
                        break;
                    }
                }
            }
            
        } else {
            return null;
        }

        return hand;
    }

    public static PlayingCard[] IsFlush(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.ByteOrderValRev);
        
        List<PlayingCard> curHand = new List<PlayingCard>();
        PlayingCard[] bestHand = null;

        PlayingCard.CardSuits curSuit = cards[0].cardSuit;

        foreach (PlayingCard card in cards) {
            if (card.cardSuit != curSuit) {
                curSuit = card.cardSuit;
                curHand.Clear();
                curHand.Add(card);
            } else {
                curHand.Add(card);
            }

            if (curHand.Count == 5) {
                if (bestHand == null || curHand[0].cardValue > bestHand[0].cardValue) {
                    bestHand = curHand.ToArray();
                } else {
                    curHand.Clear();
                }
            }
        }

        if (bestHand == null) {
            return null;
        }

        return bestHand;
    }

    public static PlayingCard[] IsStraight(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.ValueOrderSort);
        cards.Reverse();
        
        List<PlayingCard> curHand = new List<PlayingCard>();
        PlayingCard[] bestHand = null;
        PlayingCard lastCard = null;

        foreach (PlayingCard card in cards) {
            if (lastCard is null) {
                curHand.Add(card);
                lastCard = card;
            } else if (((int) card.cardValue) + 1 == (int) lastCard.cardValue) {
                curHand.Add(card);
            } else if ((int)card.cardValue == (int)lastCard.cardValue) {
                // curHand.Clear();
            } else {
                curHand.Clear();
            }

            if (curHand.Count == 5) {
                if (bestHand == null || curHand[0].cardValue > bestHand[0].cardValue) {
                    bestHand = curHand.ToArray();
                } else {
                    curHand.Clear();
                }
            }

            lastCard = card;
        }

        if (bestHand == null) {
            return null;
        }
        return bestHand;
    }

    public static PlayingCard[] IsThreeOfKind(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.ValueOrderSort);
        cards.Reverse();
        
        List<PlayingCard> hand = new List<PlayingCard>();

        PlayingCard.CardSuits curSuit = cards[0].cardSuit;
        PlayingCard lastCard = cards[0];

        foreach (PlayingCard card in cards) {
            if (card.cardValue == lastCard.cardValue) {
                hand.Add(card);
            } else {
                lastCard = card;
                hand.Clear();
                hand.Add(card);
            }

            if (hand.Count == 3) {
                return hand.ToArray();
            }
        }

        return null;
    }

    public static PlayingCard[] IsTwoPair(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.byteOrder);
        
        List<PlayingCard> hand = new List<PlayingCard>();


        for (int x = 0; x < 2; x++) {
            PlayingCard[] tmpPair = OnePair(cards);
            if (!(tmpPair is null)) {
                for (int i = cards.Count - 1; i > -1; i--) {
                    for (int j = 0; j < tmpPair.Length; j++) {
                        if (cards[i].cardByte == tmpPair[j].cardByte) {
                            hand.Add(cards[i]);
                            cards.RemoveAt(i);
                            break;
                        }
                    }
                }
            } else {
                return null;
            }
        }

        return hand.ToArray();
    }

    public static PlayingCard[] OnePair(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.ValueOrderSort);
        cards.Reverse();
        List<PlayingCard> hand = new List<PlayingCard>();

        PlayingCard.CardSuits curSuit = cards[0].cardSuit;
        PlayingCard lastCard = cards[0];

        foreach (PlayingCard card in cards) {
            if (card.cardValue == lastCard.cardValue) {
                hand.Add(card);
            } else {
                lastCard = card;
                hand.Clear();
                hand.Add(card);
            }

            if (hand.Count == 2) {
                return hand.ToArray();
            }
        }

        return null;
    }

    public static PlayingCard[] IsHighCard(List<PlayingCard> cards) {
        cards = PlayingCardSorter.Sort(cards, PlayingCardSorter.Sorts.ValueOrderSort);
        cards.Reverse();

        if (cards.Count == 0) {
            return null;
        }

        return new PlayingCard[] {cards[0]};
    }

    private static PlayingCard[] FillHand(PlayingCard[] og, List<PlayingCard> allCards) {
        List<PlayingCard> hand = new List<PlayingCard>(og);
        allCards = PlayingCardSorter.Sort(allCards, PlayingCardSorter.Sorts.ValueOrderSort);
        int i = 0;
        while (hand.Count < 5) {
            foreach (PlayingCard card in hand) {
                if (card.cardByte == allCards[i].cardByte) {
                    break;
                }
            }
            hand.Add(allCards[i]);
            i++;
        }

        return hand.ToArray();
    }
}
