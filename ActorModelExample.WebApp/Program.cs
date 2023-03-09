using ActorModelExample.AkkaNet.Hosting;
using ActorModelExample.WebApp.Util;

var builder = WebApplication.CreateBuilder(args);

var venue = DataGenerator.GenerateVenue();
//builder.Services.AddDummy(venue);

// add this line for akka implementation
builder.Services.AddAkkaNetService();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton(venue);


var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

