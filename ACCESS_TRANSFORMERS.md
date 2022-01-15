# Access Transformers
Access transformers are files that give you the ability to modify the accessibility and readonly state of fields, methods, and classes.

### Accessors
`public` - Changes to `public`.
`internal` - Changes to `internal`.
`private` - Changes to `private`.
`protected` - Changes to `protected`.
`private-protected` - Changes to `private protected`.
`protected-internal` - Changes to `protected internal`.
`=` - No change.

### Readonly
`+r` - Changes to `readonly`.
`-r` - Removes `readonly`.
`=` - No change.

Readonly's meaning changes depending on targets in our case.

In classes, they decide whether a class is sealed. If a class is abstract, this is ignored.

in methods, they decide whether a method is sealed.

In fields, they decide whether a field is readonly.

### Targets
Targets can be classes, methods, or fields. Properties, events, and operators are all methods. Interfaces are currently not supported.

### Comments
Comments start with `#` and are completely ignored.
