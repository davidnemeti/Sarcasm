#Sarcasm

Sarcasm is an SDK for creating typesafe and typeless domain bound grammar and using the general unparser.

It provides the following features:

  - Index-free AST building using domain-grammar bindings.
  - Typesafe grammar and typesafe AST building using domain-grammar bindings.
  - Automatic unparsing provided by the general unparser based on domain bound grammar.

In a domain bound grammar you can write this e.g.:

```
binaryExpression.Rule =
    expression.BindTo(binaryExpression, t => t.Term1)
    + binaryOperator.BindTo(binaryExpression, t => t.Op)
    + expression.BindTo(binaryExpression, t => t.Term2)
    ;
```

For more information see the [Introduction](https://github.com/davidnemeti/Sarcasm/wiki/Introduction).

===

###License information

Sarcasm has been released under the **GNU Lesser General Public License (LGPL)**. It means that you can use it **freely** as a library even in your **prorietary (closed source) software**. However, if you copy or modify the whole source code or part of it, that **derivate work** should also be released under the LGPL.

Hint: if you want to use Sarcasm in your proprietary software, but you need to modify Sarcasm's source code in order to get some extra features, modified behavior or bugfixes, you can simply *clone/fork* Sarcasm repository, do the changes in the clone repository, release it under LGPL, and use that modified library in your proprietary software, while keeping your proprietary software's source code closed. You can even ask for a *pull request* to the original Sarcasm repository if your changes are bugfixes, or you find that your extra features, modified behavior should be in the original Sarcasm SDK.

For legal license see [License](License/License.txt).

For *precise, legal* license information see [License descriptions](License).

For *usable, comprehensible* information see [LGPL on Wikipedia](http://en.wikipedia.org/wiki/GNU_Lesser_General_Public_License).

As of Aug 18, 2013, **Irony SDK** is released under the *MIT License*, which is even more permissive than LGPL. For more and up to date information see [Irony's License](http://irony.codeplex.com/license). About the MIT License see [MIT License on Wikipedia](http://en.wikipedia.org/wiki/Mit_license).
