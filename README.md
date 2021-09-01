[![.NET](https://github.com/EugeneKrapivin/JsonTranslate.NET/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/EugeneKrapivin/JsonTranslate.NET/actions/workflows/dotnet.yml)
[![CodeQL](https://github.com/EugeneKrapivin/JsonTranslate.NET/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/EugeneKrapivin/JsonTranslate.NET/actions/workflows/codeql-analysis.yml)
[![codecov](https://codecov.io/gh/EugeneKrapivin/JsonTranslate.NET/branch/main/graph/badge.svg?token=91KJVNDKWQ)](https://codecov.io/gh/EugeneKrapivin/JsonTranslate.NET)

# JsonTranslate.NET

> **Note**: This library is a work in progress and should probably not be used in any
> production scenarios. Questions, proposals (pull requests) and bugs are very welcome :)

library provides the ability to translate JSONs into other JSONs by using a DSL. 
Based on ideas inspired by [JUST.net](https://github.com/WorkMaze/JUST.net) library by WorkMaze.

## Quick start

### Installation

1. Install the nugets into your target project  
1.1. `JsonTranslate.NET.Core` - core logic, contains the transformer implementations, 
factory and visitors  
1.2. `JsonTranslate.NET.Core.JsonDsl` - a skinny wrapper (5 lines of code really) to 
wrap serialization and deserialization of [the Instruction](#The-Instruction) into
JSON format.  
1.3. `JsonTranslate.NET.Core.FluentDsl` - a DSL allowing concise serialization and
deserialization operations into a format akin to the [JUST.net library](https://github.com/WorkMaze/JUST.net).   

### Example

Given a source JSON:
```json
{
  "phone_numbers": [
    {
      "type": "home",
      "number": "+1-555-5551"
    },
    {
      "type": "work",
      "number": "+1-555-5552"
    }
  ],
  "addresses": [
    {
      "type": "home",
      "city": "New-York",
      "street1": "1st Ave 1",
      "street2": "Apt 11",
      "country": "USA"
    },
    {
      "type": "work",
      "city": "New-York",
      "street1": "1st Ave 2",
      "street2": "floor 100",
      "country": "USA"
    }
  ]
}
```

You'd like to transform it into the following JSON:

```json
{
  "phoneNumbers": {
    "home": "+1-555-5551",
    "work": "+1-555-5552"
  },
  "addrs": {
    "home": {
      "city": "New-York",
      "country": "USA",
      "street": "1st Ave 1, Apt 11"
    },
    "work": {
      "city": "New-York",
      "country": "USA",
      "street": "1st Ave 2, floor 100"
    }
  }
}
```

You can always approach it programmatically and write specialized code to acheive 
this goal. However, this may not be the best scalable solution, especially if the
data is unstructured or there are multiple data formats owned by different tenants.

Given a recipe:


> WORK IN PROGRESS
```code
#toobj(
    #property(
        #unit({"value":"phoneNumbers"}), 
        #toobj(
            #valueof({"path":"$.phone_numbers"}), 
            #current(
                #valueof({"path":"$.type"})), 
                #current(#valueof({"path":"$.number"})))), 
    #property(
        #unit({"value":"addrs"}), 
        #toobj(
            #valueof({"path":"$.addresses"}), 
            #current(
                #valueof({"path":"$.type"})), 
            #current(
                #toobj(
                    #property(
                        #unit({"value":"city"}), 
                        #valueof({"path":"$.city"})), 
                    #property(
                        #unit({"value":"country"}), 
                        #valueof({"path":"$.country"})), 
                    #property(
                        #unit({"value":"street"}), 
                        #str_join({"separator":", "}, 
                            #valueof({"path":"$.street1"}), 
                            #valueof({"path":"$.street2"}))))))))

```

with the following code:

> WORK IN PROGRESS
```csharp

// TBD

```

## Usage

The library is intended to be integrated using 2 built-in 
[DSL](https://en.wikipedia.org/wiki/Domain-specific_language)s:

* `JSON` based DSL that will probably be easier to compose in a frontend.
* a custom DSL inspire by [JUST.net](https://github.com/WorkMaze/JUST.net), 
providing a lean language to describe transformation chains.

### The Instruction

In the application level, both `DSL`s are translated into a tree of nested object of 
type [`Instruction`](src/JsonTranslate.NET.Core.Abstractions/Instruction.cs).
The instruction object describes the transformers and the bindings between them in a 
tree structure. Hence, when the instruction object is "built" into the transformer, 
all the relevant transformers are created and bound properly.

> Future plans: Strict instruction mode will require a json schema to ensure that 
> type compatibility inside the transformation chain

#### Name

Each transformer has a uniquely identifing name. The name will be used inorder to
serialize the instruction set, deserialized an instruction set, lookup the transformers
in the transformer factory. The name should not change as it constitutes a breaking
change.

> Note: a planned feature will allow to alias transformers so that breaking changes are
> in naming is gradual.

#### Config

As stated previosly, some transformers require an external configuration to operate
properly. There aren't many such transformers, however, most of those actually require
the configuration. Example: `valueof` transformer must have a configuration pointing
it to the correct place in the input JSON so that it could extract the correct value.

#### Bindings

Bindings are actually the nested nodes of any instruction. Taking a swift look at a
reducted version of a JSON serialized instruction:

```json
{
  "name": "str_join",
  "config": { "separator": " " },
  "bindings": [
    {
      "name": "lookup",
      "config": {
        "lookup": [
          {
            "key": "look me up",
            "value": "test!!!"
          }
        ],
        "onMissing": "default",
        "default": "test???"
      },
      "bindings": [
        {
          "name": "valueof",
          "config": { "path": "$.test" }
        }
      ]
    },
    {
      "name": "unit",
      "config": { "value": "this is my unit value" }
    }
  ]
}
```

As you can see, each `Instruction` node consists of 3 properties:
* **name** - the name of the transformer represented by this instruction node.
* **config** - some transformers require an external configuration to operate properly.
* **bindings** - nested instruction nodes that will feed their transformation result
upwards as input for the current instruction node.

We can clearly see that `str_join` will receive inputs from a `lookup` transformer and
a `unit` transformer. In order to properly execute the `lookup` transformation, we will
first have to execute an even deeply nested transformer called `valueof`.

### Transformer

When de/serializing a transformation tree, the tree is de/serialized as an instruction
set. This design choice allows to decouple the serialized representation of the
otherwise polymorphic nature of the transformer.

A transformer is the execution unit of any instruction. There are many various
transformer implemetations all based of a shallow abstraction hierarchy.

```csharp

public interface IJTokenTransformer : IAccepting<IJTokenTransformer> {...}

public abstract class TransformerBase : IJTokenTransformer {...}

public abstract class SinglyBoundTransformer : TransformerBase {...}

public abstract class MultiBoundTransformer : TransformerBase {...}

public abstract class ValueProvidingTransformer : TransformerBase {...}

```

this hierarchy allows for better controll of cross cutting concerns such as binding and
inputs.

Since transformers are the execution unit of the whole library, extending the library
is done by adding new transformer classes.

> Note: All transformers must be of the outlined hierarchy **and** have a TransformerAttribute 

### JSON DSL

This section will describe the moving parts of the `JSON` based description language and how to use it.

The DSL is really serialized `Instruction` object, that describes a single transformation chain.

```json
{
  "name": "str_join",
  "config": { "separator": " " },
  "bindings": [
    {
      "name": "lookup",
      "config": {
        "lookup": [
          {
            "key": "look me up",
            "value": "test!!!"
          }
        ],
        "onMissing": "default",
        "default": "test???"
      },
      "bindings": [
        {
          "name": "valueof",
          "config": { "path": "$.test" }
        }
      ]
    },
    {
      "name": "unit",
      "config": { "value": "this is my unit value" }
    }
  ]
}
```

The main advantage of this was of describing a transformation chain is that it is very easy to compose those structures, especially easy for the front end to compose this in-order to send to the backend for processing.

On the other side, it is very verbose and not very intuitive when trying to describe a full `JSON` transformation (i.e. trying to remap and transform the whole `JSON`).

#### Understanding the example

As stated before, the `JSON` is really a tree of `Instruction` objects. To begin to understand it, first thing to understand is that the first transformation to be executed is the most nested one.

```json
{
    "name": "valueof",
    "config": { "path": "$.test" }
}
```

> Detailed explanation with some examples will be added [later in the readme](#Supported%20transformers).

The `valueof` transformer is really the only method to fetch data from inside the source `JSON`. The result of executing this transformer will return the value found by the  predefined [json path](https://www.newtonsoft.com/json/help/html/QueryJsonSelectToken.htm) to the next transformer in the chain.

```json

{
    "name": "lookup",
    "config": {
    "lookup": [
        {
        "key": "look me up",
        "value": "test!!!"
        }
    ],
    "onMissing": "default",
    "default": "test???"
    },
    "bindings": // edited out for brevity //
}

```

> Bindings are edited out for brevity.

The `lookup` transformer will recieve the value found by the nested `valueof` transformer and try to look it up in the lookup list.

After finding the value in the dictionary (or handling an event of a missing value), the result will be returned to the `str_join` transformer.

```json
{
  "name": "str_join",
  "config": { "separator": " " },
  "bindings": [
    // lookup transformer edited for brevity //
    {
      "name": "unit",
      "config": { "value": "this is my unit value" }
    }
  ]
}
```

> Bindings are edited out for brevity.

Notice that the `str_join` bindings contains 2 bindings: `lookup` and `unit`. The fact that the bindings property is an array allows the user to pass multiple values to be processed into the transformer.

> The `unit` transformer is used to pass a constant value into the chain, without resorting to hacks like adding the value on top of the source json.

Now that we've chewed on the transformation directive, lets see it at work.

Given a source json

```json
```

and the noted about example, when running the 

### "JUST" inspired DSL

The `JUST` inspired DSL is less verbose than the JSON DSL. However, it is also less
humanly readable and probably a bit harder for construction in the front end since it 
requires the frontend to know the DSL in order to build an expression.

A structure of an expression is recuresive

```
#<operator> (<json config>? [, #<operator>]*)
```

> Note: some grammar definitions are removed for brevity.

Notice that this DSL is capble of expression all that the JSON DSL can, 
and vise-a-versa. They are designed to be compatible. As a general use case, it is 
possible that you'd want to receive the JSON DSL from the front-end and store it in the 
database using the JUST DSL to save space.

### Supported transformers

#### `ValueOf` and `Unit`

##### ValueOf

The `valueof` transformer is realy the only transformer that can extract data from the
input JSON.

It doesn't not support any bindings (will throw an exception) and has a required config

```csharp
public class ValueOfTransformerConfig
{
    [JsonProperty("path")]
    public string Path { get; set; }
}
```

or in JSON

```json
{
    "path" : ""
}
```

The value of the property `Path` is expected to be a valid JSON Path expression 
supported by Newtonsfost JSON.net library.

###### usage

Input:
```json
{
  "menu": {
    "popup": {
      "menuitem": [{
          "value": "Open",
          "onclick": "OpenDoc()"
        }, {
          "value": "Close",
          "onclick": "CloseDoc()"
        }
      ],
	  "submenuitem": "CloseSession()"
    }
  }
}
```

Recipe:

```just
#obj(
  #property(
    #unit({"value":"result"}), 
    #obj(
      #property(
        #unit({"value":"Open"}), 
        #valueof({"path":"$.menu.popup.menuitem[?(@.value=='Open')].onclick"})), 
      #property(
        #unit({"value":"Close"}), 
        #valueof({"path":"$.menu.popup.menuitem[?(@.value=='Close')].onclick"})))))
```

or

```json
{
  "name": "obj",
  "bindings": [
    {
      "name": "property",
      "bindings": [
        {
          "name": "unit",
          "config": {
            "value": "result"
          }
        },
        {
          "name": "obj",
          "bindings": [
            {
              "name": "property",
              "bindings": [
                {
                  "name": "unit",
                  "config": {
                    "value": "Open"
                  }
                },
                {
                  "name": "valueof",
                  "config": {
                    "path": "$.menu.popup.menuitem[?(@.value=='Open')].onclick"
                  }
                }
              ]
            },
            {
              "name": "property",
              "bindings": [
                {
                  "name": "unit",
                  "config": {
                    "value": "Close"
                  }
                },
                {
                  "name": "valueof",
                  "config": {
                    "path": "$.menu.popup.menuitem[?(@.value=='Close')].onclick"
                  }
                }
              ]
            }
          ]
        }
      ]
    }
  ]
}
```

Result:

```json
{
  "result": {
    "Open": "OpenDoc()",
	"Close": "CloseDoc()"
  }
}
```

> Note: there are other transformers used here, as the `valueof` transformer is really 
> use less in a context other than in conjunction with other transformers.

###### Context awareness
Notice that when working within a `#current` transformer the path will be relative to
the current item in an iteration, not the root of the entire input JSON.

##### Unit

The `unit` transformer, just like the `valueof` transformer does not support any bindings
and will also throw an exception when attempting to bind it.

The purpose of the `unit` transformer is to allow passing constant values into the
expression. A common use case could be when providing property names or constant parameters
for a predicate expression.

Just like the `valueof` transformer, the `unit` transformer also requires configuration:

```csharp
public class UnitTransformerConfig
{
    [JsonProperty("value")]
    public object Value { get; set; }
}
```

or in JSON

```json
{
    "value" : ""
}
```

Notice that the value can be any valid JSON value, as long as it makes sense in the
context of the whole expression.

#### Type converters

Currently there are 4 type converters supported out of the box:

* `tostring`
* `todecimal`
* `toboolean`
* `tonumber`

all these transformers support transformations strictly from and to those types only.

#### Aggregators

##### `toarray` Aggregator

#### Collection

##### `current` item selector

used in a loop to reference the current value. A nested `valueof` transformer will
execute the JSON Path on the current item as root element.

##### `select` Operator

##### `first` Operator

##### `single` Operator

##### `where` Operator

#### Structural

##### `deconstruct` Transformer

Takes an object and transforms it to an array of (key, value) tuples

##### `agr_obj` Transformer

> Note: will be removed in favor of a generic aggregate collection operator

Takes an array and constructs an object out of it using key and value selectors

##### `property` operator

creates a `JProperty` from a given key and value

#### Math

##### Reducers

###### `min` Reducer

###### `max` Reducer

###### `avg` Reducer

###### `sum` Reducer

##### Operators

**TBD**

#### String

##### Reducers

###### `concat` Reducer

###### `str_join` Reducer

##### Operators

###### `firstindexof` Operator

###### `lastindexof` Operator

###### `substring` Operator
