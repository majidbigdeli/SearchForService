using System;
using System.ComponentModel.DataAnnotations;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.Inputs
{
    public class CheckMovieAssetsModel
    {
        [Required]
        public Guid MovieId { get; set; }

        [Required]
        public AssetsCheckStatus Status { get; set; }
    }
}

