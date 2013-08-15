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

For more info see the [[Introduction]].
