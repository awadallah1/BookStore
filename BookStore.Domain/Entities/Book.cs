using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.Domain.Entities
{
   public class Book
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ISBN { get; set; }
        [Required(ErrorMessage ="Please enter a book title")]
        public string Title { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="Please enter a description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01,double.MaxValue,ErrorMessage ="Enter a Positive price")]
        public decimal Price { get; set; }
        [Required]
        [StringLength(10,MinimumLength =2)]
        public string Category { get; set; }
     
    }
}
