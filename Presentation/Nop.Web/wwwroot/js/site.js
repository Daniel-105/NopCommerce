                            /*The following is the authorize function, which triggers Klarna to perform a risk assessment of the purchase
    The successful response of this risk assessment is an authorization token, which in this example is logged in the console*/
    klarnaRequest.addEventListener("click", () => {

        Klarna.Payments.authorize({
            payment_method_category: 'pay_later'
        }, JSON.parse('{"AcquiringChannel":null,"Attachment":null,"BillingAddress":null,"CustomPaymentMethodIds":null,"Customer":null,"Design":null,"locale":"pt - PT","MerchantData":null,"MerchantReference1":null,"MerchantReference2":null,"MerchantUrls":null,"Options":null,"order_amount":3000,"order_lines":[{"ImageUrl":null,"MerchantData":null,"name":"Ardidas","ProductIdentifiers":null,"ProductUrl":null,"quantity":1,"QuantityUnit":null,"reference":"1","tax_rate":0,"total_amount":1000,"total_discount_amount":0,"total_tax_amount":0,"Type":"physical","unit_price":1000,"Subscription":null},{"ImageUrl":null,"MerchantData":null,"name":"Nike","ProductIdentifiers":null,"ProductUrl":null,"quantity":1,"QuantityUnit":null,"reference":"2","tax_rate":0,"total_amount":2000,"total_discount_amount":0,"total_tax_amount":0,"Type":"physical","unit_price":2000,"Subscription":null}],"order_tax_amount":0,"purchase_country":"PT","purchase_currency":"EUR","ShippingAddress":null,"Intent":null}'),        function (res) {
                console.log("Response from the authorize call:");
                console.log(res);
                authorizationToken = res["authorization_token"];
                //authorizationToken = { "authorizationToken": res["authorization_token"] }
                console.log(authorizationToken);
                //let authorizationToken = res["authorization_token"];
                //console.log(`${authorizationToken} is of type ${typeof authorizationToken}`)
                //placeOrderButton.style.display = "block";
                confirm();

            })
    });