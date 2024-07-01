using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Payment.StripePayment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace Event_Management.API.Views.Checkout;

public partial class Index 
{
    [Inject]
    public HttpClient HttpClient { get; set; } = default!;

    [Inject]
    public IJSRuntime JsRuntime { get; set; } = default!;

    private List<Event>? _events;
    private IEnumerable<Event[]>? _eventChunksOf4;

    private const string DevApiBaseAddress = "https://localhost:7153";

    //protected async Task OnInitializedAsync()
    //{
    //    _events = await HttpClient.GetFromJsonAsync<List<Event>>($"{DevApiBaseAddress}/api/v1/events/GetAll");

    //    if (_events is not null)
    //    {
    //        _eventChunksOf4 = _events.Chunk(4);
    //    }
    //}

    public async Task OnClickBtnBuyNowAsync(Guid eventId)
    {
        var response = await HttpClient.PostAsJsonAsync($"{DevApiBaseAddress}/api/v1/stripepayment", eventId);

        response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var checkoutOrderResponse = JsonConvert.DeserializeObject<CheckoutOrderResponse>(responseBody);

        // Opens up Stripe.
        await JsRuntime.InvokeVoidAsync("checkout", checkoutOrderResponse.PubKey, checkoutOrderResponse.SessionId);
    }
}