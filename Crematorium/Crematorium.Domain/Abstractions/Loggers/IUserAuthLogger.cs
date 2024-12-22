using Crematorium.Domain.Enums;

namespace Crematorium.Domain.Abstractions.Loggers;

public interface IUserAuthLogger
{
    public void Log(string numPassport, LogAction action);
}
