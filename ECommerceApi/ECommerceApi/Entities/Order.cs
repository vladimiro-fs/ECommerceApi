﻿namespace ECommerceApi.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Order
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string? Address { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal TotalValue { get; set; }

        public DateTime OrderDate { get; set; }

        public int UserId { get; set; }

        public ICollection<OrderDetail>? OrderDetails { get; set; }
    }
}