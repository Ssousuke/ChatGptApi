var builder = WebApplication.CreateBuilder(args);

// Define a rota base para a aplica��o
builder.WebHost.UseUrls(builder.Configuration.GetSection("HomeUrl").Value);

// Adicione servi�os ao cont�iner.
builder.Services.AddControllers();

var app = builder.Build();

// Configure o pipeline de solicita��o HTTP.
app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
           name: "default",
           pattern: "{controller=ChatGpt}/{action=Chat}/{message?}");
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
