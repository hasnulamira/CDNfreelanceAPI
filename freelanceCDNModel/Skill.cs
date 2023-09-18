using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CDNAPI.freelanceCDNModel;

[Table("skills")]
public partial class Skill
{
    [Key]
    [Column("skillID")]
    public long? SkillId { get; set; }

    [Column("userID")]
    public long? UserId { get; set; }

    [Column("skillName")]
    [StringLength(45)]
    public string? SkillName { get; set; }
}
