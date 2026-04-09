using DocumentFormat.OpenXml.Office2016.Drawing.Command;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace biblioteca.Models
{
    public class Book
    {
        public int Id { get; set; }
        private string title { get; set; }
        public string Autor { get; set; }
        public int CategoriaId { get; set; }

        [ValidateNever]
        public Categoria Categoria { get; set; }
        public int Quantidade { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

        public string Title 
        {
            get { return this.title; }

            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Title é obrigatório.");

                this.title = value;
            }

        }

        //public Book(string title, string  autor)
        //{
        //   this.title = title;
        //   this.Autor = autor;
            
      //  }



    }
}
