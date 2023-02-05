using System;
using System.ComponentModel.DataAnnotations;
using SearchForApi.Models.Entities;

namespace SearchForApi.Models.Inputs
{
    public class ExcludeSceneModel
    {
        [Required]
        public SceneCheckType Type { get; set; }

        [Required]
        public SceneCheckResultType ResultType { get; set; }

        public SceneCheckExcludeType? ExcludeType { get; set; }
    }
}
