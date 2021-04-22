---
layout: default
title: Patterns Used
parent: Getting Started
nav_order: 3
---

# Design Patterns Used

## Specification Pattern

In the [Specification Pattern](https://deviq.com/design-patterns/specification-pattern), specifications are used to define a query. Using a specification eliminates the need for scattering LINQ logic throughout the codebase, as the LINQ expressions can instead be encapsulated in the specification object. Additionally, using a specification to define the exact data required in a given query increases performance by ensuring only one query needs to be made at a time (as opposed to lazily loading each piece of data as it is required). As used in the Ardalis.Specification package, this pattern is used in conjunction with the Repository Pattern. When used to define an object's state, the Specification Pattern can be used with the Rules Pattern or the Factory Pattern.
