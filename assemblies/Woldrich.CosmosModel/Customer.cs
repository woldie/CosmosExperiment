using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Woldrich.CosmosModel;

[Table("customer")]
[Comment("Woldrich, Inc. Customer Data")]
public class Customer : CosmosEntityBase 
{
    public String? FirstName { get; set; }

    public String? LastName { get; set; }

    public int? Age { get; set; }
}