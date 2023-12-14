using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace COS.Models{
    public class Menu
    {
        [Key]
        public int Food_id {get; set;}
        [Required]
        public int Item_id{get;set;}
        [Required]
        public string? Item_name {get; set;}
        [Required]
        public double? Item_price {get; set;}
        [Required]
        public string? Item_image{get;set;}
         [Required]
        public string? Item_quantity{get;set;}
    }
}