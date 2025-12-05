var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddSingleton(provider =>
{
    var config = builder.Configuration.GetSection("Supabase");
    
    var url = config["Url"];
    var key = config["AnonKey"]; 
    
    if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
    {
        throw new Exception($"⚠️ ERRO: Configuração vazia! \nUrl: {url} \nKey (Anon): {key}");
    }

    var options = new Supabase.SupabaseOptions
    {
        AutoConnectRealtime = true,
        AutoRefreshToken = true
    };

    var client = new Supabase.Client(url, key, options);
    client.InitializeAsync().Wait();

    return client;
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Gera o JSON do Swagger
    app.UseSwagger();
    // Gera a Tela HTML interativa
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
