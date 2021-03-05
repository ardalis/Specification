---
layout: default
title: Overview
nav_order: 1
has_children: false
---
# Overview

The [Specification pattern](https://deviq.com/design-patterns/specification-pattern) encapsulates query logic in its own class, which helps classes follow [Single Responsibility Principle](https://deviq.com/principles/single-responsibility-principle) and promotes reuse of common queries. Specifications can be independently unit tested and when combined with [Repository](https://deviq.com/design-patterns/repository-pattern) help keep the Repository from growing with many additional custom query methods. Specification is commonly used on projects that leverage [Domain-Driven Design](https://deviq.com/domain-driven-design/ddd-overview).

## Installing Ardalis.Specification

[View the repo](https://github.com/ardalis/Specification) | [View the docs](https://ardalis.github.io/Specification)

Install Ardalis.Specification from NuGet. The latest version is available here:

[https://www.nuget.org/packages/Ardalis.Specification/](https://www.nuget.org/packages/Ardalis.Specification/)

Alternately, add it to a project using this CLI command:

```powershell
dotnet add package Ardalis.Specification
```

## Docs theme notes

This docs site is using the [Just the Docs theme](https://pmarsceill.github.io/just-the-docs/docs/navigation-structure/). Details on how to configure its metadata and navigation can be found [here](https://pmarsceill.github.io/just-the-docs/docs/navigation-structure/).
