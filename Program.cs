using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var conecctionString = builder.Configuration.GetConnectionString("DefaultConnection"); 
// Add services to the container.

builder.Services.AddDbContext<WebApiFarm.DataContext>(options => options.UseSqlite(conecctionString));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(builder => {
    builder.AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
