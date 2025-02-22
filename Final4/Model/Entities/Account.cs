﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Final4.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Final4.Model.Entities
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public string? AccountName { get; set; }
        public required string AccountEmail { get; set; }
        public required string AccountPassword { get; set; }
        public required string AccountRoleID { get; set; }
        public DateTime CreatedAt { get; set; }
        public Account()
        {
            CreatedAt = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SE Asia Standard Time");
        }

        [JsonIgnore]
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>(); // Một User có nhiều Order

    }


}
