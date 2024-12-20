﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Shared.Entity;

public class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
}