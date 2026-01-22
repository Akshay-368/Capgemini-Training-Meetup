# C# Interview / Concepts Notes

This document organizes the provided content into topic-based sections for easy reference and revision. I've grouped related concepts logically (e.g., basics, types, operators, OOP). Duplicates (e.g., repeated explanations of structs vs enums or extension methods) have been consolidated. I've removed conversational elements like "Let me know! 😄" or follow-up questions to keep it focused as notes. Code examples, tables, and summaries are preserved for clarity.

## 1. Basics & Performance

### Boxing and Unboxing
**Boxing** and **Unboxing** are concepts that exist in languages that have both **primitive types** and their corresponding **wrapper/reference types** (like Java, C#, Kotlin).

Here's the clear difference:

| Feature              | Boxing                                   | Unboxing                                 |
|----------------------|------------------------------------------|------------------------------------------|
| **What happens**     | Primitive → Wrapper object               | Wrapper object → Primitive               |
| **Direction**        | Value type → Reference type              | Reference type → Value type              |
| **Example (Java)**   | `int` → `Integer`                        | `Integer` → `int`                        |
| **Example (C#)**     | `int` → `object` or `Integer`            | `object`/`Integer` → `int`               |
| **Memory effect**    | Creates a new object on the heap         | Extracts the primitive value from object |
| **Performance**      | Slower (object allocation + overhead)    | Slightly slower (null check + extraction)|
| **Common when**      | Putting primitives in collections        | Getting primitives out of collections    |
| **Code example**     | `Integer x = 100;`                       | `int y = x;`                             |

#### Real-life code examples

**Java**

```java
int a = 42;                // primitive

// Boxing (automatic / manual)
Integer boxed = a;         // autoboxing
Integer boxed2 = Integer.valueOf(a);  // explicit boxing

// Happens automatically in these cases:
List<Integer> numbers = new ArrayList<>();
numbers.add(100);          // 100 (int) → Integer (boxing)

// Unboxing
int value = boxed;         // auto-unboxing
int sum = boxed + 10;      // unboxing + addition
```

**C#**

```csharp
int x = 100;

// Boxing
object obj = x;           // boxing happens here
IEnumerable<int> list = new List<int> { x };  // also boxing in some older contexts

// Unboxing (must be explicit in most cases)
int y = (int)obj;         // explicit unboxing
```

#### Quick memory aid
- **Boxing** = putting the small primitive value **inside a box** (wrapper object)
- **Unboxing** = **taking the value out** of the box

#### Most important practical points (2025 perspective)
- Autoboxing/unboxing is convenient but creates hidden performance cost
- In performance-critical code (games, high-frequency trading, Android, large datasets) → avoid unnecessary boxing
- Modern Java (records, value types projects), Project Valhalla → trying to reduce boxing pain in future
- In most normal business/web apps → you usually don't need to worry about it

So in one sentence:

**Boxing** = primitive → object  
**Unboxing** = object → primitive

### ref and out Keywords
**`ref`** and **`out`** are both **parameter modifiers** in C# that allow passing arguments **by reference** (instead of by value), so changes inside the method affect the original variable.

They look very similar, but they have important differences in **initialization rules** and **intended usage**.

#### Key Differences Table (C# as of .NET 9 / C# 13 – 2025/2026)

| Feature                          | `ref`                                      | `out`                                      |
|----------------------------------|--------------------------------------------|--------------------------------------------|
| **Must initialize before call?** | **Yes** – variable must have a value       | **No** – can be uninitialized              |
| **Must assign inside method?**   | **No** – optional (can read without write) | **Yes** – compiler enforces definite assignment before return |
| **Data flow intention**          | Two-way (read + write possible)            | One-way out (method → caller)              |
| **Compiler treats initial value**| Keeps & can read it                        | Ignores / treats as unassigned             |
| **Typical use case**             | Modify existing value                      | Return additional result(s)                |
| **Common pattern**               | Swap, increment counters, in-place changes | `TryParse`, `TryGetValue`, multiple outputs |
| **Overloading allowed?**         | Cannot overload only on `ref` vs `out`     | Same rule – treated same at runtime        |
| **Performance**                  | Identical (both pass reference)            | Identical                                  |

#### Code Examples – Side by Side

```csharp
// ────────────────────────────────────────────────
// Using ref
void Increment(ref int number)
{
    number++;           // Can read & write
    // Could also just read: Console.WriteLine(number);
}

int x = 10;             // MUST initialize
Increment(ref x);
Console.WriteLine(x);   // 11
```

```csharp
// ────────────────────────────────────────────────
// Using out
bool TryDivide(int a, int b, out int quotient, out int remainder)
{
    if (b == 0) 
    {
        quotient = 0;   // MUST assign
        remainder = 0;
        return false;
    }

    quotient = a / b;   // MUST assign before return
    remainder = a % b;
    return true;
}

int q, r;               // No need to initialize
if (TryDivide(17, 5, out q, out r))
{
    Console.WriteLine($"{q} remainder {r}");   // 3 remainder 2
}
```

#### Real-World Patterns You See Every Day

| Pattern                              | Usually uses   | Why?                                                                 |
|--------------------------------------|----------------|----------------------------------------------------------------------|
| `int.TryParse(string, out int)`      | `out`          | Returns success + result without throwing exception                  |
| `Dictionary.TryGetValue(key, out val)` | `out`        | Same reason — success + value                                        |
| `Swap(ref a, ref b)`                 | `ref`          | Needs to read current values to swap                                 |
| `ModifyInPlace(ref largeStruct)`     | `ref`          | Avoid copy + allow mutation                                          |
| `UpdateCoordinates(ref x, ref y)`    | `ref`          | Method changes existing position                                     |
| Multiple return values (pre-ValueTuple) | `out`       | Classic way before `(int, int)` tuples existed                       |

#### Quick Memory Aid (2025 style)
- **`ref`** → "reference to something that **already exists** and **I might change it**"
- **`out`** → "**output** — I'm **giving you** a value you didn't have yet"

#### Bonus: Modern Alternatives (less need for `out` today)
Since C# 7:

```csharp
// Old out style
bool success = dict.TryGetValue(key, out var value);

// Modern – deconstruct + discard
if (dict.TryGetValue(key, out var value))
{ ... }

// Even cleaner with tuples
(int quotient, int remainder) = Divide(17, 5);   // no out needed
```

But `out` is still **very common** (especially in .NET libraries for compatibility and performance).

### Compilation Process of C# Code
**C# code compilation process** (modern .NET with Roslyn compiler – 2025/2026 perspective)

Today almost all C# compilation uses **Roslyn** (the .NET Compiler Platform) — whether you run `dotnet build`, Visual Studio Build, Rider, VS Code + C# Dev Kit, or call the compiler API yourself.

The high-level flow looks like this:

**Your .cs files**  
↓  
**Roslyn compiler pipeline**  
↓  
**.NET assembly** (.dll / .exe) containing **IL** (Intermediate Language) + metadata  
↓  
**.NET runtime** (CoreCLR / .NET Runtime) → JIT → machine code (or AOT → native code)

#### Detailed Phases Inside Roslyn (what actually happens)

| Phase                  | What happens                                                                 | Main Roslyn API / concept                  | Output / Artifact                          | Can you hook into it?                     |
|------------------------|------------------------------------------------------------------------------|--------------------------------------------|--------------------------------------------|--------------------------------------------|
| **1. Parsing**         | Text → tokens → **Syntax Tree** (full fidelity – every whitespace, comment preserved) | `CSharpSyntaxTree.ParseText()`            | **SyntaxTree**                             | Yes – heavily (analyzers, source generators) |
| **2. Declaration**     | Discover all types, members, namespaces (but **not** resolve what they mean yet) | Internal – part of semantic model creation | Declaration symbols (rough)                | Indirectly                                 |
| **3. Binding**         | Resolve names → types, overload resolution, type inference, constant folding, etc. | **SemanticModel** (`GetSymbolInfo()`, `GetTypeInfo()`) | **Bound tree** + **Symbols** (ITypeSymbol, IMethodSymbol, etc.) | Yes – analyzers, refactorings, IntelliSense |
| **4. Lowering**        | High-level constructs → lower-level IL-friendly form (loops → jumps, lambdas → classes, async → state machines, etc.) | Internal (very hard to hook)               | Lowered bound tree                         | Almost no                                  |
| **5. Emit**            | Generate **IL bytecode** + metadata + debug info + resources                | `compilation.Emit()`                       | PE file (.dll/.exe) with IL                | Yes – limited (emit options)               |
| **6. Optional: Source Generators** | Run **before** emit – can read syntax/semantic model and **add new source code** | `ISourceGenerator` / `IIncrementalGenerator` | Additional **SyntaxTrees** injected        | Yes – very powerful (most used extensibility) |
| **7. Optional: Analyzers** | Run during compilation – report warnings/errors, style issues               | `DiagnosticAnalyzer`                       | Diagnostics (IDE squiggles + build errors) | Yes – Roslyn analyzers                     |

#### Typical command-line / everyday view

```bash
# You type:
dotnet build MyApp.csproj

# Under the hood (simplified):
csc.exe /target:exe /out:bin/Debug/net9.0/MyApp.dll *.cs ...
# or more realistically today:
dotnet build → MSBuild → Roslyn task → csc.dll (hosted in dotnet.exe)
```

#### Visual Summary of Flow

```
Your .cs files
   │
   ▼
 Roslyn parses → Syntax Trees (C# code as tree structure)
   │
   ▼
 Semantic analysis / Binding → Symbols & meaning
   │
   ▼
 (here run: Source Generators → can add more .cs code)
   │
   ▼
 (here run: Analyzers → warnings, errors, code fixes)
   │
   ▼
 Lowering → simpler IL-friendly form
   │
   ▼
 Emit → IL bytes + metadata + PDB (if debugging)
   │
   ▼
 Assembly (.dll or .exe)
   │
   ▼  (at runtime)
 .NET Runtime (JIT) → machine code (x64, Arm64, etc.)
   or
 Ahead-of-Time (AOT) publish → native code directly
```

#### Important Modern Details (2025–2026)
- **Source Generators** (since C# 9 / .NET 5) run **during compilation** and can generate huge amounts of boilerplate (e.g., JSON serializers, MVVM code, EF Core models, AutoMapper mappings, endpoint metadata in Minimal APIs)
- **Incremental generators** (preferred since ~.NET 6/7) are much faster because they re-run only when needed
- **AOT compilation** (`publish -c Release -r win-x64 --self-contained`) skips JIT → native ahead-of-time (NativeAOT)
- **ReadyToRun** images = hybrid (IL + pre-generated native code for hot paths)
- **Trimming** + **ILLink** removes unused code → smaller executables

#### Quick Memory Aid
1. **Text → Tree** (parsing)  
2. **Tree → Meaning** (binding/symbols)  
3. **Meaning → IL** (lowering + emit)  
4. **IL → Machine code** (JIT or AOT at runtime/publish)

### Garbage Collector
In C#, the **Garbage Collector (GC)** is an automatic memory manager. Its job is to manage the allocation and release of memory so you don't have to manually delete objects like you would in C++.

It works using a **Mark-and-Sweep** algorithm and a **Generational** system to stay efficient.

#### 1. The Three Phases of Collection
When the GC decides to run (usually because memory is full), it goes through these three steps:
1. **Marking Phase:** The GC starts from "Roots" (static variables, local variables on the stack, and CPU registers). It follows every reference to see which objects are still reachable. It "marks" these objects as **live**.
2. **Relocating Phase:** It updates the references of the live objects so they point to the new locations where the objects will be moved in the next phase.
3. **Compacting Phase:** It "sweeps" away the dead (unmarked) objects. It then slides the surviving objects together to fill the gaps, making the memory contiguous again. This prevents **fragmentation**.

#### 2. The Generational System (Gen 0, 1, 2)
To save time, the GC doesn't check every object every time. It uses the "Generational Hypothesis": *the newer an object is, the shorter its lifespan is likely to be.*

| Generation | Purpose | Description |
| --- | --- | --- |
| **Gen 0** | **Youngest** | Newly created objects live here. This is the smallest and most frequently collected generation. |
| **Gen 1** | **Buffer** | If an object survives a Gen 0 collection, it is "promoted" to Gen 1. It acts as a buffer between short-lived and long-lived data. |
| **Gen 2** | **Oldest** | Long-lived objects (like static data or global caches) end up here. Collecting Gen 2 is expensive and happens rarely. |

#### 3. When does it trigger?
The GC is "non-deterministic," meaning you don't know exactly when it will run. It typically triggers when:
* The system is low on physical memory.
* The memory allocated to a specific generation exceeds a certain threshold.
* You manually call `GC.Collect()` (though this is usually discouraged).

#### 4. Special Case: The Large Object Heap (LOH)
Objects larger than **85,000 bytes** go to a special area called the **Large Object Heap**.
* Because moving massive objects is slow, the LOH is **not compacted** by default.
* LOH objects are considered part of **Generation 2** and are only collected during a "Full GC."

#### Summary Table: Comparison of Generations
| Generation | Collection Frequency | Typical Object Type |
| --- | --- | --- |
| **Gen 0** | High | Local variables, temporary strings, loop objects. |
| **Gen 1** | Medium | Objects that survived a quick function call. |
| **Gen 2** | Low | Application settings, database connections, static lists. |

### Value Types vs Reference Types
In C#, the distinction between value types and reference types is one of the most fundamental concepts for understanding how memory and performance work.

#### 1. Where do they live?
A common "rule of thumb" is that value types are on the stack and reference types are on the heap. While mostly true, the real rule depends on **context**:

| Scenario | Value Type (`struct`, `int`, `bool`) | Reference Type (`class`, `string`, `array`) |
| --- | --- | --- |
| **Local Variable** | **Stack** (The actual value is stored here) | **Stack** (Stores the *address*); **Heap** (Stores the *actual data*) |
| **Field in a Class** | **Heap** (Stored inside the parent object) | **Heap** (The pointer and the data are both on the heap) |
| **Field in a Struct** | Wherever the struct is (Stack or Heap) | **Heap** (Only the address is stored inside the struct) |
| **Static Field** | **Heap** (In the High-Frequency/Loader Heap) | **Heap** (In the High-Frequency/Loader Heap) |

#### 2. Key Differences at a Glance
| Feature | Value Types | Reference Types |
| --- | --- | --- |
| **Examples** | `int`, `float`, `bool`, `char`, `enum`, `struct`, `ValueTuple` | `class`, `interface`, `delegate`, `string`, `dynamic`, `Array` |
| **Assignment** | **Copies the data** (creating a new independent version). | **Copies the reference** (two variables point to the same object). |
| **Default Value** | `0`, `false`, or a "zeroed out" version of the struct. | `null` |
| **Memory Cleanup** | Removed immediately when the scope ends (if on stack). | Managed by the **Garbage Collector (GC)**. |
| **Performance** | Fast access; no GC overhead. Bad for very large objects (copying cost). | Flexible size; shared data. GC overhead and "pointer chasing" (indirection). |

#### 3. How Assignment Works
This is usually where bugs happen if you aren't careful.
* **Value Types (The "Photocopy" Analogy):** If you have a piece of paper (a variable) and give your friend a photocopy, they can scribble on their copy without affecting your original.
```csharp
int a = 10;
int b = a; // b is a new 10
b = 20; // a is still 10
```
* **Reference Types (The "Google Doc" Analogy):** If you give your friend a link to a Google Doc, you are both looking at the same document. If they delete a paragraph, you see the change immediately.
```csharp
MyClass a = new MyClass { Name = "Alice" };
MyClass b = a; // b points to the SAME object as a
b.Name = "Bob";
Console.WriteLine(a.Name); // Prints "Bob"!
```

#### 4. Special Case: The `string`
A `string` is a **Reference Type**, but it is **immutable**. This means it *behaves* like a value type in many ways. When you "change" a string, C# actually creates a brand new string in memory and points your variable to the new one, leaving the old one for the Garbage Collector.

## 2. Types & Declarations

### Nullable Types
**Nullable types** in C# come in **two completely different flavors** — and confusing them is very common.

| Feature                        | Nullable Value Types (`T?`)                          | Nullable Reference Types                             |
|--------------------------------|------------------------------------------------------|------------------------------------------------------|
| Introduced in                  | C# 2.0 (2005)                                        | C# 8.0 (2019)                                        |
| What it affects                | Value types (`int`, `bool`, `double`, `struct`, etc.)| Reference types (`string`, `object`, classes, interfaces, arrays, delegates…) |
| Syntax                         | `int?` , `bool?` , `DateTime?`                       | `string?` , `Person?` , `List<string>?`              |
| Real underlying type           | `Nullable<T>` struct                                 | Still the same type — just compiler annotation      |
| Can be `null` by default?      | Yes — that's the whole point                         | No — non-nullable by default (when feature enabled) |
| Main goal                      | Allow value types to represent "no value"            | Prevent most `NullReferenceException` at compile time |
| Enabled how?                   | Always available                                     | Must turn on `<Nullable>enable</Nullable>` in project |
| Warning when assigning `null`  | Never (it's allowed)                                 | Yes — if you try to assign `null` to non-nullable    |
| Most common use-case           | Database fields, API responses, optional numbers     | Almost everywhere (parameters, properties, returns)  |
| Performance cost               | Small boxing + struct overhead when you use `.Value` | Zero — purely compile-time feature                   |
| `HasValue` / `.Value`          | Yes                                                  | No (use `is null`, `??`, `!` null-forgiving instead) |

#### 1. Nullable Value Types (`int?`, `double?`, …)

Before C# 2.0 you **could not** write:

```csharp
int number = null;   // compile error
```

Since C# 2.0 you can:

```csharp
int? number = null;          // perfectly fine
int? count = 42;
int? nothing = null;

if (count.HasValue)
{
    Console.WriteLine(count.Value);     // 42
}

int realValue = count ?? 0;             // coalescing
int realValue2 = count!.Value;          // dangerous — throws if null
```

Very common in:
- Entity Framework / database columns that allow NULL
- JSON deserialization (missing field → null)
- Optional method parameters

#### 2. Nullable Reference Types (`string?`, `Person?`, …)

This is **not** about making something nullable — reference types were always nullable.

It's about **making the compiler warn you** when you might be using something that is null.

You turn it on in the project:

```xml
<PropertyGroup>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

After that:

```csharp
string name;               // Warning: non-nullable but never assigned!
string? maybeName = null;  // OK — explicitly nullable

Person? author = GetAuthor();          // may return null

// Warning: possible dereference of null!
Console.WriteLine(author.Name);

// Safe patterns the compiler understands
if (author is not null)
{
    Console.WriteLine(author.Name);    // no warning
}

string display = author?.Name ?? "Anonymous";
```

#### Quick Comparison Table (2025 perspective)

| Question                              | Nullable Value Type (`int?`) | Nullable Reference Type (`string?`) |
|---------------------------------------|-------------------------------|--------------------------------------|
| Can hold `null`?                      | Yes                           | Yes                                  |
| Is `null` the default value?          | No (default is `null` anyway) | No — default is non-nullable         |
| Adds runtime overhead?                | Yes (tiny)                    | No                                   |
| Helps prevent `NullReferenceException`? | Indirectly                  | Directly (compile-time warnings)     |
| Syntax sugar for                       | `Nullable<T>`                 | Just annotation + flow analysis      |
| Can use `.Value` / `HasValue`?        | Yes                           | No                                   |
| Very frequently used in modern code   | Yes (databases, DTOs)         | Yes — almost mandatory in new code   |

#### One-liner summary (2025)
- `int?`   → lets value types be missing (`null`)
- `string?` → tells the compiler "this string is allowed to be null — please warn me if I forget to check"

In modern C# projects (especially .NET 6/7/8/9) almost everyone enables `<Nullable>enable</Nullable>` and uses nullable reference types heavily.

### Structs vs Enums
**Structs** vs **Enums** in C# are two very different value types — they serve completely separate purposes.

Here's a clear, practical comparison (C# / .NET 2025–2026 perspective):

| Aspect                     | **Struct**                                      | **Enum**                                           |
|----------------------------|-------------------------------------------------|----------------------------------------------------|
| **Purpose**                | Small, lightweight **data container** (like a mini-class) | Named set of **constant values** (usually choices/states) |
| **Kind of type**           | Value type (can have fields, properties, methods, constructors) | Value type (backed by int by default, just named constants) |
| **Can hold multiple values at once?** | **Yes** (multiple fields)                       | **No** — only **one** value at a time              |
| **Can have behavior?**     | **Yes** — fields, properties, methods, indexers, events, etc. | **Limited** — only underlying value + optional attributes |
| **Can implement interfaces?** | **Yes**                                      | **Yes** (since C# 7.3+) but rare                   |
| **Can have methods?**      | **Yes** (instance + static)                     | **No** (only extension methods from outside)       |
| **Memory layout**          | Sequential fields (stack or inline)             | Single integer (usually 4 bytes)                   |
| **Default value**          | All fields zeroed (0, false, null for refs inside) | First member (usually 0)                           |
| **Typical size**           | Small (< 16–32 bytes recommended)               | 1, 2, 4, 8 bytes (byte, short, int, long backing)  |
| **Common real-world use**  | Point, Rectangle, Date (pre-DateOnly), Vector2/3, Color, small DTOs | DaysOfWeek, Status (Success/Failure), LogLevel, Suit (Card games), HTTP methods |
| **Can be null?**           | No (unless Nullable<T>)                         | No (unless Nullable<T>)                            |
| **Inheritance**            | Cannot inherit from other structs/classes (only interfaces) | Cannot inherit                                     |
| **Switch / pattern matching** | Normal object matching                          | **Very powerful** — exhaustive by default          |

#### Side-by-Side Code Examples

```csharp
// Enum → fixed set of named constants
enum OrderStatus
{
    Pending,        // 0
    Processing,     // 1
    Shipped,        // 2
    Delivered,      // 3
    Cancelled = 99  // explicit value
}

OrderStatus status = OrderStatus.Shipped;

switch (status)                     // compiler helps with exhaustiveness
{
    case OrderStatus.Pending:
    case OrderStatus.Processing:
        Console.WriteLine("Still working...");
        break;
    case OrderStatus.Shipped:
    case OrderStatus.Delivered:
        Console.WriteLine("On the way or arrived!");
        break;
    case OrderStatus.Cancelled:
        Console.WriteLine("Order cancelled.");
        break;
}
```

```csharp
// Struct → small data grouping with possible behavior
struct Point3D
{
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    public Point3D(float x, float y, float z)
    {
        X = x; Y = y; Z = z;
    }

    public double DistanceTo(Point3D other)
    {
        float dx = X - other.X;
        float dy = Y - other.Y;
        float dz = Z - other.Z;
        return Math.Sqrt(dx*dx + dy*dy + dz*dz);
    }

    public override string ToString() => $"({X}, {Y}, {Z})";
}

Point3D p1 = new(1.5f, 2.0f, -3.2f);
Point3D p2 = new(0, 0, 0);
Console.WriteLine(p1.DistanceTo(p2));   // some distance
```

#### When to Choose Which (Modern C# Guidelines)

| You want to represent...                          | Use this          | Why / Examples                                      |
|---------------------------------------------------|-------------------|-----------------------------------------------------|
| Fixed set of named states / options / categories  | **Enum**          | DaysOfWeek, FileAccess (Read/Write), LogLevel (Debug/Info/Error) |
| Small, immutable data bundle (value semantics)    | **Struct**        | Point, Vector2, Rectangle, DateOnly, TimeOnly, Color |
| Something that can have **behavior**              | **Struct**        | Complex (with operators), small math types          |
| Exhaustive switch / pattern matching on states    | **Enum**          | Compiler warns if you miss a case                   |
| Type that needs multiple pieces of data together  | **Struct**        | Coordinates, dimensions, RGB values                 |
| Flag combination (multiple choices at once)       | **Enum** + `[Flags]` | FileAttributes, BindingFlags                        |
| Small performance-critical value type             | **Struct**        | Avoid heap allocation in hot paths                  |

#### Quick Memory Aid
- **Enum**   → "This thing can be **one of these fixed named choices**" (like a dropdown)
- **Struct** → "This is a **small bundle of related data** that behaves like a value" (like int, but custom)

In modern C# (especially with records, primary constructors, `readonly struct`, etc.) structs are getting even more powerful, while enums stay simple but very expressive with pattern matching.

### Implicitly Typed Local Variables (var)
**Implicitly typed local variables** in C# (using the **`var`** keyword) allow you to declare a local variable **without explicitly writing its type**.  
The compiler **infers** (figures out) the exact type from the right-hand side of the assignment.

This feature was introduced in **C# 3.0** (2007) and is still heavily used in modern .NET code (including .NET 9 / C# 14 in 2025–2026).

#### Key Rules & Facts

| Aspect                          | Rule / Behavior                                                                 | Example                                      |
|---------------------------------|----------------------------------------------------------------------------------|----------------------------------------------|
| **Must initialize immediately** | You **must** assign a value at declaration time                                 | `var x = 42;` OK<br>`var y;` → compile error |
| **Compiler decides the type**   | Type is determined at **compile time** — **strongly typed**, not dynamic        | `var n = 10;` → `int`<br>`var list = new List<string>();` → `List<string>` |
| **Not dynamic**                 | `var` ≠ `dynamic` — no runtime type changes                                     | You cannot later assign a different type     |
| **Local variables only**        | Only for **local** variables (method scope) — **not** fields, properties, parameters | OK in methods, **not** in class fields       |
| **Anonymous types require it**  | You **must** use `var` for anonymous objects (LINQ projections, etc.)           | `var person = new { Name = "Akshay", Age = 30 };` |
| **Readability guideline**       | Microsoft recommends: use `var` **when the type is obvious** from the right side | `var customers = new List<Customer>();` → obvious |

#### Common Examples

```csharp
// Obvious cases — most people use var here
var count = 42;                        // → int
var name = "Akshay";                   // → string
var today = DateTime.Today;            // → DateTime
var numbers = new List<int> { 1, 2, 3 }; // → List<int>
var dict = new Dictionary<string, int>(); // → Dictionary<string, int>

// LINQ – almost always var (type would be very long / anonymous)
var query = from p in products
            where p.Price > 100
            select new { p.Name, p.Price };   // anonymous type → must use var

// Modern tuple deconstruction (C# 7+)
var (latitude, longitude) = GetCoordinates();   // var (double, double)
```

#### When to Use `var` vs Explicit Type (Modern Style Guidelines 2025–2026)

Microsoft's official recommendation (from .NET coding conventions):

- **Use `var`** when the type is **clear / obvious** from the right-hand side  
  → new object creation, literals, casts, method calls that clearly return one type

- **Prefer explicit type** when the type is **not immediately obvious**  
  → method calls whose return type is not clear from the name  
  → complex LINQ without projection  
  → when reading code without IntelliSense / hover

```csharp
// Good var usage
var message = GetGreeting();           // if GetGreeting() clearly returns string → OK
var users = dbContext.Users.ToList();  // clearly List<User> → good

// Better to use explicit type here (not obvious)
Customer currentCustomer = repository.GetCurrentCustomer();  
//    ^^^^^^^^ better than var — reader immediately knows type without hover
```

Popular style preferences (many teams / tools like ReSharper / Rider / .editorconfig):

- **Prefer var everywhere possible** (cleaner, less repetition)  
- **Prefer explicit** for primitives (int, bool, string) in simple cases  
- **Always explicit** when type carries important meaning (e.g. `Guid userId = ...`)

#### Quick Summary Table

| Situation                                 | Recommended Style (2025+)          | Reason / Comment                              |
|-------------------------------------------|------------------------------------|-----------------------------------------------|
| `new SomeClass()`                         | `var x = new SomeClass();`         | Type repeated twice → redundant               |
| Simple literals (`42`, `"hello"`)         | `var` or explicit (`int`, `string`) | Both fine — many prefer explicit for clarity  |
| LINQ query with anonymous type            | **Must** use `var`                 | No choice — type has no name                  |
| Method call whose return type is unclear  | Explicit type                      | Improves readability without IDE              |
| Tuple deconstruction                      | `var (a, b) = ...`                 | Very common & clean                           |
| Complex / factory method                  | Usually explicit                   | Avoids "what type is this?" questions         |

**Bottom line (one sentence):**

`var` tells the compiler: "Please infer the type from what I'm assigning right now — but it's still 100% strongly typed at compile time."

### Anonymous Types
An **anonymous type** is a way to create a simple object to hold data without having to explicitly define a class first. It is a "read-only" object that the compiler generates for you at compile time.

They are most commonly used in **LINQ queries** when you want to return a subset of properties from a database or a list without creating a permanent "Data Transfer Object" (DTO) class.

#### 1. Basic Syntax
You create an anonymous type using the `new` keyword followed by an **object initializer**. Because the type has no name, you **must** use the `var` keyword.
```csharp
var person = new { Name = "Alice", Age = 30, City = "London" };
Console.WriteLine(person.Name); // Accessing properties normally
```

#### 2. Key Characteristics
* **Properties are Read-Only:** You cannot change a property once it’s set. `person.Age = 31;` will cause a compilation error.
* **Type Safety:** Even though you didn't name the type, it is still strongly typed. The compiler creates a name like `<>f__AnonymousType0` behind the scenes.
* **Equality:** Two anonymous objects are considered equal only if they have the same properties, in the same order, with the same values.
* **Property Name Inference:** If you are creating an anonymous type from existing variables, you don't always have to name the properties.
```csharp
string product = "Laptop";
decimal price = 1200.00m;
var item = new { product, price }; // Properties will be named 'product' and 'price'
```

#### 3. Usage in LINQ
This is the "killer feature" for anonymous types. It allows you to "shape" or "project" data on the fly.
```csharp
var employees = new List<Employee> { /* ... data ... */ };
// We only want the Name and ID, not the whole Employee object
var simpleList = employees.Select(e => new {
    e.EmployeeID,
    FullName = e.FirstName + " " + e.LastName
});
foreach (var emp in simpleList)
{
    Console.WriteLine($"{emp.EmployeeID}: {emp.FullName}");
}
```

#### 4. Comparison: Anonymous Types vs. Tuples
Since C# 7.0, **Tuples** have become very popular. Here is when to use which:
| Feature | Anonymous Type | ValueTuple `(name, age)` |
| --- | --- | --- |
| **Mutability** | **Read-Only** (Immutable) | **Read/Write** (Mutable) |
| **Method Return** | Hard to return from methods (requires `object` or `dynamic`). | **Easy** to return from methods. |
| **Naming** | Meaningful property names. | Names are optional (Item1, Item2). |
| **Best Use Case** | Local LINQ queries and data shaping. | Returning multiple values from a function. |

#### 5. What happens in Memory?
Anonymous types are **Reference Types** (classes).
* They live on the **Heap**.
* The Garbage Collector manages them just like any other class instance.
* The compiler automatically overrides `.Equals()` and `.GetHashCode()` so they work correctly in collections like Dictionaries or Sets.

#### Important Limitation
You cannot use an anonymous type as a parameter for a method or as a return type easily. If you find yourself needing to pass this "temporary" data around your whole application, it's a sign you should probably define a formal `struct` or `record`.

### Namespace
**Namespace** in C# is basically a **container** or **logical grouping** mechanism that helps you **organize code** and **avoid name collisions**.

Think of it like folders on your computer — or like packages in Java — it groups related classes, interfaces, structs, enums, delegates, etc., under one named "folder" so that two people/teams can create a class with the **same name** without fighting.

#### Why do we need namespaces? (Main Purposes – 2025 perspective)
1. **Avoid name conflicts**  
   You can have 10 different classes named `User` or `Logger` in a large solution — no problem if they live in different namespaces.

2. **Organize large projects**  
   In real-world apps (especially enterprise, libraries, NuGet packages), code would be chaos without them.

3. **Make libraries reusable & discoverable**  
   When someone uses your NuGet package, they see clean structure like `Company.Project.Feature` instead of everything dumped in global scope.

4. **Improve readability & maintainability**  
   Related code lives together → easier to navigate in IDE.

#### How to declare & use namespaces

```csharp
// Classic block style (most common)
namespace MyCompany.Ecommerce.Orders
{
    public class OrderService
    {
        // ...
    }

    public class OrderRepository
    {
        // ...
    }
}
```

```csharp
// File-scoped namespace (C# 10+ – very popular in modern .NET)
namespace MyCompany.Ecommerce.Orders;

public class OrderService
{
    // All code in this file belongs to this namespace
    // No curly braces needed for the namespace
}
```

#### Using namespaces (the `using` directive)

```csharp
using System;                    // brings System namespace into scope
using MyCompany.Ecommerce.Orders; // your own namespace

// Now you can write:
var service = new OrderService();   // no need to write full name

// Or fully qualified name (no using needed)
var service2 = new MyCompany.Ecommerce.Orders.OrderService();
```

#### Real-world examples you see every day

| Namespace you use daily       | What it contains                              | Typical full name example                     |
|-------------------------------|-----------------------------------------------|-----------------------------------------------|
| `System`                      | Basic types (Console, DateTime, etc.)         | `System.Console.WriteLine()`                  |
| `System.Collections.Generic`  | List<T>, Dictionary<TKey,TValue>, etc.        | `List<string>`                                |
| `Microsoft.EntityFrameworkCore` | EF Core DbContext, DbSet, etc.             | `DbContext`                                   |
| `Newtonsoft.Json`             | Json.NET (JsonConvert, etc.)                  | `JsonConvert.SerializeObject()`               |
| `YourCompany.Api.Controllers` | Your own API controllers                      | `WeatherForecastController`                   |

#### Quick Comparison: With vs Without Namespace

Without namespace (bad in big projects):

```csharp
public class User { }           // collides with everyone else's User
public class Logger { }         // collides
```

With namespaces (clean & safe):

```csharp
namespace Banking.Domain
{
    public class User { }       // Banking.Domain.User
}

namespace Identity.Domain
{
    public class User { }       // Identity.Domain.User – no conflict!
}
```

#### Quick Memory Aid (2025 style)
- Namespace = **logical folder** for your types  
- `namespace Company.Project.Module;` = like folder path `Company/Project/Module`  
- `using` = "import this folder so I don't have to type the full path every time"  
- No namespace = goes into **global namespace** (avoid in real apps)

#### One-sentence summary
**Namespace** in C# is a named scope that groups related types together to **organize code**, **prevent name clashes**, and make large projects and libraries manageable.

### Static Classes
In C#, a **static class** is a class that cannot be instantiated (you can't use the `new` keyword with it) and can only contain static members.

Regarding your question about where they "live," the answer is a bit more nuanced than just "stack" or "heap."

#### 1. The Short Answer: Heap
Static classes and their data (static fields) are stored on the **Heap**. Specifically, they reside in a special part of the heap often called the **"High-Frequency Heap"** or **"Loader Heap."**
* **Not on the Stack:** The stack is used for local variables and method call data that disappear when a method finishes. Since static data needs to persist for the entire life of the application, the stack is not a suitable place for it.
* **On the Heap:** Because static members must be accessible globally and survive as long as the program (or the `AppDomain`) is running, the runtime allocates them on the heap.

#### 2. How Memory is Organized
When the .NET runtime (CLR) loads your program, it sets up different memory areas:
| Memory Area | What it stores | Lifetime |
| --- | --- | --- |
| **Stack** | Local variables, primitive types (int, bool), and method references. | Created on method call; destroyed on method exit. |
| **Managed Heap** | Objects created with `new` (instances of classes). | Managed by the Garbage Collector (GC). |
| **High-Frequency Heap** | **Static classes**, static fields, and Type objects. | Lifetime of the application; **not** garbage collected. |

#### 3. Key Characteristics of Static Classes
* **No Instances:** You cannot create an object of a static class. You access its members directly via the class name (e.g., `Math.Abs(-5)`).
* **Lifetime:** They are loaded by the CLR when the class is first referenced and stay in memory until the application shuts down.
* **Static Fields:** Whether a static field is a **value type** (like `static int x`) or a **reference type** (like `static string name`), the actual storage for that variable is on the heap.
* For a `static string`, the heap holds the *reference* (the pointer) and the *actual string* data.
* For a `static int`, the heap holds the *actual integer* value.

#### 4. Why does this matter?
Because static members are **not garbage collected** until the application ends, they can be a source of "memory leaks" if you aren't careful. For example, if you have a `static List<T>` and keep adding items to it without ever clearing it, that memory will never be reclaimed by the system while your app is running.

## 3. Operators & Methods

### is vs as (and Direct Cast)
Here are the **main differences** between **`is`**, **`as`** and **direct cast** `(Type)` in C#  
(very frequent interview + real-life usage question)

| Operator       | What does it do?                                 | Returns                          | Throws exception when fail? | Most common usage pattern                          | When to prefer it                     |
|----------------|--------------------------------------------------|----------------------------------|------------------------------|----------------------------------------------------|----------------------------------------|
| `is`           | **Type check** + **pattern matching**            | `bool`                           | **Never**                    | `if (obj is string s)`<br>`if (obj is not null)`   | **Safest** type checking               |
| `as`           | **Try cast** → if fails returns `null`           | The type **or** `null`           | **Never**                    | `var btn = control as Button;`                     | When you **expect it can be null**     |
| `(Type)` cast  | **Hard/direct cast**                             | The type                         | **Yes** (`InvalidCastException`) | `string s = (string)obj;`                          | When you are **100% sure** it's correct|
| `is T` + cast  | Modern & very clean pattern                      | —                                | **Never**                    | `if (obj is string s)`<br>`if (obj is Person p)`   | **Most recommended** today             |

#### Quick Comparison Table with Examples

```csharp
object obj = "hello";

// 1. is (classic + modern with pattern)
if (obj is string)                  // true
{
    string s = (string)obj;         // still need to cast
}

// Modern & best way (C# 7+)
if (obj is string s)                // ← declaration + check + cast in one line
{
    Console.WriteLine(s.Length);
}

// 2. as
string str = obj as string;         // returns string or null
if (str != null)
{
    Console.WriteLine(str.Length);
}

// 3. Direct cast → dangerous
string dangerous = (string)obj;     // works
string willCrash = (string)123;     // ← throws InvalidCastException !!!
```

#### Most Popular & Recommended Patterns (2025)

```csharp
// Winner - most clean & safe (use this the most!)
if (value is string text)
{
    Console.WriteLine($"Length is {text.Length}");
}
else if (value is int number)
{
    Console.WriteLine($"Number: {number}");
}
else if (value is null)
{
    Console.WriteLine("It was null");
}
else if (value is not null)
{
    Console.WriteLine("Some other type");
}

// Very common pattern with interfaces
if (service is IDisposable disposable)
{
    disposable.Dispose();
}

// Very common with as (especially older code / controls)
var button = sender as Button;
if (button != null)
{
    button.BackColor = Color.Red;
}

// Still see in very old code (not recommended anymore)
if (sender is Button)
{
    ((Button)sender).BackColor = Color.Blue;    // ugly double check + cast
}
```

#### Quick Decision Table – Which should I use?

| Situation                                         | Best choice                  | Why / Notes                                      |
|---------------------------------------------------|------------------------------|--------------------------------------------------|
| I just want to know the type                      | `is`                         | Cleanest, safest                                 |
| I want to use the variable if it's correct type   | `is Type var`                | **Best modern pattern**                          |
| I expect it can be null / wrong type              | `as`                         | Very readable + no exception risk                |
| I'm 100000% sure about the type                   | `(Type)`                     | Fastest, but dangerous                           |
| Very old code / WinForms / legacy                 | `as`                         | Very common in controls events                   |
| Pattern matching with many types                  | `is` + `switch` / `if-else`  | Most readable & powerful                         |

#### One-line Summary

```csharp
is      → "Is it this type?"                    → bool
as      → "Give me this type or give me null"   → Type? / null
(Type)  → "I command you to be this type!"      → Type or **boom!** exception
```

**Most used today (modern C#):**  
`if (x is string s)` → `if (x is not null)` → `if (x is SomeInterface i)`

### equals vs == (Double Equals)
**`==`** (double equals, equality operator) vs **`.Equals()`** (method) in C# — this is one of the most frequently misunderstood topics in interviews and real code.

They both check for "equality", but behave **differently** depending on **value types**, **reference types**, and whether the type has overridden behavior.

#### Quick Comparison Table (2025/2026 .NET perspective)

| Feature                        | `==` (double equals)                              | `.Equals()` method                               |
|--------------------------------|---------------------------------------------------|--------------------------------------------------|
| **Type**                       | Operator (can be overloaded)                      | Virtual instance method (can be overridden)      |
| **Defined in**                 | Language syntax + type-specific overloads         | `System.Object` (base class)                     |
| **Default for value types**    | Compares **values**                               | Compares **values** (usually same as `==`)       |
| **Default for reference types**| Compares **references** (identity)                | Compares **references** (identity)               |
| **For `string`**               | Compares **content** (overloaded)                 | Compares **content** (overridden)                |
| **Polymorphic?**               | **No** — resolved at compile time based on **static** type | **Yes** — resolved at runtime (virtual dispatch) |
| **Null safety**                | Safe (null == null → true, null == obj → false)   | Can throw `NullReferenceException` if called on null |
| **Performance**                | Usually slightly faster (no virtual call)         | Slightly slower (virtual call + possible override) |
| **Can be customized**          | Yes — overload operator                           | Yes — override method                            |
| **Common pitfall**             | `object` typed variables → reference comparison   | Calling on null → crash                          |

#### Side-by-Side Examples

```csharp
// 1. Value types (int, double, bool, struct, etc.)
int a = 42;
int b = 42;
Console.WriteLine(a == b);          // True  (value comparison)
Console.WriteLine(a.Equals(b));     // True  (same)

// 2. Reference types – default behavior
object x = new object();
object y = new object();
Console.WriteLine(x == y);          // False  (different references)
Console.WriteLine(x.Equals(y));     // False (default = reference equality)

// 3. string – special case (both compare content)
string s1 = "hello";
string s2 = new string("hello".ToCharArray());  // different object
Console.WriteLine(s1 == s2);        // True   (string overloads ==)
Console.WriteLine(s1.Equals(s2));   // True   (string overrides Equals)

// 4. The classic trap – when typed as object
object obj1 = "hello";
object obj2 = new string("hello".ToCharArray());

Console.WriteLine(obj1 == obj2);          // False  ← reference comparison!
Console.WriteLine(obj1.Equals(obj2));     // True   ← virtual call → string.Equals
```

#### When to Use Which (Modern Guidelines)

| Situation                                      | Prefer this          | Why / Recommendation                                      |
|------------------------------------------------|----------------------|-------------------------------------------------------------------|
| Comparing **value types** (int, double, bool)  | Either               | Both behave the same                                          |
| Comparing **strings**                          | `==` or `.Equals()`  | Both safe & readable (most people use `==` for strings)       |
| Comparing **custom classes/structs**           | `.Equals()`          | More polymorphic & future-proof                               |
| You have variables typed as `object`           | `.Equals()`          | `==` will do reference comparison even for strings!           |
| You need **null-safe** comparison              | `==`                 | Won't throw if left side is null                              |
| Implementing **custom equality** (GetHashCode) | Override `.Equals()` + overload `==`/`!=` | Follow the contract (record types do this automatically)      |
| Performance-critical hot path                  | `==`                 | Avoids virtual call overhead                                  |

#### Best Practice Summary (2025)
- Use **`==`** for:
  - Numbers, booleans, chars
  - Strings (very idiomatic)
  - Situations where you want reference identity explicitly

- Use **`.Equals()`** when:
  - Variables are typed as `object` or base class/interface
  - You want polymorphic behavior
  - You're implementing `IEquatable<T>` or overriding equality

- For **records** (C# 9+) → both `==` and `.Equals()` compare by **value** automatically (no boilerplate needed)

- Avoid calling `.Equals()` on a variable that **might be null** → use null-conditional or `==` instead:

```csharp
if (str?.Equals(other) == true)     // safe
// or better
if (string.Equals(str, other))      // static helper – very safe
```

### Extension Methods
An **extension method** allows you to "add" new methods to an existing type (like `string`, `int`, or even classes you didn't write) without modifying the original code, using inheritance, or recompiling the type.

Even though they are technically **static methods**, you call them as if they were **instance methods** using the dot (`.`) operator.

#### 1. How to Create an Extension Method
To create one, you must follow three strict rules:
1. The method must be inside a **static class**.
2. The method itself must be **static**.
3. The first parameter must use the **`this`** keyword followed by the type you are extending.

##### Example: Adding a `WordCount` method to the `string` class
```csharp
public static class StringExtensions
{
    // The 'this string' tells the compiler to attach this to all strings
    public static int WordCount(this string str)
    {
        if (string.IsNullOrWhiteSpace(str)) return 0;
        return str.Split(new[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}
// Usage:
string text = "Hello world from C#!";
int count = text.WordCount(); // Looks like a built-in method!
```

#### 2. How the Compiler Sees It
Extension methods are essentially "syntactic sugar." When you write `text.WordCount()`, the C# compiler actually transforms it into a standard static call behind the scenes:
* **Your Code:** `text.WordCount();`
* **What runs:** `StringExtensions.WordCount(text);`

Because of this, extension methods **cannot access private or protected members** of the class they are extending. They can only access what is publicly available.

#### 3. Why Use Them?
* **Extend Sealed Classes:** You cannot inherit from a `sealed` class (like `string` or `DateTime`). Extension methods are the only way to add functionality to them.
* **Clean LINQ Syntax:** Almost all of LINQ (e.g., `.Where()`, `.Select()`, `.OrderBy()`) is built using extension methods on the `IEnumerable<T>` interface.
* **Readability:** `myDate.IsWeekend()` is much easier to read than `DateHelper.IsWeekend(myDate)`.
* **Third-Party Libraries:** If you use a library and wish a class had one extra method, you can add it yourself without needing the source code.

#### 4. Important Limitations
| Limitation | Description |
| --- | --- |
| **Precedence** | If a class has a real instance method with the same name and signature as your extension, the **instance method always wins**. |
| **Namespace Scope** | You must import the `using` namespace where the static class lives, or the extension won't show up in IntelliSense. |
| **No Properties** | You can only create extension **methods**, not extension properties or fields. |

### Object Initializer
In C#, an **object initializer** is a piece of "syntactic sugar" that allows you to assign values to an object's fields or properties immediately when you create it, without having to write multiple lines of assignment code afterwards.

It makes your code more **declarative** (describing *what* the object should look like) rather than **imperative** (giving step-by-step instructions to set it up).

#### 1. The Syntax Comparison
Before object initializers (C# 3.0), you had to create the object first and then set its properties one by one.

**Old Way (Step-by-Step):**
```csharp
Person p = new Person();
p.FirstName = "Alice";
p.LastName = "Smith";
p.Age = 30;
```

**New Way (Object Initializer):**
```csharp
Person p = new Person
{
    FirstName = "Alice",
    LastName = "Smith",
    Age = 30
};
```

#### 2. How it works "Under the Hood"
It is important to know that the compiler actually translates the initializer back into the "Old Way."
1. The compiler calls the **parameterless constructor** (`new Person()`).
2. It then generates code to set each property you listed.
3. Finally, it assigns the finished object to your variable.

> **Note on Atomicity:** In a multi-threaded environment, using an initializer can be slightly "safer" because the variable `p` is only assigned the reference *after* the properties are set. In the "Old Way," another thread might see the object while `FirstName` is set but `LastName` is still null.

#### 3. Key Features and Rules
* **Mix with Constructors:** You can call a specific constructor and then use an initializer for the "extra" optional properties.
```csharp
var p = new Person("Alice", "Smith") { Age = 30 };
```
* **Nested Objects:** You can initialize complex objects in one go.
```csharp
var employee = new Employee
{
    Name = "Bob",
    OfficeAddress = new Address { City = "New York", Zip = "10001" }
};
```
* **Required Keyword:** In modern C# (11+), you can mark properties as `required`. This forces anyone using an object initializer to set those specific values, or the code won't compile.
* **Anonymous Types:** Object initializers are the **only** way to create anonymous types (often used in LINQ).
```csharp
var person = new { Name = "Alice", Age = 30 }; // No class definition needed
```

#### 4. Collection Initializers
A related feature is the **Collection Initializer**, which works the same way for lists and dictionaries. It automatically calls the `.Add()` method for you behind the scenes.
```csharp
// List Initializer
var names = new List<string> { "Alice", "Bob", "Charlie" };
// Dictionary Initializer
var ages = new Dictionary<string, int>
{
    ["Alice"] = 30,
    ["Bob"] = 25
};
```

#### When to use which?
| Feature | Best For... |
| --- | --- |
| **Constructor** | Enforcing **mandatory** data needed for the object to function (e.g., a Database connection string). |
| **Object Initializer** | Setting **optional** or "data-only" properties (DTOs, configuration objects, or Mocks). |

## 4. OOP & Inheritance

### Extension Methods, Partial Types, and Sealed Classes
**Extension Methods**, **Partial Types** and **Sealed Classes** in C# – Quick & Practical Comparison (2025 edition)

| Feature              | Extension Methods                          | Partial Types / Partial Classes            | Sealed Classes                            |
|----------------------|--------------------------------------------|---------------------------------------------|-------------------------------------------|
| Main purpose         | Add methods to existing types without modifying them | Split definition of one class across multiple files | Prevent inheritance (stop someone from deriving from your class) |
| Syntax keyword       | `this` parameter in static method          | `partial` keyword                           | `sealed` keyword                          |
| Where defined        | Must be in **static class**                | Same class name + `partial` in multiple files | On class declaration                      |
| Can add to          | Any type (class, struct, interface, enum, etc) | Only your own classes/structs/interfaces    | —                                         |
| Inheritance impact   | None                                       | None                                        | Blocks inheritance completely             |
| Most common use-case | LINQ, better readability, "fluent" APIs    | Very big classes, code generation (ORMs, designers) | Security, performance, design intention   |
| Performance          | Almost same as normal method call          | No impact                                   | Very tiny positive (JIT/devirtualization) |
| Can override?        | No (but can be shadowed by real method)    | —                                           | Cannot be overridden anyway               |
| Typical visibility   | Usually `public static`                    | Usually same visibility across parts        | Often `public` or `internal`              |

#### Quick Real-life Examples

```csharp
// 1. Extension Methods
public static class StringExtensions
{
    public static bool IsNullOrWhiteSpaceFast(this string? s)  // ← notice "this"
    {
        return string.IsNullOrWhiteSpace(s);
    }

    public static string FirstCharToUpper(this string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        return char.ToUpper(s[0]) + s.Substring(1);
    }
}

// Usage:
"hello world".FirstCharToUpper();           // "Hello world"
"   ".IsNullOrWhiteSpaceFast();             // true
```

```csharp
// 2. Partial Classes (very common with code generators)
partial class UserViewModel   // Part 1 - hand written
{
    public string Name { get; set; }
    public int Age { get; set; }

    public bool IsAdult => Age >= 18;
}

partial class UserViewModel   // Part 2 - generated by MVVM toolkit / EF / designer
{
    // Generated properties
    public ICommand SaveCommand { get; }
    public bool HasErrors { get; private set; }
}
```

```csharp
// 3. Sealed Classes
public sealed class CryptoService
{
    public byte[] EncryptFast(ReadOnlySpan<byte> data, byte[] key)
    {
        // Very security sensitive implementation
        // We DON'T want anyone to inherit & possibly break crypto rules
    }
}

// This will NOT compile:
// public class EvilCrypto : CryptoService { }  ← Error!
```

#### When should you use each? (2025 practical guide)

| Situation                                          | Best Choice              | Why?                                                                 |
|----------------------------------------------------|--------------------------|----------------------------------------------------------------------|
| You want to add `.ToTitleCase()` to string         | Extension Method         | Cleanest, most readable way                                          |
| Class has 2000+ lines                              | Partial Class            | Split logic / generated code / designer code                         |
| Building utility class for everyone to use         | Static class + Extensions| Most discoverable                                                    |
| Writing security critical / performance critical class | Sealed class        | Prevents dangerous inheritance + helps JIT                           |
| You want to force people to use composition instead | Sealed class         | Very strong design signal                                            |
| Writing LINQ-heavy code                            | Extension Methods        | Industry standard pattern                                            |
| Working with Blazor / WinForms / WPF designer      | Partial Classes          | Almost mandatory                                                     |

#### Very Quick Memory Aid
- **Extension methods** → "fake adding methods to existing types" ✨  
- **Partial** → "my class lives in multiple files" 🗂️  
- **Sealed** → "nobody can inherit from me, final boss" 🔒

### Abstract Classes and Method Hiding
**Abstract classes** and **method hiding** (using the `new` keyword) are two different concepts in C# inheritance — they often get confused because both involve base/derived classes and method names.

Here's a clear, side-by-side comparison (modern C# / .NET perspective, 2025–2026):

| Feature                        | **Abstract Class**                                   | **Method Hiding** (`new` keyword)                     |
|--------------------------------|------------------------------------------------------|-------------------------------------------------------|
| **Main purpose**               | Provide **partial implementation** + force derived classes to implement certain behavior | **Hide** (shadow) a base class member without polymorphism |
| **Keyword**                    | `abstract` (on class and/or methods)                 | `new` (on derived class member)                       |
| **Can the class be instantiated?** | **No** — cannot create objects directly             | Yes (base and derived both can be instantiated)       |
| **Method must be overridden?** | **Yes** — abstract methods **must** be overridden in non-abstract derived classes | **No** — hiding is optional & not polymorphic         |
| **Base method signature**      | Abstract methods have **no body**                    | Base method usually has body (can be non-virtual)     |
| **Polymorphism?**              | **Yes** — virtual dispatch (runtime binding)         | **No** — compile-time binding (static resolution)     |
| **Behavior depends on**        | **Reference type** (polymorphic)                     | **Variable type** (declared type)                     |
| **Warning if you forget?**     | Compiler error if you don't override abstract method | Warning if you hide without `new`                     |
| **Typical use case**           | Template / blueprint for family of classes (e.g., Animal → Dog, Cat) | Rare — usually when you **cannot change base class** or want different behavior for same name |
| **Recommended?**               | Very common & encouraged                             | **Avoid when possible** — leads to confusing code     |
| **Can change return type?**    | No (same signature for override)                     | Yes (covariant-like hiding possible)                  |

#### Code Examples – Side by Side

```csharp
// 1. Abstract Class + Abstract Method (polymorphic – good practice)
public abstract class Shape
{
    public abstract double CalculateArea();          // no body – MUST override

    public void Display()                            // can have concrete methods
    {
        Console.WriteLine("This is a shape.");
    }
}

public class Circle : Shape
{
    public double Radius { get; }

    public Circle(double r) => Radius = r;

    public override double CalculateArea()           // MUST use override
    {
        return Math.PI * Radius * Radius;
    }
}

// Usage – polymorphic
Shape s = new Circle(5);
Console.WriteLine(s.CalculateArea());   // calls Circle version
s.Display();                            // calls base version
```

```csharp
// 2. Method Hiding with 'new' (non-polymorphic – usually avoid)
public class BaseLogger
{
    public void Log(string message)
    {
        Console.WriteLine("Base: " + message);
    }
}

public class ConsoleLogger : BaseLogger
{
    public new void Log(string message)              // hides base method
    {
        Console.WriteLine("Console: " + message.ToUpper());
    }
}

// Usage – behavior depends on variable type!
BaseLogger b = new ConsoleLogger();
b.Log("hello");           // → "Base: hello"          (hides, but base version called)

ConsoleLogger c = new ConsoleLogger();
c.Log("hello");           // → "Console: HELLO"       (derived version called)
```

#### Real-World Decision Guide

| You want...                                      | Use this                  | Why / Recommendation                                      |
|--------------------------------------------------|---------------------------|-------------------------------------------------------------------|
| Force derived classes to implement method        | Abstract method           | Clean contract – compiler enforces                                |
| Provide default implementation + allow override  | Virtual method            | Most flexible polymorphism                                        |
| Partial common behavior + some must-be-implemented | Abstract class          | Classic use (Animal, Vehicle, ControllerBase, etc.)               |
| Base class not under your control & you need different method with same name | Method hiding (`new`)   | Rare legitimate use – but document it heavily                     |
| Different return type for "same" method          | Method hiding             | Sometimes used for covariance simulation (pre-C# 9)               |
| Predictable polymorphic behavior                 | Abstract / virtual + override | **Always prefer this** over hiding                                |

#### Quick Memory Aid
- **Abstract class/method** → "I'm incomplete – you **must** finish me" + polymorphism  
- **Method hiding (`new`)** → "I'm **pretending** to be something else with the same name" + no polymorphism

**Bottom line (2025 best practice):**

Use **abstract classes** (and virtual/override) for proper OOP design and polymorphism.  
Use **method hiding** (`new`) **only** when you really have no other choice (e.g., third-party base class you can't modify) — and even then, consider composition over inheritance instead.