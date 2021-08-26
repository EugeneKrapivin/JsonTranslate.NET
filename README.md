# JsonTranslate.NET

library provides the ability to translate JSONs into other JSONs by using a DSL. 
Based on ideas inspired by [JUST.net](https://github.com/WorkMaze/JUST.net) library by WorkMaze.

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
> gradual.

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

### Supported transformers

#### `ValueOf` and `Unit`

#### Type converters

#### Mapper

#### Aggregators

#### Math Reducers

#### String Reducers
