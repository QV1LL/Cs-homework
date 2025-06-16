using System;
using System.ComponentModel.DataAnnotations;

namespace LowpriceProductsApp.Domain.Entities;

public interface IEntity
{
    [Key]
    public Guid? Id { get; set; }
}
