using Microsoft.AspNetCore.Mvc;

namespace ReimbursementApp.API.Utils;

public static class ApiResponseHandler
{
    public static Dictionary<string, dynamic> OkResponse(string message, dynamic result =null)
    {
        var response = new Dictionary<string, dynamic>()
        {
            {"message" , message},
            {"result", result}
        };
        return response;
    }
}