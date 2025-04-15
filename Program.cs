namespace EnChat;

public class Program
{
    private const string Origin = "all";

    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(name: Origin,
                policy =>
                {
                    policy.WithOrigins("http://localhost:5173", "https://andyxie.cn:12000")
                        .WithHeaders("Content-Type").AllowCredentials();
                });
        });
        builder.Services.AddDistributedMemoryCache();

        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromDays(7);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
        });
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.WebHost.ConfigureKestrel(c =>
        {
            c.Limits.KeepAliveTimeout = TimeSpan.FromSeconds(10);
        });
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        app.UseCors(Origin);

        app.UseAuthorization();
        app.UseSession(); 

        app.MapControllers();
        try
        {
            app.Run();

        }
        catch (Exception e)
        {
            
        }
    }
}