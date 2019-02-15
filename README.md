[![NuGet](https://img.shields.io/nuget/dt/Ardalis.Specification.svg)](https://www.nuget.org/packages/Ardalis.Specification)

NuGet: [Ardalis.Specification](https://www.nuget.org/packages/Ardalis.Specification)

# Specification

Base class with tests for adding specifications to a DDD model.

## Usage

Create individual specification classes that inherit from the base. Specify an expression to use for a particular query. Optionally add caching and include (for EF) details. In your repository implementation, accept a Specification in your List or Get methods and implement it as follows:

```

```
