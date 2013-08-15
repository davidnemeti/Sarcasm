Sarcasm
=======
Sarcasm is an SDK for creating typesafe and typeless domain bound grammar and using the general unparser.

It provides the following features:

  - Index-free AST building using domain-grammar bindings.
  - Typesafe grammar and typesafe AST building using domain-grammar bindings.
  - Automatic unparsing provided by the general unparser based on domain bound grammar.

In a domain bound grammar you can write this:

```
binaryExpression.Rule =
    expression.BindTo(binaryExpression, t => t.Term1)
    + binaryOperator.BindTo(binaryExpression, t => t.Op)
    + expression.BindTo(binaryExpression, t => t.Term2)
    ;
```

For more info see the [Introduction](https://github.com/davidnemeti/Sarcasm/wiki/Introduction).

===
#License information

Sarcasm is released under the GNU Lesser General Public License (LGPL). It means that you can use it freely as a library even in your prorietary (closed source) software. However if you copy or modify the whole source code or part of it, that derivate work should also be released under the LGPL. For legal license see [License](License/License.txt). For precise license information see [License description](Licenses). For usable information see [LGPL on Wikipedia](http://en.wikipedia.org/wiki/GNU_Lesser_General_Public_License).
