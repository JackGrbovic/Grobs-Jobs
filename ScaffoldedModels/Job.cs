using Nest;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Job_Post_Website.ScaffoldedModels;

public partial class Job
{
    [Key]
    public int Id { get; set; }
    [Required]
    [Display(Name = "Job Name")]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Job Description")]
    public string Description { get; set; }
    [Required]
    [Display(Name = "Job Poster Id")]
    public string JobPosterId { get; set; }
    [Required]
    [Display(Name = "Job Poster User Name")]
    public string JobPosterNormalizedUserName { get; set; }
    [Required]
    public DateTime DateTimePosted { get; set; }
}
