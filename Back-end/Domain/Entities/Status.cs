using System;

namespace Domain.Entities;

public class Status
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDeleted { get; set; } = false;
}