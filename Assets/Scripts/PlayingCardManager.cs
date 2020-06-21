using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayingCardManager : MonoBehaviour
{
    static string[] CardValues = new string[] {"2", "3", "4", "5", "6", "7", "8", "9", "10", "jack", "queen", "king", "ace"};
    static string[] CardSuits = new string[] {"clubs", "diamonds", "hearts", "spades"};
    
    public static GameObject prefab;
    public GameObject prefabToStatic;
    public static List<PlayingCard> playingCards = new List<PlayingCard>();
    public static List<byte> newCardHandBytes = new List<byte>();
    public static GameObject objectHideFolder;
    public GameObject objectHideFolderToStatic;

    public static GameObject CreateCard(int value, int suit) {
        if (prefab == null) {
            // Debug.Log("Loading prefab");
            // prefab = Resources.Load("Prefabs/PlayingCard") as GameObject;
        }
        GameObject card = Instantiate(prefab);
        card.GetComponent<Image>().sprite = getCardSprite(value, suit);
        card.GetComponent<PlayingCard>().cardValue = (PlayingCard.CardValues)value;
        card.GetComponent<PlayingCard>().cardSuit = (PlayingCard.CardSuits)suit;
        card.GetComponent<PlayingCard>().cardByte = valsToByte((byte)value, (byte)suit);
        card.name = CardValues[value] + "_" + CardSuits[suit];
        card.transform.SetParent(objectHideFolder.transform);
        return card;
    }

    public static GameObject CreateCard(byte cardByte) {
        byte[] vals = byteToVals(cardByte);
        byte value = vals[0], suit = vals[1];

        return CreateCard(value, suit);
    }

    public static Sprite getCardSprite(int value, int suit) {
        return Resources.Load("Textures/CardPictures/" + CardValues[value] + "_of_" + CardSuits[suit], typeof(Sprite)) as Sprite;
    }

    public static Sprite getCardSprite(byte cardByte) {
        byte[] vals = byteToVals(cardByte);
        byte value = vals[0], suit = vals[1];
        return getCardSprite(value, suit);
    }

    public static byte[] byteToVals(byte cardByte) {
        byte suit = 0;

        while (cardByte >= CardValues.Length) {
            cardByte -= (byte)CardValues.Length;
            suit++;
        }

        return new byte[] {cardByte, suit};
    }

    public static byte valsToByte(byte value, byte suit) {
        return (byte)(value + (suit * CardValues.Length));
    }

    void Start() {
        prefab = prefabToStatic;
        objectHideFolder = objectHideFolderToStatic;
    }

    void Update() {
        byte[] tmpNewCardHandBytes = newCardHandBytes.ToArray();

        foreach (byte cardByte in tmpNewCardHandBytes) {
            byte[] vals = byteToVals(cardByte);
            GameObject card = CreateCard(vals[0], vals[1]);
            playingCards.Add(card.GetComponent<PlayingCard>());
            newCardHandBytes.Remove(cardByte);
        }

        if (tmpNewCardHandBytes.Length > 0) {
            SortCards((int)PlayingCardSorter.Sorts.byteOrder);
        }
    }

    public void SortCards(int sort) {
        playingCards = PlayingCardSorter.Sort(playingCards, (PlayingCardSorter.Sorts)sort);
        CardDisplayManager.RedisplayHand();
    }
}
