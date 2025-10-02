# InvokerApp

A program that invokes and shows the result of a CLI command.

# Requirements

You need .NET 9.0 for this.

# Compilation

Just decompress the folder anywhere and use the common procedure to build & run.

```bash
$ dotnet build
$ dotnet run
```

# Example

Say that you want to execute a *Python* program.

In the *Exe* textbox, write `python`.

In the *Args* textbox, write `$f`.

In the input box, write a *Python* program, such as:

```python
print("Hola")
```
Click the "..." button for the input file. Write `input.py`.
Then click `Launch`.

The Output panel will show:

```
Exited: True
Exit code: 0
Cmd: python input.py
Hola
```

