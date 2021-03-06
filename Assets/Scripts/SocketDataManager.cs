﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using UnityEngine;

public class SocketDataManager : MonoBehaviour
{
    public enum Commands {
        GiveHand = 1,
        GetHand = 2,
        ReadyForGame = 3,
        BidForExtraHand = 4,
        BidTurn = 5,
        ExtraHandTopCard = 6,
        ChipCount = 7,
        PlayCard = 8,
        Message = 9,
        Join = 10,
        PlayPoker = 11,
        BidPoker = 12,
        PokerBidTurn = 13,
        PokerResults = 14,
        AlertCard = 15,
        RemoveCard = 16,
        JackpotResults = 17,
        WonBoardSlot = 18,
        GiveBoard = 19,
        JoinState = 20
    }

    public TextComponent _WarningMessageText;
    public static TextComponent WarningMessageText;

    public static CanvasController GameCanvas;
    public CanvasController _GameCanvas;
    public static CanvasController LoginCanvas;
    public CanvasController _LoginCanvas;

    // Start is called before the first frame update
    void Start()
    {
        WarningMessageText = _WarningMessageText;
        GameCanvas = _GameCanvas;
        LoginCanvas = _LoginCanvas;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SocketManager.SendData(new byte[] {(byte)Commands.Join, 1, 1});
        } else if (Input.GetKeyDown(KeyCode.LeftAlt)) {
            SocketManager.SendData(new byte[] {(byte)Commands.ReadyForGame, 1, 1});
        }
    }

    public static void GiveData(byte[] data) {
        List<byte[]> commandStrings = new List<byte[]>();

        byte[] curData = data;

        while (curData.Length > 0) {
            commandStrings.Add(curData.Take(curData[1] + 2).ToArray());
            curData = curData.Skip(curData[1] + 2).ToArray();
        }

        ProcessData(commandStrings);
    }

    public static void ProcessData(List<byte[]> dataList){
        foreach (byte[] data in dataList) {
            Commands command = (Commands)data[0];
            switch(command) {
                case Commands.GiveHand: GiveHand(data); break;
                case Commands.BidTurn: BidTurn(data); break;
                case Commands.ExtraHandTopCard: ExtraHandTopCard(data); break;
                case Commands.ChipCount: ChipCount(data); break;
                case Commands.Message: Message(data); break;
                case Commands.PokerBidTurn: PokerBidTurn(data); break;
                case Commands.PokerResults: PokerResults(data); break;
                case Commands.AlertCard: AlertCard(data); break;
                case Commands.RemoveCard: RemoveCard(data); break;
                case Commands.JackpotResults: JackpotResults(data); break;
                case Commands.WonBoardSlot: WonBoardSlot(data); break;
                case Commands.GiveBoard: GiveBoard(data); break;
                case Commands.JoinState: JoinState(data); break;
            }
        }
    }

    public static void GiveHand(byte[] data) {
        int length = data[1];
        data = data.Skip(2).ToArray();
        PlayingCardManager.playingCards.Clear();
        for (int i = 0; i < length; i++) {
            PlayingCardManager.newCardHandBytes.Add(data[i]);
        }
    }

    public static void BidTurn(byte[] data) {
        int length = data[1];
        data = data.Skip(2).ToArray();
        bool isTurn = data[0] == 0 ? false : true;

        Debug.Log($"Hand Bid Turn: {isTurn}");

        GuiManager.SetBidMenuActive(isTurn);
        
        byte[] numberBytes = data.Skip(1).Take(4).ToArray();

        

        if (BitConverter.IsLittleEndian)
            Array.Reverse(numberBytes);
        int number = BitConverter.ToInt32(numberBytes, 0);
        GuiManager.SetCurBid(number);
    }

    public static void ExtraHandTopCard(byte[] data) {
        int length = data[1];
        data = data.Skip(2).ToArray();
        for (int i = 0; i < length; i++) {
            CardDisplayManager.setSingleCard(data[i]);
        }
    }

    public static void ChipCount(byte[] data) {
        byte[] numberBytes = data.Skip(2).Take(data[1]).ToArray();
        if (BitConverter.IsLittleEndian)
            Array.Reverse(numberBytes);
        int number = BitConverter.ToInt32(numberBytes, 0);
        GuiManager.SetChips(number);
    }

    public static void Message(byte[] data) {
        data = data.Skip(2).ToArray();
        WarningMessageText.Message = Encoding.UTF8.GetString(data, 0, data.Length);
    }

    public void BidForExtraHand(int chips) {
        if (chips == 0) {
            SocketManager.SendData(new byte[] {(byte)Commands.BidForExtraHand, 0});
            return;
        }

        byte[] data = new byte[3];

        data[0] = (byte)Commands.BidForExtraHand;
        data[1] = 1;
        data[2] = (byte)chips;

        SocketManager.SendData(data);
    }

    public static void PokerBidTurn(byte[] data) {
        int length = data[1];
        data = data.Skip(2).ToArray();
        bool isTurn = data[0] == 0 ? false : true;

        Debug.Log($"Poker Turn: {isTurn}");

        GuiManager.SetPokerBidMenuActive(isTurn);
        
        byte[] numberBytes = data.Skip(1).Take(4).ToArray();

        

        if (BitConverter.IsLittleEndian)
            Array.Reverse(numberBytes);
        int number = BitConverter.ToInt32(numberBytes, 0);
        GuiManager.SetCurCall(number);
    }

    public void BidForPoker(int increase) {
        if (increase < 0) {
            SocketManager.SendData(new byte[] {(byte)Commands.BidPoker, 0});
            return;
        }

        byte[] data = new byte[] {(byte) Commands.BidPoker, 1, (byte) increase};
        SocketManager.SendData(data);
    }

    public static void PokerResults(byte[] data) {
        Debug.Log("set it to false");
        CardDisplayManager.playingPoker = false;
        if (data[1] == 1) {
            WarningMessageText.Message = "You Lost";
            return;
        }
        

        
        byte[] numberBytes = data.Skip(3).Take(4).ToArray();
        if (BitConverter.IsLittleEndian)
            Array.Reverse(numberBytes);
        int number = BitConverter.ToInt32(numberBytes, 0);
        
        
        WarningMessageText.Message = ($"You won {number} chips");

    }

    public static void AlertCard(byte[] data) {
        bool visible = data[2] == 1;
        byte cardByte = data[3];
    }

    public static void RemoveCard(byte[] data) {
        byte cardByte = data[2];
        PlayingCardManager.removeCardHandBytes.Add(cardByte);
    }

    public void PlayCard(PlayingCard card) {
        byte[] data = new byte[] {(byte) Commands.PlayCard, 1, card.cardByte};
        SocketManager.SendData(data);
    }

    public static void JackpotResults(byte[] data) {
        int length = data[1];
        bool won = data[2] == 1;

        if (won) {
            WarningMessageText.Message = "You won!";
        } else {
            WarningMessageText.Message = "You lost!";
        }
    }

    public static void WonBoardSlot(byte[] data) {
        int length = data[1];
        int slot_num = data[2];
        
        byte[] byteOfInt = data.Skip(3).Take(4).ToArray();

        if (BitConverter.IsLittleEndian) 
            Array.Reverse(byteOfInt);

        int chipCount = BitConverter.ToInt32(byteOfInt, 0);
        WarningMessageText.Message = ($"You won {chipCount} chips");
    }

    public static void GiveBoard(byte[] data) {
        int length = data[1];
        int blockLength = data[2];
        data = data.Skip(3).ToArray();

        while (data.Count() > 0) {
            GameBoardSlot.BoardSlots slot = (GameBoardSlot.BoardSlots) data[0];
            byte[] byteOfInt = data.Skip(1).Take(4).ToArray();

            if (BitConverter.IsLittleEndian)
                Array.Reverse(byteOfInt);
            int number = BitConverter.ToInt32(byteOfInt, 0);

            GameBoard.GetSlot(slot).setChips(number);

            data = data.Skip(blockLength).ToArray();
        }
    }

    public static void JoinState(byte[] data) {
        if (data[2] == 1) {
            LoginCanvas.Active = (false);
            GameCanvas.Active = (true);
        } else {
            LoginCanvas.Active = (true);
            GameCanvas.Active = (false);
        }
    }
}
