# Sarcasm

[![NuGet](https://img.shields.io/nuget/v/Sarcasm.svg)](https://www.nuget.org/packages/Sarcasm) [![NuGet](https://img.shields.io/github/release/davidnemeti/Sarcasm?display_name=tag&sort=semver)](../../releases/latest) [![Build](https://github.com/davidnemeti/Sarcasm/actions/workflows/build.yml/badge.svg)](https://github.com/davidnemeti/Sarcasm/actions/workflows/build.yml) [![License](https://img.shields.io/badge/license-LGPLv3-green)](https://licenses.nuget.org/LGPL-3.0-only)

## Summary

Sarcasm is an SDK for creating typesafe, domain-bound grammars. It also has a grammar driven general unparser.

Sarcasm provides the following features:

  - Index-free AST building using domain-grammar bindings.
  - Typesafe AST building using typesafe grammar.
  - Automatic unparsing provided by the general unparser based on the grammar.

In a Sarcasm's domain-bound grammar you can write this e.g.:

```c#
binaryExpression.Rule =
    expression.BindTo(binaryExpression, t => t.Term1)
    + binaryOperator.BindTo(binaryExpression, t => t.Op)
    + expression.BindTo(binaryExpression, t => t.Term2)
    ;
```

For more information see the [Documentation](https://github.com/davidnemeti/Sarcasm/wiki). You can start with [Introduction](https://github.com/davidnemeti/Sarcasm/wiki/Introduction).

---

### License information

**Sarcasm** has been released under the **GNU Lesser General Public License (LGPL)**. It means that you can use it **freely** as a library even in your **prorietary (closed source) software**. However, if you copy or modify the whole source code or part of it, that **derivate work** should also be released under the LGPL.

Hint: if you want to use Sarcasm in your proprietary software, but you need to modify Sarcasm's source code in order to get some extra features, modified behavior or bugfixes, you can simply *clone/fork* Sarcasm repository, do the changes in the clone repository, release it under LGPL, and use that modified library in your proprietary software, while keeping your proprietary software's source code closed. You can even ask for a *pull request* to the original Sarcasm repository if your changes are bugfixes, or you find that your extra features, modified behavior should be in the original Sarcasm SDK.

For legal license see [License](License/License.txt).

For *precise, legal* license information see [License descriptions](License).

For *usable, comprehensible* information see [LGPL on Wikipedia](http://en.wikipedia.org/wiki/GNU_Lesser_General_Public_License).

As of Jan 31, 2023, **Irony** is released under the *MIT License*, which is even more permissive than LGPL. For more and up to date information see [Irony's License](https://github.com/IronyProject/Irony/blob/master/LICENSE). About the MIT License see [MIT License on Wikipedia](http://en.wikipedia.org/wiki/Mit_license).
