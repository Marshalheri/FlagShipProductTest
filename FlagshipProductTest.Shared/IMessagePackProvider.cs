using FlagshipProductTest.Shared.Services;

namespace FlagshipProductTest.Shared
{
    public interface IMessagePackProvider
    {
        MessagePack GetPack();
    }
}
