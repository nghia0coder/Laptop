﻿using Laptop.Models;
using Laptop.Models.Momo;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using System.Security.Cryptography;
using System.Text;

namespace Laptop.Service
{
    public class MomoService : IMomoService
    {
        private readonly IOptions<MomoOptionModel> _options;

        public MomoService(IOptions<MomoOptionModel> options)
        {
            _options = options;
        }

        public async Task<MomoCreatePaymentResponseModel> CreatePaymentAsync(long total)
        {
            
            Order model = new Order();

            model.OrderId = DateTime.UtcNow.Ticks.ToString();
            model.PriceTotal = total;

            string infor = "Thacong";

            var rawData =
                $"partnerCode={_options.Value.PartnerCode}&accessKey={_options.Value.AccessKey}&requestId={model.OrderId}&amount={model.PriceTotal}&orderId={model.OrderId}&orderInfo={infor}&returnUrl={_options.Value.ReturnUrl}&notifyUrl={_options.Value.NotifyUrl}&extraData=";

            var signature = ComputeHmacSha256(rawData, _options.Value.SecretKey);

            var client = new RestClient(_options.Value.MomoApiUrl);
            var request = new RestRequest() { Method = RestSharp.Method.Post };
            request.AddHeader("Content-Type", "application/json; charset=UTF-8");

            // Create an object representing the request data
            var requestData = new
            {
                accessKey = _options.Value.AccessKey,
                partnerCode = _options.Value.PartnerCode,
                requestType = _options.Value.RequestType,
                notifyUrl = _options.Value.NotifyUrl,
                returnUrl = _options.Value.ReturnUrl,
                orderId = model.OrderId,
                orderInfo = infor,
                amount = model.PriceTotal.ToString(),
                requestId = model.OrderId,
                extraData = "",
                signature = signature
            };

            request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

            var response = await client.ExecuteAsync(request);

         

            return JsonConvert.DeserializeObject<MomoCreatePaymentResponseModel>(response.Content);
        }

        public MomoExecuteResponseModel PaymentExecuteAsync(IQueryCollection collection)
        {
            var amount = collection.First(s => s.Key == "amount").Value;
            var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
            var orderId = collection.First(s => s.Key == "orderId").Value;
            return new MomoExecuteResponseModel()
            {
                Amount = amount,
                OrderId = orderId,
                OrderInfo = orderInfo
            };
        }

        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
    }
}
