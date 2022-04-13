using NUnit.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using DiabetesManagement.Extensions.Extensions;
using DiabetesManagement.Contracts;
using DiabetesManagement.Core.Convertors;
using DiabetesManagement.Core.Defaults;

namespace DiabetesManagement.Tests;

public class Tests
{
    internal class MyRequestObject
    {
        public Guid Item1 { get; set; }
        public int Item2 { get; set; }
        public string? Item3 { get; set; }
        public DateTimeOffset ItemDate { get; set; }
    }

    internal class MyRequestObject2
    {
        public Guid? Item1 { get; set; }
        public int? Item2 { get; set; }
        public string? Item3 { get; set; }
    }

    private IConvertorFactory? convertorFactory;

    [SetUp]
    public void Setup()
    {
        convertorFactory = new DefaultConvertorFactory(new IConvertor[] { new GuidConvertor(), new IntConvertor(), new StringConvertor() });
    }

    [Test]
    public void Test1()
    {
        var requestDictionary = new List<KeyValuePair<string, StringValues>>();
        requestDictionary.Add(KeyValuePair.Create("Item1", new StringValues("0fda06cf9d18468188cb9a81e6e90f9e")));
        requestDictionary.Add(KeyValuePair.Create("Item2", new StringValues("22")));
        requestDictionary.Add(KeyValuePair.Create("Item3", new StringValues("test")));
        requestDictionary.Add(KeyValuePair.Create("ItemDate", new StringValues("2022-04-12T20:14:43.8375256+00:00")));
        var requestObject = requestDictionary.Bind<MyRequestObject>(convertorFactory!);
        Assert.AreEqual(Guid.Parse("0fda06cf9d18468188cb9a81e6e90f9e"), requestObject.Item1);
        Assert.AreEqual(22, requestObject.Item2);
        Assert.AreEqual("test", requestObject.Item3);
        Assert.AreEqual(new DateTimeOffset(2022,04,12, 20, 14, 43, TimeSpan.FromHours(0)), requestObject.ItemDate);
        var requestObject2 = requestDictionary.Bind<MyRequestObject2>(convertorFactory!);
        Assert.AreEqual(Guid.Parse("0fda06cf9d18468188cb9a81e6e90f9e"), requestObject2.Item1);
        Assert.AreEqual(22, requestObject2.Item2);
        Assert.AreEqual("test", requestObject2.Item3);
    }
}