using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FreddyBaseApi.Core.Model.Models
{
    [Table("DummyNestedEntity")]
    public class DummyNestedEntity
    {
        /// <summary>
        /// Dummy entity primary key id
        /// </summary>
        [Key]
        public int DummyNestedEntityId { get; set; }

        /// <summary>
        /// Dummy entity name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dummy entity description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Dummy entity creation datetime
        /// </summary>
        [Required(ErrorMessage = "Date created is required")]
        [DisplayFormat(DataFormatString = "dd/MM/yyyy")]
        public DateTime DateCreated { get; set; }
        public ICollection<DummyEntity> DummyEntities { get; set; }
    }
}
