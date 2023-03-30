namespace App.Domain.Primitives;

public interface IInitializationModule
{
    ValueTask Init();
}