using Microsoft.AspNetCore.Authorization;
using TalentManager.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/account/login";   // redirect when not logged in
        options.LogoutPath = "/account/logout";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("CanDeleteEmployee", policy =>
        policy.Requirements.Add(new CanDeleteEmployeeRequirement()));
});
builder.Services.AddSingleton<IAuthorizationHandler, CanDeleteEmployeeHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
} else
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//Changes Default page to Swagger when running
//app.MapGet("/", context =>
//{
//    context.Response.Redirect("/swagger");
//return Task.CompletedTask;
//});

app.Run();
