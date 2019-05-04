# Cartographer
Cartographer is a lightweight, asynchronous, .Net logging library.

https://www.nuget.org/packages/CartographerLogger/

# Features
Cartographer is a logging library designed to log messages asynchronously so that it impacts the speed of your code as little as possible.
  - Easily log at any time with context data such as caller class, method, line number and thread ID.
  - Log exception stack traces.
  - Use logging levels and apply filters to set the logging level.
  - Print to the console.
  - Log file rollover to keep max file sizes down.
  - Set a custom padding size to keep your log files lined up.
  - Use the experimental stack trace functionality to see how your code gets compiled in release mode.

# Usage example:
Supply a logging location file path, include the file extension, and set your configuration.

```c#
var cartographer = new Cartographer.Cartographer("log_file_location")
{
    LoggingLevelToPrint = LoggingLevel.Debug,
    PrintContextData = true,
    UseStackTrace = true,
    MaxFileSize = 40000000,
    PaddingSize = 12
};
cartographer.Log("Your log message", LoggingLevel.Info);
```    

Caught an exception?

```c#
cartographer.Log("Caught an exception", LoggingLevel.Error, exception);
```

Get the Cartographer task status:

```c#
cartographer.Status();
```

You do not need to supply the optional arguments. Cartographer uses callerMember attributes to get the class name, method name and line numbers.

You can supply an array of messages and each message will be printed in the same log message, all seperated with a comma.

If you have specified a MaxFileSize, then your log files will rollover, meaning that you logging.txt will become logging.0000.txt and will then increase in numerical value each time you fill a log file. A MaxFileSize of 0 turns off log file rollover.

The PaddingSize option allows you to specify the minimum of space that method and class names take up in the file.
If you use long class and/or method names, increase it to a value that's a factor of 4 (space length of a tab) and it will space out all logging to use that minimum space, keeping your logging lined up.

If you opt to use the stack trace capability. You may see a decrease in performance as generating a stack trace per log is expensive, Especially if you log aggressively.

The default configuration:
```
PrintToConsole = false;
LoggingLevelToPrint = LoggingLevel.Trace;
PrintContextData = true;
UseStackTrace = false;
MaxFileSize = 0;
PaddingSize = 16;
```


License
----

MIT
