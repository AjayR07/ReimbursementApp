namespace ReimbursementApp.Application.Exceptions;

public class PasswordMismatchException:Exception
{
    public PasswordMismatchException(string message):base(message)
    {
        
    }
}