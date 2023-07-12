using System.ComponentModel.DataAnnotations;

namespace BackendLi.DataAccess.Enums;

public enum TypeImage
{
    [Display(Name = "Popular")] Popular,

    [Display(Name = "Fresh")] Fresh,

    [Display(Name = "Editor Choice")] EditorChoice,

    [Display(Name = "Basic")] Basic
}