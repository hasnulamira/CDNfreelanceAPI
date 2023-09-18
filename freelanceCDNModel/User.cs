using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CDNAPI.freelanceCDNModel;

[Table("users")]
public partial class User
{
    [Key]
    [Column("UserID")]
    public long UserId { get; set; }

    [StringLength(50)]
    public string UserName { get; set; } = null!;

    [StringLength(25)]
    public string? UserEmail { get; set; }

    [StringLength(15)]
    public string? UserPhoneNum { get; set; }
}
