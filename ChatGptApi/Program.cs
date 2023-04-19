var builder = WebApplication.CreateBuilder(args);

// Define a rota base para a aplicação
builder.WebHost.UseUrls(builder.Configuration.GetSection("HomeUrl").Value);

// Adicione serviços ao contêiner.
builder.Services.AddControllers();

var app = builder.Build();

// Configure o pipeline de solicitação HTTP.
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
