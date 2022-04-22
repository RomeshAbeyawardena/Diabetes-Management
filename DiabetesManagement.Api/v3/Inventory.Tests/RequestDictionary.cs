using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using Inventory.Core.Convertors;
using Inventory.Core.Defaults;
using System.ComponentModel.DataAnnotations;
using Inventory.Contracts;
using Inventory.Extensions;

namespace Inventory.Tests;

public class RequestDictionary
{
    internal class MyRequestObject
    {
        public Guid? Item1 { get; set; }
        public int? Item2 { get; set; }
        public string? Item3 { get; set; }
        public DateTimeOffset? ItemDate { get; set; }
        public DateTime? ItemDate2 { get; set; }
    }

    internal class MyRequestObject2
    {
        [Required]
        public Guid? Item1 { get; set; }
        [Required]
        public int? Item2 { get; set; }
        [Required]
        public string? Item3 { get; set; }
        [Required]
        public DateTimeOffset? ItemDate { get; set; }
        [Required]
        public DateTime? ItemDate2 { get; set; }
    }

    private IConvertorFactory? convertorFactory;

    [SetUp]
    public void Setup()
    {
        convertorFactory = new DefaultConvertorFactory(new IConvertor[] { new DateTimeOffsetConvertor(), new GuidConvertor(), new IntConvertor(), new StringConvertor() });
    }

    [Test]
    public void Bind_Success()
    {
        var requestDictionary = new List<KeyValuePair<string, StringValues>>
        {
            KeyValuePair.Create("Item1", new StringValues("0fda06cf9d18468188cb9a81e6e90f9e")),
            KeyValuePair.Create("Item2", new StringValues("22")),
            KeyValuePair.Create("Item3", new StringValues("test")),
            KeyValuePair.Create("ItemDate", new StringValues("2022-04-12T20:14:43.0000000+00:00")),
            KeyValuePair.Create("ItemDate2", new StringValues("2022-04-12T20:14:43.0000000+00:00"))
        };
        var requestObject = requestDictionary.Bind<MyRequestObject>(convertorFactory!);
        Assert.AreEqual(Guid.Parse("0fda06cf9d18468188cb9a81e6e90f9e"), requestObject.Item1);
        Assert.AreEqual(22, requestObject.Item2);
        Assert.AreEqual("test", requestObject.Item3);
        Assert.AreEqual(new DateTimeOffset(2022, 04, 12, 20, 14, 43, TimeSpan.FromHours(0)), requestObject.ItemDate);
        Assert.AreEqual(new DateTime(2022, 04, 12, 20, 14, 43), requestObject.ItemDate2);
    }

    [Test]
    public void Bind_Fail()
    {
        var requestDictionary = new List<KeyValuePair<string, StringValues>>();

        var requestObject = Assert.Throws<ValidationException>(() => requestDictionary.Bind<MyRequestObject2>(convertorFactory!));

    }
}