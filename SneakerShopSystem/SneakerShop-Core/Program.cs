using Grpc.Net.Client;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin() // Cho phép tất cả các nguồn
              .AllowAnyMethod() // Cho phép tất cả các phương thức (GET, POST, PUT, DELETE, ...)
              .AllowAnyHeader(); // Cho phép tất cả các headers
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Tạo HttpClientHandler nếu cần thiết
var httpHandler = new HttpClientHandler
{
    // Có thể cấu hình thêm nếu cần, ví dụ như kiểm tra chứng chỉ SSL
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

// Định nghĩa các kênh gRPC với các URL localhost và cổng tương ứng
using var userChannel = GrpcChannel.ForAddress("http://localhost:30000", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

using var authChannel = GrpcChannel.ForAddress("http://localhost:30001", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

using var productChannel = GrpcChannel.ForAddress("http://localhost:30002", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

using var stockChannel = GrpcChannel.ForAddress("http://localhost:30003", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

using var orderChannel = GrpcChannel.ForAddress("http://localhost:30004", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

using var shippingChannel = GrpcChannel.ForAddress("http://localhost:30005", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

using var invoiceChannel = GrpcChannel.ForAddress("http://localhost:30006", new GrpcChannelOptions
{
    HttpHandler = httpHandler
});

// Tạo các client gRPC
var userClient = new UserService.User.UserClient(userChannel);
var authClient = new AuthService.Auth.AuthClient(authChannel);
var productClient = new ProductService.Product.ProductClient(productChannel);
var stockClient = new StockService.Stock.StockClient(stockChannel);
var orderClient = new OrderService.Order.OrderClient(orderChannel);
var shippingClient = new ShippingService.Shipping.ShippingClient(shippingChannel);
var invoiceClient = new InvoiceService.Invoice.InvoiceClient(invoiceChannel);

// Thêm client gRPC vào DI container
builder.Services.Add(ServiceDescriptor.Singleton(typeof(UserService.User.UserClient), userClient));
builder.Services.Add(ServiceDescriptor.Singleton(typeof(AuthService.Auth.AuthClient), authClient));
builder.Services.Add(ServiceDescriptor.Singleton(typeof(ProductService.Product.ProductClient), productClient));
builder.Services.Add(ServiceDescriptor.Singleton(typeof(StockService.Stock.StockClient), stockClient));
builder.Services.Add(ServiceDescriptor.Singleton(typeof(OrderService.Order.OrderClient), orderClient));
builder.Services.Add(ServiceDescriptor.Singleton(typeof(ShippingService.Shipping.ShippingClient), shippingClient));
builder.Services.Add(ServiceDescriptor.Singleton(typeof(InvoiceService.Invoice.InvoiceClient), invoiceClient));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseCors();
app.UseCors("AllowAnyOrigin");

app.MapControllers();

app.Run();
