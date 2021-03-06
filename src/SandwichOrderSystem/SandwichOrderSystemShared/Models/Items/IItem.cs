﻿using System;

namespace SandwichOrderSystemShared.Models
{
    public interface IItem
    {
        int Id { get; set; }
        string Name { get; set; }
        decimal Price { get; set; }
        string Type { get; set; }
    }
}
