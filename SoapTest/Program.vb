Module Program

    Sub Main()

        Console.WriteLine("Indica Acción")
        Console.WriteLine("( C ) - Crear o editar")
        Console.WriteLine("( X ) - Cancelar")
        Console.WriteLine("( O ) - Obtener")
        Dim action = Console.ReadLine()
        Dim result As EVASoap.SendOrderResponseComplex

        Try
            If action.ToUpper().Trim() = "C" Then
                Console.WriteLine("Ingrese Folio (Dejar en blanco para crear uno nuevo)")
                Dim folio As String = Console.ReadLine()
                result = create_or_update_order(folio)
            ElseIf action.ToUpper().Trim() = "X" Then
                Console.WriteLine("Ingresa el ID de la orden")
                Dim orderId As String = Console.ReadLine()
                result = cancel_order(orderId)

            ElseIf action.ToUpper().Trim() = "O" Then
                Console.WriteLine("Ingresa el ID de la orden")
                Dim orderId As String = Console.ReadLine()
                result = get_order(orderId)
            Else
                Throw New Exception(String.Format("Invalid Option: {0}", action
                                                  ))
            End If
            'Console.WriteLine(result.Order.Folio & " -> " & result.Order.Id)
            print_order_data(result)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        Console.ReadLine()


    End Sub

    Private Sub print_order_data(data As EVASoap.SendOrderResponseComplex)
        Console.WriteLine(String.Format("Order ID:          {0}", data.Order.Id))
        Console.WriteLine(String.Format("Order Folio:       {0}", data.Order.Folio))
        Console.WriteLine(String.Format("Order Estatus:     {0}", data.Order.Status))

        Console.WriteLine(String.Format("Paciente ID:       {0}", data.Patient.id))
    End Sub

    Private Function get_auth_token() As EVASoap.TokenHeader
        Dim soapClient = get_soap_client()

        Dim requestheader = New EVASoap.RequestHeader()
        requestheader.email = "user@evacenter.com"
        requestheader.password = "My$ecureP@ssw0rd"

        Dim loginResponse = soapClient.authenticate(requestheader, New EVASoap.authenticate)
        Dim token = loginResponse.authenticateResult.token

        Dim tokenHeader = New EVASoap.TokenHeader()
        tokenHeader.Token = token
        Return tokenHeader
    End Function

    Private Function get_soap_client() As EVASoap.pacs_soap_serviceClient
        Return New EVASoap.pacs_soap_serviceClient("pacs_soap_service")
    End Function

    Private Function get_order(orderId As String) As EVASoap.SendOrderResponseComplex
        Dim soapClient = get_soap_client()
        Dim tokenHeader = get_auth_token()

        Dim getOrderRequest As New EVASoap.GetOrder()
        getOrderRequest.OrderId = orderId

        Dim result = soapClient.GetOrder(tokenHeader, getOrderRequest)
        Return result.get_orderResult

    End Function

    Private Function cancel_order(orderId As String) As EVASoap.SendOrderResponseComplex
        Dim soapClient = get_soap_client()
        Dim tokenHeader = get_auth_token()


        Dim cancelRequest = New EVASoap.CancelOrder()
        cancelRequest.CancelReason = "other"
        cancelRequest.CancelDescription = Faker.Lorem.Sentence(5)
        cancelRequest.OrderId = orderId

        Dim result = soapClient.CancelOrder(tokenHeader, cancelRequest)
        Return result.cancel_orderResult

    End Function
    Private Function create_or_update_order(folio As String) As EVASoap.SendOrderResponseComplex
        Dim soapClient = get_soap_client()
        Dim tokenHeader = get_auth_token()

        Dim orderRequest = New EVASoap.SendOrderRequest()


        orderRequest.Order = New EVASoap.OrderComplexModel()

        If folio.Length <> 0 Then
            orderRequest.Order.Folio = Integer.Parse(folio).ToString("00000")
        Else
            orderRequest.Order.Folio = Faker.RandomNumber.Next(10000, 99999).ToString()
        End If

        orderRequest.Order.Description = Faker.Lorem.Paragraph()
        orderRequest.Order.StudyCode = Faker.RandomNumber.Next(1, 999).ToString("000")
        orderRequest.Order.StudyName = Faker.Lorem.Sentence(5)
        orderRequest.Order.Modality = "CT"

        orderRequest.Facility = New EVASoap.FacilityComplexModel()
        orderRequest.Facility.Identifier = "eva-centro"

        orderRequest.Patient = New EVASoap.PatientComplexModel
        orderRequest.Patient.Name = Faker.Name.First()
        orderRequest.Patient.FirstSurname = Faker.Name.Middle()
        orderRequest.Patient.LastSurname = Faker.Name.Last()
        orderRequest.Patient.Email = Faker.Internet.Email()
        orderRequest.Patient.Gender = "O"
        orderRequest.Patient.PhoneCode = Faker.RandomNumber.Next(1, 999).ToString()
        orderRequest.Patient.PhoneNumber = Faker.Phone.Number
        orderRequest.Patient.BirthDate = Faker.Identification.DateOfBirth


        orderRequest.Physician = New EVASoap.PractitionerComplexModel()
        orderRequest.Physician.Name = Faker.Name.First
        orderRequest.Physician.FirstSurname = Faker.Name.Middle
        orderRequest.Physician.LastSurname = Faker.Name.Last
        orderRequest.Physician.Email = Faker.Internet.Email()
        orderRequest.Physician.Gender = "M"
        orderRequest.Physician.PhoneCode = Faker.RandomNumber.Next(1, 999).ToString()
        orderRequest.Physician.PhoneNumber = Faker.Phone.Number



        Dim sendOrderRequest = New EVASoap.SendOrder()
        sendOrderRequest.OrderRequest = orderRequest

        Dim result = soapClient.SendOrder(tokenHeader, sendOrderRequest)
        Return result.send_orderResult

    End Function

End Module
