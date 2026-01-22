I have thoroughly organized your Viva questions and notes into a clean, professional **Markdown** format. I removed redundant AI conversational filler, consolidated similar topics, and used a consistent structure to make it easy for you to skim during future study sessions or interviews.

---

# 🚀 C# Interview & Viva Reference Guide

This document contains a structured breakdown of core C# and .NET concepts, memory management, and modern language features.

---

## 1. Memory Management & Execution

### **Static Classes: Where do they live?**

Static classes and their data (static fields) are stored on the **Heap**. Specifically, they reside in a special part of the heap called the **"High-Frequency Heap"** (or Loader Heap).

| Memory Area | What it stores | Lifetime |
| --- | --- | --- |
| **Stack** | Local variables, primitives (`int`, `bool`), and method references. | Destroyed on method exit. |
| **Managed Heap** | Objects created with `new`. | Managed by the Garbage Collector. |
| **High-Frequency Heap** | **Static classes**, static fields, and Type objects. | Application lifetime; not garbage collected. |

### **The Garbage Collector (GC)**

The GC is an automatic memory manager that uses a **Mark-and-Sweep** algorithm and a **Generational system**.

* **Gen 0 (Youngest):** Short-lived objects (local variables). Frequent, fast collections.
* **Gen 1 (Buffer):** Objects that survived one collection.
* **Gen 2 (Oldest):** Long-lived objects (static data, caches). Expensive to collect.
* **LOH (Large Object Heap):** Objects >85,000 bytes. Not compacted by default; treated as Gen 2.

### **The C# Compilation Process**

1. **Parsing:** Text → Syntax Tree (Roslyn).
2. **Binding:** Resolve names → Semantic Model.
3. **Source Generators:** Optional step where new code is injected.
4. **Lowering:** High-level code → low-level IL-friendly form.
5. **Emit:** Generate **IL (Intermediate Language)** bytecode + Metadata.
6. **JIT/AOT:** At runtime, the Just-In-Time compiler turns IL into native machine code.

---

## 2. Type System: Value vs. Reference

### **Value Types vs. Reference Types**

| Feature | Value Types (`struct`, `int`, `enum`) | Reference Types (`class`, `string`, `array`) |
| --- | --- | --- |
| **Storage** | Stored where declared (usually Stack). | Data on Heap; Reference (pointer) on Stack. |
| **Assignment** | **Copies the data** (Photocopy). | **Copies the reference** (Google Doc link). |
| **Default Value** | `0` or "zeroed out" state. | `null`. |

### **Boxing and Unboxing**

* **Boxing:** Converting a Value type → Reference type (`int` to `object`). This creates a new object on the heap (Performance cost).
* **Unboxing:** Converting a Reference type → Value type. Requires an explicit cast.

### **`==` vs `.Equals()**`

* **`==` (Operator):** Resolved at **compile-time**. For reference types, it checks identity (pointer address) unless overloaded (like `string`).
* **`.Equals()` (Method):** Resolved at **run-time** (virtual). Checks for "Value Equality" if overridden.

---

## 3. Parameter Modifiers: `ref` vs `out`

| Feature | `ref` | `out` |
| --- | --- | --- |
| **Initial Value** | Must be initialized before calling. | Can be uninitialized. |
| **Inside Method** | Optional to assign a new value. | **Must** be assigned before the method returns. |
| **Purpose** | Two-way data flow (Read/Write). | One-way output (Return extra values). |

---

## 4. Modern C# Syntax & Features

### **Extension Methods**

Allows adding methods to existing types without modifying the source.

* **Requirements:** Must be in a `static class`, must be a `static method`, and the first parameter must use the `this` keyword.
* **Example:** `public static int WordCount(this string s) => s.Split(' ').Length;`

### **Nullable Types**

* **Nullable Value (`int?`):** Allows primitives to be `null` (e.g., for Database fields).
* **Nullable Reference (`string?`):** A compiler annotation (C# 8+) to help prevent `NullReferenceExceptions`.

### **`is` vs `as` vs Cast**

* **`is`:** Checks type and returns a bool. Best used with pattern matching: `if (obj is string s)`.
* **`as`:** Attempts to cast; returns `null` on failure. Never throws an exception.
* **`(Type)`:** A hard cast. Throws `InvalidCastException` if it fails.

### **Object & Anonymous Types**

* **Object Initializers:** `var p = new Person { Name = "Akshay" };` (Syntactic sugar for cleaner code).
* **Anonymous Types:** `var item = new { Name = "Laptop", Price = 1000 };`. These are read-only and excellent for LINQ projections.

---

## 5. Classes, Structs, and Inheritance

### **Structs vs. Enums**

* **Struct:** A lightweight data container. Best for small, immutable data (e.g., `Point`, `Color`).
* **Enum:** A set of named constants backed by an integer (e.g., `OrderStatus.Pending`).

### **Abstract Classes vs. Method Hiding**

* **Abstract Class:** An "incomplete" blueprint. Forces derived classes to implement `abstract` methods via `override`. Supports polymorphism.
* **Method Hiding (`new`):** "Shadows" a base method. Binding is determined by the **variable type**, not the object type. **Avoid this** unless necessary for third-party compatibility.

### **Partial Classes**

The `partial` keyword allows a single class definition to be split across multiple `.cs` files.

* **Common Use:** Separating machine-generated code (like UI designers) from handwritten logic.

---

### **Next Steps**

Would you like me to generate a **practice quiz** based on these notes to help you prepare for your next viva?



### List of the questions that were asked are :

what are static classes in c# , on stack or heap 

object initialiser ?

value types vs reference types 

explain the working of garbage collector

annyonymous types

structures and enums

what is extension method and extension method vs inheritence vs interface

diff between boxing and unboxing

what are extensions methods and partial types and shield classes ?

what are nullable types in c#

difference between ref and out keywords

structs vs enums

compilation process of c# code

is vs as
equals vs double equalto

implicit typed local variables ??

namespace means

abstract classes , method hiding

what are static classes ?