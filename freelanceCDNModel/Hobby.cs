using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CDNAPI.freelanceCDNModel;

[Table("hobbies")]
public partial class Hobby
{
    [Key]
    [Column("hobbyID")]
    public long? HobbyId { get; set; }

    [Column("userID")]
    public long? UserId { get; set; }

    [Column("hobbyName")]
    [StringLength(45)]
    public string? HobbyName { get; set; }
}
