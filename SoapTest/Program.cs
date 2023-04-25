


internal class Program
{
    private static void Main(string[] args)
    {
        CallService();
        Console.ReadLine();

    }

    private static async void CallService()
    {
        EvaSoap.pacs_soap_serviceClient client = new EvaSoap.pacs_soap_serviceClient(EvaSoap.pacs_soap_serviceClient.EndpointConfiguration.pacs_soap_service);



        var tokenRequest = new EvaSoap.RequestHeader();
        tokenRequest.email = "user@example.com";
        tokenRequest.password = "M1P@$$word!";

        var authMethod = new EvaSoap.authenticate();

        var tokenResult = await client.authenticateAsync(tokenRequest, authMethod);
        string token = tokenResult.authenticateResponse.authenticateResult.token;
        DateTime expires = tokenResult.authenticateResponse.authenticateResult.expires;


        Console.WriteLine(String.Format("Token: {0}, Expires: {1}", token, expires));
        
        
    }
}
