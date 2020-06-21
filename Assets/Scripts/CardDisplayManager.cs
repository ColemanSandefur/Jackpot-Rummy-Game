using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDisplayManager : MonoBehaviour
{
    static List<GameObject> CardsDisplayed = new List<GameObject>();
    static List<GameObject> CardsToBeDisplayed = new List<GameObject>();
    static List<GameObject> PokerHand = new List<GameObject>();
    static byte[] singleCard =  new byte[] {0, 0}; // {should be updated, new card byte}
    public GameObject cardDisplayer;
    public GameObject singleCardDisplay;
    public GameObject bestPokerHandBar;
    public PokerHand pokerHand;
    public GameObject hiddenObjects;

    void Update()
    {

        //Update Cards
        GameObject[] tmpCardsToBeDisplayed = CardsToBeDisplayed.ToArray();
        GameObject handDisplay = cardDisplayer.transform.Find("HandDisplayMask").Find("HandDisplay").gameObject;

        foreach (GameObject card in tmpCardsToBeDisplayed)
        {
            card.transform.SetParent(handDisplay.transform);
            CardsToBeDisplayed.Remove(card);
            CardsDisplayed.Add(card);
        }

        if (tmpCardsToBeDisplayed.Length > 0) {
            //Get a PlayingCard List from the displayed Cards
            GameObject[] cardGameObjects = CardsDisplayed.ToArray();
            List<PlayingCard> tmpCards = new List<PlayingCard>();

            string x = "";
            foreach (GameObject obj in cardGameObjects) {
                GameObject dup = Object.Instantiate(obj, hiddenObjects.transform, false);
                tmpCards.Add(dup.GetComponent<PlayingCard>());
                x += obj.GetComponent<PlayingCard>().ToString() + ", ";
            }
            // Debug.Log(x);

            //Remove any cards currently displayed for PokerHands
            if (pokerHand != null) {
                foreach (PlayingCard card in pokerHand.cards) {
                    Destroy(card.gameObject);
                }
            }

            //Find and display best PokerHand
            pokerHand = PokerHandManager.FindBestPokerHand(tmpCards);
            GameObject bestPokerHandDisplay = bestPokerHandBar.transform.Find("BestPokerHandDisplay").gameObject;
            foreach (PlayingCard card in pokerHand.cards) {
                card.gameObject.transform.SetParent(bestPokerHandDisplay.transform);
                card.gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
                card.gameObject.transform.position = new Vector3(0, 0, 0);
            }
            bestPokerHandBar.transform.Find("BestPokerHandName").GetComponent<Text>().text = pokerHand.pokerHand + "";
            
            Debug.Log("Best Hand: " + pokerHand.pokerHand);
        }

        //Update Single Card Display
        if (singleCard[0] == 1) {
            singleCard[0] = 0;
            singleCardDisplay.transform.Find("CardDisplay").GetComponent<Image>().sprite = PlayingCardManager.getCardSprite(singleCard[1]);
        }
    }

    public static void AddCard(GameObject card) {
        CardsToBeDisplayed.Add(card);
    }

    public static void RedisplayHand() {
        DeleteHand();

        PlayingCard[] tmpPlayingCards = PlayingCardManager.playingCards.ToArray();
        foreach(PlayingCard playingCard in tmpPlayingCards) {
            byte byteVal = playingCard.cardByte;
            CardsToBeDisplayed.Add(PlayingCardManager.CreateCard(byteVal));
        }
    }

    public static void DeleteHand() {
        GameObject[] tmpCardsDisplayed = CardsDisplayed.ToArray();
        foreach (GameObject card in tmpCardsDisplayed) {
            Destroy(card);
            CardsDisplayed.Remove(card);
        }
    }

    public void ToggleDisplayHand() {
        cardDisplayer.SetActive(!cardDisplayer.activeSelf);
    }

    public static void setSingleCard(byte cardByte) {
        singleCard[0] = 1;
        singleCard[1] = cardByte;
    }
}
