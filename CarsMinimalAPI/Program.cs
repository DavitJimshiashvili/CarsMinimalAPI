using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var cars = new List<Car>
{
    new Car { Id = 1, Model = "Toyota Camry", Year = 2020, Color = "Blue" },
    new Car { Id = 2, Model = "Honda Accord", Year = 2019, Color = "Black" },
    new Car { Id = 3, Model = "Ford Mustang", Year = 2021, Color = "Red" },
    new Car { Id = 4, Model = "Chevrolet Camaro", Year = 2018, Color = "White" },
    new Car { Id = 5, Model = "Tesla Model 3", Year = 2022, Color = "Silver" }
    };


app.MapGet("/car", () =>
{
    return cars;
});

app.MapGet("/car/{id}", (int id) =>
{
    var result = cars.Find(c => c.Id == id);
    if (result is null)
    {
        return Results.NotFound($"{nameof(Car)} with id {id} doesn't exist.");
    }

    return Results.Ok(result);
});

app.MapPost("/car", (Car car)=>
{
    cars.Add(car);
    return Results.Ok(cars); ;
});

app.MapPut("/car/{id}", (int id, Car updated) =>
{
    var car = cars.Find(c => c.Id == id);
    if (car is null)
    {
        return Results.NotFound($"{nameof(Car)} with id {id} doesn't exist.");
    }
    car.Color = updated.Color;
    car.Model = updated.Model;
    car.Year = updated.Year;

    return Results.Ok(car);
});

app.MapDelete("/car/{id}", (int id) =>
{
    var car = cars.Find(c => c.Id == id);
    if (car is null)
    {
        return Results.NotFound($"{nameof(Car)} with id {id} doesn't exist.");
    }
    cars.Remove(car);

    return Results.Ok(cars);
});



app.Run();

public class Car
{
    public int Id { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
}