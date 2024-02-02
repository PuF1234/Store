using Store;
using Store.Contractors;
using Store.Memory;
using Store.Messages;
using Store.PayPalPayment;
using Store.Web.App;
using Store.Web.Contractors;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddSingleton<IBicycleRepository, BicycleRepository>();
        builder.Services.AddSingleton<BicycleService>();
        builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
        builder.Services.AddSingleton<INotificationService, DebugNotificationService>();
        builder.Services.AddSingleton<IDeliveryService, PostomateDeliveryService>();
        builder.Services.AddSingleton<IPaymentService, CashPaymentService>();
        builder.Services.AddSingleton<IPaymentService, PayPalPaymentService>();
        builder.Services.AddSingleton<IWebContractorService, PayPalPaymentService>();
        builder.Services.AddSingleton<OrderService>();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(20);
            options.Cookie.HttpOnly = true;
            options.Cookie.IsEssential   = true;
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseSession();

        app.MapControllerRoute(
            name: "areas",
            pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");                        

        app.Run();
    }
}