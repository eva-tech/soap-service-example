


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
        tokenRequest.email = "hmac@server.local";
        tokenRequest.password = "Hmac123!";

        var authMethod = new EvaSoap.authenticate();

        var tokenResult = await client.authenticateAsync(tokenRequest, authMethod);
        string token = tokenResult.authenticateResponse.authenticateResult.token;
        DateTime expires = tokenResult.authenticateResponse.authenticateResult.expires;


        Console.WriteLine(String.Format("Token: {0}, Expires: {1}", token, expires));


        var tokenHeader = new EvaSoap.TokenHeader();
        tokenHeader.Token= token;


        var order = new EvaSoap.OrderComplexModel();
        order.Folio = "123";
        order.Description = "Refiere dolor en la zona";
        order.StudyCode = "001421";
        order.StudyName = "CR Bilateral";
        order.Modality = "CR";

        var facility = new EvaSoap.FacilityComplexModel();
        facility.Identifier = "PBL6";

        var patient = new EvaSoap.PatientComplexModel();
        patient.Email = "john+doe@mail.com";
        patient.Name = "John";
        patient.FirstSurname = "Doe";
        patient.LastSurname = "Perry";
        patient.PhoneNumber = "5522000000";
        patient.PhoneCode = "52";
        patient.Gender = "M";


        var physician = new EvaSoap.PractitionerComplexModel();
        physician.Email = "dr.john@hospital.com";
        physician.Name = "John";
        physician.FirstSurname = "Jhonson";
        physician.LastSurname = "Matew";
        physician.PhoneNumber = "5522000000";
        physician.PhoneCode = "52";


        var request = new EvaSoap.SendOrderRequest();
        request.Order = order;
        request.Facility = facility;
        request.Patient = patient;
        request.Physician = physician;

        var sendOrderMethod = new EvaSoap.SendOrder();
        sendOrderMethod.OrderRequest = request;

        var response = await client.SendOrderAsync(tokenHeader, sendOrderMethod);
        Console.WriteLine(String.Format("Order Generated: {0}", response.send_orderResponse.send_orderResult.Order.id));
        
    }
}
