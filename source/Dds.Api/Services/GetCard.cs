﻿using ServiceStack.ServiceInterface;

namespace Dds.Api.Services
{
    public class GetCardRequest
    {
        public string PBN { get; set; } 
    }

    public class GetCardResponse
    {
        public string Rank { get; set; } 
        public string Suit { get; set; }
        public int Score { get; set; }
    }

    public class GetCardService:Service
    {
        public object Any(GetCardRequest request)
        {
            return new GetCardResponse();
        }
    }
}