![ES-5](https://github.com/user-attachments/assets/3360a2e6-5e1d-4643-b946-b61e9bef3b49)

## 🥁 CarnaCode 2026 - Challenge 10 - Facade

Hi, I'm Felipe Parizzi Galli, and this is the space where I share my learning journey during the **CarnaCode 2026** challenge, hosted by [balta.io](https://balta.io). 👻

Here you will find projects, exercises, and code that I am building throughout the challenge. The goal is to get hands-on, test ideas, and document my growth in the tech world.

### About this challenge
In the **Facade** challenge, I had to solve a real-world problem by implementing the corresponding **Design Pattern**.
During this process, I learned:
* ✅ Software Best Practices
* ✅ Clean Code
* ✅ SOLID
* ✅ Design Patterns

## Problem
The checkout flow involves multiple subsystems (inventory, payment, shipping, notifications, and coupons), each with complex interfaces.
The client code must know and orchestrate all of them, resulting in complex and tightly coupled code.

## About CarnaCode 2026
The **CarnaCode 2026** challenge consists of implementing all 23 design patterns in real-world scenarios. Across the 23 challenges in this journey, participants practice identifying non-scalable code and solving problems using industry-standard patterns.

### eBook - Design Pattern Fundamentals
My main knowledge source during this challenge was the free eBook [Fundamentos dos Design Patterns](https://lp.balta.io/ebook-fundamentos-design-patterns).

## What was implemented to apply the Facade pattern
To implement the Facade pattern in this challenge, the following was done:

* The e-commerce workflow complexity was identified (inventory, payment, shipping, coupons, and notifications).
* Interfaces were introduced for each subsystem contract, reducing direct coupling with concrete implementations.
* Subsystems were organized into dedicated folders and files, with one class/interface per file.
* Namespaces were aligned with the folder structure to improve maintainability and project organization.
* `Challenge.cs` was kept as the entry point, while orchestration was prepared to consume abstractions instead of concrete details.
* The codebase was validated after refactoring to ensure it still compiles and preserves behavior.
