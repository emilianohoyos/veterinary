using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Veterinary.API.Data;
using Veterinary.API.Helpers;
using Veterinary.Shared.Entities;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<SeedDb>();

// Configurar la política de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirTodos",
        policy =>
        {
            policy.SetIsOriginAllowed(origin => true)     // Permitir cualquier origen (puedes restringirlo si lo necesitas)
                  .AllowAnyMethod()     // Permitir cualquier método HTTP (GET, POST, PUT, etc.)
                  .AllowAnyHeader()    // Permitir cualquier encabezado
                  .AllowCredentials();
        });
});
builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserHelper, UserHelper>();


var app = builder.Build();

// Ejecutar SeedDb
using (var scope = app.Services.CreateScope())
{
    var seedDb = scope.ServiceProvider.GetRequiredService<SeedDb>();
    await seedDb.SeedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("PermitirTodos");
app.Run();
