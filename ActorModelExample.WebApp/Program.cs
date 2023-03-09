using ActorModelExample.WebApp.Util;
using ActorModelExample.Orleans.Server;

var builder = WebApplication.CreateBuilder(args);

var venue = DataGenerator.GenerateVenue();
builder.Services.AddSingleton(venue);
builder.AddOrleans();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


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

