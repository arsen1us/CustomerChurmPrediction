﻿namespace CustomerChurmPrediction.Entities.CartEntity
{
    public class Cart : AbstractEntity
    {
        public string ProductId { get; set; } = null!;
        public string UserId { get; set; } = null!;
    }
}