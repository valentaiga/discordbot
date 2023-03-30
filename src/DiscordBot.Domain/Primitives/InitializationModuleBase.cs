namespace DiscordBot.Domain.Primitives;

public abstract class InitializationModuleBase : IInitializationModule
{
    private static readonly SemaphoreSlim Locker = new(1);
    private bool IsWorkDone { get; set; } 
    
    public async ValueTask InitAsync()
    {
        if (IsWorkDone) return;
        
        await Locker.WaitAsync();
        if (!IsWorkDone)
            await InitializeAsync();
        
        IsWorkDone = true;
        Locker.Release();
    }

    public abstract ValueTask InitializeAsync();
}