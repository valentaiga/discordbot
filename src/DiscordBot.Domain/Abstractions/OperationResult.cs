namespace DiscordBot.Domain.Abstractions;

public readonly struct OperationResult
{
    public string? Error { get; }
    public bool Success => Error is null;

    public OperationResult() => Error = null;
    public OperationResult(string error) => Error = error;

    public static OperationResult FromSuccess() => new();
    public static OperationResult FromError(string error) => new(error);
}