using System;
using System.Collections.Generic;
using JsonTranslate.NET.Core.Transformers.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace JsonTranslate.NET.Transformer.UnitTests.Collections
{
    [TestFixture]
    public class DeconstructTransformerUnitTests
    {
        public static IEnumerable<TestCaseData> DeconstructTransformerTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new JObject
                    {
                        ["full_name"] = "john doe"
                    },
                    new JArray
                    {
                        new JObject
                        { 
                            ["key"] = "full_name", 
                            ["value"] = "john doe"
                        } 
                    }
                );

                yield return new TestCaseData(
                    new JObject 
                    { 
                        ["age"] = 1 
                    },
                    new JArray 
                    { 
                        new JObject 
                        { 
                            ["key"] = "age",
                            ["value"] = 1 
                        } 
                    }
                );

                yield return new TestCaseData(
                    new JObject 
                    { 
                        ["phones"] = new JArray("0544762541", "089922222", "0509519559") 
                    },
                    new JArray 
                    { 
                        new JObject 
                        { 
                            ["key"] = "phones", 
                            ["value"] = new JArray("0544762541", "089922222", "0509519559") 
                        } 
                    }
                );

                yield return new TestCaseData(
                    new JObject 
                    { 
                        ["addresses"] = new JArray
                        {
                            new JObject
                            {
                                ["home"] = "elm street 41"
                            },
                            new JObject
                            {
                                ["work"] = "saseme street"
                            }
                        }
                    },
                    new JArray
                    {
                        new JObject
                        {
                            ["key"] = "addresses",
                            ["value"] = new JArray
                            {
                                new JObject
                                {
                                    ["home"] = "elm street 41"
                                },
                                new JObject
                                {
                                    ["work"] = "saseme street"
                                }
                            }
                        }
                    }
                );

                yield return new TestCaseData(
                    new JObject
                    {
                        ["phones"] = new JArray 
                        { 
                            "0544762541", 
                            "089922222", 
                            "0509519559" 
                        },
                        ["addresses"] = new JArray
                        {
                            new JObject
                            {
                                ["home"] = "elm street 41"
                            },
                            new JObject
                            {
                                ["work"] = "saseme street"
                            }
                        }
                    },
                    new JArray {
                        new JObject
                        {
                            ["key"] = "phones",
                            ["value"] = new JArray
                            {
                                "0544762541",
                                "089922222",
                                "0509519559"
                            }
                        },
                        new JObject { 
                            ["key"] = "addresses",
                            ["value"] = new JArray
                            {
                                new JObject
                                {
                                    ["home"] = "elm street 41"
                                },
                                new JObject
                                {
                                    ["work"] = "saseme street"
                                }
                            }
                        }
                    });

                yield return new TestCaseData(
                    new JObject
                    {
                        ["single1"] = new JArray(new JObject { ["singleInnerArr1"] = "property" }, new JObject { ["singleInnerArr2"] = "property" }),
                        ["single2"] = new JObject { ["singleInner"] = "property" },
                        ["single3"] = new JArray(new JObject { ["single"] = "property" }, new JObject { ["single"] = "property" })
                    },
                    new JArray {
                        new JObject { ["key"] = "single1", ["value"] = new JArray(new JObject { ["singleInnerArr1"] = "property" }, new JObject { ["singleInnerArr2"] = "property" })},
                        new JObject { ["key"] = "single2", ["value"] = new JObject { ["singleInner"] = "property" }},
                        new JObject { ["key"] = "single3", ["value"] = new JArray(new JObject { ["single"] = "property" }, new JObject { ["single"] = "property" })},
                    });
            }
        }

        [TestCaseSource(nameof(DeconstructTransformerTestCases))]
        public void Deconstruct_Should_Return_An_Array_Of_Object_Properties_As_Key_Value_Pairs(JObject data, JArray expected)
        {
            var sut = new DeconstructTransformer();

            sut.Bind(data.AsTransformationResult());
            var result = sut.Transform(new JObject());

            Assert.That(JToken.DeepEquals(result, expected));
        }
    }
}
