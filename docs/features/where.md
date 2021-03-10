---
layout: default
title: Where
nav_order: 1
has_children: true
parent: Base Features
grand_parent: Features
---

`Where` is used to select objects meeting a certain criteria, as defined by a lambda expression. For example:

```csharp
Query.Where(x => x.Id == Id);
```

This `Query` will select an object `x` if `x.Id` is equal to `Id`. Note that while this particular query likely selects a single object (since Id's should generally be unique), the `Where` operator will select *all* objects matching the specified criteria.
