using FxRatesProvider;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddFxRatesProvider(builder.Configuration);

var host = builder.Build();
host.Run();
