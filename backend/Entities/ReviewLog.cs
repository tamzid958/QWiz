﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using QWiz.Entities.Abstract;

namespace QWiz.Entities;

public class ReviewLog : AbstractAuditable<long, AppUser, string>
{
    public required long QuestionId { set; get; }

    [ForeignKey("QuestionId")] [Required] public required Question Question { set; get; }

    public bool IsApproved { set; get; }

    [MaxLength(600)] public string? Comment { set; get; }
}