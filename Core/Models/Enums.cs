using System.ComponentModel.DataAnnotations;

namespace Models;
public enum FlatType
{
    [Display(Name = "собственная")]
    Owner = 1,
    [Display(Name = "приватизированная")]
    Privatized,
    [Display(Name = "государственная")]
    StateOwned
}

public enum VoteType
{
    For = 1,
    Against,
    Abstain
}

public enum LivingQuater
{
    [Display(Name = "жилое")]
    Living,
    [Display(Name = "нежилое")]
    NonLiving
}