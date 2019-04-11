# Cartographer
Cartographer is a lightweight, asynchronous, .Net logging library.

# Features
Cartographer is a logging library designed to log messages asynchronously so that it impacts the speed of your code as little as possible.
  - Easily log at any time with context data such as caller class, method, line number and thread ID.
  - Log exception stack traces.
  - Use logging levels and apply filters to set the logging level.
  - Print to the console.
  - Log file rollover to keep max file sizes down.
  - Use the experimental stack trace functionality to see how your code gets compiled in release mode.

# Usage example:
Supply a logging location file path, include the file extension, and set your configuration.

```c#
var cartographer = new Cartographer.Cartographer("log_file_location")
{
    LoggingLevelToPrint = LoggingLevel.Debug,
    PrintContextData = true,
    UseStackTrace = true,
    MaxFileSize = 40000000      
};
cartographer.Log("Your log message", LoggingLevel.Info);
```    
Caught an exception?

```c#
cartographer.Log("Caught an exception", LoggingLevel.Error, exception);
```

You do not need to supply the optional arguments. Cartographer uses callerMember attributes to get the class name, method name and line numbers.

If you have specified a MaxFileSize, then your log files will rollover, meaning that you logging.txt will become logging0000.txt
and will then increase in numerical value each time you fill a log file. A MaxFileSize of 0 turns off log file rollover.

If you opt to you the stack trace capability. You may see a decrease in performance as generating a stack trace per log is expensive, Especially if you log aggressively.

The default configuration:
```
PrintToConsole = false;
LoggingLevelToPrint = LoggingLevel.Trace;
PrintContextData = true;
UseStackTrace = false;
MaxFileSize = 0;
```


License
----

MIT
