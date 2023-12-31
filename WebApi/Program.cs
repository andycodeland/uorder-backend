using WebApi;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services); // calling ConfigureServices method
var app = builder.Build();
startup.Configure(app, builder.Environment); // calling Configure method

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
//}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();