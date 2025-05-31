---
layout: default
title: ORM-Specific Features
nav_order: 2
has_children: true
parent: Features
---

# ORM-Specific Features

The features described below are provider-specific and are excluded from in-memory evaluation. Please refer to each feature’s sub-section for information about which ORM providers are supported.

All builder extension methods offer an overload that accepts a `bool condition` parameter. When the provided condition is false, the expression or value is not added to the specification state. This is especially useful in dynamic scenarios where a condition may or may not apply, functionally similar to `WhereIf` style extensions often found in LINQ helper libraries.