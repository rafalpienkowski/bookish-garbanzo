namespace CRM.Web.Infrastructure;

public interface IBus
{
    void Send(string message);
}