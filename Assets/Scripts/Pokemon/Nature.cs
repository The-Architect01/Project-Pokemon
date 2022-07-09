using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Nature {
    public readonly static decimal[] Hardy = new decimal[] { 1, 1, 1, 1, 1, 1 };
    public readonly static decimal[] Docile = new decimal[] { 1, 1, 1, 1, 1, 1 };
    public readonly static decimal[] Bashful = new decimal[] { 1, 1, 1, 1, 1, 1 };
    public readonly static decimal[] Quirky = new decimal[] { 1, 1, 1, 1, 1, 1 };
    public readonly static decimal[] Serious = new decimal[] { 1, 1, 1, 1, 1, 1 };

    public readonly static decimal[] Lonely = new decimal[] { 1, 1.1M, 1, 0.9M, 1, 1 };
    public readonly static decimal[] Brave = new decimal[] { 1, 1.1M, 1, 1, 1, 0.9M };
    public readonly static decimal[] Adamant = new decimal[] { 1, 1.1M, 0.9M, 1, 1, 1 };
    public readonly static decimal[] Naughty = new decimal[] { 1, 1.1M, 1, 1, 0.9M, 1 };

    public readonly static decimal[] Bold = new decimal[] { 1, 0.9M, 1, 1.1M, 1, 1 };
    public readonly static decimal[] Relaxed = new decimal[] { 1, 1, 1, 1.1M, 1, 0.9M };
    public readonly static decimal[] Impish = new decimal[] { 1, 1, 0.9M, 1.1M, 1, 1 };
    public readonly static decimal[] Lax = new decimal[] { 1, 1, 1, 1.1M, 0.9M, 1 };

    public readonly static decimal[] Timid = new decimal[] { 1, 0.9M, 1, 1, 1, 1.1M };
    public readonly static decimal[] Hasty = new decimal[] { 1, 1, 1, 0.9M, 1, 1.1M };
    public readonly static decimal[] Jolly = new decimal[] { 1, 1, 0.9M, 1, 1, 1.1M };
    public readonly static decimal[] Naive = new decimal[] { 1, 1, 1, 1, 0.9M, 1.1M };

    public readonly static decimal[] Modest = new decimal[] { 1, 0.9M, 1.1M, 1, 1, 1 };
    public readonly static decimal[] Mild = new decimal[] { 1, 1, 1.1M, 0.9M, 1, 1 };
    public readonly static decimal[] Quiet = new decimal[] { 1, 1, 1.1M, 1, 1, 0.9M };
    public readonly static decimal[] Rash = new decimal[] { 1, 1, 1.1M, 1, 0.9M, 1 };

    public readonly static decimal[] Calm = new decimal[] { 1, 0.9M, 1, 1, 1.1M, 1 };
    public readonly static decimal[] Gentle = new decimal[] { 1, 1, 1, 0.9M, 1.1M, 1 };
    public readonly static decimal[] Sassy = new decimal[] { 1, 1, 1, 1, 1.1M, 0.9M };
    public readonly static decimal[] Careful = new decimal[] { 1, 1, 0.9M, 1, 1.1M, 1 };

    public decimal[] nature { get; }

    public Nature(uint PersonalityValue) {
        nature = (PersonalityValue % 25) switch {
            0 => Hardy,
            1 => Lonely,
            2 => Brave,
            3 => Adamant,
            4 => Naughty,
            5 => Bold,
            6 => Docile,
            7 => Relaxed,
            8 => Impish,
            9 => Lax,
            10 => Timid,
            11 => Hasty,
            12 => Serious,
            13 => Jolly,
            14 => Naive,
            15 => Modest,
            16 => Mild,
            17 => Quiet,
            18 => Bashful,
            19 => Rash,
            20 => Calm,
            21 => Gentle,
            22 => Sassy,
            23 => Careful,
            24 => Quirky,
            _ => Hardy,
        };
        Debug.Log(PersonalityValue % 25);
    }

}
