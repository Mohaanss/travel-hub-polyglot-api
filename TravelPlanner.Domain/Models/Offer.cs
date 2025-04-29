using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TravelPlanner.Domain.Models;

public class Leg
{
    [BsonElement("flightNum")]
    public string FlightNum { get; set; } = string.Empty;
    
    [BsonElement("dep")]
    public string Dep { get; set; } = string.Empty;
    
    [BsonElement("arr")]
    public string Arr { get; set; } = string.Empty;
    
    [BsonElement("duration")]
    public int Duration { get; set; }
}

public class Hotel
{
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("nights")]
    public int Nights { get; set; }

    [BsonElement("price")]
    public double Price { get; set; }
}

public class Activity
{
    [BsonElement("title")]
    public string Title { get; set; } = string.Empty;

    [BsonElement("price")]
    public double Price { get; set; }
}

public class Offer
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("from")]
    public string From { get; set; } = string.Empty;

    [BsonElement("to")]
    public string To { get; set; } = string.Empty;

    [BsonElement("departDate")]
    public DateTime DepartDate { get; set; }

    [BsonElement("returnDate")]
    public DateTime ReturnDate { get; set; }

    [BsonElement("provider")]
    public string Provider { get; set; } = string.Empty;

    [BsonElement("price")]
    public double Price { get; set; }

    [BsonElement("currency")]
    public string Currency { get; set; } = "EUR";

    [BsonElement("legs")]
    public List<Leg> Legs { get; set; } = new();

    [BsonElement("hotel")]
    public Hotel? Hotel { get; set; }

    [BsonElement("activity")]
    public Activity? Activity { get; set; }
}
