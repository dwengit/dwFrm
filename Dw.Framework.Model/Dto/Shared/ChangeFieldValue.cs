using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Dw.Framework.Model.Dto.Shared
{
    public class ChangeFieldValue
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string FieldVal { get; set; }
    }
}
