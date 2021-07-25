# JsonTranslate.NET

library provides the ability to translate JSONs into other JSONs by using a DSL. Based on ideas inspired by [JUST.net](https://github.com/WorkMaze/JUST.net) library by WorkMaze.

[[__TOC__]]

## Usage

The library is intended to be use in using 2 build it [DSL](https://en.wikipedia.org/wiki/Domain-specific_language)s:

* `JSON` based DSL that will probably be easier to compose in a frontend.
* a custom DSL inspire by [JUST.net](https://github.com/WorkMaze/JUST.net), providing a lean language to describe transformation chains.

### The Instruction

In the application level, both `DSL`s are translated into a tree of nested object of type [`Instruction`](src/JsonTranslate.NET.Core.Abstractions/Instruction.cs).
The instruction object describes the transformers and the bindings between them in an structure akin to a tree.
Hence, when the instruction object is "built" into the transformer, all the relevant transformers are created and bound properly.

> Future plans: Strict instruction mode will require a json schema to ensure that type compatibility inside the transformation chain

#### Bindings

#### Config

#### Name

### Transformer

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

Notice that the `str_join` bindings contains 2 bindings: `lookup` and `unit`. The fact that the bindings propery is an array allows the user to pass multiple values to be processed into the transformer.

> The `unit` transformer is used to pass a constant value into the chain, without resorting to hacks like adding the value on top of the source json.

Now that we've chewed on the transformation directive, lets see it at work.

Given a source json

```json
```

and the noted about example, when running the 

### "JUST" inspired DSL

### Supported transformers

#### `ValueOf` and `Unit`

#### Type converters

#### Mapper

#### Aggregators

#### Math Reducers

#### String Reducers
