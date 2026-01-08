# Calculator – Coding Exercise

This repository contains a **C# console application** that implements a string-based calculator following a step-by-step set of requirements.  
Each requirement was treated as an independent **requirement change** and implemented incrementally, with a strong focus on **readability, separation of concerns, and testability**.

---

## 📌 Problem Summary

Create a calculator that:
- Accepts a **single formatted string**
- Supports **addition** (and extended operations as stretch goals)
- Handles multiple delimiter formats
- Applies validation rules (invalid numbers, negatives, upper bounds)
- Is fully covered by **unit tests**
- Shows each requirement as a **separate commit**

---

## 🧱 Solution Architecture

The solution is split into **three projects**:

/src
├── StringCalculator → Console application
├── StringCalculator.Core → Core logic (parser, rules, calculator)
/tests
└── StringCalculator.Tests → xUnit test suite


### Why this structure?
- **Console app** only handles input/output
- **Core library** contains all business logic (fully testable)
- **Tests** validate every requirement and edge case independently

---

## 🧠 Core Design Principles

- **Single Responsibility**  
  Parsing, validation, calculation, formatting, and operations are separated.
- **Readable over clever**  
  No complex regex-heavy logic unless necessary.
- **Extensible**  
  New delimiters, rules, or operations can be added without touching existing code.
- **Test-driven mindset**  
  Every requirement is backed by unit tests.

---

## ✅ Implemented Requirements

### 1️ Basic Addition (max 2 numbers, comma delimiter)
- `"20"` → `20`
- `"1,5000"` → `5001`
- Empty or missing values → `0`
- Invalid values → `0`

> Initially limited to 2 numbers (later removed per requirement #2).

---

### 2 Unlimited Numbers
- `"1,2,3,4,5"` → `15`

---

### 3 Newline as Delimiter
- `"1\n2,3"` → `6`

---

### 4 Deny Negative Numbers
- Any negative value causes an exception
- Exception message includes **all negative numbers**

Negatives not allowed: -2,-3


---

### 5️ Upper Bound Rule
- Any value **greater than 1000** is considered invalid
- `"2,1001,6"` → `8`

---

### 6️ Custom Single-Character Delimiter
Format:
//{delimiter}\n{numbers}


Example:
//#\n2#5 → 7


---

### 7 Custom Delimiter of Any Length
Format:
//[{delimiter}]\n{numbers}


Example:
//[]\n1122***33 → 66


---

### 8️ Multiple Custom Delimiters of Any Length
Format:
//[{d1}][{d2}]...\n{numbers}


Example:
//[][!!][r9r]\n11r9r22hh*33!!44 → 110


---

## 🌟 Stretch Goals Implemented

### ⭐ 1. Formula Display
In addition to returning the result, the calculator can return:
2+0+4+0+0+6 = 12


This is done via:
- `CalculationResult`
- `CalculateDetailed(...)`

---

### ⭐ 2. Continuous Input Until Ctrl+C
The console application:
- Continuously processes user input
- Gracefully exits on `Ctrl+C`

---

### ⭐ 3. Configurable via CLI Arguments
Supported arguments:
- `--alt-delim=";"`
- `--deny-negatives=false`
- `--upper=2000`
- `--formula=true`
- `--op=add|sub|mul|div`

---

### ⭐ 4. Dependency Injection
- Uses `Microsoft.Extensions.DependencyInjection`
- All core services (`IInputParser`, `INumberRules`, calculator, operations) are registered via DI

---

### ⭐ 5. Multiple Operations
Supported operations:
- Addition
- Subtraction
- Multiplication
- Division (with divide-by-zero protection)

Each operation implements:
```csharp
ICalculatorOperation


🧪 Unit Testing

Framework: xUnit

Coverage includes:

All requirements (#1–#8)

All stretch features

Edge cases:

Missing values

Invalid tokens

Negative numbers

Upper bound behavior

Custom delimiters

Divide-by-zero

Formula formatting

Example test:

Assert.Equal("2+0+4+0+0+6 = 12", result.Formula);

▶️ How to Run
Run the console app
dotnet run --project src/StringCalculator

Run with options
dotnet run --project src/StringCalculator -- \
  --alt-delim=";" \
  --deny-negatives=true \
  --upper=1000 \
  --formula=true \
  --op=add

Run tests
dotnet test

🧾 Commit Strategy

Each requirement was implemented as a separate commit, following this order:

Basic addition (max 2 numbers)

Unlimited numbers

Newline delimiter

Negative number validation

Upper bound rule

Custom single-character delimiter

Custom delimiter of any length

Multiple delimiters

Stretch goals (clearly separated commits)

This allows reviewers to follow the evolution of the solution step by step.

📎 Final Notes

The solution prioritizes clarity, maintainability, and correctness

Business rules are fully isolated from I/O concerns

The design is intentionally extensible for future requirements