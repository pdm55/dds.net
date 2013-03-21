﻿using System.Collections.Generic;
using Bridge.Domain;
using Bridge.Domain.Utils;
using Dds.Net;
using PBN;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;

namespace Dds.Api.Services
{
    [Route("/get-all-cards")]
    [Route("/get-all-cards/{PBN}")]
    public class GetAllCardsRequest
    {
        public string PBN { get; set; } 
    }

    public class GetAllCardsResponse
    {
        /// <summary>
        /// Number of searched nodes 
        /// </summary>
        public int Nodes { get; set; }


        /// <summary>
        /// Possible plays
        /// </summary>
        public List<CardResult> Cards { get; set; }
    }

    public class GetAllCardsService: Service
    {
        public string testPbn = "\r\n[Dealer \"S\"]\r\n[Deal \"S:K4.AT974.543.AK3 QJT83.K5.KJ72.Q5 A92.62.A.JT97642 765.QJ83.QT986.8\"]\r\n[Auction \"S\"]\r\n6C Pass Pass Pass\r\n[Play \"W\"]\r\nSQ SA S5 S4\r\nS9 S6 SK S3\r\nHA H5 H2 H8\r\nHT HK H6 H3\r\nSJ S2 S7 C3\r\nCK C5 C2 C8\r\nCA CQ C4 D6\r\nD3 D2 DA D8\r\nCT D9 D4 D7\r\nC9 DT D5 S8\r\nCJ DQ H9\r\n\r\n";


        public object Any(GetAllCardsRequest getAllCards)
        {
            var parseResult = PbnParser.ParseGame(testPbn);
            var dds = new DdsConnect();
            var game = BridgeHelper.GetGameFromPbn(parseResult.Deal);
            var player = PlayerPosition.Players.Find(x => x.FirstLetter == parseResult.FirstPlayer);
            if (!string.IsNullOrEmpty(parseResult.Play))
            {
                var play = PbnParser.ParsePlay(parseResult.Play);
                foreach (var trick in play)
                {
                    foreach (var card in trick)
                    {
                        player = game.PlayCard(BridgeHelper.GetCard(card), player);
                    }
                }
            }
            var result = dds.SolveBoardPbnBestCard(game);
            return new GetAllCardsResponse
            {

            };
        }
    }

    public class CardResult
    {
        public string Rank { get; set; }
        public string Suit { get; set; }
        public int Score { get; set; }
    }
}