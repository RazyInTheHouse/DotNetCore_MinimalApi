using StarterM.Models;

var builder = WebApplication.CreateBuilder(args);
//MinimalAPI Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllersWithViews();
//builder.Services.AddControllers();
//builder.Services.AddControllers(o => o.RespectBrowserAcceptHeader = true).AddXmlDataContractSerializerFormatters();
builder.Services.AddSwaggerGen();

List<Customer> customers = new()
{
      new Customer {
        CustomerID ="ALFKI",
        CompanyName = "Alfreds Futterkiste",
        Country ="Germany"
      } ,
      new Customer {
        CustomerID ="ANATR",
        CompanyName = "Ana Trujillo Emparedados y helados",
        Country ="Mexico"
      } ,
      new Customer {
        CustomerID = "ANTON",
        CompanyName = "Antonio Moreno Taqueria",
        Country = "Mexico"
      }
 };

builder.Services.AddSingleton(customers);



var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseStatusCodePages();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
//only WebAPI
//app.MapControllers();

///MVC&WebAPI
app.MapDefaultControllerRoute();
//app.MapGet("/hello", () => "Hello World");
//app.MapGet("/customer", () => new Customer { CompanyName="aaa",Country="sss"});
//app.MapGet("/404", () => Results.NotFound());
Delegate a = (string v) => { };
//@
app.MapGet("/api/customers", (List<Customer> _customers) => _customers);
//"{id}沒加會變querystring去接"
app.MapGet("/api/customers/{id}", (List<Customer> _customers, string id) => {
    var item = _customers.Find(c => c.CustomerID == id);
    if (item == null) return Results.NotFound();
    return Results.Ok(item);
});
app.MapPost("/api/customers", (List<Customer> _customers, Customer customer) =>
{
    _customers.Add(customer);
    return Results.Created($"/api/customers/{customer.CustomerID}", customer);
});
app.MapPut("/api/customers/{id}", (List<Customer> _customers, Customer customer, string id) =>
{
    var item = _customers.Find(c => c.CustomerID == id);
    if (item == null) return Results.NotFound();
    item.CompanyName = customer.CompanyName;
    item.Country = customer.Country;
    return Results.NoContent();
});
app.MapDelete("/api/customers/{id}", (List<Customer> _customers, string id) =>
{
    var item = _customers.Find(c => c.CustomerID == id);
    if (item == null) return Results.NotFound();
    _customers.Remove(item);
    return Results.NoContent();
});
app.Run();
