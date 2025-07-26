namespace Domain.DTO.Key;

public record class ResultUseKey
{
    public required bool Succeeded { get; set; }
    public string? errorMessage { get; set; }
}
